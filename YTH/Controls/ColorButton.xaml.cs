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

namespace YTH.Controls
{
    /// <summary>
    /// ColorButton.xaml 的交互逻辑
    /// </summary>
    public partial class ColorButton : UserControl
    {
        public static DependencyProperty normalColor1Property = null;
        public Color normalColor1
        {
            get { return (Color)GetValue(normalColor1Property); }
            set { SetValue(normalColor1Property, value); }
        }
        public static DependencyProperty normalColor2Property = null;
        public Color normalColor2
        {
            get { return (Color)GetValue(normalColor2Property); }
            set { SetValue(normalColor2Property, value); }
        }

        public static DependencyProperty downColor1Property = null;
        public Color downColor1
        {
            get { return (Color)GetValue(downColor1Property); }
            set { SetValue(downColor1Property, value); }
        }
        public static DependencyProperty downColor2Property = null;
        public Color downColor2
        {
            get { return (Color)GetValue(downColor2Property); }
            set { SetValue(downColor2Property, value); }
        }
        public static DependencyProperty tFontSizeProperty;
        public int tFontSize
        {
            get { return (int)GetValue(tFontSizeProperty); }
            set { SetValue(tFontSizeProperty, value);
                t.FontSize = tFontSize;
            }
        }
        public static DependencyProperty tForegroundProPerty;
        public Color tForeground
        {
            get { return (Color)GetValue(tForegroundProPerty); }
            set
            {
                SetValue(tForegroundProPerty, value);
                t.Foreground = new SolidColorBrush(tForeground);
            }
        }
        public static DependencyProperty tTextProperty;
        public string tText
        {
            get { return (string)GetValue(tTextProperty); }
            set
            {
                SetValue(tTextProperty, value);
                t.Text = tText;
            }
        }

        public ColorButton()
        {
            InitializeComponent();
            if (tFontSizeProperty == null)
                tFontSizeProperty = DependencyProperty.Register(
                    "tFontSize",
                    typeof(int),
                    typeof(ColorButton),
                    new PropertyMetadata(20));
            if (tForegroundProPerty == null)
                tForegroundProPerty = DependencyProperty.Register(
                    "tForeground",
                    typeof(Color),
                    typeof(ColorButton),
                    new PropertyMetadata(Color.FromRgb(0, 0, 0)));
            if (tTextProperty == null)
                tTextProperty = DependencyProperty.Register(
                    "tText",
                    typeof(string),
                    typeof(ColorButton),//TextBox
                    new PropertyMetadata("12121212"));

            if (normalColor1Property == null)
                normalColor1Property = DependencyProperty.Register(
                    "normalColor1",
                    typeof(Color), 
                    typeof(ColorButton), 
                    new PropertyMetadata(Color.FromArgb(0xFF,0x18,0x65,0xE2)));
            if (normalColor2Property == null)
                normalColor2Property = DependencyProperty.Register(
                    "normalColor2",
                    typeof(Color),
                    typeof(ColorButton),
                    new PropertyMetadata(Color.FromArgb(0xFF, 0x02, 0xB2, 0xFE)));
            if (downColor1Property == null)
                downColor1Property = DependencyProperty.Register(
                    "downColor1",
                    typeof(Color),
                    typeof(ColorButton),
                    new PropertyMetadata(Color.FromArgb(0xFF, 0x18, 0x65, 0xE2)));
            if (downColor2Property == null)
                downColor2Property = DependencyProperty.Register(
                    "downColor2",
                    typeof(Color),
                    typeof(ColorButton),
                    new PropertyMetadata(Color.FromArgb(0xFF, 0x02, 0xB2, 0xFE)));
            Loaded += HadLoaded;
        }

        private void HadLoaded(object sender, RoutedEventArgs args)
        {
            int t = tFontSize;
            tFontSize = t;
            string text = tText;
            tText = text;
            Color color = tForeground;
            tForeground = color;
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            c1.Color = downColor1;
            c2.Color = downColor2;
        }

        private void Border_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            c1.Color = normalColor1;
            c2.Color = normalColor2;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            c1.Color = normalColor1;
            c2.Color = normalColor2;
        }
    }
}
