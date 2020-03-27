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

namespace AnimationTest1
{
    /// <summary>
    /// SelectColor.xaml 的交互逻辑
    /// </summary>
    public partial class SelectColor : UserControl
    {
        public SelectColor()
        {
            InitializeComponent();
        }

        bool hasDown = false;
        bool hasStart = false;
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            hasStart = true;
            hasDown = true;
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            string data = string.Format("M {0},{1} A 3,3 10 1,0 {2},{1} Z", p.X, p.Y + 3, p.X - 0.01);
            select1.Data = Geometry.Parse(data);

            var pos = e.GetPosition(null);
            Point pos1 = this.PointToScreen(pos);
            Color c1 = GetPixelColor(pos1);
            see.Background = new SolidColorBrush(c1);
            seeText.Text = string.Format("#FF{0}{1}{2}", c1.R.ToString("X2"), c1.G.ToString("X2"), c1.B.ToString("X2"));
        }

        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            hasDown = false;
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(hasDown)
            {
                Point p = Mouse.GetPosition(e.Source as FrameworkElement);
                string data = string.Format("M {0},{1} A 3,3 10 1,0 {2},{1} Z", p.X, p.Y + 3, p.X - 0.01);
                select1.Data = Geometry.Parse(data);

                var pos = e.GetPosition(null);
                Point pos1 = this.PointToScreen(pos);
                Color c1 = GetPixelColor(pos1);
                see.Background = new SolidColorBrush(c1);
                seeText.Text = string.Format("#FF{0}{1}{2}", c1.R.ToString("X2"), c1.G.ToString("X2"), c1.B.ToString("X2"));
            }
        }

        /*
             （0 ，y-5），（0 ，y+5），（5 ，y）
             （20，y-5），（20，y+5），（15，y）
        */
        bool hasDown2 = false;
        private void grid2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            hasDown2 = true;
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            double y = p.Y;
            string data1 = string.Format("M0,{0} L0,{1} L5,{2} Z", y - 5, y + 5, y);
            string data2 = string.Format("M20,{0} L20,{1} L15,{2} Z", y - 5, y + 5, y);
            select21.Data = Geometry.Parse(data1);
            select22.Data = Geometry.Parse(data2);

            var pos = e.GetPosition(null);
            Point pos1 = this.PointToScreen(pos);
            Color c1 = GetPixelColor(pos1);
            Point pos2 = new Point(pos1.X - 5, pos1.Y);
            Color c2 = GetPixelColor(pos2);
            Point pos3 = new Point(pos1.X + 5, pos1.Y);
            Color c3 = GetPixelColor(pos3);

            List<Color> cs = new List<Color>();
            if (!(c1.R == 0 && c1.G == 0 && c1.B == 0))
                cs.Add(c1);
            if (!(c2.R == 0 && c2.G == 0 && c2.B == 0))
                cs.Add(c2);
            if (!(c3.R == 0 && c3.G == 0 && c3.B == 0))
                cs.Add(c3);
            Color c4 = cs[0];
            foreach (Color c in cs)
            {
                if (c4.R > 0 && c4.G > 0 && c4.B > 0)
                    c4 = c;
                if (c.R == 255 && c.G == 255 && c.B == 255)
                    continue;
                if ((c4.R < c.R || c4.G < c.G || c4.B < c.B) && !(c.R > 0 && c.G > 0 && c.B > 0))
                    c4 = c;
            }
            if (c4.R != 0 && c4.G != 0 && c4.B != 0)
                c4.G = c4.B = 0;
            gs.Color = c4;
        }


        private void Container_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void grid2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            hasDown2 = false;
        }

        private void grid2_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (hasDown2)
            {
                Point p = Mouse.GetPosition(e.Source as FrameworkElement);
                double y = p.Y;
                string data1 = string.Format("M0,{0} L0,{1} L5,{2} Z", y - 5, y + 5, y);
                string data2 = string.Format("M20,{0} L20,{1} L15,{2} Z", y - 5, y + 5, y);
                select21.Data = Geometry.Parse(data1);
                select22.Data = Geometry.Parse(data2);

                var pos = e.GetPosition(null);
                Point pos1 = this.PointToScreen(pos);
                Color c1 = GetPixelColor(pos1);
                Point pos2 = new Point(pos1.X - 5, pos1.Y);
                Color c2 = GetPixelColor(pos2);
                Point pos3 = new Point(pos1.X + 5, pos1.Y);
                Color c3 = GetPixelColor(pos3);

                List<Color> cs = new List<Color>();
                if (!(c1.R == 0 && c1.G == 0 && c1.B == 0))
                    cs.Add(c1);
                if (!(c2.R == 0 && c2.G == 0 && c2.B == 0))
                    cs.Add(c2);
                if (!(c3.R == 0 && c3.G == 0 && c3.B == 0))
                    cs.Add(c3);
                Color c4 = cs[0];
                foreach(Color c in cs)
                {
                    if (c4.R > 0 && c4.G > 0 && c4.B > 0)
                        c4 = c;
                    if (c.R == 255 && c.G == 255 && c.B == 255)
                        continue;
                    if ((c4.R < c.R || c4.G < c.G || c4.B < c.B) && !(c.R > 0 && c.G > 0 && c.B > 0))
                        c4 = c;
                }
                if (c4.R != 0 && c4.G != 0 && c4.B != 0)
                    c4.G = c4.B = 0;
                gs.Color = c4;
                
            }
        }

        Point getBasePoint()
        {
            Point ptRightDown = new Point(this.ActualWidth, this.ActualHeight);
            return this.PointToScreen(ptRightDown);
        }














        [DllImport("gdi32")]
        private static extern int GetPixel(int hdc, int nXPos, int nYPos);
        [DllImport("user32")]
        private static extern int GetWindowDC(int hwnd);
        [DllImport("user32")]
        private static extern int ReleaseDC(int hWnd, int hDC);

        private Color GetPixelColor(Point point)
        {

            int lDC = GetWindowDC(0);
            int intColor = GetPixel(lDC, (int)point.X, (int)point.Y);
            ReleaseDC(0, lDC);
            byte b = (byte)((intColor >> 0x10) & 0xffL);
            byte g = (byte)((intColor >> 8) & 0xffL);
            byte r = (byte)(intColor & 0xffL);
            Color color = Color.FromRgb(r, g, b);
            //return new SolidColorBrush(color);
            return color;
        }

        public byte b;

        public static double MousePositionX
        { get; set; }

        public static double MousePositionY
        {
            get;
            set;
        }
    }
}





