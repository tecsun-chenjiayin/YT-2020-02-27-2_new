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
    /// Years.xaml 的交互逻辑
    /// </summary>
    public partial class Years : UserControl
    {
        public int selectYear = 2018;

        Brush blue = new SolidColorBrush(Color.FromRgb(0x07, 0x63, 0xAE));
        Brush black = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
        Brush white = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));

        List<Label> labels = new List<Label>();
        int startYear = 1961;//1991
        int maxYear = 1991;
        Label old = null;
        TextBlock targetTB = null;
        Action nextStep = null;
        public Years()
        {
            InitializeComponent();
            maxYear = System.DateTime.Now.Year;
            for(int i = maxYear; i >= startYear; i--)
            {
                Label label = new Label(); 
                label.Content = i.ToString();
                label.SetValue(Label.StyleProperty, Resources["btn"]);
                wrap.Children.Add(label);
                labels.Add(label);
            }
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

        //临界点-跨年
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
            {
                if(maxYear != System.DateTime.Now.Year)
                {
                    Label label = new Label();
                    label.Content = System.DateTime.Now.Year.ToString();
                    label.SetValue(Label.StyleProperty, Resources["btn"]);
                    wrap.Children.Insert(0, label);
                    maxYear = System.DateTime.Now.Year;
                }
            }
        }

        private void WrapPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label now = (Label)e.Source;
            if (now.Tag.ToString() != "Label") return;
            if (now == old) return;
            if(old != now && old != null)
            {
                old.Background = white;
                old.Foreground = black;
            }
            old = now;
            now.Background = blue;
            now.Foreground = white;
            if (targetTB != null)
                targetTB.Text = "(" + now.Content.ToString() + ")";
            selectYear = int.Parse(now.Content.ToString());
            if (nextStep != null)
                nextStep();
        }
    }
}
