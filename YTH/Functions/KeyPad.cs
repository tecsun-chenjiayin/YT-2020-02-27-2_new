using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace YTH.Functions
{
    class KeyPad
    {
        [DllImport("TSCardDriver.dll", EntryPoint = "iOpenKeyBoard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenKeyBoard(int iPort, int iBaud, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iCloseKeyBoard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseKeyBoard(StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iInitKeyBoard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iInitKeyBoard(int ucTextModeFormat, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iGetPressKey", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iGetPressKey(StringBuilder keyInfo, StringBuilder pOutInfo);

        [DllImport("TSCardDriver.dll", EntryPoint = "iGetPinInfo", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iGetPinInfo(int iType, int iLen, int iTime, StringBuilder keyInfo, StringBuilder pOutInfo);

        private static StringBuilder pOutInfo = new StringBuilder(1024);
        public delegate void KeyDown(string c);
        static ThreadProperty tp = new ThreadProperty(100, false, false, readKey, null);
        static ThreadProperty ui = null;
        static KeyDown Input = null;
        static Action Delete = null;
        static Action Clear = null;
        static Action Ok = null;
        static Action Cancel = null;
        static bool canReadKey = false;
        public static void startInput(KeyDown input,Action delete, Action clear, Action ok, Action cancel, UIElement u)
        {
            if (!init()) return;
            if (ui == null)
                ui = new ThreadProperty(50, true, false, uiHandle, u);
            Input = input;
            Delete = delete;
            Clear = clear;
            Ok = ok;
            Cancel = cancel;
            tp.start();

        }

        public static void stop()
        {
           // tp.stop();
        }

        static bool hasInit = false;
        private static bool init()
        {
            if (hasInit) return hasInit;
            int ret = iOpenKeyBoard(int.Parse(Config.dic("KeyPad")), int.Parse(Config.dic("KeyPadBaud")), pOutInfo);
            if (ret == 0)
            {
                pOutInfo.Clear();
                ret = iInitKeyBoard(1, pOutInfo);
                if(ret == 0)
                    hasInit = true;
                else
                {
                    string error = "键盘打开成功，但是初始化失败：ret:" + ret + " error:" + pOutInfo.ToString();
                    TipWin.showTip(error, 5000, null);
                    Log.AddLog("密码键盘", error);
                }
            } 
            else
            {
                string error = "键盘初打开失败：ret:" + ret + " error:" + pOutInfo.ToString();
                TipWin.showTip(error, 5000, null);
                Log.AddLog("密码键盘", error);
            }
                
            return hasInit;
        }

        private static StringBuilder keyValue2 = null;

        private static void readKey()
        {
            pOutInfo.Clear();
            StringBuilder keyValue = new StringBuilder();
            //if (canReadKey == false)
            //    return;
            int ret = iGetPressKey(keyValue, pOutInfo);
            if(ret == 0)
            {
                keyValue2 = keyValue;
                ui.start();
            }
            else
            {
                Log.AddLog("密码键盘", "error:" + pOutInfo.ToString());
            }
        }
        private static void uiHandle()
        {
            try
            {
                string keyVal = ((int)(keyValue2[0])).ToString("X2");
                Log.AddLog("密码键盘", keyVal);
                int keyInt = Convert.ToInt32(keyVal, 16);
                string c = ((char)keyInt).ToString();
                if (keyInt >= 0x30 && keyInt <= 0x39)
                    //Input(c);\
                    YTH.Function.KeyPad2.SendKey((byte)keyInt);
                else if (keyInt == 0x20 && Clear != null)
                    Clear();
                else if (keyInt == 0x0D && Ok != null)
                    Ok();
                else if (keyInt == 0x08)
                    //Delete();
                    YTH.Function.KeyPad2.SendKey(8);
                else if (keyInt == 0x1B && Cancel != null)
                    Cancel();
                keyValue2.Clear();
            }
            catch(Exception e)
            {
                Log.AddLog("密码键盘Error", e.ToString());
            }
        }

        public static void normal_Input(string val)
        {
            if (val.Length == 1 && val[0] >= '0' && val[0] <= '9')
                Send(val);
        }
        public static void normal_Delete()
        {
            Send("{BACKSPACE}");
        }
        private static void Send(string txt)
        {
            try
            {
                if (txt[0] == '{' && txt[txt.Length - 1] == '}')
                    System.Windows.Forms.SendKeys.SendWait(txt);
                else
                {
                    System.Windows.Clipboard.SetText(txt);
                    System.Windows.Forms.SendKeys.SendWait("^v");
                }
            }
            catch { }
        }
    }
}
