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
using YTH.Functions;

namespace YTH.Controls
{
    /// <summary>
    /// MPassword.xaml 的交互逻辑
    /// </summary>
    public partial class MPassword : UserControl
    {
        static MPassword mp = null;
        Action nextStep = null;
        int inputStyle = 0;//1:输入新密码  2:调用接口校验密码
        int inputTime = 0;//已经输入的完整密码次数

        List<Label> ps = new List<Label>();
        Stack<string> psw1 = new Stack<string>();
        Stack<string> psw2 = new Stack<string>();

        private MPassword()
        {
            InitializeComponent();
            ps.Add(p1);
            ps.Add(p2);
            ps.Add(p3);
            ps.Add(p4);
            ps.Add(p5);
            ps.Add(p6);
            foreach (Label L in ps)
                L.Content = "";
        }
        static bool isBussiness1 = false;
        public static MPassword getObject(bool isBussiness1_)
        {
            isBussiness1 = isBussiness1_;
            if (mp == null)
                mp = new MPassword();
            return mp;
        }

        string newPsw = "";
        public delegate void NextStepPar1(string newPsw);
        NextStepPar1 nextStepPar1 = null;
        //入口
        public void Goin(NextStepPar1 nextStepPar1, int inputStyle, string title)
        {
            newPsw = "";
            this.title.Text = title;
            inputTime = 0;
            this.inputStyle = inputStyle;
            this.nextStepPar1 = nextStepPar1;
            reset();
            KeyPad.startInput(input, Delete, clear, OK, Back_, this);
        }
        //对比密码
        private bool isEqual()
        {
            if (psw1.Count != 6 || psw2.Count != 6) return false;
            string psw1_ = "";
            string psw2_ = "";
            for (int i = 0; i < 6; i++)
            {
                psw1_ = psw1.Pop() + psw1_;
                psw2_ = psw2.Pop() + psw2_;
            }
            if (psw1_ == psw2_)
            {
                newPsw = psw1_;
                return true;
            }
            return false;
        }
        //获取密码
        private string getPsw()
        {
            string psw1_ = "";
            for (int i = 0; i < 6; i++)
            {
                psw1_ = psw1.Pop() + psw1_;
            }
            return psw1_;
        }

        //输入
        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            FButton fb = (FButton)e.Source;
            if (fb.Content == null) return;
            string val = fb.Content.ToString();
            if (val.Length != 1) return;
            input(val);
        }
        private void input(string val)
        {
            if (Visibility != Visibility.Visible) return;
            if (inputTime == 0 && psw1.Count < 6)
                psw1.Push(val);
            else if (inputTime == 1 && psw2.Count < 6)
                psw2.Push(val);
            //界面设置
            int i = 0;
            for (; i < ps.Count; i++)
            {
                if (ps[i].Content.ToString() != "●")
                {
                    ps[i].Content = "●";
                    break;
                }
            }
        }
        //删除
        private void Delete()
        {
            if (Visibility != Visibility.Visible) return;
            delete_Click(null, null);
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (inputTime == 0 && psw1.Count > 0)
            {
                psw1.Pop();
                ps[psw1.Count].Content = "";
            }
            else if (inputTime == 1 && psw2.Count > 0)
            {
                psw2.Pop();
                ps[psw2.Count].Content = "";
            }
        }
        //确认
        private void OK()
        {
            if (Visibility != Visibility.Visible) return;
            ok_Click(null, null);
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            //密码是否输入完毕
            if ((inputTime == 0 && psw1.Count < 6) || (inputTime == 1 && psw2.Count < 6))
            {
                if (isBussiness1)
                    TipWinB1.showTip("请输入6位密码", 3000, null);
                else
                    TipWin.showTip("请输入6位密码", 3000, null);
                return;
            }
            //设置新密码
            if (inputStyle == 1)
            {
                if (inputTime == 0)
                {
                    title.Text = "请再次输入新密码";
                    clearTextBlocks();
                    inputTime = 1;
                }
                else if (inputTime == 1)
                {
                    if (isEqual())
                    {
                        if (nextStepPar1 != null)
                            nextStepPar1(newPsw);
                    }
                    else
                    {
                        if (isBussiness1)
                            TipWinB1.showTip("两次密码输入不一致，请重新输入！", 3000, null);
                        else
                            TipWin.showTip("两次密码输入不一致，请重新输入！", 3000, null);
                        reset();
                        title.Text = "请输入新密码";
                    }
                }
            }
            //网络校验密码
            else if (inputStyle == 2)
            {
                if(netPsw == getPsw())
                    nextStepPar1(getPsw());
            }
        }
        //清除
        private void clear()
        {
            if (Visibility != Visibility.Visible) return;
            for (int i = 0; i < ps.Count; i++)
                ps[i].Content = "";
            if (inputTime == 0)
                psw1.Clear();
            else if (inputTime == 1)
                psw2.Clear();
        }
        private void clearTextBlocks()
        {
            if (Visibility != Visibility.Visible) return;
            for (int i = 0; i < ps.Count; i++)
                ps[i].Content = "";
        }
        //重置
        public void reset()
        {
            psw1.Clear();
            psw2.Clear();
            for (int i = 0; i < 6; i++)
                ps[i].Content = "";
            inputTime = 0;
        }
        //返回
        private void Back_()
        {
            return;
            if (Visibility != Visibility.Visible) return;
            BackExit.Back();
        }

        static string netPsw = null;
        public static string initNetPsw()
        {
            if(netPsw == null)
            {

            }
            return null;//
        }
    }
}
