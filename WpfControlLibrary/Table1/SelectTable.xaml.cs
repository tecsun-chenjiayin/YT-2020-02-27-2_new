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
    /// SelectTable.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTable : UserControl
    {
        bool isDown = false;
        double y = 0;
        POINT p = new POINT();
        POINT pD = new POINT();
        public SelectTable()
        {
            InitializeComponent();
            
        }

        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (list.Children.Count > 3)
            //    title.Margin = new Thickness(0, 0, 16, 0);
            //else
            //    title.Margin = new Thickness(0);
        }

        public void setList(List<string[]> datas)
        {
            list.Children.Clear();
            int i = 1;
            foreach(string[] d in datas)
            {
                SelectTableItem sti = new SelectTableItem();
                sti.setData(d, i);
                list.Children.Add(sti);
                i++;
            }
        }
        public List<int> getSelectIndex()
        {
            List<int> selectIndex = new List<int>();
            int i = 0;
            foreach(UIElement ui in list.Children)
            {
                SelectTableItem st = ui as SelectTableItem;
                if (st.IsSelect())
                    selectIndex.Add(i);
                i++;
            }
            return selectIndex;
        }
        public void setTitle(string ds)
        {
            string[] datas = ds.Split(',');
            title.setData(datas, 0);
            title.isCheck = false;
        }

        private void ScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)e.Source;
            isDown = true;
            GetCursorPos(out p);
            pD = p;
            SelectTableItem.isMoving = false;
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
                SelectTableItem.isMoving = true ;
            }
        }

        public List<int> getSelectItems()
        {
            List<int> rets = new List<int>();
            int i = 0;
            foreach(SelectTableItem st in list.Children)
            {
                if (st.IsSelect())
                    rets.Add(i);
                i++;
            }
            return rets;
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
