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
using System.Windows.Shapes;

namespace YTH.Controls.InputPersion
{
    /// <summary>
    /// InputPersionidWinxaml.xaml 的交互逻辑
    /// </summary>
    public partial class InputPersionidWin : Window
    {
        static List<InputPersionidWin> objs = new List<InputPersionidWin>();
        StringBuilder str = new StringBuilder(18);
        public InputPersionidWin()
        {
            InitializeComponent();
            // 设置全屏
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.Topmost = true;
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            ShowInTaskbar = false;

            objs.Add(this);
        }

        public delegate void SetData(string perionid);
        SetData sd = null;
        public void show(SetData sd)
        {
            this.sd = sd;
            Show();
            str.Clear();
            value.Text = "";
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            value.Text = "";
            Hide();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            string val = value.Text;
            FButton btn = (FButton)e.Source;
            if (btn.Tag.ToString() == "Num")
            {
                if (val.Length < 18)
                    str.Append(btn.Content.ToString());
            }
            else if(btn.Tag.ToString() == "Ok")
            {
                if (sd != null)
                    sd(value.Text);
                value.Text = "";
                Hide();
            }
            else if(btn.Tag.ToString() == "Delete")
            {
                if (val.Length > 0)
                    str.Remove(val.Length - 1, 1);
            }
            value.Text = str.ToString();
        }

        public static void CloseAll()
        {
            foreach (InputPersionidWin win in objs)
                win.Close();
        }
    }
}
