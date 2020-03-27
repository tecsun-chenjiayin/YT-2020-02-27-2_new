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

namespace YTH
{
    /// <summary>
    /// NodeGraph.xaml 的交互逻辑
    /// </summary>
    public partial class NodeGraph : UserControl
    {
        List<POINT> points = new List<POINT>();
        string[] names = null;
        public NodeGraph()
        {
            InitializeComponent();
            for(int i = 0; i < 9; i++)
            {
                points.Add(new POINT(area));
            }
        }

        public void setPointNames(string[] names)
        {
            if(names == null || names.Length == 0)
            {
                foreach (POINT p in points)
                    p.hiden();
                return;
            }
            this.names = names;
            int i = 0;
            for (; i < names.Length && i < points.Count; i++)
            {
                points[i].show(names[i]);
                points[i].setStatus(1);
            }
            points[i - 1].setAsLastOne();
            for (; i < points.Count; i++)
                points[i].hiden();
        }

        public void setIndex(int i)
        {
            if (names == null || names.Length == 0) return;
            points[i - 1].setStatus(2);
            for (int k = 0; k < names.Length && k < (i - 1); k++)
                points[k].setStatus(3);
            for (int k = i; k < names.Length; k++)
                points[k].setStatus(1);
            for(int k = 0; k < i - 1; k++)
            {
                points[k].line.Source = POINT.lineImage2;
            }
            for(int k = i - 1; k < names.Length; k++)
            {
                points[k].line.Source = POINT.lineImage;
            }
        }
    }

    class POINT
    {
        StackPanel sp = null;
        TextBlock pointName = null;
        Image p = null;
        static SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(0x03, 0xAB, 0xDC));
        public Image line = null;

        static BitmapImage pointImage = new BitmapImage(new Uri(@"Soruce/Inages_ZQ/灰色.png", UriKind.Relative));
        public static BitmapImage lineImage = new BitmapImage(new Uri(@"Soruce/Inages_ZQ/灰色线.png", UriKind.Relative));
        public static BitmapImage lineImage2 = new BitmapImage(new Uri(@"Soruce/Inages_ZQ/蓝色线.png", UriKind.Relative));
        static BitmapImage normal = new BitmapImage(new Uri(@"Soruce/Inages_ZQ/灰色.png", UriKind.Relative));
        static BitmapImage ing = new BitmapImage(new Uri(@"Soruce/Inages_ZQ/蓝色.png", UriKind.Relative));
        static BitmapImage finish = new BitmapImage(new Uri(@"Soruce/Inages_ZQ/蓝色.png", UriKind.Relative));
        public POINT(StackPanel parent)
        {
            sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.HorizontalAlignment = HorizontalAlignment.Right;

            pointName = new TextBlock();
            pointName.Margin = new Thickness(0, 0, 40, 0);
            pointName.VerticalAlignment = VerticalAlignment.Center;
            pointName.FontSize = 27;

            p = new Image();
            p.Source = normal;
            p.Stretch = Stretch.UniformToFill;
            //p.FlowDirection = FlowDirection.LeftToRight;
            p.Width = 35;
            p.Height = 35;

            sp.Children.Add(pointName);
            sp.Children.Add(p);

            line = new Image();
            line.Source = lineImage;
            line.Stretch = Stretch.None;
            line.HorizontalAlignment = HorizontalAlignment.Right;
            line.Margin = new Thickness(0, 5, 17, 5);
            parent.Children.Add(sp);
            parent.Children.Add(line);
        }
        
        public void hiden()
        {
            sp.Visibility = Visibility.Collapsed;
            line.Visibility = Visibility.Collapsed;
        }

        public void show(string pName)
        {
            pointName.Text = pName;
            sp.Visibility = Visibility.Visible;
            line.Visibility = Visibility.Visible;
        }

        public void setStatus(int style)
        {
            //if(isGray)
            //    p.FlowDirection = FlowDirection.LeftToRight;
            //else
            //    p.FlowDirection = FlowDirection.RightToLeft;
            if (style == 1) { 
                p.Source = normal;
                pointName.Foreground = Brushes.Black;
            }
            else {
                pointName.Foreground = blue;
                p.Source = ing;
            }
        }

        public void setAsLastOne()
        {
            line.Visibility = Visibility.Collapsed;
        }
    }
}
