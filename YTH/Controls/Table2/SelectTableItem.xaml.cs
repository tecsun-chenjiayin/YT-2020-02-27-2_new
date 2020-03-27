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

namespace YTH.Controls.Table2
{
    /// <summary>
    /// SelectTableItem.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTableItem : UserControl
    {
        List<Border> bs = new List<Border>();
        Brush clickB = new SolidColorBrush(Color.FromArgb(0xFF,0xE7,0xE7,0xE7));//#FF E7 E7 E7
        Brush normalB = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
        Brush normal2 = new SolidColorBrush(Color.FromRgb(0xF9, 0xF9, 0xF9));//#F9F9F9

        List<TextBlock> tbs = new List<TextBlock>();
        int is2 = 1;

        public static DependencyProperty isCheckProperty = null;
        //是否可以选择
        public bool isCheck
        {
            get { return (bool)GetValue(isCheckProperty); }
            set { SetValue(isCheckProperty, value);
                //if (isCheck)
                //    check.Visibility = Visibility.Visible;
                //else
                //    check.Visibility = Visibility.Hidden;
            }
        }
        public static DependencyProperty itemDatasProperty = null;
        public string itemDatas
        {
            get { return (string)GetValue(itemDatasProperty); }
            set { SetValue(itemDatasProperty, value);
                if (itemDatas == null)
                    return;
                string[] datas = itemDatas.Split(',');
                for (int i = 0; i < tbs.Count && i < datas.Length; i++)
                    tbs[i].Text = datas[i];
            }
        }

        public static bool isMoving = false;

        bool isSelect = false;
        public bool IsSelect()
        {
            return isSelect;
        }

        public SelectTableItem()
        {
            InitializeComponent();
            bs.Add(b0);
            bs.Add(b1);
            bs.Add(b2);
            bs.Add(b3);
            bs.Add(b4);
            bs.Add(b5);
            bs.Add(b6);

            tbs.Add(t1);
            tbs.Add(t2);
            tbs.Add(t3);
            tbs.Add(t4);
            tbs.Add(t5);
            tbs.Add(t6);

            if(isCheckProperty == null)
                isCheckProperty = DependencyProperty.Register("isCheck", typeof(bool),
                typeof(SelectTableItem),
                new PropertyMetadata(false));
            if(itemDatasProperty == null)
            itemDatasProperty = DependencyProperty.Register("itemDatas", typeof(string), typeof(SelectTableItem), new PropertyMetadata("1,2,3,4,5,6"));
            isCheck = true;
        }
       

        public void setData(string[] datas, int is2)
        {
            this.is2 = is2;
            if (datas == null)
                return;
            for (int i = 0; i < tbs.Count && i < datas.Length; i++)
                tbs[i].Text = datas[i];
            if (is2 > 0 && is2 % 2 == 0)
                Background = normal2;
            else if (is2 > 0)
                Background = Brushes.Transparent;
        }

        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isCheck == false || isMoving)
                return;
            isSelect = check.click();
            Brush b = clickB;
            if (isSelect == false)
            {
                //b = normalB;
                if (is2 > 0 && is2 % 2 == 0)
                    b = normal2;
                else if (is2 > 0)
                    b = Brushes.Transparent;
                else
                    b = normalB;
            }
            foreach (Border bo in bs)
                bo.Background = b;

        }
    }
}
