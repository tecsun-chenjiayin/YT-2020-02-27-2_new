using DevPrinter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using YTH.Controls;
using YTH.Functions.ReadCarAndSQCode;
using YTH.Network;

namespace YTH.Functions.MSDLL
{
    class MSNT
    {
        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevOpenDevice", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevOpenDevice(string strIP, int port, int baud);//打开

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevCloseDevice", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCloseDevice(int hDevice);//关闭

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevAddCardBatch", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevAddCardBatch(int hDevice, int feederno, ref int addno, short timeout);//批量装卡（设备装卡操作，把原料仓的卡片装载到料盘。）

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevClearCardBatch", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevClearCardBatch(int hDevice, ref int delno, short timeout);//批量清卡（设备装卡清卡，把卡盘的卡片清除到废料盒。）

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevTakeCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevTakeCard(int hDevice,string cardno,int slotno,int verify, short timeout);//指定卡号或者卡槽领取卡片

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevStop", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevStop(int hDevice, short timeout);//设备正在装卡，清卡等操作时，可以停止当前操作，该指令会完成当前卡片操作后停止，并非立即中止。

        /*
        对设备进行复位操作。
        包含基本的两个功能：
        1.	在连接设备完成以后，就要对设备进行复位，这时候的复位，设备会进行初始化，排除上次没有正常清理的卡。
        2.	在做卡流程中，如果卡片加工过程出现异常报错，设备会暂停，需要执行复位，否则设备会一直等待。
        */
        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevReset", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevReset(int hDevice, short timeout);//对设备进行复位操作。

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevRetainCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevRetainCard(int hDevice, short timeout);//将卡片从领卡口回收到废卡盒。

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevCheckCardOut", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCheckCardOut(int hDevice,ref int hasCard, short timeout);//检查出卡口是否有卡。

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevRecoveryCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevRecoveryCard(int hDevice, short timeout);//等待用户从插卡口插入卡片，接着将该卡片回收到废卡盒。

        /*
        序号	接口字段	接口字段说明	类型	    备注	                                                输入/输出
        1	    hDevice	    设备地址	    HANDLE		连接设备时返回的指针变量	                            输入
        2	    printSlip	凭条模块	    char*		yes:有纸 no:无纸 nearly:纸快用完 error:状态异常	        输出
        3	    CardHopper1	卡盒模块1	    char*		yes:有卡 no:无卡 nearly:卡快用完 error:状态异常	        输出
        4	    CardHopper2	卡盒模块2	    char*		yes:有卡 no:无卡 nearly:卡快用完 error:状态异常	        输出
        5	    CardHopper3	卡盒模块3	    char*		yes:有卡 no:无卡 nearly:卡快用完 error:状态异常	        输出
        6	    Printer	    平印模块	    char*		yes:有色带 no:无色带 nearly:快用完 error:状态异常	    输出
        7	    EMB	        凸字模块	    char*		yes:有色带 no:无色带 nearly:快用完 error:状态异常	    输出
        8	    REJECT	    废卡盒模块	    char*		yes:废卡满 no:没满 nearly:快满 error:错误	            输出
        9	    STORE	    卡库状态	    char*		yes:卡库满 no:没满 nearly:快满 error:错误	            输出
        10	    Machine	    设备状态	    char*		offline：不在线  ready：空闲 Busy：在运行 Error：异常	输出
        11	    timeout	    超时间隔	    DWORD		超时间隔，默认30s	                                    输入
        */
        //检查并且返回设备各个模组的状态。
        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevModuleStatus", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevModuleStatus(int hDevice,StringBuilder szStatus, int timeout);

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevputCardIn", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevputCardIn(int hDevice, int hopperNo, short timeout);//设备从指定的料仓发出一张卡片到设备内。

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevCardToReject", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCardToReject(int hDevice, short timeout);//将卡片送到废卡盒。

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevGetLastError", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevGetLastError(int hDevice, StringBuilder szErrorInfo, ref int iErrorCode);//将当执行指令时发生错误，通过指令可以获取最后一条指令的错误信息

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevUserInsertCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevUserInsertCard(int hDevice, int time, short timeout);//等待用户插入卡片。

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevCarPrint", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCarPrint(int hDevice, int cmd);//小车与平印卡片过渡

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevExportSendCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevExportSendCard(int hDevice, int timeout);//将卡片送到出卡口

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevStoreSendCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevStoreSendCard(int hDevice,string cid,int slotno,int timeout);//成品卡库出卡

