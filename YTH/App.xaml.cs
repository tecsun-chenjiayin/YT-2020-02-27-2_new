using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using tools;

namespace YTH
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    //当前运行WPF程序的进程实例
        //    Process process = Process.GetCurrentProcess();
        //    //遍历WPF程序的同名进程组
        //    foreach (Process p in Process.GetProcessesByName(process.ProcessName))
        //    {
        //        //不是同一进程并且本进程启动时间最晚,则关闭较早进程
        //        if (p.Id != process.Id && (p.StartTime - process.StartTime).TotalMilliseconds <= 0)
        //        {
        //            p.Kill();//这个地方用kill 而不用Shutdown();的原因是,Shutdown关闭程序在进程管理器里进程的释放有延迟不是马上关闭进程的
        //            //Application.Current.Shutdown();
        //            return;
        //        }
        //    }
        //    base.OnStartup(e);
        //}
        System.Threading.Mutex mutex;

        public App()
        {
            //==============添加到 当前登陆用户的 注册表启动项========
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            RegistryKey RKey1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            RKey1.SetValue("AutoUpdate", path);

            this.Startup += new StartupEventHandler(App_Startup);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException1;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "AutoUpdate", out ret);

            if (!ret)
            {
                MessageBox.Show("已有一个程序实例运行");
                Environment.Exit(0);
            }
        }

        private void App_DispatcherUnhandledException1(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("终端出现未处理异常，请联系管理员处理：\r\n" + e.Exception.Message.ToString());
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.AddLog("未经处理的异常", Environment.NewLine + e.Exception.Message);
            //MessageBox.Show("Error encountered! Please contact support." + Environment.NewLine + e.Exception.Message);
            //Shutdown(1);
            e.Handled = false;
        }






    }
}
