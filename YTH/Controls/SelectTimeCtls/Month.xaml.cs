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

namespace YTH.Controls.SelectTimeCtls
{
    /// <summary>
    /// Month.xaml 的交互逻辑
    /// </summary>
    public partial class Month : UserControl
    {
        public int selectMonth = 1;
        Brush blue = new SolidColorBrush(Color.FromRgb(0x07, 0x63, 0xAE));
        Brush black = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
        Brush white = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));

        Label old = null;
        TextBlock targetTB = null;
        Action nextStep = null;
        public Month()
        {
            InitializeComponent();
        }

        public void setTargetTextBlock(TextBlock tb, Action nextStep)
        {
            targetTB = tb;
            this.nextStep = nextStep;
        }


        public void resetStatus()
        {
            if (old != null)
            {
                old.Background = white;
                old.Foreground = black;
            }
            old = null;
        }

        private void WrapPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label now = (Label)e.Source;
            if (now.Tag.ToString() != "Label") return;
            if (old != now && old != null)
            {
                old.Background = white;
                old.Foreground = black;
            }
            old = now;
            now.Background = blue;
            now.Foreground = white;
            if (targetTB != null)
                targetTB.Text = "(" + now.Content.ToString() + ")";
            selectMonth = int.Parse(now.Content.ToString().Replace("月", ""));
            if (nextStep != null)
                nextStep();
        }
    }
}
