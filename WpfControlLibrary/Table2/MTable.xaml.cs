using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace WpfControlLibrary
{
    /// <summary>
    /// MTable.xaml 的交互逻辑
    /// </summary>
    public partial class MTable : UserControl
    {
        List<double> ratios = new List<double>();
        bool isDown = false;
        double y = 0;
        POINT p = new POINT();
        POINT pD = new POINT();
        List<TabelItem> items = new List<TabelItem>();
        int itemIndex = 0;
        public MTable()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
        }

        public void clear()
        {
            foreach (TabelItem item in items)
                item.Visibility = Visibility.Collapsed;
            itemIndex = 0;
        }

        TabelItem topItem = null;
        public void setTopItem(List<string> vals, List<double> ratios, int fontsize, Brush color, bool isBord)
        {
            try
            {
                List<Brush> colors = new List<Brush>();
                for (int i = 0; i < vals.Count; i++)
                    colors.Add(color);
                if (topItem == null)
                {
                    topItem = new TabelItem();
                }
                topItem.setProperty(vals.Count, colors, fontsize, isBord, ratios, 0);
                topItem.setText(vals);
                TopItem.Child = topItem;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void addTableItem(List<string> vals, List<double> ratios, int fontsize, List<Brush> colors, bool isbord)
        {
            try
            {
                if (items.Count < itemIndex + 1)
                {
                    TabelItem item = new TabelItem();
                    item.setProperty(vals.Count, colors, fontsize, isbord, ratios, 0);
                    item.setText(vals);
                    item.Margin = new Thickness(0, 5, 0, 10);
                    table.Children.Add(item);
                    items.Add(item);
                }
                else
                {
                    items[itemIndex].setText(vals);
                    items[itemIndex].Visibility = Visibility.Visible;
                }
                itemIndex++;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        public void updateScrollBar()
        {
            tools.TaskMore.Run(new Action(() =>
            {
                tools.TaskMore.Delay(1000);
                Dispatcher.Invoke(new Action(() =>
                {
                    if (double.IsNaN(table.ActualHeight))
                    {
                        //canvas.Visibility = Visibility.Hidden;
                        return;
                    }
                    double height = sv.ActualHeight / table.ActualHeight;

                    canvas.Visibility = Visibility.Visible;
                    height = height * sv.ActualHeight;
                    re.Height = height;
                    Canvas.SetTop(re, 0.0);
                }));
            }));
            sv.ScrollToTop();
        }

        private double MeasureTextWidth(string text, double fontSize, string fontFamily)
        {
            FormattedText formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface(fontFamily.ToString()),
            fontSize,
            Brushes.Black
            );

            return formattedText.WidthIncludingTrailingWhitespace;
        }

        private void ScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)e.Source;
            isDown = true;
            GetCursorPos(out p);
            pD = p;
        }

        private void ScrollViewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
            POINT p3 = new POINT();
            GetCursorPos(out p3);

            int my = System.Math.Abs(p3.Y - pD.Y);
            int mx = System.Math.Abs(p3.X - pD.X);
            if (my > 2 || mx > 2)
                return;
        }

        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                double m = 0;
                POINT p2 = new POINT();
                GetCursorPos(out p2);
                m = p2.Y - p.Y;
                p = p2;
                y -= m;
                if (y <= 0)
                    y = 0;
                else if (y >= sv.ScrollableHeight)
                    y = sv.ScrollableHeight;
                sv.ScrollToVerticalOffset(y);
                re.SetValue(Canvas.TopProperty, (y / sv.ScrollableHeight) * (canvas.ActualHeight - re.ActualHeight));
            }
        }

        /// <summary>   
        /// 设置鼠标的坐标   
        /// </summary>   
        /// <param name="x">横坐标</param>   
        /// <param name="y">纵坐标</param>   
        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        /// <summary>   
        /// 获取鼠标的坐标   
        /// </summary>   
        /// <param name="lpPoint">传址参数，坐标point类型</param>   
        /// <returns>获取成功返回真</returns>   
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            sv.ScrollToVerticalOffset(0);
            y = 0;
        }
    }
}