        [DllImport("TSCardDriver.dll", EntryPoint = "iTranseDevCardToStore", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCardToStore(int hDevice, string cid, ref int slotNo, int timeout);
        ///////////写卡相关////////////////
        [DllImport("TSCardDriver.dll", EntryPoint = "iReadCardBas", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadCardBas(int iType, StringBuilder pOutInfo);//读基本信息。

        [DllImport("TSCardDriver.dll", EntryPoint = "iVerifyPINK", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iVerifyPINK(int iType, string pPIN, StringBuilder pOutInfo);//PIN入参校验。

        [DllImport("TSCardDriver.dll", EntryPoint = "iChangePINK", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iChangePINK(int iType,string pOldPIN, string pNewPIN, StringBuilder pOutInfo);//PIN入参修改

        [DllImport("TSCardDriver.dll", EntryPoint = "iRepairCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iRepairCard(int iType, string pFileAddr, string pFileInfo, StringBuilder pOutInfo);//卡修复。

        [DllImport("TSCardDriver.dll", EntryPoint = "iReadCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadCard(int iType, int iAuthType, string pPIN, string pFileAddr, StringBuilder pOutInfo);//小车与平印卡片过渡

        [DllImport("TSCardDriver.dll", EntryPoint = "ICC_Reader_Open", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int ICC_Reader_Open(string pDev_Name);//打开读写器

        [DllImport("TSCardDriver.dll", EntryPoint = "ICC_Reader_PowerOnEX", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int ICC_Reader_PowerOnEX(int ReaderHandle,byte ICC_Slot_No, StringBuilder pOutInfo);//读写器上电

        [DllImport("TSCardDriver.dll", EntryPoint = "iReadBankNum", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadBankNum(int iType, StringBuilder pOutInfo);//读写器上电

        //////////////////////////////////////////////////////////////////////////
        //读ATR
        static int icHandle = -1;
        public static string getART(out string error)
        {
           
            error = null;
            if (icHandle <= 0)
                icHandle = ICC_Reader_Open("AUTO");
            if(icHandle <= 0)
            {
                error = "打开读写器失败，获取ATR失败。";
                Log.AddLog(log, error);
                return null;
            }
            Log.AddLog(log, "读取ATR");
            StringBuilder outData = new StringBuilder(1024);
            int ret = ICC_Reader_PowerOnEX(icHandle, 0x01, outData);
            if(ret == 0)
            {
                Log.AddLog(log, "ATR:" + outData.ToString());
                return outData.ToString();
            }
            else
            {
                Log.AddLog(log, "读取ATR失败:" + outData.ToString());
                error = outData.ToString();
                return null;
            }
        }
        //////////////////////////////////////////////////////////////////////////
        const string log = "发卡机器硬件日志";
        static int handle = 0;
        static StringBuilder atr = new StringBuilder(1024);
        public static string open()
        {
            if (handle != 0) return null;
            handle = iTranseDevOpenDevice("127.0.0.1", 6200, 100);
            if (handle == 0)
            {
                Log.AddLog(log, "设备连接失败,入参：\"127.0.0.1\",6200");
                return "设备连接失败";
            }
            else
            {
                Log.AddLog(log, "设备连接成功,入参：\"127.0.0.1\",6200");
                return null;
            }   
        }

        public static string open2()
        {
            iTranseDevCloseDevice(handle);
            handle = iTranseDevOpenDevice("127.0.0.1", 6200, 100);
            if (handle == 0)
            {
                Log.AddLog(log, "设备连接失败,入参：\"127.0.0.1\",6200");
                return "设备连接失败";
            }
            else
            {
                Log.AddLog(log, "设备连接成功,入参：\"127.0.0.1\",6200");
                return null;
            }
        }

        public static void reset()
        {
            int ret = iTranseDevStop(handle, 30);
            int ret2 = iTranseDevReset(handle, 30);
            Zebra7 z7 = Zebra7.GetDev();
            z7.Reset();
        }

        //检查机器
        public static string checkMachine()
        {
            if (handle == 0) return "请连接设备";
            Log.AddLog(log, "开始检查设备状态");
            StringBuilder outData = new StringBuilder(1024);
            int ret = iTranseDevModuleStatus(handle, outData, 30);
            if (ret != 0)
            {
                Log.AddLog(log, "检查社保状态失败");
                return "检查社保状态失败";
            }
            Log.AddLog(log, "设备状态：" + outData.ToString());
            //凭条模块|卡盒模块1 |卡盒模块2|卡盒模块3|平印模块|凸字模块|废卡盒模块|卡库状态|设备状态
            //0       |1         |2        |3        |4       |5       |6         |7       |8
            string[] status = outData.ToString().Split('|');
            string error = "";
            if (status[0] == "no")
                error += "凭条打印模块缺纸|";
            if (status[6] == "yes")
                error += "废卡盒满，请联系管理员清理|";
            if (status[8] == "offine")
                error += "设备不在线，请先启动设备服务|";
            if (status[7] == "yes")
                error += "卡库满";
            return error == "" ? null : error;
        }
        //获取当前发生的错误（在某指令出现错误后调用）
        public static string getNowError()
        {
            try
            {
                string errorMsg2 = "";
                int code = 0;
                StringBuilder errorMsg = new StringBuilder(1024);
                int ret = iTranseDevGetLastError(handle, errorMsg, ref code);

                if (ret != 0)
                {
                    errorMsg2 = "获取指令执行错误信息失败:" + errorMsg.ToString();
                }
                else
                    errorMsg2 = errorMsg.ToString();
                Log.AddLog(log, "获取到的指令执行错误信息：" + errorMsg2);
                return errorMsg2;
            }
            catch(Exception e)
            {
                Log.AddLog(log, "获取指令错误信息失败：" + e.ToString());
                return "指令执行失败，获取失败原因失败";
            }
        }

        public static List<string> printdatas = new List<string>();

        static string pictureName = "";
        /// <summary>
        /// 制卡流程
        /// </summary>
        /// <param name="name"></param>
        /// <param name="persionid"></param>
        /// <param name="sseid">社保卡号</param>
        /// <param name="birth_year">出生日期-年</param>
        /// <param name="birth_bmonth">出生日期-月</param>
        /// <param name="pic">照片信息</param>
        /// <param name="bankno">银行编号</param>
        /// <returns></returns>
        public static string makeCar(string name, string persionid, string sseid,string pic, string date)
        {

            printdatas.Clear();
            //int index = pic.IndexOf("base64,");
            //if (index != -1)
            //{
            //    pic = pic.Substring(index + 7, pic.Length - index - 7);
            //}
            string[] hopperNos = new string[] { "1", "2", "3", "4", "5", "6" };
            int ret = 0;
            Log.AddLog(log, "停止+复位");
            Loading.updateTip("正在重置硬件状态...");
            reset();
            //1.料盒发卡
            Log.AddLog(log, "准备从料盒发卡");
            Loading.updateTip("准备从料盒发卡");
            for (int i = 0; i < hopperNos.Length; i++)
            {
                Log.AddLog(log, "尝试从料盒" + hopperNos[i] + "发卡");
                ret = iTranseDevputCardIn(handle, int.Parse(hopperNos[i]), 60);
                Log.AddLog(log, "尝试结果：" + ret);
                if (ret == 0)
                    break;
            }
            if (ret != 0)
                return "准备从料盒发卡失败：" + getNowError();

            //2.准备平印
            Loading.updateTip("准备平印");
            Log.AddLog(log, "准备平印");
            ret = iTranseDevCarPrint(handle, 1);
            Log.AddLog(log, "ret:" + ret);
            if (ret != 0)
                return "准备平印失败：" + getNowError();

            //3.设置打印信息
            string year = System.DateTime.Now.Year.ToString("D4");
            string month = System.DateTime.Now.Month.ToString("D2");
            Loading.updateTip("设置打印信息");
            Log.AddLog(log, "设置打印信息");
            Log.AddLog(log, "入参：");
            Log.AddLog(log, "姓名：" + name);
            Log.AddLog(log, "社会保障号码：" + persionid);
            Log.AddLog(log, "社会保障卡号：" + sseid);
            Log.AddLog(log, "发卡日期：" + date);
            Log.AddLog(log, "照片：" + pic);

            printdatas.Clear();
            printdatas.Add("============================");
            printdatas.Add("****************************");
            printdatas.Add("制卡凭条");
            printdatas.Add("姓名：" + name);
            printdatas.Add("社会保障号码：" + persionid);
            printdatas.Add("社会保障卡号：" + sseid);
            printdatas.Add("发卡日期(非卡面)：" + year + month);
            printdatas.Add("****************************");
            printdatas.Add("============================");

            //Log.AddLog(log, "重新获取句柄");
            //string error = open2();
            //if(error != null)
            //{
            //    return error;
            //}

            if (Config.dic("IgnorePrintCar") == "1") return null;//System.DateTime.Now.ToString("yyyy年MM月dd日")
            string[] info = { "姓名 " + name, "社会保障号码 " + persionid, "社会保障卡号 " + sseid, "发卡日期 " + date, "" };
            Zebra7 z7 = Zebra7.GetDev();
            z7.PrintFomat(info, pic);

            //将卡片送到平印位置
            Log.AddLog(log, "ret:将卡片送到平印位置");
            ret = iTranseDevCarPrint(handle, 1);
            if (0 != ret)
            {
                Log.AddLog(log, "移到打印失败" + ret);
                return "移到打印失败" + ret;
            }

            //将卡片从小车送到平印
            Log.AddLog(log, "将卡片从小车送到平印" );
            ThreadPool.QueueUserWorkItem(PrinterFeed);//打印机和小车同时运行
            Thread.Sleep(3000);
            ret = iTranseDevCarPrint(handle, 2);
            if (0 != ret)
            {
                Log.AddLog(log, "移到打印失败" + ret);
                return "移到打印失败" + ret;
            }
            if (z7.Print() == 0)
            {
                ThreadPool.QueueUserWorkItem(PrinterReject);//打印机和小车同时运行
                ret = iTranseDevCarPrint(handle, 3);
                if (0 != ret)
                {
                    return "移出打印失败" + ret;
                }
            }
            else
            {
                Log.AddLog(log, "打印失败："+ z7.Msg);
                ThreadPool.QueueUserWorkItem(PrinterReject);//打印机和小车同时运行
                ret = iTranseDevCarPrint(handle, 3);
                return "打印失败：" + z7.Msg;
            }
    
            Loading.updateTip("打印流程执行完毕");
            Log.AddLog(log, "打印流程执行完毕");


            return null;
        }

        //料盒出卡
        public static string iTranseDevputCardIn(int index)
        {
            int ret = 0;
            for (int i = 0; i < 3; i++)
            {
                ret = iTranseDevputCardIn(handle, index, 60);
                if (ret == 0)
                    return null;
            }
            return "尝试从料盒出卡失败：" + ret;
        }

        //卡库出卡
        public static string putCarToIC(string tag, int kc)
        {
            string error = open();
            if(error == null)
            {
                Log.AddLog(log, "卡库出卡：Tag:" + tag + "    kc:" + kc);
                int ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
                if (ret != 0)
                    return getNowError();
                else
                    return null;
            }
            return error;
        }

        private static void PrinterFeed(object obj)
        {
            Zebra7 z7 = Zebra7.GetDev();
            if (z7.PositionCard() != 0)
            {
                z7.Reject();
            }
        }

        private static void PrinterReject(object obj)
        {
            Zebra7 z7 = Zebra7.GetDev();
            z7.Reject();
        }

        public static string test1()
        {
            int ret = 0;
            //1.料盒发卡
            string[] hopperNos = new string[] { "1", "2", "3", "4", "5", "6" };
            Log.AddLog(log, "准备从料盒发卡");
            Loading.updateTip("准备从料盒发卡");
            for (int i = 0; i < hopperNos.Length; i++)
            {
                Log.AddLog(log, "尝试从料盒" + hopperNos[i] + "发卡");
                ret = iTranseDevputCardIn(handle, int.Parse(hopperNos[i]), 30);
                Log.AddLog(log, "尝试结果：" + ret);
                if (ret == 0)
                    break;
            }
            if (ret != 0)
                return "准备从料盒发卡失败：" + getNowError();
            else
                return null;
        }

        public static string toWriteCar(string password,string SSSEEF05, string SSSEEF06, Dictionary<string,string> inData2, string picName)
        {
            string error0 = "";
            getART(out error0);
            //读卡
            Loading.updateTip("正在读卡信息");
            Log.AddLog(log, "正在读卡信息");
            StringBuilder pOut = new StringBuilder(1024);
            int ret50 = iReadCardBas(1, pOut);
            if (ret50 != 0)
            {
                ret50 = iReadCardBas(1, pOut);
                if (ret50 != 0)
                {
                    ret50 = iReadCardBas(1, pOut);
                    if (ret50 != 0)
                    {
                        Log.AddLog(log, "读卡信息失败：" + pOut.ToString());
                        return "读卡信息失败：" + pOut.ToString();
                    }
                }
            }
            Log.AddLog(log, "卡信息：" + pOut);
            CD.makeCarDic["deviceID"] = NetData.deviceID;
            //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
            ////0         1       2         3               4         5         6     7             8     9     10    11      12
            string[] array0 = pOut.ToString().Split('|');


            pictureName = picName;// ReadIDCar.persionid + "_" + System.DateTime.Now.ToString("YYYYMMddHHmmss") + "_replacecard";
            //string[] hopperNos = new string[] { "1", "2", "3", "4", "5", "6" };
            //int ret = 0;
            //Log.AddLog(log, "停止+复位");
            //Loading.updateTip("正在重置硬件状态...");
            //reset();
            ////1.料盒发卡
            //Log.AddLog(log, "准备从料盒发卡");
            //Loading.updateTip("准备从料盒发卡");
            //for (int i = 0; i < hopperNos.Length; i++)
            //{
            //    Log.AddLog(log, "尝试从料盒" + hopperNos[i] + "发卡");
            //    ret = iTranseDevputCardIn(handle, int.Parse(hopperNos[i]), 30);
            //    Log.AddLog(log, "尝试结果：" + ret);
            //    if (ret == 0)
            //        break;
            //}
            //if (ret != 0)
            //    return "准备从料盒发卡失败：" + getNowError();
            ///////////////////////////////////////////
            Log.AddLog(log, "组合数据：");
            StringBuilder data1 = new StringBuilder(1024);
            StringBuilder data2 = new StringBuilder(1024);
            string[] ssse_ef05 = SSSEEF05.Split(',');
            string[] ssse_ef06 = SSSEEF06.Split(',');
            string[] ssse_ef05Names = new string[] { "01", "02", "03", "04", "05", "06", "07" };
            string[] ssse_ef06Names = new string[] { "08", "09", "4E", "0A", "0B", "0C", "0D" };
            int index = 0;
            bool hasAddName = false;
            foreach (string str in ssse_ef05)
            {
                if (str != "")
                {
                    if(hasAddName == false)
                    {
                        hasAddName = true;
                        data1.Append("SSSEEF05|");
                        data2.Append("SSSEEF05|");
                    }
                    data1.Append(ssse_ef05Names[index] + "|");
                    data2.Append(str + "|");
                }
                index++;
            }
            if(hasAddName)
            {
                data1.Append("$");
                data2.Append("$");
            }
            index = 0;
            hasAddName = false;
            foreach (string str in ssse_ef06)
            {
                if (str != "")
                {
                    if (hasAddName == false)
                    {
                        hasAddName = true;
                        data1.Append("SSSEEF06|");
                        data2.Append("SSSEEF06|");
                    }
                    data1.Append(ssse_ef06Names[index] + "|");
                    data2.Append(str + "|");
                }
                index++;
            }
            if (hasAddName)
            {
                data1.Append("$");
                data2.Append("$");
            }
            Log.AddLog(log, "参数2：" + data1.ToString());
            Log.AddLog(log, "参数3：" + data2.ToString());
            ///////////////////////////////////////////
            Loading.updateTip("正在写卡");
            Log.AddLog(log, "正在写卡");
            StringBuilder pOutInfo = new StringBuilder(1024);
            string data1_ = data1.ToString();
            string data2_ = data2.ToString();
            System.DateTime beforDT = System.DateTime.Now;
            int ret1 = iRepairCard(1, data1_, data2_, pOutInfo);
            if (ret1 != 0)
            {
                Log.AddLog(log, "第一次失败，即将尝试第二次，错误信息：" + pOutInfo.ToString());
                ret1 = iRepairCard(1, data1_, data2_, pOutInfo);
                if (ret1 != 0)
                {
                    Log.AddLog(log, "第二次失败，即将尝试第三次，错误信息：" + pOutInfo.ToString());
                    ret1 = iRepairCard(1, data1_, data2_, pOutInfo);
                    if (ret1 != 0)
                    {
                        Log.AddLog(log, "尝试3次写入内容失败，错误信息：" + pOutInfo.ToString());
                        return "尝试3次写入内容失败" + pOutInfo.ToString();
                    }
                }
            }
            System.DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Log.AddLog(log, "写入花费时间：" + ts.TotalSeconds + "秒");
            Console.WriteLine("DateTime总共花费{0}ms.", ts.TotalMilliseconds);
            ///////////////////////////////////////////
            Loading.updateTip("正在校验写入结果");
            Log.AddLog(log, "正在校验写入结果");
            StringBuilder readData = new StringBuilder(1024);
            beforDT = System.DateTime.Now;
            int ret2 = iReadCard(1, 0, null, data1_, readData);
            afterDT = System.DateTime.Now;
            ts = afterDT.Subtract(beforDT);
            Log.AddLog(log, "校验花费时间：" + ts.TotalSeconds + "秒");
            if (ret2 != 0)
                return "校验写入结果失败：" + readData;
            Log.AddLog(log, "读出结果：" + readData);
            if (data2_ != readData.ToString())
                return "校验失败：读出内容与写入内容不一致";
            else
            {
                //修改密码
                if(password != null && password != "")
                {
                    Loading.updateTip("正在设置密码");
                    Log.AddLog(log, "正在设置密码");
                    pOutInfo.Clear();
                    int ret4 = iChangePINK(1, "123456", password, pOutInfo);
                    if (ret4 != 0)
                    {
                        Log.AddLog(log, "修改密码失败：" + pOutInfo.ToString());
                        return "修改密码失败：" + pOutInfo.ToString();
                    }
                    Log.AddLog(log, "修改密码完成");
                }
                //读卡
                Loading.updateTip("正在读卡信息");
                Log.AddLog(log, "正在读卡信息");
                pOutInfo.Clear();
                int ret5 = iReadCardBas(1, pOutInfo);
                if (ret5 != 0)
                {
                    Log.AddLog(log, "读卡信息失败：" + pOutInfo.ToString());
                    return "读卡信息失败：" + pOutInfo.ToString();
                }
                Log.AddLog(log, "卡信息：" + pOutInfo);
                CD.makeCarDic["deviceID"] = NetData.deviceID;
                //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                ////0         1       2         3               4         5         6     7             8     9     10    11      12
                string[] array = pOutInfo.ToString().Split('|');
                //CD.makeCarDic["idcard"] = array[7];
                //CD.makeCarDic["name"] = array[8];
                //CD.makeCarDic["ksbm"] = array[0];
                //CD.makeCarDic["time"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //CD.makeCarDic["cardtype"] = "0";
                //CD.makeCarDic["signature"] = "";// ImgToBase64String(CD.getBasePath() + "Autograph.bmp");
                //读ATR
                string outError = null;
                string atr = getART(out outError);
                if(outError != null)
                {
                    Loading.updateTip("读读ATR失败：" + outError);
                    return outError;
                }
                //读银行卡号
                StringBuilder outError2 = new StringBuilder(1024);
                int re = iReadBankNum(1, outError2);
                if(re != 0)
                {
                    Loading.updateTip("读取银行卡号失败：" + outError2.ToString());
                }

                //回盘 
                inData2["replacecardNo"] = array0[6] ;
                inData2["atr"] = atr.Substring(8, atr.Length - 8) ;
                inData2["ksbm"] = array[0];
                inData2["failType"] = "";//失败环节
                inData2["failReason"] = "";//失败原因(50字节内)
                inData2["photoUrl"] = pictureName;//照片路径
                inData2["bankAccount"] = outError2.ToString();
                inData2["cardid"] = array[6];
                Loading.updateTip("正在回盘");
                Log.AddLog(log, "正在回盘");
                MJson hp = Network2.Post(inData2, "replyCardInfo");
                if (hp.error != null)
                    return "回盘失败:" + hp.error;
                else if (hp["statusCode"].ToString() != "200")
                    return "回盘失败：" + hp["message"].ToString();




                //string moreError = "";
                //Loading.updateTip("正在回盘");
                //Log.AddLog(log, "正在回盘");
                //Dictionary<string, string> dic = new Dictionary<string, string>();
                //dic["deviceID"] = CD.makeCarDic["deviceID"];
                //dic["idcard"] = CD.makeCarDic["idcard"];
                //dic["name"] = CD.makeCarDic["name"];
                //dic["atr"] = CD.makeCarDic["atr"];
                //dic["ksbm"] = CD.makeCarDic["ksbm"];
                //dic["batchno"] = CD.makeCarDic["batchno"];
                //MJson mj2 = NetData.dataReport(dic);
                //if(mj2.error != null && mj2.error != "")
                //{
                //    Loading.updateTip("回盘失败");
                //    Log.AddLog(log, "回盘失败");
                //    CD.makeCarDic["status"] = "2";
                //    CD.makeCarDic["discrip"] = mj2.error;
                //    moreError = "回盘失败:" + mj2.error;
                //}
                //else
                //{
                //    CD.makeCarDic["status"] = "1";
                //    CD.makeCarDic["discrip"] = "回盘成功";
                //}
                ////上传
                //Loading.updateTip("正在上传数据");
                //Log.AddLog(log, "正在上传数据");
                //MJson mj3 = NetData.updateRecord(CD.makeCarDic);
                //if (mj3.error != null && mj3.error != "")
                //{
                //    Loading.updateTip("上传数据失败");
                //    Log.AddLog(log, "上传数据失败");
                //    moreError += "|上传数据失败:" + mj3.error;
                //}
                //if (moreError != "")
                //    return moreError;




                int ret3 = iTranseDevExportSendCard(handle, 30);
                if (ret3 != 0)
                    return "写卡成功，但是出卡失败！";
            }
            return null;
        }

        public static string testData(string perison, string name, string ksbm,string atr)
        {
            string error = open();
            if (error == null)
            {
                Log.AddLog(log, "正在读卡数据");
                StringBuilder pOutInfo = new StringBuilder(1024);
                int ret5 = iReadCardBas(1, pOutInfo);
                if(ret5 != 0)
                {
                    ret5 = iReadCardBas(1, pOutInfo);
                    if (ret5 != 0)
                    {
                        ret5 = iReadCardBas(1, pOutInfo);
                    }
                }

                if(ret5 == 0)
                {
                    Log.AddLog(log, "数据校验:" + pOutInfo.ToString() + "  -----   " + perison + " - " + name + " - " + ksbm);
                    //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                    //0         1       2         3               4         5         6     7             8     9     10    11      12
                    string[] array = pOutInfo.ToString().Split('|');
                    if(array[0] != ksbm || array[7] != perison || array[8] != name)
                    {
                        Log.AddLog(log, "数据校验失败");
                        return "数据校验失败";
                    }
                    else
                    {
                        string outError = null;
                        string atr2 = getART(out outError);
                        if (outError != null)
                            return outError;
                        else if (atr2 != atr)
                            return "ATR校验失败";
                        Log.AddLog(log, "数据校验成功");
                        return null;
                    }
                }
                else
                {
                    Log.AddLog(log, "读卡信息失败：" + pOutInfo.ToString());
                    return "读卡信息失败：" + pOutInfo.ToString();
                }
            }
            else
            {
                Log.AddLog(log, "端口打开失败:" + error);
                return error;
            }
        }

        public static string moveCarToReject()
        {
            int ret = iTranseDevCardToReject(handle, 30);
            if (ret == 0)
                return null;
            else
                return getNowError();
        }
        public static void moveCarToReject2()
        {
            int ret = iTranseDevCardToReject(handle, 30);
        }

        protected static string ImgToBase64String(string ImagePath)
        {
            try
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ImagePath);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                Log.AddLog(log, "图片转Base64失败");
                return "图片转Base64失败";
            }
        }

        public static bool hasCarIn()
        {
            int ret = 0;
            int ret2 = iTranseDevCheckCardOut(handle, ref ret, 30);
            if (ret2 == 0)
                return ret == 1;
            else
                return false;
        }

        public static string setPsw(string psw)
        {
            //修改密码
            Log.AddLog(log, "正在设置密码");
            StringBuilder pOutInfo = new StringBuilder();
            int ret4 = iChangePINK(1, "123456", psw, pOutInfo);
            if (ret4 != 0)
            {
                Log.AddLog(log, "修改密码失败：" + pOutInfo.ToString());
                return "修改密码失败：" + pOutInfo.ToString();
            }
            Log.AddLog(log, "修改密码完成");
            return null;
        }

        public static int putCarIn()
        {
            return iTranseDevUserInsertCard(handle, 30, 30);
        }

        public static void putCarOut()
        {
            int ret3 = iTranseDevExportSendCard(handle, 30);
            if (ret3 != 0)
            {
                Log.AddLog(log, "出卡失败");
                TipWinB1.showTip("出卡失败,请联系管理员处理！", 10000, null);
            }
               
        }

        public static string readSSCar(ref string persionid, ref string name)
        {
            StringBuilder pOutInfo = new StringBuilder(1024);
            int ret5 = iReadCardBas(1, pOutInfo);
            if (ret5 != 0)
            {
                ret5 = iReadCardBas(1, pOutInfo);
                if (ret5 != 0)
                {
                    ret5 = iReadCardBas(1, pOutInfo);
                    if (ret5 != 0)
                    {
                        Log.AddLog(log, "读卡信息失败：" + pOutInfo.ToString());
                        return "读卡信息失败：" + pOutInfo.ToString();
                    }
                }
            }

            else
            {
                //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                //0         1       2         3               4         5         6     7             8     9     10    11      12
                string[] array = pOutInfo.ToString().Split('|');
                persionid = array[7];
                name = array[8];
            }
            return null;
        }

        public static string iTranseDevCardToStore(string tag, ref int slotNo)
        {
            int ret = iTranseDevCardToStore(handle, tag, ref slotNo, 30);
            if (ret == 0)
                return null;
            else
                return getNowError();
        }
    }
}
