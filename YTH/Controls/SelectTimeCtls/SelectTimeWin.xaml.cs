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

namespace YTH.Controls.SelectTimeCtls
{
    /// <summary>
    /// SelectTimeWin.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTimeWin : Window
    {
        Brush blue = new SolidColorBrush(Color.FromRgb(0x1E, 0x77, 0xC2));
        Brush black = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
        int index = 1;
        TextBlock upSelectTb = null;

        Years y = new Years();
        Month m = new Month();
        Days d = new Days();
        TextBlock tagetTB = null;

        static List<SelectTimeWin> objs = new List<SelectTimeWin>();

        public SelectTimeWin()
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

        public void show(TextBlock tagetTB)
        {
            this.tagetTB = tagetTB;
            index = 1;
            b1.Visibility = Visibility.Visible;
            b2.Visibility = Visibility.Hidden;
            b3.Visibility = Visibility.Hidden;
            tb12.Text = "";
            tb22.Text = "";
            tb32.Text = "";
            tb11.Foreground = blue;
            tb21.Foreground = black;
            tb31.Foreground = black;
            upSelectTb = tb12;
            Show();
            step1();
            y.resetStatus();
            m.resetStatus();
            d.resetStatus();
        }

        private void step1()
        {
            g1_PreviewMouseDown(null, null);
        }
        private void g1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            b1.Visibility = Visibility.Visible;
            b2.Visibility = Visibility.Hidden;
            b3.Visibility = Visibility.Hidden;
            upSelectTb = tb12;
            selectValue.Child = y;
            y.setTargetTextBlock(tb12, step2);
            if(e != null)
                e.Handled = true;
        }

        private void step2()
        {
            g2_PreviewMouseDown(null, null);
        }
        private void g2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (index <= 1 && sender != null) return;
            b1.Visibility = Visibility.Hidden;
            b2.Visibility = Visibility.Visible;
            b3.Visibility = Visibility.Hidden;
            tb21.Foreground = blue;
            upSelectTb = tb22;
            selectValue.Child = m;
            m.setTargetTextBlock(tb22, step3);
            if(index < 2)
                index = 2;
            if (e != null)
                e.Handled = true;
        }

        private void step3()
        {
            g3_PreviewMouseDown(null, null);
        }
        private void g3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (index <= 2 && sender != null) return;
            b1.Visibility = Visibility.Hidden;
            b2.Visibility = Visibility.Hidden;
            b3.Visibility = Visibility.Visible;
            tb31.Foreground = blue;
            upSelectTb = tb32;
            selectValue.Child = d;
            d.setTargetTextBlock(tb32, step4, System.DateTime.DaysInMonth(y.selectYear, m.selectMonth));
            if(index < 3)
            index = 3;
            if (e != null)
                e.Handled = true;
        }

        private void step4()
        {
            if (tagetTB != null)
            {
                tagetTB.Text = string.Format("{0}-{1}-{2}", y.selectYear, m.selectMonth.ToString("D2"), d.selectDay.ToString("D2"));
                tagetTB.Foreground = Brushes.Black;
            }
            Hide();
        }

        private void area_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }

        public static void CloseAll()
        {
            foreach (SelectTimeWin win in objs)
                win.Close();
        }
    }
}
