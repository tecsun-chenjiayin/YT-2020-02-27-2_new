using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tools;
using YTH.Functions;

namespace YTH.Controls
{
    /// <summary>
    /// CheckPassword3.xaml 的交互逻辑
    /// </summary>
    public partial class CheckPassword3 : UserControl
    {
        public CheckPassword3()
        {
            InitializeComponent();
        }

        static CheckPassword3 self = null;
        public static CheckPassword3 GetObject()
        {
            if (self == null)
                self = new CheckPassword3();
            return self;
        }

        Action nextStep = null;
        public void Goin(Action NextSetp, bool isBusiness1, string tipText)
        {
            tip2.Text = tipText;
            enable_true();
            nextStep = NextSetp;
            if (isBusiness1 && CD.business2 != null)
                CD.business2.setBusinessValue(null);
            else if (isBusiness1 == false && CD.business1 != null)
                CD.business1.setBusinessValue(null);
            if (isBusiness1)
                CD.business1.setBusinessValue(this);
            else
                CD.business2.setBusinessValue(this);

            tip.Text = "";
            p2.Password = "";

            Functions.TaskMore.Run(new Action(() => {
                Thread.Sleep(1000);
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => {
                    if (p1.Text == "")
                        p1.Focus();
                    else
                        p2.Focus();
                }));
            }));
        }

        private void input_Click(object sender, RoutedEventArgs e)
        {
            string tag = input.Content.ToString();
            if(tag == "输入")
            {
                hiden();
                
                
                input.Content = "完成";
                p1.Focus();
            }
            else
            {
                show();
                
                
                input.Content = "输入";
                p2.Focus();
            }
        }

        private void p1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (p1.Text.Length == 0)
                bk1.Text = "请输入用户名";
            else
                bk1.Text = "";
        }

        private void p2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (p2.Password.Length == 0)
                bk2.Text = "请输入密码";
            else
                bk2.Text = "";
        }

        private void p2_GotFocus(object sender, RoutedEventArgs e)
        {
            show();
            tip2.Visibility = Visibility.Visible;
            keyboard.Visibility = Visibility.Collapsed;
            input.Content = "输入";
            KeyPad.startInput(KeyPad.normal_Input, KeyPad.normal_Delete, null, null, null, this);
        }

        Task check = null;
        async private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (p1.Text == "" || p2.Password == "")
                if (check != null && check.Status == TaskStatus.Running)
                    return;
            enable_false();
            tip.Visibility = Visibility.Visible ;
            string name = p1.Text;
            string psw = tools.CommonFunction.GetMD5String(p2.Password);
            string timeTag = CD.timeTag.updateTag();
            string error = null;
            string src = "正在验证，请稍候...";
            Task task = new Task(new Action(() =>
            {
                int num = 3;
                while (CD.timeTag.equal(timeTag))
                {
                    string tag = src.Substring(0, src.Length - num);
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    {
                        tip.Text = tag;
                    }));
                    num--;
                    if (num < 0)
                        num = 3;
                    //if (check != null && check.Status == TaskStatus.RanToCompletion)
                    //{
                    //    if (error != null)
                    //    {
                    //        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    //        {
                    //            ShowTip.show(false, null, error);
                    //        }));
                    //    }
                    //    return;
                    //}
                    Thread.Sleep(1000);
                }
            }));
            task.Start();

            check = new Task(new Action(() =>
            {
                tools.MakeJson inJson = new tools.MakeJson();
                inJson.add("username", name);
                inJson.add("password", psw.ToUpper());
                tools.AnalyzeJson retJson = YTH.Functions.Post.Post_Json("checkAdmin", inJson.ToString());
                error = retJson.error;
            }));
            check.Start();
            await check.ConfigureAwait(true);
            tip.Visibility = Visibility.Hidden;
            if (error == null)
            {
                if (CD.timeTag.equal(timeTag))
                    nextStep();
            }
            else
            {
                enable_true();
                CD.timeTag.updateTag();
                ShowTip.show(false, null, error);
            }
        }
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            p2.Password = "";
        }

        private void enable_false()
        {
            p1.IsEnabled = false;
            p2.IsEnabled = false;
            ok.IsEnabled = false;
            reset.IsEnabled = false;
        }
        private void enable_true()
        {
            p1.IsEnabled = true;
            p2.IsEnabled = true;
            ok.IsEnabled = true;
            reset.IsEnabled = true;
        }

        private void hiden()
        {
            tip2.Visibility = Visibility.Hidden;
            keyboard.Visibility = Visibility.Visible;
            ok.Visibility = Visibility.Hidden;
            reset.Visibility = Visibility.Hidden;
            tipLeft.Visibility = Visibility.Hidden;
            border.Visibility = Visibility.Hidden;
            rightTip.Visibility = Visibility.Hidden;
            tip.Visibility = Visibility.Hidden;
        }

        private void show()
        {
            tip2.Visibility = Visibility.Visible;
            keyboard.Visibility = Visibility.Collapsed;
            ok.Visibility = Visibility.Visible;
            reset.Visibility = Visibility.Visible;
            tipLeft.Visibility = Visibility.Visible;
            border.Visibility = Visibility.Visible;
            rightTip.Visibility = Visibility.Visible;
            tip.Visibility = Visibility.Visible;
        }
    }
}
