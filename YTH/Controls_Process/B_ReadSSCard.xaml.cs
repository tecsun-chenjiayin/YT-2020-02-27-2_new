using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YTH.Functions;

namespace YTH.BuKa
{
    /// <summary>
    /// B_ReadSSCard.xaml 的交互逻辑
    /// </summary>
    public partial class B_ReadSSCard : UserControl
    {
        #region --------电动机--------

        [DllImport("TSCardDriver.dll", EntryPoint = "iOpenMotor", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenMotor(int iPort, int iBaud, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iCloseMotor", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseMotor(StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iInitMotor", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iInitMotor(byte eject, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iSetEnterCardType", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iSetEnterCardType(byte type, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iMoveCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iMoveCard(byte position, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iReadMagcardDecode", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadMagcardDecode(byte Track, StringBuilder pOutInfo);




        [DllImport("TSSelfSendDev.dll", EntryPoint = "iReadCardBasExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadCardBasExt(int iReaderCtrl, int iType, StringBuilder pOutInfo);//读基本信息。

        [DllImport("TSSelfSendDev.dll", EntryPoint = "iRepairCardExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iRepairCardExt(int iReaderCtrl, int iType, int iAuthType, string pFileAddr, string pFileInfo, string pUserInfo, StringBuilder pOutInfo);//卡修复。

        [DllImport("TSSelfSendDev.dll", EntryPoint = "iReadCardExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadCardExt(int iReaderCtr, int iType, int iAuthType, string pPIN, string pFileAddr, string pUserInfo, StringBuilder pOutInfo);

        [DllImport("TSSelfSendDev.dll", EntryPoint = "iReadBankNumExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadBankNumExt(int iReaderCtrl, int iType, StringBuilder pOutInfo);

        [DllImport("TSSelfSendDev.dll", EntryPoint = "iGetATRExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iGetATRExt(int iReaderCtrl, int iType, StringBuilder pOutInfo);

        [DllImport("TSSelfSendDev.dll", EntryPoint = "iChangePINKExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iChangePINKExt(int iReaderCtrl, int iType, string pOldPIN, string pNewPIN, StringBuilder pOutInfo);//PIN修改
        #endregion

        private static bool hasCardIn = false;
        Action nextStep = null;
        string timeTag = null;
        string carMsg = null;
        public static string name = "";
        public static string persionid = "";
        private static void reset()
        {
            name = "";
            persionid = "";
        }

        static object readcardlock = new object();

        static B_ReadSSCard self = null;
        public static B_ReadSSCard GetObject()
        {
            if (self == null)
                self = new B_ReadSSCard();
            return self;
        }


        public static bool HasCardIn()
        {
            return hasCardIn;
        }
        public void BeforeGoin(Action action)
        {
            nextStep = action;
        }
        async public void Goin()
        {
            reset();
            carMsg = null;
            //BackExit.setBack(Goin);
            timeTag = CD.timeTag.updateTag();
            CD.setTopUI(this);
            time.start();

            string error = null;
            bool isSuccess = false;
            await TaskMore.Run(new Action(() => {
                lock(readcardlock)
                {
                    if (CD.timeTag.equal(timeTag) == false)
                        return;
                    //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                    //0         1       2         3               4         5         6     7             8     9     10    11      12
                    isSuccess = putCardIn(out error);
                    if (isSuccess)
                        isSuccess = ReadCard(out carMsg, out error);
                    if (isSuccess == false)
                        putCardOut();
                    else
                    {
                        string[] data = carMsg.Split('|');
                        name = data[8];
                        persionid = data[7];

                    }
                }
            })).ConfigureAwait(true);
            if (CD.timeTag.equal(timeTag) == false)
                return;
            if (isSuccess == false)
            {
                ShowTip.show(false, BackExit.Exit, error);
            }
            else
            {
                hasCardIn = true;
                if (nextStep != null && CD.timeTag.equal(timeTag))
                    nextStep();
            }
            time.stop();
        }



        static int port = 0;
        static int baud = 0;
        public B_ReadSSCard()
        {
            InitializeComponent();
        }

        public static bool isOPen = false;
        public static bool putCardIn(out string error)
        {
            error = null;
            int ret = 0;
            //if (isOPen == false) {
            //    StringBuilder pOutInfo = null;
            //    ret = iOpenMotor(port, baud, pOutInfo);
            //    if (ret != 0)
            //    {
            //        error = "电动机打开失败：" + ret;
            //        return false;
            //    }
            //    else
            //        isOPen = true;
            //}
            port = int.Parse(Config.dic("DDZ"));
            baud = int.Parse(Config.dic("DDZBaud"));
            ret = MoveCard(port, baud, true, out error);
            if (error.IndexOf("命令不能执行") != -1)
                error = "没有检测到插卡，请重试";
            hasCardIn = (ret == 0);
            return ret == 0;
        }
        //读社保卡
        public static bool putCardOut()
        {
            reset();
            string error = null;
            int ret = 0;
            if (isOPen == false)
            {
                StringBuilder pOutInfo = null;
                port = int.Parse(Config.dic("DDZ"));
                baud = int.Parse(Config.dic("DDZBaud"));
                ret = iOpenMotor(port, baud, pOutInfo);
                if (ret != 0)
                {
                    error = "电动机打开失败：" + ret;
                    return false;
                }
                else
                    isOPen = true;
            }
            string outstr = null;
            ret = MoveCard(port, baud, false, out outstr);
            hasCardIn = !(ret == 0);
            return ret == 0;
        }

        public static int MoveCard(int port, int baud, bool moveTye, out string outInfo)
        {
            try
            {
                outInfo = string.Empty;
                StringBuilder sbOutInfo = new StringBuilder(1024);
                int ret = 0;
                if (isOPen == false) {
                    ret = iOpenMotor(port, baud, sbOutInfo);
                    if (ret != 0)
                    {
                        outInfo = sbOutInfo.ToString();
                        return ret;
                    }
                    else
                        isOPen = true;
                }
                sbOutInfo.Clear();
                if (moveTye)
                {
                    ret = iInitMotor(0, sbOutInfo);
                    if (ret != 0)
                    {
                        outInfo = sbOutInfo.ToString();
                        iCloseMotor(sbOutInfo);
                        isOPen = false;
                        return ret;
                    }

                    sbOutInfo.Clear();
                    ret = iSetEnterCardType(3, sbOutInfo);
                    if (ret != 0)
                    {
                        outInfo = sbOutInfo.ToString();
                        iCloseMotor(sbOutInfo);
                        isOPen = false;
                        return ret;
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        sbOutInfo.Clear();
                        ret = iMoveCard(4, sbOutInfo);
                        if (ret != 0)
                        {
                            outInfo = sbOutInfo.ToString();
                            System.Threading.Thread.Sleep(1500);
                            continue;
                        }
                        else
                        {
                            outInfo = "Move succeed.";
                            isOPen = true;
                            break;
                        }
                    }

                }
                else
                {
                    ret = iMoveCard(1, sbOutInfo);
                    if (ret != 0)
                    {
                        outInfo = sbOutInfo.ToString();
                        iCloseMotor(sbOutInfo);
                        isOPen = false;
                        return ret;
                    }
                }

                iCloseMotor(sbOutInfo);
                isOPen = false;
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
        //0         1       2         3               4         5         6     7             8     9     10    11      12
        public static bool ReadCard(out string outInfo, out string error)
        {
            error = null;
            outInfo = null;
            try
            {
                int ctl = int.Parse(Config.dic("Up"));
                StringBuilder sbOutInfo = new StringBuilder(1024);
                int ret = iReadCardBasExt(ctl, 1, sbOutInfo);
                outInfo = sbOutInfo.ToString();
                return ret == 0;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }

        //修改密码
        public static int ChangePINKExt(int iReaderCtrl, string oldPin, string newPin, out string pOutInfo)
        {
            try
            {
                int ret = 0;
                pOutInfo = string.Empty;

                StringBuilder sbOutInfo = new StringBuilder(1024);

                ret = iChangePINKExt(iReaderCtrl, 1, oldPin, newPin, sbOutInfo);

                pOutInfo = sbOutInfo.ToString();
                return ret;
            }
            catch (Exception ex)
            {
                pOutInfo = ex.ToString();
                return -99;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CD.timeTag.updateTag();
            CD.setTopUI(null);
            time.stop();
        }
    }

}
