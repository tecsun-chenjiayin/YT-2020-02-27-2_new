using DevPrinter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using YTH.NHNet;
using System.Threading;
using System.Threading.Tasks;

namespace YTH.Functions.MSDLL
{
    class MS2
    {

        #region MSTerm.dll
        //添加检查卡是否存在
        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_FeederStatus", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_FeederStatus(int hDevice,
                                                     StringBuilder cardHoper1,
                                                     StringBuilder cardHoper2,
                                                     StringBuilder cardHoper3,
                                                     StringBuilder cardHoper4,
                                                     StringBuilder cardHoper5,
                                                     StringBuilder cardHoper6);
        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_ModuleStatus", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_ModuleStatus(IntPtr hDevice, StringBuilder printSlip, StringBuilder CardHopper1, StringBuilder CardHopper2, StringBuilder CardHopper3, StringBuilder Printer, StringBuilder EMB, StringBuilder REJECT, StringBuilder STORE, StringBuilder Machine, int timeout);
        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_OpenDevice", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr MsTerm_OpenDevice(string strIP, int port);

        //moduleID:模块ID定义，卡库ID:1 
        //outInfo:json格式,描述模块状态
        //例如卡库: {"LEFT":"123","SIZE":"300"}
        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_ModuleInfo", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_ModuleInfo(IntPtr hDevice, int moduleID, StringBuilder outInfo, int infolen, int timeout);//获取模块状态


        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_putCardIn", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_putCardIn(IntPtr hDevice, int hopperNo, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_bulgeCardFace", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_bulgeCardFace(IntPtr hDevice, string strCardData, int iGilding, int timeout);


        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_CloseDevice", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_CloseDevice(IntPtr hDevice);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_AddCardBatch", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_AddCardBatch(IntPtr hDevice, int feederno, out int addno, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_ClearCardBatch", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_ClearCardBatch(IntPtr hDevice, out int delno, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_TakeCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_TakeCard(IntPtr hDevice, string cardno, int slotno, int verify, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_Stop", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_Stop(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_Reset", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_Reset(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_RetainCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_RetainCard(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_CheckCardOut", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_CheckCardOut(IntPtr hDevice, out int hasCard, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_RecoveryCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_RecoveryCard(IntPtr hDevice, int timeout);

  
        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_powerOnIMC", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_powerOnIMC(IntPtr hDevice, StringBuilder resp, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_powerOffIMC", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_powerOffIMC(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_APDU", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_APDU(IntPtr hDevice, string apdu, out string resp, int timeout);

      
        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_bulgeCardFaceM", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_bulgeCardFaceM(IntPtr hDevice, string setupName, string field, string data, int iGilding, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_printCardFace", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_printCardFace(IntPtr hDevice, string strCardData, string barData, string strImgData, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_printCardFaceM", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_printCardFaceM(IntPtr hDevice, string setupName, string field, string data, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_ExportSendCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_ExportSendCard(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_CardToStore", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_CardToStore(IntPtr hDevice, string cid, ref int slotNo, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_readMagnet2Card", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_readMagnet2Card(IntPtr hDevice, StringBuilder track1, StringBuilder track2, StringBuilder track3, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_writeMagnet2Card", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_writeMagnet2Card(IntPtr hDevice, string track1, string track2, string track3, int mode, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_CardToReject", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_CardToReject(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_StoreSendCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_StoreSendCard(IntPtr hDevice, string cid, int slotno, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_GetLastErrMsg", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr MsTerm_GetLastErrMsg(IntPtr hDevice);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_GetLastErrCode", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_GetLastErrCode(IntPtr hDevice);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_UserInsertCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_UserInsertCard(IntPtr hDevice, int time, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_SetCardType", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_SetCardType(IntPtr hDevice, string cardType);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_CAR_PRINTER", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_CAR_PRINTER(IntPtr hDevice, int cmd);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_MagToCar", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_MagToCar(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_CardToMag", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_CardToMag(IntPtr hDevice, int timeout);

        [DllImport("MSTerm.dll", EntryPoint = "MsTerm_Ocr", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MsTerm_Ocr(IntPtr hDevice, int ocrID, StringBuilder outData, ref int outDataLen, int timeout);



        #endregion
        #region dll
        //const string dll = @"C:\Windows\System32\TSCardDriver.dll";
        const string dll = @"TSCardDriver.dll";
        [DllImport(dll, EntryPoint = "iTranseDevOpenDevice", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevOpenDevice(string strIP, int port, int baud);//打开

        [DllImport(dll, EntryPoint = "iTranseDevStop", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevStop(int hDevice, short timeout);//设备正在装卡，清卡等操作时，可以停止当前操作，该指令会完成当前卡片操作后停止，并非立即中止。

        [DllImport(dll, EntryPoint = "iTranseDevReset", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevReset(int hDevice, short timeout);//对设备进行复位操作。

        [DllImport(dll, EntryPoint = "iTranseDevModuleStatus", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevModuleStatus(int hDevice, StringBuilder szStatus, int timeout);

        [DllImport(dll, EntryPoint = "iTranseDevGetLastError", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevGetLastError(int hDevice, StringBuilder szErrorInfo, ref int iErrorCode);//将当执行指令时发生错误，通过指令可以获取最后一条指令的错误信息

        [DllImport(dll, EntryPoint = "iTranseDevCheckCardOut", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCheckCardOut(int hDevice, ref int hasCard, short timeout);//检查出卡口是否有卡。

        [DllImport(dll, EntryPoint = "iTranseDevCardToReject", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCardToReject(int hDevice, short timeout);//将卡片送到废卡盒。

        [DllImport(dll, EntryPoint = "iTranseDevExportSendCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevExportSendCard(int hDevice, int timeout);//将卡片送到出卡口

        [DllImport(dll, EntryPoint = "iReadCardBas", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadCardBas(int iType, StringBuilder pOutInfo);//读基本信息。

        [DllImport(dll, EntryPoint = "ICC_Reader_Open", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int ICC_Reader_Open(string pDev_Name);//打开读写器

        [DllImport(dll, EntryPoint = "ICC_Reader_PowerOnEX", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int ICC_Reader_PowerOnEX(int ReaderHandle, byte ICC_Slot_No, StringBuilder pOutInfo);//读写器上电

        [DllImport(dll, EntryPoint = "ICC_Reader_PowerOff", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int ICC_Reader_PowerOff(int handle, byte icc_slot_no);

      [DllImport(dll, EntryPoint = "iTranseDevStoreSendCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevStoreSendCard(int hDevice, string cid, int slotno, int timeout);//成品卡库出卡

        [DllImport(dll, EntryPoint = "iTranseDevCardToMag", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCardToMag(int hDevice, int timeout);//卡片从小车到写磁器

        [DllImport(dll, EntryPoint = "iTranseDevPowerOnIMC", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevPowerOnIMC(int hDevice, StringBuilder outATR, int timeout);//IC上电

        [DllImport(dll, EntryPoint = "iChangePINK", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iChangePINK(int iType, string pOldPIN, string pNewPIN, StringBuilder pOutInfo);//PIN入参修改

        [DllImport(dll, EntryPoint = "iTranseDevputCardIn", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevputCardIn(int hDevice, int hopperNo, short timeout);//设备从指定的料仓发出一张卡片到设备内。

        [DllImport(dll, EntryPoint = "iTranseDevCardToStore", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevCardToStore(int hDevice, string cid, ref int slotNo, int timeout);

        [DllImport(dll, EntryPoint = "iTranseDevbulgeCardFaceM", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevbulgeCardFaceM(int hDevice, string setupName, string field, string data, int iGilding, int timeout);

        [DllImport(dll, EntryPoint = "iReadBankNum", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadBankNum(int iType, StringBuilder pOutInfo);

        [DllImport(dll, EntryPoint = "iTranseDevRecoveryCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevRecoveryCard(int hDevice, int timeout);//吸回半出状态的卡

        [DllImport(dll, EntryPoint = "iTranseDevUserInsertCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iTranseDevUserInsertCard(int hDevice, int time, int timeout);//用户插卡

        //身份证打开关闭
        [DllImport(dll, EntryPoint = "iOpenIDReader", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenIDReader(int iPort);//返回值	0表示成功；非0表示失败。

        [DllImport(dll, EntryPoint = "iCloseIDReader", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseIDReader();
        //读卡器打开关闭
        [DllImport("TSFaceDetect.dll", EntryPoint = "ICC_Reader_Open", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int ICC_Reader_Open(int iPort);//设备端口，取值范围“AUTO”“COMn”“USBn”，其中“n”的取值范围为1-9。

        [DllImport(dll, EntryPoint = "ICC_Reader_Close", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int ICC_Reader_Close(int handle);
        //摄像头
        [DllImport("TSFaceDetect.dll", EntryPoint = "iCloseCam1Test", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseCam1Test();

        [DllImport("TSFaceDetect.dll", EntryPoint = "iCloseCam2Test", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseCam2Test();

        [DllImport("TSFaceDetect.dll", EntryPoint = "iOpenCam1Test", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenCam1Test();

        [DllImport("TSFaceDetect.dll", EntryPoint = "iOpenCam2Test", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenCam2Test();
        //扫描仪
        [DllImport(dll, EntryPoint = "iOpenScanner", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenScanner(int iPort, int iBaud, StringBuilder errorMsg);

        [DllImport(dll, EntryPoint = "iCloseScanner", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseScanner(int handle, StringBuilder errorMsg);


    

        //================[TSSelfSendDev.dll]=========上下两个读卡器模式===读卡相关
        const string dll2 = "TSSelfSendDev.dll";

        //卡识别码|卡类别|规范版本|初始化机构编号|发卡日期|卡有效期|卡号|社会保障号码|姓名|性别|民族|出生地|出生日期
        //0       |1     |2       |3             |4       |5       |6   |7           |8   |9   |10  |11    |12
        [DllImport(dll2, EntryPoint = "iReadCardBasExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadCardBasExt(int iReaderCtrl, int iType, StringBuilder pOutInfo);//iReadCardBasExt “读基本信息”

        [DllImport(dll2, EntryPoint = "iVerifyPINKExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iVerifyPINKExt(int iReaderCtrl, int iType, string pPIN, StringBuilder pOutInfo);//iVerifyPINKExt“PIN入参校验”

        [DllImport(dll2, EntryPoint = "iChangePINKExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iChangePINKExt(int iReaderCtrl, int iType, string pOldPIN, string pNewPIN, StringBuilder pOutInfo);// iChangePINKExt“PIN入参修改”

        [DllImport(dll2, EntryPoint = "iReadCardExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadCardExt(int iReaderCtrl, int iType, int iAuthType, string pPIN, string pFileAddr, string pUserInfo, StringBuilder pOutInfo);//iReadCardExt “通用读卡”

        [DllImport(dll2, EntryPoint = "iReadBankNumExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadBankNumExt(int iReaderCtrl, int iType, StringBuilder pOutInfo);//iReadBankNumExt “读银行卡号”

        [DllImport(dll2, EntryPoint = "iGetATRExt", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iGetATRExt(int iReaderCtrl, int iType, StringBuilder pOutInfo);//iGetATRExt “读上电复位信息”

        #endregion


        //////////////////////////////////////////////////////////////////////////
        public static string log = "制卡日志";
        public static int handle = 0;
        static StringBuilder atr = new StringBuilder(1024);
        static int keepCardNum = 0;//机器上停留的卡片数量
        static string atr_test = "";



        #region 设备检查
        //检查IC读卡器
        public static string checkIC()
        {
            //int handle = ICC_Reader_Open(int.Parse(Config.dic("DKQ")));
            int Handle = ICC_Reader_Open("AUTO");
            Log.AddLog(log, "IC读卡器" + handle);
            if (handle < 0)
                return "读卡器异常";
            return null;
        }
        //检查身份证读卡器
        public static string checkSFZ()
        {
            int handle = iOpenIDReader(int.Parse(Config.dic("SFZ")));
            Log.AddLog(log, "身份证读卡器" + handle);
            if (handle < 0)
                return "身份证读卡器异常";
            iCloseIDReader();
            return null;
        }
        //检查摄像头
        public static string checkSam()
        {
            int handle1 = iOpenCam1Test();
            int handle2 = iOpenCam2Test();
            Log.AddLog(log, "摄像头1：" + handle1);
            Log.AddLog(log, "摄像头2：" + handle2);
            if (handle1 >= 0)
                iCloseCam1Test();
            if (handle2 >= 0)
                iCloseCam2Test();
            if (handle1 < 0 || handle2 < 0)
                return "摄像头异常";
            return null;
        }
        //检查扫描仪
        public static string checkScanner()
        {
            StringBuilder pOutInfo = new StringBuilder(1024);
            int port = int.Parse(Config.dic("Scanner"));
            int buad = int.Parse(Config.dic("ScannerBaud"));
            int handle = iOpenScanner(port, buad, pOutInfo);
            Log.AddLog(log, "扫描仪：" + handle);
            Log.AddLog(log, "pOutInfo：" + pOutInfo);
            if (handle < 0)
                return "扫描仪异常：" + pOutInfo.ToString();
            iCloseScanner(handle, pOutInfo);
            return null;
        }
        //检查机器
        public static string checkMachine()
        {
            Open();
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
            //if (status[0] == "no")
            //    error += "凭条打印模块缺纸|";
            if (status[6] == "yes")
                error += "废卡盒满，请联系管理员清理|";
            if (status[8] == "offine")
                error += "设备不在线，请先启动设备服务|";
            if (status[7] == "yes")
                error += "卡库满";
            return error == "" ? null : error;
        }
        //料盒卡剩余量检查 return 1：有卡 0：无卡 -1：异常 -2：入参错误 -3：接口调用异常
        public static int getLetfCardNum(int Box, out string error)
        {
            error = null;
            int ret = -999;
            try
            {
                StringBuilder cardHoper1 = new StringBuilder(1024);
                StringBuilder cardHoper2 = new StringBuilder(1024);
                StringBuilder cardHoper3 = new StringBuilder(1024);
                StringBuilder cardHoper4 = new StringBuilder(1024);
                StringBuilder cardHoper5 = new StringBuilder(1024);
                StringBuilder cardHoper6 = new StringBuilder(1024);
                var hDevice = MsTerm_OpenDevice("127.0.0.1", 6200);  //打开设备
                

                StringBuilder sprintSlip = new StringBuilder(1024);
                StringBuilder sPrinter = new StringBuilder(1024);
                StringBuilder sEMB = new StringBuilder(1024);
                StringBuilder sREJECT = new StringBuilder(1024);
                StringBuilder sSTORE = new StringBuilder(1024);
                StringBuilder sMachine = new StringBuilder(1024);
                if(Config.dic("BoxNum") == "6")
                    ret = MsTerm_FeederStatus(handle, cardHoper1, cardHoper2, cardHoper3, cardHoper4, cardHoper5, cardHoper6);
                else
                    ret = MsTerm_ModuleStatus(hDevice, sprintSlip,
                                                          cardHoper1,
                                                          cardHoper2,
                                                          cardHoper3,
                                                          sPrinter,
                                                          sEMB,
                                                          sREJECT,
                                                          sSTORE,
                                                          sMachine, -1);
                if (ret == 0)
                {
                    switch (Box)
                    {
                        case 1:
                            return getRetStatus(cardHoper1);
                        case 2:
                            return getRetStatus(cardHoper2);
                        case 3:
                            return getRetStatus(cardHoper3);
                        case 4:
                            return getRetStatus(cardHoper4);
                        case 5:
                            return getRetStatus(cardHoper5);
                        case 6:
                            return getRetStatus(cardHoper6);
                        default:
                            return -2;
                    }
                }
                else
                {
                    error = "料盒卡剩余量检查失败：ret：" + ret;
                    return -3;
                }
            }
            catch (Exception e)
            {
                error = "检查卡剩余量异常：" + e.ToString();
            }
            finally
            {
                Log.AddLog(log, "getLetfCardNum:error:" + (error == null ? "" : error));
            }
            return -3;
        }
        //料盒卡剩余量检查 return int[] 下标0代表卡盒1,1->2,类推   null:检查失败
        public static int[] getLetfCardNum(out string error)
        {
            error = null;
            int ret = -999;
            try
            {
                StringBuilder cardHoper1 = new StringBuilder(1024);
                StringBuilder cardHoper2 = new StringBuilder(1024);
                StringBuilder cardHoper3 = new StringBuilder(1024);
                StringBuilder cardHoper4 = new StringBuilder(1024);
                StringBuilder cardHoper5 = new StringBuilder(1024);
                StringBuilder cardHoper6 = new StringBuilder(1024);
                StringBuilder sprintSlip = new StringBuilder(1024);
                StringBuilder sPrinter = new StringBuilder(1024);
                StringBuilder sEMB = new StringBuilder(1024);
                StringBuilder sREJECT = new StringBuilder(1024);
                StringBuilder sSTORE = new StringBuilder(1024);
                StringBuilder sMachine = new StringBuilder(1024);
                var hDevice = MsTerm_OpenDevice("127.0.0.1", 6200);  //打开设备
                if (Config.dic("BoxNum") == "6")
                    ret = MsTerm_FeederStatus(handle, cardHoper1, cardHoper2, cardHoper3, cardHoper4, cardHoper5, cardHoper6);
                else
                    ret = MsTerm_ModuleStatus(hDevice, sprintSlip,
                                                          cardHoper1,
                                                          cardHoper2,
                                                          cardHoper3,
                                                          sPrinter,
                                                          sEMB,
                                                          sREJECT,
                                                          sSTORE,
                                                          sMachine, -1);
                ret = MsTerm_FeederStatus(handle, cardHoper1, cardHoper2, cardHoper3, cardHoper4, cardHoper5, cardHoper6);
                if (ret == 0)
                {
                    return new int[] {
                        getRetStatus(cardHoper1),
                        getRetStatus(cardHoper2),
                        getRetStatus(cardHoper3),
                        getRetStatus(cardHoper4),
                        getRetStatus(cardHoper5),
                        getRetStatus(cardHoper6)};
                }
                else
                {
                    error = "料盒卡剩余量检查失败：ret：" + ret;
                    return null;
                }
            }
            catch (Exception e)
            {
                error = "检查卡剩余量异常：" + e.ToString();
            }
            finally
            {
                Log.AddLog(log, "getLetfCardNum:error:" + (error == null ? "" : error));
            }
            return null;
        }
        private static int getRetStatus(StringBuilder msg)
        {
            string str = msg.ToString();
            if (str == "no" || str == "")
                return 0;
            else if (str == "error")
                return -1;
            else
                return 1;
        }
        #endregion

        #region 基础
        //连接设备
        public static string Open()
        {
            string error = null;
            try
            {
                handle = iTranseDevOpenDevice("127.0.0.1", 6200, 100);
            }
            catch(Exception e)
            {
                error = "设备连接失败：" + e.ToString();
            }
            finally
            {
                Log.AddLog(log, "handle:" + handle);
                if(error != null)
                    Log.AddLog(log, "Open:" + error);
            }
            return error;
        }
        //全部复位
        public static bool ResetAll()
        {
            bool fjk = false;
            bool dyj = false;
            int num = 0;
            return ResetAllAndGetStatus(out fjk, out dyj, ref num) == null;
        }
        //只复位发卡机，不复位打印机
        public static bool ResetWithOutPrint()
        {
            int ret1 = -1;
            int ret2 = -1;
            string error = "";
            try
            {
                Open();
                ret1 = iTranseDevStop(handle, 30);
                ret2 = iTranseDevReset(handle, 30);
                if (ret1 != 0 || ret2 != 0)
                    error = GetNowError();
                keepCardNum = 0;
            }
            catch(Exception e)
            {
                error = e.ToString();
            }
            finally
            {
                Log.AddLog(log, "ResetWithOutPrint ret1:" + ret1 + "  ret2:" + ret2 + "  error:" + error);
            }
            return ret2 == 0;
        }
        //复位并返回发卡机和打印机状态和色带剩余量
        public static string ResetAllAndGetStatus(out bool fkj, out bool dyj,ref int canPrintNum)
        {
            int ret1 = -1;
            int ret2 = -1;
            fkj = false;
            dyj = false;
            string error = null;
            try
            {
                Open();
                ret1 = iTranseDevStop(handle, 30);
                ret2 = iTranseDevReset(handle, 30);
                if (ret1 != 0 || ret2 != 0)
                    error = GetNowError();
                if(error == null)
                {
                    keepCardNum = 0;
                    Zebra7 z7 = Zebra7.GetDev();
                    RibbonInfo ri = new RibbonInfo();
                    if (z7.PrinterReset(out ri))
                    {
                        canPrintNum = ri.panelsRemaining;
                        Log.AddLog(log, "打印机复位成功,总共：" + ri.initialSize + "剩余:" + ri.panelsRemaining);
                    }
                    else
                    {
                        error = "打印机复位失败：" + z7.ErrorCode + "Message:" + z7.Message;
                    }
                }
            }
            catch (Exception e)
            {
                error = e.ToString();
            }
            finally
            {
                Log.AddLog(log, "ResetAll ret1:" + ret1 + "  ret2:" + ret2 + "  error:" + (error == null ? "" : error));
            }
            return error;
        }
        //获取当前发生的错误（在某指令出现错误后调用）
        public static string GetNowError()
        {
            int code = 0;
            int ret = 0;
            string error = "";
            try
            {
                Open();
                StringBuilder errorMsg = new StringBuilder(1024);
                ret = iTranseDevGetLastError(handle, errorMsg, ref code);
                Log.AddLog(log, "ret:" + ret + "  errorMsg:" + errorMsg + "  code:" + code);
                if (code > 0)
                {
                    error = "错误:" + errorMsg.ToString();
                }
                else
                    error = "获取指令执行错误信息失败,ret:" + ret + ", code:" + code;
            }
            catch (Exception e)
            {
                error = "Exception:" + e.ToString();
            }
            finally
            {
                Log.AddLog(log, "GetNowError:" + error);
            }
            return error;
        }
        #endregion

        #region 动作
        //检查读卡器弹出的卡是否有被取走
        public static bool IfHasCarIn()
        {
            Open();
            int ret = 0;
            int ret2 = iTranseDevCheckCardOut(handle, ref ret, 30);
            if (ret2 == 0)
                return ret == 1;
            else
                return false;
        }
        //将卡移到回收箱
        public static string PutCardToReject()
        {
            Log.AddLog(log, "PutCardToReject:");
            Open();
            if (iTranseDevCardToReject(handle, 60) == 0)
            {
                Log.AddLog(log, "success");
                keepCardNum--;
                return null;
            }
            else
                return GetNowError();
        }
        //读卡器弹卡
        public static string PutCardOut()
        {
            Log.AddLog(log, "PutCardToReject:");
            Open();
            if (iTranseDevExportSendCard(handle, 60) == 0)
            {
                Log.AddLog(log, "success");
                keepCardNum--;
                return null;
            }
            else
                return GetNowError();
        }
        //卡库出卡
        public static string PutCardToIC(string tag, int kc)
        {
            if (keepCardNum > 0)
                ResetWithOutPrint();
            int ret1 = -999;
            int ret2 = -999;
            try
            {
                Open();
                atr = null;

                ret1 = iTranseDevStoreSendCard(handle, tag, kc, 30);
                if (ret1 != 0)
                    return GetNowError();
                keepCardNum++;
                ret2 = iTranseDevCardToMag(handle, 30);
                if (ret2 != 0)
                    return GetNowError();
                StringBuilder outATR = new StringBuilder(1024);//无用
                int ret3 = iTranseDevPowerOnIMC(handle, outATR, 30);

            }
            catch (Exception e)
            {
                Log.AddLog(log, "Exception：" + e.ToString());
            }
            finally
            {
                Log.AddLog(log, "卡库出卡，标记：" + tag + "   卡槽:" + kc);
                Log.AddLog(log, "出卡结果：" + ret1);
                Log.AddLog(log, "将卡移到写磁位置失败结果：" + ret2);
            }
            return null;
        }
        public static string PutCardToIC()
        {
            int ret2 = -999;
            try
            {
                Open();
                ret2 = iTranseDevCardToMag(handle, 30);
                if (ret2 != 0)
                    return GetNowError();
                StringBuilder outATR = new StringBuilder(1024);//无用
                int ret3 = iTranseDevPowerOnIMC(handle, outATR, 30);
            }
            catch (Exception e)
            {
                Log.AddLog(log, "Exception：" + e.ToString());
            }
            finally
            {
                Log.AddLog(log, "将卡移到写磁位置失败结果：" + ret2);
            }
            return null;
        }
        //料盒出卡
        public static string GetCarFromBox(int box)
        {
            if (keepCardNum > 0)
                ResetWithOutPrint();
            int ret1 = -999;
            int ret2 = -999;
            try
            {
                Open();
                atr = null;

                ret1 = iTranseDevputCardIn(handle, box, 60);
                if (ret1 == -1)
                    return "-1";
                if (ret1 != 0)
                    return GetNowError();
                keepCardNum++;
                ret2 = iTranseDevCardToMag(handle, 30);
                if (ret2 != 0)
                    return GetNowError();
                StringBuilder outATR = new StringBuilder(1024);//无用
                int ret3 = iTranseDevPowerOnIMC(handle, outATR, 30);

            }
            catch (Exception e)
            {
                Log.AddLog(log, "Exception：" + e.ToString());
            }
            finally
            {
                Log.AddLog(log, "料盒出卡,Box：" + box);
                Log.AddLog(log, "出卡结果：" + ret1);
                Log.AddLog(log, "将卡移到写磁位置失败结果：" + ret2);
            }
            return null;
        }
        //卡入库
        public static string PutCardToStore(string tag, ref int slotNo)
        {
            keepCardNum--;
            int ret = 0;
            slotNo++;
            try
            {
                ret = iTranseDevCardToStore(handle, tag, ref slotNo, 30);
                if(ret != 0)
                {
                    PutCardToReject();
                    return "入库失败：" + GetNowError();
                }
            }
            catch (Exception e)
            {
                Log.AddLog(log, "Exception:" + e.ToString());
            }
            finally
            {
                Log.AddLog(log, "卡入库，标记：" + tag + "  卡槽：" + slotNo + " ret:" + ret);
            }
            if (ret == 0)
                return null;
            else
                return "卡入库失败";
        }
        #endregion

        #region 读卡
        static int up = -999;
        static int down = -999;
        static void initAboutReader()
        {
            if(up == -999)
            {
                up = int.Parse(Config.dic("Up"));
                down = int.Parse(Config.dic("Down"));
            }
        }

        //读ATR
        public static string GetATR(out string error, bool isUpReader = false, bool isContact = true)
        {
            Log.AddLog(log, "GetATR");
            error = null;
            string result = null;

            try
            {
                initAboutReader();
                int iReaderCtrl = (isUpReader ? up : down);
                int iType = (isContact ? 1 : 2);
                StringBuilder pOutInfo = new StringBuilder(1024);
                int ret = iGetATRExt(iReaderCtrl, iType, pOutInfo);
                atr_test = pOutInfo.ToString();
                if (ret == 0)
                {
                    result = pOutInfo.ToString();
                }
                else
                {
                    error = "ret:" + ret + "  msg:" + pOutInfo.ToString();
                }
            }
            catch (Exception e)
            {
                error = "exception:" + e.ToString();
            }
            finally
            {
                if (error == null)
                    Log.AddLog(log, result);
                else
                    Log.AddLog(log, error);
            }
            return result;
        }
        //读基本信息
        public static string[] GetBaseMsg(out string error, bool isUpReader = false, bool isContact = true)
        {
            Log.AddLog(log, "GetBaseMsg");
            error = null;
            string[] result = null;
            StringBuilder pOutInfo = new StringBuilder(1024);
            initAboutReader();
            int iReaderCtrl = (isUpReader ? up : down);
            int iType = (isContact ? 1 : 2);
            try
            {
                int ret = iReadCardBasExt(iReaderCtrl, iType, pOutInfo);
                if (ret == 0)
                {
                    result = pOutInfo.ToString().Split('|');
                }
                else
                {
                    error = "ret:" + ret + "  msg:" + pOutInfo.ToString();
                }
            }
            catch (Exception e)
            {
                error = "exception:" + e.ToString();
            }
            finally
            {
                if (error == null)
                    Log.AddLog(log, pOutInfo.ToString());
                else
                    Log.AddLog(log, error);
            }
            return result;
        }
        //PIN校验
        public static string CheckPin(string pin, bool isUpReader = false, bool isContact = true)
        {
            Log.AddLog(log, "CheckPin");
            StringBuilder pOutInfo = new StringBuilder(1024);
            initAboutReader();
            int iReaderCtrl = (isUpReader ? up : down);
            int iType = (isContact ? 1 : 2);
            try
            {
                int ret = iVerifyPINKExt(iReaderCtrl, iType, pin, pOutInfo);
                if (ret == 0)
                {
                    return null;
                }
                else
                {
                    return "ret:" + ret + "  msg:" + pOutInfo.ToString();
                }
            }
            catch (Exception e)
            {
                return "exception:" + e.ToString();
            }
        }
        //修改PIN
        public static string ChangePin(string srcPin, string decPin,string pin, bool isUpReader = false, bool isContact = true)
        {
            Log.AddLog(log, "ChangePin");
            StringBuilder pOutInfo = new StringBuilder(1024);
            initAboutReader();
            int iReaderCtrl = (isUpReader ? up : down);
            int iType = (isContact ? 1 : 2);
            try
            {
                int ret = iChangePINKExt(iReaderCtrl, iType, srcPin, decPin, pOutInfo);
                if (ret == 0)
                {
                    return null;
                }
                else
                {
                    return "ret:" + ret + "  msg:" + pOutInfo.ToString();
                }
            }
            catch (Exception e)
            {
                return "exception:" + e.ToString();
            }
        }
        //通用读卡
        public static string ReadCardEx(out string error,string readAddr, bool isUpReader = false, bool isContact = true)
        {
            error = null;
            Log.AddLog(log, "ReadCardEx");
            StringBuilder pOutInfo = new StringBuilder(1024);
            initAboutReader();
            int iReaderCtrl = (isUpReader ? up : down);
            int iType = (isContact ? 1 : 2);
            try
            {
                int ret = iReadCardExt(iReaderCtrl, iType, 2,"", readAddr, "", pOutInfo);
                if (ret == 0)
                {
                    return pOutInfo.ToString();
                }
                else
                {
                    error = "ret:" + ret + "  msg:" + pOutInfo.ToString();
                }
            }
            catch (Exception e)
            {
                error = "exception:" + e.ToString();
            }
            finally
            {
                if (error == null)
                    Log.AddLog(log, "result:" + pOutInfo.ToString());
                else
                    Log.AddLog(log, "error:" + error);
            }
            return null;
        }
        //读银行卡号
        public static string ReadBankNum(out string error, bool isUpReader = false, bool isContact = true)
        {
            error = null;

            ////test===============================
            //if (atr_test.IndexOf("3B6D00000087CF20018649618B00930612") != -1)
            //    return "6217281914006994119";
            //else if(atr_test.IndexOf("3B6D00000087CF20018649618B00930614") != -1)
            //    return "6217281914006994121";
            //else if(atr_test.IndexOf("3B6D00000087CF20018649618B00930613") != -1)
            //    return "6217281914006994120";
            //else if (atr_test.IndexOf("3B6D00000087CF20018649618B00930665") != -1)
            //    return "6217281914006994118";
            //else
            //    return "6666666666666666666";
            ////====================================


            Log.AddLog(log, "ReadBankNum");
            StringBuilder pOutInfo = new StringBuilder(1024);
            int iReaderCtrl = (isUpReader ? 1 : 2);
            int iType = (isContact ? 1 : 2);
            try
            {
                int ret = iReadBankNumExt(iReaderCtrl, iType, pOutInfo);
                if (ret == 0)
                {
                    return pOutInfo.ToString();
                }
                else
                {
                    error = "ret:" + ret + "  msg:" + pOutInfo.ToString();
                }
            }
            catch (Exception e)
            {
                error = "exception:" + e.ToString();
            }
            finally
            {
                if (error == null)
                    Log.AddLog(log, "result:" + pOutInfo.ToString());
                else
                    Log.AddLog(log, "error:" + error);
            }
            return null;
        }
        #endregion

        //public static string putCarOutFromKK(string tag, int kc)
        //{
        //    keepCardNum++;
        //    atr = null;
        //    string error = Open();
        //    //出卡
        //    if (error == null)
        //    {
        //        Log.AddLog(log, "准备卡库出卡，标记：" + tag + "----卡槽:" + kc);
        //        int ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "出卡失败，复位后再次尝试：");
        //            int ret21 = iTranseDevStop(handle, 30);
        //            Log.AddLog(log, "iTranseDevStop-返回：" + ret21);
        //            int ret22 = iTranseDevReset(handle, 30);
        //            Log.AddLog(log, "iTranseDevReset-返回：" + ret22);
        //            ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
        //            if (ret != 0)
        //            {
        //                Log.AddLog(log, "卡库出卡失败，返回：" + ret);
        //                Log.AddLog(log, "获取错误信息：");
        //                error = GetNowError();
        //                Log.AddLog(log, error);
        //                return "卡库出卡失败";
        //            }
        //        }
        //        Log.AddLog(log, "出卡成功，返回：" + ret);
        //        if (PutCardOut() == 0)
        //            return null;
        //        else
        //            return "设备故障";
        //    }
        //    return "设备打开失败";
        //}
        ////成品卡到废卡槽
        //public static int putCarToReject(string tag, int kc)
        //{
        //    Open();
        //    int ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
        //    if(ret == 0)
        //        ret = PutCardToReject();
        //    return ret;
        //}
        ////上电
        //public static string carToIC(out string atr)
        //{
        //    atr = "";
        //    string error = null;
        //    Log.AddLog(log, "将卡移到写磁位置");
        //    int ret = iTranseDevCardToMag(handle, 30);
           
           
        //    if (ret!=0)
        //    {
        //        Log.AddLog(log, "将卡移到写磁位置失败，接口返回：" + ret);
        //        Log.AddLog(log, "获取错误信息：");
        //        error = GetNowError();
        //        Log.AddLog(log, error);
        //        return "-1";
        //    }
        //    else
        //    {
        //        Log.AddLog(log, "将卡移到写磁位置成功，接口返回：" + ret);
        //    }

        //    Thread.Sleep(1000);

        //    Log.AddLog(log, "准备上电");

        //    StringBuilder outATR = new StringBuilder(1024);
        //    ret = iTranseDevPowerOnIMC(handle, outATR, 30);
        //    atr = getART_Old(out error);
        //    if (error != null)
        //        atr = getART_Old(out error);
        //    return error;
        //    if (ret != 0)
        //    {
        //        Log.AddLog(log, "上电失败，接口返回：" + ret);
        //        Log.AddLog(log, "获取错误信息：");
        //        error = GetNowError();
        //        Log.AddLog(log, error);
        //        return error;
        //    }
        //    else
        //    {
        //        atr = outATR.ToString();
        //        Log.AddLog(log, "上电成功，接口返回：" + ret + "  ATR:" + outATR.ToString());
        //    }


        //    atr = getART_Old(out error);
        //    if(error!=null)
        //        atr = getART_Old(out error);

        //    return error;
        //}
        //public static string carToIC2(out string atr, out string resetCarError)
        //{
        //    resetCarError = null;
        //    atr = "";
        //    string error = null;
        //    Log.AddLog(log, "将卡移到写磁位置");
        //    int ret = iTranseDevCardToMag(handle, 30);
        //    if (ret != 0)
        //    {
        //        Log.AddLog(log, "将卡移到写磁位置失败，接口返回：" + ret);
        //        Log.AddLog(log, "获取错误信息：");
        //        error = GetNowError();
        //        Log.AddLog(log, error);
        //        return error;
        //    }
        //    else
        //    {
        //        Log.AddLog(log, "将卡移到写磁位置成功，接口返回：" + ret);
        //    }
        //    Log.AddLog(log, "准备上电");
        //    StringBuilder outATR = new StringBuilder(1024);
        //    ret = iTranseDevPowerOnIMC(handle, outATR, 30);
        //    if (ret != 0)
        //    {
        //        Log.AddLog(log, "上电失败，接口返回：" + ret);
        //        Log.AddLog(log, "获取错误信息：");
        //        resetCarError = GetNowError();
        //        Log.AddLog(log, resetCarError);
        //        return null;
        //    }
        //    else
        //    {
        //        atr = outATR.ToString();
        //        Log.AddLog(log, "上电成功，接口返回：" + ret + "  ATR:" + outATR.ToString());
        //    }

         
        //    atr = getART_Old(out error);
        //    if (error != null)
        //        atr = getART_Old(out error);
        //    return error;
        //}
        ////读卡信息
        //public static string readCar(out string error,out string atr)
        //{
        //    MS2.Open();
        //    Log.AddLog(log, "上电读卡");
        //    error = carToIC(out atr);
        //    if (error != null)
        //    {
        //        error = carToIC(out atr);
        //        if (error != null)
        //            return error;
        //    }

        //    Log.AddLog(log, "第一次读卡");
        //    StringBuilder pOutInfo = new StringBuilder(1024);
        //    int ret = iReadCardBas(1, pOutInfo);
        //    Log.AddLog(log, "读卡结果：" + ret + "-" + pOutInfo.ToString());
        //    if (ret != 0)
        //    {
        //        Log.AddLog(log, "第二次读卡");
        //        ret = iReadCardBas(1, pOutInfo);
        //        Log.AddLog(log, "读卡结果：" + ret + "-" + pOutInfo.ToString());
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "第三次读卡");
        //            ret = iReadCardBas(1, pOutInfo);
        //            Log.AddLog(log, "读卡结果：" + ret + "-" + pOutInfo.ToString());
        //            if (ret != 0)
        //            {
        //                error = "读卡失败：" + pOutInfo.ToString();
        //                TipWin.showTip(error, 5000, null);
        //                return error;//
        //            }
        //        }
        //    }
        //    Log.AddLog(log, "读卡结果：" + ret + "-" + pOutInfo.ToString());

        //    //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
        //    //0         1       2         3               4         5         6     7             8     9     10    11      12
        //    return pOutInfo.ToString();
        //}
        ////读银行卡号
        //public static string readBankCarNum(out string error)
        //{
        //    Log.AddLog(log, "准备读银行卡号");
        //    StringBuilder info = new StringBuilder(1024);
        //    int ret = iReadBankNum(1, info);
        //    string info2 = info.ToString();
        //    if (ret == 0)
        //    {
        //        Log.AddLog(log, "银行卡号：" + info2);
        //        error = null;
        //        return info2;
        //    }
        //    else
        //    {
        //        Log.AddLog(log, "银行卡号：读取失败" + ret.ToString() + info2);
        //        error = info2;
        //        return null;
        //    }
        //}
        ////将卡移到非卡槽
        //public static string moveCarToReject()
        //{
        //    keepCardNum--;
        //    Log.AddLog(log, "准备将卡移到废卡槽");
        //    int ret = iTranseDevCardToReject(handle, 30);
        //    if (ret == 0)
        //    {
        //        Log.AddLog(log, "将卡移到废卡槽成功：" + ret);
        //        return null;
        //    }
        //    else
        //    {
        //        Log.AddLog(log, "将卡移到废卡槽失败，接口返回：" + ret);
        //        return GetNowError();
        //    }


        //}
        ////修改卡密码 PIN
        //public static string setPsw(string psw)
        //{
        //    //修改密码
        //    Log.AddLog(log, "正在设置密码");
        //    StringBuilder pOutInfo = new StringBuilder();
        //    int ret4 = iChangePINK(1, "123456", psw, pOutInfo);
        //    if (ret4 != 0)
        //    {
        //        Log.AddLog(log, "修改密码失败：" + pOutInfo.ToString());
        //        return "修改密码失败：" + pOutInfo.ToString();
        //    }
        //    Log.AddLog(log, "修改密码完成");
        //    return null;
        //}
        ////料盒出卡
        //public static string iTranseDevputCardIn(int index)
        //{
        //    if(keepCardNum != 0)
        //    {
        //        PutCardToReject();
        //        keepCardNum = 0;
        //    }

        //    keepCardNum++;
        //    int ret = 0;

        //    Log.AddLog(log, "尝试料盒" + index + "出卡");
        //    ret = iTranseDevputCardIn(handle, index, 60);
        //    if (ret == 0)
        //    {
        //        Log.AddLog(log, "料盒出卡成功");
        //        return null;
        //    }
        //    //else
        //    //{
        //    //    Log.AddLog(log, "出卡失败，复位后再次尝试：");
        //    //    int ret21 = iTranseDevStop(handle, 30);
        //    //    Log.AddLog(log, "iTranseDevStop-返回：" + ret21);
        //    //    int ret22 = iTranseDevReset(handle, 30);
        //    //    Log.AddLog(log, "iTranseDevReset-返回：" + ret22);
        //    //    ret = iTranseDevputCardIn(handle, index, 60);
        //    //    if (ret == 0)
        //    //    {
        //    //        Log.AddLog(log, "料盒出卡成功");
        //    //        return null;
        //    //    }
        //    //    else
        //    //    {
        //    Log.AddLog(log, "料盒出卡失败，返回：" + ret);
        //    //    }

        //    //}
        //    return "尝试从料盒出卡失败：" + ret;
        //}
        ////卡入库
        //public static string iTranseDevCardToStore(string tag, ref int slotNo)
        //{
        //    keepCardNum--;
        //    int ret = iTranseDevCardToStore(handle, tag, ref slotNo, 30);
        //    slotNo++;
        //    Log.AddLog(log, "卡入库，标记：" + tag + "  卡槽：" + slotNo);
        //    if (ret == 0)
        //    {
        //        Log.AddLog(log, "入库成功");
        //        return null;
        //    }
        //    else
        //    {
        //        Log.AddLog(log, "入库失败，返回：" + ret);
        //        return GetNowError();
        //    }
              
        //}

        ////===========[Old]============================
        //#region Old
        /////OCR
        ////[DllImport("MSTerm.dll", EntryPoint = "MsTerm_powerOffIMC", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        ////public static extern int MsTerm_powerOffIMC(IntPtr hDevice, int timeout);

        ////[DllImport("MSTerm.dll", EntryPoint = "MsTerm_MagToCar", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        ////public static extern int MsTerm_MagToCar(IntPtr hDevice, int timeout);

        //////OCR读取卡面信息
        ////public static string OcrGetData(out int isSuccess)
        ////{
        ////    IntPtr handle = MsTerm_OpenDevice("127.0.0.1", 6200);

        ////    int ret = MsTerm_powerOffIMC(handle, -1);
        ////    ret = MsTerm_MagToCar(handle, -1);

        ////    int ocrID = 1;
        ////    StringBuilder outData = new StringBuilder(1024);
        ////    int outDataLen = -1;
        ////    isSuccess = MsTerm_Ocr(handle, ocrID, outData, ref outDataLen, 60);
        ////    if(isSuccess != 0)
        ////    {
        ////        outData.Clear();
        ////        isSuccess = MsTerm_Ocr(handle, ocrID, outData, ref outDataLen, 60);
        ////    }
        ////    return outData.ToString();
        ////}

        ////出卡口没取走的卡回收到卡库 0成功 1失败 2机器异常
        //public static int letCarBackToKC(string tag,ref int slot)
        //{
        //    if (iTranseDevRecoveryCard(handle, 30) == 0)
        //    {
        //        string ret = iTranseDevCardToStore(tag, ref slot);
        //        if (ret != null)
        //        {
        //            if (moveCarToReject() != null)
        //                return 2;
        //            else
        //                return 1;
        //        }
        //        else
        //            return 0;
        //    }
        //    else
        //        return 2;
        //}
        //public static int waitToPutCarIn()
        //{
        //    Open();
        //    return iTranseDevUserInsertCard(handle, 5, 5);
        //}
        //static int max = 0;
        //public static int getKkLeft()
        //{
        //    try
        //    {
        //        max = int.Parse(Config.dic("KKMAX"));
        //        string path = @"D:\bin\Debug\Archive\RollInfo.ini";
        //        string[] lines = File.ReadAllLines(path, Encoding.Default);
        //        int num = 0;
        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            string line = lines[i];
        //            line = line.Replace(" ", "");
        //            if (line == "")
        //                continue;
        //            string[] ars = line.Split('=');
        //            if (ars.Length == 2 && ars[1] != "")
        //                num++;
        //        }
        //        return max - num;
        //    }
        //    catch (Exception e)
        //    {
        //        return -1;
        //    }
        //}
        ////检查卡库余量
        //public static int getKCLeft()
        //{
        //    IntPtr handle = MsTerm_OpenDevice("127.0.0.1", 6200);
        //    StringBuilder sb = new StringBuilder(1024);
        //    int left = MsTerm_ModuleInfo(handle, 1, sb, 1024, 60);
        //    //{"LEFT":"123","SIZE":"300"}
        //    if (left == 0)
        //    {
        //        AnalyzeJson aj = new AnalyzeJson(sb.ToString());
        //        return int.Parse(aj["LEFT"].ToString());
        //    }
        //    return -1;
        //} 
        //public static void setLogFileName_Old(string LOG)
        //{
        //    //log = LOG;
        //}
        //public static int PutCardOut_Old()
        //{
        //    keepCardNum--;
        //    Open();
        //    return iTranseDevExportSendCard(handle, 60);
        //}
        //public static int PutCardToReject_Old()
        //{
        //    keepCardNum--;
        //    Open();
        //    return iTranseDevCardToReject(handle, 60);
        //}
        //public static string getART_Old(out string error)
        //{
        //    error = null;
        //    Log.AddLog(log, "读ATR流程：准备打开读卡器");
        //    //if(icHandle>=0)
        //    //ICC_Reader_Close(icHandle);
        //    icHandle = ICC_Reader_Open("AUTO");
        //    Log.AddLog(log, "icHandle:" + icHandle);
        //    if (icHandle <= 0)
        //    {
        //        icHandle = ICC_Reader_Open("AUTO");
        //        Log.AddLog(log, "icHandle:" + icHandle);
        //        if (icHandle <= 0)
        //        {
        //            icHandle = ICC_Reader_Open("AUTO");
        //            Log.AddLog(log, "icHandle:" + icHandle);
        //            if (icHandle <= 0)
        //            {
        //                Log.AddLog(log, "准备打开读卡器，返回" + icHandle);
        //                error = "打开读写器失败，获取ATR失败。";
        //                return null;
        //            }
        //        }

        //    }
        //    Log.AddLog(log, "准备读取ATR");
        //    StringBuilder outData = new StringBuilder(1024);
        //    int ret = ICC_Reader_PowerOnEX(icHandle, 0x01, outData);
        //    ICC_Reader_PowerOff(icHandle, 0x01);
        //    //ICC_Reader_PowerOff(icHandle, 0x11);
        //    ICC_Reader_Close(icHandle);
        //    if (ret == 0)
        //    {
        //        Log.AddLog(log, "ATR:" + outData.ToString());
        //        return outData.ToString();
        //    }
        //    else
        //    {
        //        //ret = ICC_Reader_PowerOnEX(icHandle, 0x01, outData);
        //        //ICC_Reader_Close(icHandle);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "读取ATR失败:" + outData.ToString() + "|" + ret.ToString());
        //            error = outData.ToString();
        //            return null;
        //        }
        //        else
        //        {
        //            Log.AddLog(log, "ATR:" + outData.ToString());
        //            return outData.ToString();
        //        }

        //    }
        //    return null;
        //}
        //public static string putCarToIC_Old1(string tag, int kc, out string atr)
        //{
        //    keepCardNum++;
        //    atr = null;
        //    string error = Open();
        //    //出卡
        //    if (error == null)
        //    {
        //        Log.AddLog(log, "准备卡库出卡，标记：" + tag + "----卡槽:" + kc);
        //        int ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "出卡失败，复位后再次尝试：");
        //            int ret21 = iTranseDevStop(handle, 30);
        //            Log.AddLog(log, "iTranseDevStop-返回：" + ret21);
        //            int ret22 = iTranseDevReset(handle, 30);
        //            Log.AddLog(log, "iTranseDevReset-返回：" + ret22);
        //            ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
        //            if (ret != 0)
        //            {
        //                Log.AddLog(log, "卡库出卡失败，返回：" + ret);
        //                Log.AddLog(log, "获取错误信息：");
        //                error = GetNowError();
        //                Log.AddLog(log, error);
        //                return error;
        //            }
        //            else
        //            {
        //                Log.AddLog(log, "出卡成功，返回：" + ret);
        //            }
        //        }
        //        else
        //        {
        //            Log.AddLog(log, "出卡成功，返回：" + ret);
        //        }
        //    }

        //    int isSuccess = 0;
        //    Status.ocrRead = MS2.OcrGetData(out isSuccess);
        //    Log.AddLog(log, "OCR:" + Status.ocrRead + " Ret:" + isSuccess);
        //    if (isSuccess < 0)
        //    {
        //        Status.ocrRead = MS2.OcrGetData(out isSuccess);
        //        Log.AddLog(log, "OCR:" + Status.ocrRead + " Ret:" + isSuccess);
        //        if (isSuccess < 0)
        //        {
        //            PutCardToReject();
        //            return "OCR异常";
        //        }
        //    }

        //    //上电
        //    if (error == null)
        //    {
        //        Log.AddLog(log, "将卡移到写磁位置");
        //        int ret = iTranseDevCardToMag(handle, 30);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "将卡移到写磁位置失败，接口返回：" + ret);
        //            Log.AddLog(log, "获取错误信息：");
        //            error = GetNowError();
        //            Log.AddLog(log, error);
        //            return error;
        //        }
        //        else
        //        {
        //            Log.AddLog(log, "将卡移到写磁位置成功，接口返回：" + ret);
        //        }



        //        Log.AddLog(log, "准备上电");
        //        StringBuilder outATR = new StringBuilder(1024);
        //        ret = iTranseDevPowerOnIMC(handle, outATR, 30);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "上电失败，接口返回：" + ret);
        //            Log.AddLog(log, "获取错误信息：");
        //            error = GetNowError();
        //            Log.AddLog(log, error);
        //            return error;
        //        }
        //        else
        //        {
        //            atr = outATR.ToString();
        //            Log.AddLog(log, "上电成功，接口返回：" + ret + "  ATR:" + outATR.ToString());
        //        }

        //        atr = getART_Old(out error);
        //        if (error != null)
        //            atr = getART_Old(out error);
        //    }
        //    return error;
        //}
        //public static string putCarToIC_Old2(string tag, int kc, out string atr, out bool carHasOut)
        //{
        //    carHasOut = false;
        //    keepCardNum++;
        //    atr = null;
        //    string error = Open();
        //    //出卡
        //    if (error == null)
        //    {
        //        Log.AddLog(log, "准备卡库出卡，标记：" + tag + "----卡槽:" + kc);
        //        int ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "出卡失败，复位后再次尝试：");
        //            int ret21 = iTranseDevStop(handle, 30);
        //            Log.AddLog(log, "iTranseDevStop-返回：" + ret21);
        //            int ret22 = iTranseDevReset(handle, 30);
        //            Log.AddLog(log, "iTranseDevReset-返回：" + ret22);
        //            ret = iTranseDevStoreSendCard(handle, tag, kc, 30);
        //            if (ret != 0)
        //            {
        //                Log.AddLog(log, "卡库出卡失败，返回：" + ret);
        //                Log.AddLog(log, "获取错误信息：");
        //                error = GetNowError();
        //                Log.AddLog(log, error);
        //                return error;
        //            }
        //            else
        //            {
        //                carHasOut = true;
        //                Log.AddLog(log, "出卡成功，返回：" + ret);
        //            }
        //        }
        //        else
        //        {
        //            Log.AddLog(log, "出卡成功，返回：" + ret);
        //        }
        //    }
        //    //上电
        //    if (error == null)
        //    {
        //        Log.AddLog(log, "将卡移到写磁位置");
        //        int ret = iTranseDevCardToMag(handle, 30);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "将卡移到写磁位置失败，接口返回：" + ret);
        //            Log.AddLog(log, "获取错误信息：");
        //            error = GetNowError();
        //            Log.AddLog(log, error);
        //            return error;
        //        }
        //        else
        //        {
        //            Log.AddLog(log, "将卡移到写磁位置成功，接口返回：" + ret);
        //        }
        //        Log.AddLog(log, "准备上电");
        //        StringBuilder outATR = new StringBuilder(1024);
        //        ret = iTranseDevPowerOnIMC(handle, outATR, 30);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "上电失败，接口返回：" + ret);
        //            Log.AddLog(log, "获取错误信息：");
        //            error = GetNowError();
        //            Log.AddLog(log, error);
        //            return error;
        //        }
        //        else
        //        {
        //            atr = outATR.ToString();
        //            Log.AddLog(log, "上电成功，接口返回：" + ret + "  ATR:" + outATR.ToString());
        //        }

        //        atr = getART_Old(out error);
        //        if (error != null)
        //            atr = getART_Old(out error);
        //    }
        //    return error;
        //}
        ///// <summary>
        ///// 校验卡信息
        ///// </summary>
        ///// <param name="perison">身份证号/社会保障号</param>
        ///// <param name="name">姓名</param>
        ///// <param name="ksbm">卡识别码</param>
        ///// <param name="atr">ATR</param>
        ///// <returns>null:成功 其他:错误信息</returns>
        //public static string testData(string perison, string name, string ksbm, string atr_net, string atr_read)
        //{
        //    string error = Open();
        //    if (error != null)
        //        return error;
        //    StringBuilder pOutInfo = new StringBuilder(1024);

        //    Log.AddLog(log, "准备进行卡信息校验，第一次读卡：");
        //    int ret = iReadCardBas(1, pOutInfo);
        //    if (ret != 0)
        //    {
        //        Log.AddLog(log, "第一次读卡失败，返回：" + ret + "-" + pOutInfo.ToString());
        //        pOutInfo.Clear();
        //        Log.AddLog(log, "准备进行第二次读卡：");
        //        ret = iReadCardBas(1, pOutInfo);
        //        if (ret != 0)
        //        {
        //            Log.AddLog(log, "第二次读卡失败，返回：" + ret + "-" + pOutInfo.ToString());
        //            pOutInfo.Clear();
        //            Log.AddLog(log, "准备进行第三次读卡：");
        //            ret = iReadCardBas(1, pOutInfo);
        //            if (ret != 0)
        //            {
        //                Log.AddLog(log, "第三次读卡失败，返回：" + ret + "-" + pOutInfo.ToString());
        //                return "读卡信息失败：" + pOutInfo.ToString();
        //            }
        //        }
        //    }
        //    Log.AddLog(log, "ATR-接口：" + atr_net);
        //    Log.AddLog(log, "ATR-读卡：" + atr_read);
        //    Log.AddLog(log, "数据校验:" + pOutInfo.ToString() + "  -----   " + perison + " - " + name + " - " + ksbm);
        //    //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
        //    //0         1       2         3               4         5         6     7             8     9     10    11      12
        //    string[] array = pOutInfo.ToString().Split('|');
        //    Status.ssid = array[6];
        //    if (array[0] != ksbm || array[7] != perison || array[8] != name || atr_net != atr_read)
        //    {
        //        Log.AddLog(log, "数据校验失败");
        //        return "数据校验失败";
        //    }
        //    else
        //    {
        //        Log.AddLog(log, "数据校验成功");
        //        return null;
        //    }
        //}
        //#endregion
    }
}
