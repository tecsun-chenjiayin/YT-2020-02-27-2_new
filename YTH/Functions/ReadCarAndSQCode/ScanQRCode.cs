using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace YTH.Functions.ReadCarAndSQCode
{
    class ScanQRCode
    {
        [DllImport("TSCardDriver.dll", EntryPoint = "iOpenScanner", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenScanner(int iPort, int iBaud, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iInitScanner", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iInitScanner(int hCom, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iStopScanner", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iStopScanner(int hCom, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iCloseScanner", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseScanner(int hCom, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iGetScannerData", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iGetScannerData(int hCom, StringBuilder pScanData, StringBuilder pOutInfo);

        const string log = "扫描仪";
        static bool hasInit = false;
        static int handle = -1;
        static bool hasOpen = false;
        public static string error = null;
        public static string code = null;

        static ThreadProperty keepTP = null;
        public static void Init()
        {
            if (keepTP == null)
                keepTP = new ThreadProperty(50, false, true, scan, null);
        }

        private static bool init()
        {
            if (hasInit) return hasInit;
            StringBuilder pOutInfo = new StringBuilder(1024);
            int port = int.Parse(Config.dic("Scanner"));
            int buad = int.Parse(Config.dic("ScannerBaud"));
            handle = iOpenScanner(port, buad, pOutInfo);
            Log.AddLog(log, string.Format("port:{0} buad:{1} ret:{2}", port, buad, handle));
            if (handle <= 0)
            {
                error = pOutInfo.ToString();
                Log.AddLog(log, "init:ret-" + handle + " error:" + error);
                hasInit = false;
                TipWin.showTip("扫描仪初始化失败：" + error, 5000, BackExit.Exit);
            }
            else
            {
                hasInit = true;
            }
            return hasInit;
        }

        private static void scan()
        {
            scanQRCode();
        }

        public static void clearOldMsg()
        {
            code = null;
        }
        public static string getCode()
        {
            return code;
        }

        private static string scanQRCode()
        {
            error = null;
            if (Config.dic("UseTestPersionalData") == "1")
            {
                Thread.Sleep(3000);
                code = Config.dic("QRCode");
            }
            else
            {
                if (!init())
                    return "Exit";
                StringBuilder pScanData = new StringBuilder(1024);
                StringBuilder pOutInfo = new StringBuilder(1024);
                if (hasOpen == false)
                {
                    int ret = iInitScanner(handle, pOutInfo);
                    if (ret != 0)
                    {
                        error = "开启扫描失败：" + ret;
                        Log.AddLog(log, error);
                        return error;
                    }
                    else
                    {
                        hasOpen = true;
                        Log.AddLog(log, "open:ret-" + ret);
                    }

                }

                Log.AddLog(log, "handle：" + handle);
                int ret2 = iGetScannerData(handle, pScanData, pOutInfo);
                if (ret2 > 0)
                {
                    string[] array = pScanData.ToString().Split('\r');
                    if(array.Length < 2)
                    {
                        Log.AddLog(log, "扫描结果异常：结果里没有换行符：" + pScanData.ToString());
                    }
                    else
                    {
                        code = array[array.Length - 2];//倒数第二个是最新的结果
                        Log.AddLog(log, "扫描成功：" + code);
                    }
                }
                else
                {
                    error = pOutInfo.ToString();
                    if(code == null)
                        Log.AddLog(log, "扫描失败：ret:" + ret2 + " error:" + error);
                }

            }
            return error;
        }

        //废弃
        private static void stopScan()
        {
            if (handle <= 0) return;
            try
            {
                StringBuilder pOutInfo = new StringBuilder(1024);
                int ret = iStopScanner(handle, pOutInfo);
                Log.AddLog(log, "stopScan：" + ret);
            }
            catch (Exception e)
            {
                Log.AddLog(log, "error:" + e.ToString());
            }
        }
    }
}
