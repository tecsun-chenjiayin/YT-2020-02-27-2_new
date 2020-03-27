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
    /// Days.xaml 的交互逻辑
    /// </summary>
    public partial class Days : UserControl
    {
        public int selectDay = 1;
        Brush blue = new SolidColorBrush(Color.FromRgb(0x07, 0x63, 0xAE));
        Brush black = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
        Brush white = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));

        List<Label> labels = new List<Label>();
        Label old = null;
        TextBlock targetTB = null;
        Action nextStep = null;
        public Days()
        {
            InitializeComponent();
            for (int i = 1; i <= 31; i++)
            {
                Label label = new Label();
                label.Content = i.ToString();
                label.SetValue(Label.StyleProperty, Resources["btn"]);
                wrap.Children.Add(label);
                labels.Add(label);
            }
        }

        public void setTargetTextBlock(TextBlock tb, Action nextStep, int dayNum)
        {
            targetTB = tb;
            this.nextStep = nextStep;
            for (int i = 0; i < dayNum && i < labels.Count; i++)
                labels[i].Visibility = Visibility.Visible;
            for (int i = dayNum; i < labels.Count; i++)
                labels[i].Visibility = Visibility.Hidden;
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
                targetTB.Text = "(" + now.Content.ToString() + "日)";
            selectDay = int.Parse(now.Content.ToString());
            if (nextStep != null)
                nextStep();
        }
    }
}
