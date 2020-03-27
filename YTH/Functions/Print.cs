using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace YTH.Functions.MSDLL
{
    class Print
    {
        [DllImport("TSCardDriver.dll", EntryPoint = "iOpenPrinter", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenPrinter(int iPort, int iBaud, StringBuilder pOutInfo);//打开

        [DllImport("TSCardDriver.dll", EntryPoint = "iClosePrinter", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iClosePrinter(StringBuilder pOutInfo);//关闭

        [DllImport("TSCardDriver.dll", EntryPoint = "iInitPrinter", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iInitPrinter(StringBuilder pOutInfo);//初始化

        [DllImport("TSCardDriver.dll", EntryPoint = "iCutPaper", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCutPaper(int n, StringBuilder pOutInfo);//剪纸

        [DllImport("TSCardDriver.dll", EntryPoint = "iFeedPaper", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iFeedPaper(int n, StringBuilder pOutInfo);//走纸

        [DllImport("TSCardDriver.dll", EntryPoint = "iSetLeftMargin", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iSetLeftMargin(int n, StringBuilder pOutInfo);//设置左间距

        [DllImport("TSCardDriver.dll", EntryPoint = "iSetPrinterAlign", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iSetPrinterAlign(int n, StringBuilder pOutInfo);//设置对齐模式

        [DllImport("TSCardDriver.dll", EntryPoint = "iPrintStrOnLine", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iPrintStrOnLine(int iXSize, int iYSize, int iBold, string pInData, StringBuilder pOutInfo);//行打印 

        [DllImport("TSCardDriver.dll", EntryPoint = "iGetPrinterStatus", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iGetPrinterStatus(StringBuilder pOutInfo);//获取打印机状态

        [DllImport("TSCardDriver.dll", EntryPoint = "iPrintBitmap", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iPrintBitmap(string pFilePath, int m, StringBuilder pOutInfo);//打印位图

        public static string checkPrint()
        {
            StringBuilder outError = new StringBuilder(2048);
            int ret = iOpenPrinter(int.Parse(Config.dic("PrintPort")), int.Parse(Config.dic("PrintBaud")), outError);
            Log.AddLog("设备自检", "ret:" + ret + " out:" + outError.ToString());
            if (ret != 0)
                return outError.ToString();
            iClosePrinter(outError);
            return null;
        }
        public static string checkPrint(ref string status2)
        {
            StringBuilder outError = new StringBuilder(2048);
            int ret = iOpenPrinter(int.Parse(Config.dic("PrintPort")), int.Parse(Config.dic("PrintBaud")), outError);
            Log.AddLog("设备自检", "凭条打印机-打开ret:" + ret + " out:" + outError.ToString());
            if (ret != 0)
                return outError.ToString();
            ret = iInitPrinter(outError);
            Log.AddLog("设备自检", "凭条打印机-初始化ret:" + ret + " out:" + outError.ToString());
            if (ret != 0)
                return outError.ToString();
            StringBuilder status = new StringBuilder(1024);
            iGetPrinterStatus(status);
            status2 = status.ToString();
            iClosePrinter(outError);
            return null;
        }
        const string log = "打印机";
        static StringBuilder outError = new StringBuilder(2048);
        public static string print(List<string> lines)
        {
            try
            {
                Log.AddLog(log, "打开打印机");
                outError.Clear();
                int ret = iOpenPrinter(int.Parse(Config.dic("PrintPort")), int.Parse(Config.dic("PrintBaud")), outError);
                if (ret != 0)
                {
                    Log.AddLog(log, "打开失败,原因:" + outError.ToString());
                    return outError.ToString();
                }

                Log.AddLog(log, "初始化");
                outError.Clear();
                outError = new StringBuilder(2048);
                ret = iInitPrinter(outError);
                if (ret != 0)
                {
                    Log.AddLog(log, "初始化失败,ret:" + ret);
                    Log.AddLog(log, "初始化失败,原因:" + outError.ToString());
                    return outError.ToString();
                }
                //打印图片
                outError.Clear();
                string pFilePath = CD.getBasePath() + @"Soruce\logo2.bmp";// System.Windows.Forms.Application.StartupPath  /*System.Environment.CurrentDirectory*/ + "\\河南12333微信公众号.bmp";

                Log.AddLog(log, "pFilePath:" + pFilePath);

                iPrintBitmap(pFilePath, 1, outError);
                Log.AddLog(log, "打印图片:" + outError);

                Log.AddLog(log, "设置左间距");
                outError.Clear();
                ret = iSetLeftMargin(5, outError);
                if (ret != 0)
                {
                    Log.AddLog(log, "设置左间距失败,原因:" + outError.ToString());
                    return outError.ToString();
                }

                ret = iFeedPaper(1, outError);
                for (int i = 0; i < lines.Count; i++)
                {
                    Log.AddLog(log, "打印行");
                    outError.Clear();

                    //if(i == 0)
                    //    ret = iPrintStrOnLine(1, 1, 1, lines[i], outError);
                    //else
                        ret = iPrintStrOnLine(0, 0, 1, lines[i], outError);

                    
                    if (ret != 0)
                    {
                        Log.AddLog(log, "打印行失败,原因:" + outError.ToString());
                        return outError.ToString();
                    }

                    Log.AddLog(log, "走纸");
                    outError.Clear();
                    ret = iFeedPaper(1, outError);
                    if (ret != 0)
                    {
                        Log.AddLog(log, "走纸失败,原因:" + outError.ToString());
                        return outError.ToString();
                    }
                }

                outError.Clear();
                ret = iSetPrinterAlign(1, outError);

      

                ret = iFeedPaper(5, outError);
                Log.AddLog(log, "剪纸");
                outError.Clear();
                ret = iCutPaper(0, outError);
                if (ret != 0)
                {
                    Log.AddLog(log, "剪纸失败,原因:" + outError.ToString());
                    return outError.ToString();
                }
                Log.AddLog(log, "打印完成");
                return "";
            }
            catch (Exception e)
            {
                return "打印凭条失败：" + e.ToString();
            }

        }
    }


}
