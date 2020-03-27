using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YTH.BuKa;
using YTH.Functions;

namespace YTH.ZhanJiang
{
    /// <summary>
    /// ChangePsw.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePsw : UserControl
    {
        public ChangePsw()
        {
            InitializeComponent();
        }

        static ChangePsw self = null;
        public static ChangePsw GetObject()
        {
            if (self == null)
                self = new ChangePsw();
            return self;
        }

        public void Goin()
        {
            B_ReadSSCard.GetObject().BeforeGoin(show);
            B_ReadSSCard.GetObject().Goin();
            
        }

        public void show()
        {
            KeyPad.startInput(KeyPad.normal_Input, KeyPad.normal_Delete, clear, ok_, null, this);
            BackExit.LetNextClickToMain();
            p1.Password = "";
            p2.Password = "";
            p3.Password = "";
            p1.Focus();
            Business2.Init("社保密码修改");
            CD.business2.setBusinessValue(this);
        }

        async private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (p1.Password.Length != 6)
                ShowTip.show(false, null, "请输入6位数原密码");
            else if (p2.Password.Length != 6 || p3.Password.Length != 6)
                ShowTip.show(false, null, "请输入6位数新密码");
            else if (p2.Password != p3.Password)
                ShowTip.show(false, null, "两次输入的新密码不一致！");
            else
            {
                string pOutInfo = null;
                int ret = -1;
                Loading.show2("正在修改密码，请稍候...");
                await TaskMore.Run(new Action(() => {
                    int ctl = int.Parse(Config.dic("Up"));
                    ret = B_ReadSSCard.ChangePINKExt(ctl, p1.Password, p2.Password, out pOutInfo);
                })).ConfigureAwait(true);
                if (ret == 0)
                    ShowTip.show(true, BackExit.Exit, "密码修改成功");
                else
                    ShowTip.show(false, BackExit.Exit, "密码修改失败：" + pOutInfo);
            }
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            p1.Password = "";
            p2.Password = "";
            p3.Password = "";
            p1.Focus();
        }
        private void P3_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (p3.Password.Length == 0)
                bk3.Text = "请输入新密码";
            else
                bk3.Text = "";
        }
        private void P2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (p2.Password.Length == 0)
                bk2.Text = "请输入新密码";
            else
                bk2.Text = "";
            if (p2.Password.Length == 6)
                p3.Focus();
        }     
        private void P1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (p1.Password.Length == 0)
                bk1.Text = "请输入原密码";
            else
                bk1.Text = "";
            if (p1.Password.Length == 6)
                p2.Focus();
        }
        private void clear()
        {
            Reset_Click(null, null);
        }
        private void ok_()
        {
            Ok_Click(null, null);
        }

    }
}
