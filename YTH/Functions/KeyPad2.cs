using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;


namespace YTH.Function
{
    class KeyPad2
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        static Action OK = null;
        //仅仅调用这个就可以了
        //public static void setNextStep(Action ok)
        //{
        //    return;
        //    OK = ok;
        //    Keyboard.KeyBoard.GetKeyBoard().readCipherText(delete, OK2, input, showError);
        //}

        private static void delete()
        {
            SendKey(8);
        }

        private static void input(string val)
        {
            //正式
            SendKey((byte)(96 + byte.Parse(val)));
        }

        private static void OK2()
        {
            if (OK == null)
                return;
            OK();
        }

        private static void showError(string error)
        {
            System.Windows.MessageBox.Show("密码键盘打开失败：" + error);
        }
        /// <summary>
        /// 发送按键
        /// </summary>
        /// <param name="asiiCode">键盘ascii码</param>
        public static void SendKey(byte asiiCode)
        {
            AttachThreadInput(true);
            int getFocus = Win32API.GetFocus();
            //向前台窗口发送按键消息
            Win32API.PostMessage(getFocus, Win32API.WM_KEYDOWN, asiiCode, 0);
            AttachThreadInput(false); //取消线程亲和的关联
        }
        /// <summary>
        /// 设置线程亲和,附到前台窗口所在线程,只有在线程内才可以获取线程内控件的焦点
        /// </summary>
        /// <param name="b">是否亲和</param>
        private static void AttachThreadInput(bool b)
        {
            Win32API.AttachThreadInput(
                   Win32API.GetWindowThreadProcessId(
                   Win32API.GetForegroundWindow(), 0),
                   Win32API.GetCurrentThreadId(), Convert.ToInt32(b));
        }

        //=======================================================
        static string val1;
        private static void sendKey()
        {
            SendKeys.Send(val1);
        }

        /// <summary>
        /// Win32接口功能类
        /// </summary>
        public static class Win32API
        {
            /// <summary>
            /// 键入
            /// </summary>
            public const int WM_KEYDOWN = 0x100;

            [DllImport("user32.dll", EntryPoint = "SendMessageW")]
            public static extern int SendMessage(
                 int hwnd,
                 int wMsg,
                 int wParam,
                 int lParam);
            [DllImport("user32.dll", EntryPoint = "PostMessageW")]
            public static extern int PostMessage(
                 int hwnd,
                 int wMsg,
                 int wParam,
                 int lParam);
            [DllImport("user32.dll")]
            public static extern int GetForegroundWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetLastActivePopup(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern int GetFocus();
            [DllImport("user32.dll")]
            public static extern int AttachThreadInput(
                 int idAttach,
                 int idAttachTo,
                 int fAttach);
            [DllImport("user32.dll")]
            public static extern int GetWindowThreadProcessId(
                 int hwnd,
                 int lpdwProcessId);
            [DllImport("kernel32.dll")]
            public static extern int GetCurrentThreadId();

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
        }

    }
}
