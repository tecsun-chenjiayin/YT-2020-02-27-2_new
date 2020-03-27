using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using YTH.Functions.MSDLL;
using YTH.NHNet;

namespace YTH.ZhanJiang
{
    /// <summary>
    /// AddCard.xaml 的交互逻辑
    /// </summary>
    public partial class AddCard : UserControl
    {
        public AddCard()
        {
            InitializeComponent();
            time.whenExit = WhenExit;
            MediaPlayer1.MediaEnded += WhenPlayEnd;//循环播放
            MediaPlayer2.MediaEnded += WhenPlayEnd;
            MediaPlayer3.MediaEnded += WhenPlayEnd;
        }
        bool hasPath1 = false;
        bool hasPath2 = false;
        bool hasPath3 = false;
        int sNum = 0;
        int fNum = 0;
        static AddCard obj = null;
        public static AddCard GetObject()
        {
            if (obj == null)
                obj = new AddCard();
            return obj;
        }

        public void Goin()
        {
            BackExit.setBack(Goin);
            sNum = 0;
            fNum = 0;
            CD.setMainUI(this);
            time.start();
            successNum.Text = "0";
            failedNum.Text = "0";
            status.Text = "空闲";

            hasPath1 = false;
            hasPath2 = false;
            hasPath3 = false;
            string path1 = CD.getBasePath() + "Media\\openBox.mp4";
            string path2 = CD.getBasePath() + "Media\\addCar.mp4";
            string path3 = CD.getBasePath() + "Media\\closeBox.mp4";
            if (File.Exists(path1))
            {
                MediaPlayer1.Source = new Uri(path1);
                MediaPlayer1.Play();
                hasPath1 = true;
            }
            else
                MessageBox.Show("文件丢失：" + path1);

            if (File.Exists(path2))
            {
                MediaPlayer2.Source = new Uri(path2);
                MediaPlayer2.Play();
                hasPath2 = true;
            }
            else
                MessageBox.Show("文件丢失：" + path2);

            if (File.Exists(path3))
            {
                MediaPlayer3.Source = new Uri(path3);
                MediaPlayer3.Play();
                hasPath3 = true;
            }
            else
                MessageBox.Show("文件丢失：" + path3);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (status.Text == "空闲" || status.Text == "加卡结束")
            {
                WhenExit();
                time.stop();
                BackExit.Back();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (status.Text == "空闲")
            {
                time.stop();
                status.Text = "正在加卡";
                Thread t = new Thread(new ThreadStart(addCar2));
                t.Start();
            }
            else
                return;
        }

        private void WhenPlayEnd(object sender, RoutedEventArgs args)
        {
            MediaElement me = (MediaElement)args.Source;
            me.Stop();
            me.Play();
        }

        private void WhenExit()
        {

            if (hasPath1)
                MediaPlayer1.Stop();
            if (hasPath2)
                MediaPlayer2.Stop();
            if (hasPath3)
                MediaPlayer3.Stop();
        }

        private void Exit()
        {
            WhenExit();
            BackExit.Exit();
            time.stop();
        }

        private void addSuccess()
        {
            sNum++;
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                successNum.Text = sNum.ToString();
            }));
        }
        private void addFailed()
        {
            fNum++;
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                failedNum.Text = fNum.ToString();
            }));
        }
        private void End()
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                status.Text = "加卡结束";
                ShowTip.show(true, null, "加卡完成!");
            }));
        }
        private void ShowError(string error)
        {
            Log(error);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                ShowTip.show(false, null, error);
                status.Text = "加卡结束";
            }));
        }

        //业务流程

        public void addCar2()
        {
            try
            {
                MS2.Open();
                string error = MS2.checkMachine();
                if (error != null)
                {
                    Log(error);
                    ShowError("设备异常：" + error);
                    return;
                }

                string addCarError = null;
                MakeJson mj = new MakeJson();
                tools.AnalyzeJson aj = Network3.getJson(mj, "deviceLogin", out error);
                if (error != null)
                {
                    Log(error);
                    addCarError = "网络异常";
                }
                else if (aj != null && aj["statusCode"].ToString() != "200")
                {
                    Log(error);
                    addCarError = "设备登录失败。";
                }
                //处理
                if (addCarError != null)
                {
                    ShowError(addCarError);
                    return;
                }

                MS2.ResetWithOutPrint();
                string boxs = Config.dic("addCardBoxs");
                string[] boxs2 = boxs.Split('|');
                foreach (string box_ in boxs2)
                {
                    int box = int.Parse(box_);
                    while (true)
                    {
                        //1、料盒出卡
                        error = MS2.GetCarFromBox(box);
                        if (error != null && error == "-1")
                            break;
                        else if (error != null)
                        {
                            ShowError("出卡异常，请联系管理员处理");
                            return;
                        }
                        //2、移到读卡器
                        string atr = MS2.GetATR(out error);
                        if (error != null)
                        {
                            addFailed();
                            MS2.PutCardToReject();
                            if (error.IndexOf("-1") != -1)
                            {
                                ShowError("读卡异常，请联系管理员处理");
                                return;
                            }
                            continue;
                        }
                        //3、读银行卡号
                        string bankcarNum = MS2.ReadBankNum(out error);
                        if (error != null)
                        {
                            addFailed();
                            MS2.PutCardToReject();
                            continue;
                        }
                        //4、读社保基本信息
                        string[] carDatas = MS2.GetBaseMsg(out error);
                        if (error != null)
                        {
                            addFailed();
                            MS2.PutCardToReject();
                            continue;
                        }
                        //5、入库
                        int stlo = 0;
                        error = MS2.PutCardToStore(atr, ref stlo);
                        if (error != null)
                        {
                            addFailed();
                            ShowError(error);
                            return;
                        }
                        //6、接口入库
                        /*
                         channelcode	String	是	渠道编码
    orgCode	String	是	网点编码(接口2.5返回orgCode)
    devSeq	String	是	设备序号(接口2.5返回devSeq)
    atr	String	是	ATR
    ksbm	String	是	卡识别码（社保卡时必填）
    yhkh	String	是	银行卡号
    shbzh	String	是	社会保障号（社保卡时必填）
    sfzh	String	是	身份证（社保卡时必填）
    xm	String	是	姓名（社保卡时必填）
    slotno	int	是	槽号
    orgId	long	是	网点id（2.5接口返回的orgId）
    klb	String	是	卡类别 01:社保卡 02:借记卡 03:信用卡 
    gfbb	String	否	规范版本
    jgbm	String	否	机构编码
    fkrq	String	否	发卡日期yyyyMMdd
    kyxq	String	否	卡有效期
    kh	String	否	卡号
    sex	String	否	性别
    nation	String	否	民族
    csrq	String	否	出生日期

                         */

                        MakeJson mj2 = new MakeJson();
                        mj2.add("orgCode", aj["data"]["orgCode"], DataStyle.STR);
                        mj2.add("devSeq", aj["data"]["devSeq"], DataStyle.STR);
                        mj2.add("orgId", int.Parse(aj["data"]["orgId"].ToString()), DataStyle.INT);
                        mj2.add("atr", atr, DataStyle.STR);
                        mj2.add("yhkh", bankcarNum, DataStyle.STR);
                        mj2.add("slotno", stlo, DataStyle.INT);
                        mj2.add("boxno", 1, DataStyle.INT);
                        //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                        //0         1       2         3               4         5         6     7             8     9     10    11      12
                        mj2.add("ksbm", carDatas[0], DataStyle.STR);
                        mj2.add("shbzh", carDatas[7], DataStyle.STR);
                        mj2.add("kh", carDatas[6], DataStyle.STR);
                        mj2.add("sfzh", carDatas[7], DataStyle.STR);
                        mj2.add("xm", carDatas[8], DataStyle.STR);
                        mj2.add("klb", "01", DataStyle.STR);
                        mj2.add("gfbb", carDatas[2], DataStyle.STR);
                        mj2.add("jgbm", carDatas[3], DataStyle.STR);
                        mj2.add("fkrq", carDatas[4], DataStyle.STR);
                        mj2.add("kyxq", carDatas[5], DataStyle.STR);
                        mj2.add("sex", carDatas[9], DataStyle.STR);
                        mj2.add("nation", carDatas[10], DataStyle.STR);
                        mj2.add("csrq", carDatas[12], DataStyle.STR);
                        tools.AnalyzeJson aj2 = Network3.getJson(mj2, "uploadCPCardInfo");
                        if (aj2.error != null)
                        {
                            addFailed();
                            addCarError = "后台入库接口调用失败";
                            error = MS2.PutCardToIC(atr, stlo);
                            if (error == null)
                                error = MS2.PutCardToReject();
                            if(error != null)
                            {
                                ShowTip.show(false, null, "机器故障:" + error);
                                End();
                                return;
                            }
                            continue;
                        }
                        addSuccess();
                    }
                }
                End();
            }
            catch (Exception e)
            {
                Log("加卡异常：" + e.ToString());
                ShowError("加卡异常：" + e.ToString());
            }
        }

        private void Log(string log)
        {
            YTH.Functions.Log.AddLog("加卡", log);
        }
    }
}
