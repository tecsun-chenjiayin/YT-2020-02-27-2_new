using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using YTH.Functions;

namespace YTH.Controls
{
    /// <summary>
    /// CheckPassword.xaml 的交互逻辑
    /// </summary>
    public partial class CheckPassword : Window
    {
        static CheckPassword cp = null;
        Label[] p = new Label[6];
        const string tipTxt = "密码输入有误，请重新输入";
        static Action nextStep = null;
        const string testPassword = "123456";
        bool canBeChange = true;

        public CheckPassword()
        {
            InitializeComponent();
            Topmost = true;
            p[0] = p1;
            p[1] = p2;
            p[2] = p3;
            p[3] = p4;
            p[4] = p5;
            p[5] = p6;
            tip.Text = "";

        }

        public static void Goin(Action nextStep_)
        {
            nextStep = nextStep_;
            if (cp == null)
                cp = new CheckPassword();
            cp.Show();
            cp.canBeChange = true;
        }

        int index = 0;
        List<string> ps = new List<string>();

        private void TXButton_Click(object sender, RoutedEventArgs e)
        {
            if (canBeChange == false)
                return;
            TXButton tx = sender as TXButton;
            if(tx.Content != null)
            {
                string txt = tx.Content as string;
                if(txt.Length == 1)
                {
                   
                    if (index >= p.Length)
                        return;
                    if (tip.Text != "")
                        tip.Text = "";
                    ps.Add(txt);
                    p[index].Content = "●";
                    index++;

                    if (index >= p.Length)
                    {
                        canBeChange = false;
                        TH.addOnceData(checkPsw);
                    }
                }
                else
                {
                    foreach (Label l in p)
                        l.Content = "";
                    ps.Clear();
                    index = 0;
                    Hide();
                }
            }
            else
            {
                if (index == 0)
                    return;
                ps.RemoveAt(ps.Count - 1);
                index--;
                p[index].Content = "";
            }
        }


        private void checkPsw()
        {
            stop = false;
            TH.addOnceData(checkAnimation);
            Thread.Sleep(2000);
            StringBuilder psw = new StringBuilder();
            foreach (string s in ps)
                psw.Append(s);
            if (psw.ToString() == testPassword)
            {
                TH.addOnceUI(new Action(() => {
                    Visibility = Visibility.Hidden;
                    if (nextStep != null)
                        nextStep();
                    tip.Text = "";
                    foreach (Label l in p)
                        l.Content = "";
                    ps.Clear();
                    index = 0;
                }));
            }
            else
            {
                stop = true;
            }
        }

        StringBuilder antip = new StringBuilder();
        bool stop = false;
        private void checkAnimation()
        {
            while (Visibility == Visibility.Visible && stop == false)
            {
                for (int i = 0; i < 6 && Visibility == Visibility.Visible && stop == false; i++)
                {
                    Thread.Sleep(500);
                    antip.Append(". ");
                    TH.addOnceUI(new Action(() => { tip.Text = antip.ToString(); }));
                }
            }
            if (stop)
            {
                TH.addOnceUI(new Action(() => {
                    tip.Text = tipTxt;
                    foreach (Label l in p)
                        l.Content = "";
                    ps.Clear();
                    index = 0;
                }));
            }
            antip.Clear();
            canBeChange = true;
        }
    }
}
