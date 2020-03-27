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
using YTH.Controls.Table;

namespace YTH.Controls
{
    /// <summary>
    /// StatisitcsTable.xaml 的交互逻辑
    /// </summary>
    public partial class StatisitcsTable : UserControl
    {
        List<TableItem> items = new List<TableItem>();
        int LineNum = 11;
        public StatisitcsTable()
        {
            InitializeComponent();
            for (int i = 0; i < 12; i++)
            {
                TableItem item = new TableItem();
                items.Add(item);
                itemList.Children.Add(item);
                if (i != 0)
                    item.Visibility = Visibility.Hidden;
            }  
        }

        public void setTopItem(string[] itemsName)
        {
            items[0].setValues(true, itemsName);
        }

        int nowPageIndex = 1;
        int maxPageIndex = 1;
        List<string[]> showVals = new List<string[]>();
        public void setTableValues(List<string[]> vals)
        {
            showVals = vals;
            if (showVals == null)
                showVals = new List<string[]>();
            maxPageIndex = vals.Count / (items.Count - 1) + (vals.Count % (items.Count - 1) == 0 ? 0 : 1);
            nowPageIndex = 1;
            show();
        }

        public void clearValues()
        {
            setTableValues(new List<string[]>());
        }

        private void firstPage_Click(object sender, RoutedEventArgs e)
        {
            nowPageIndex = 1;
            show();
        }

        private void lastPate_Click(object sender, RoutedEventArgs e)
        {
            if (nowPageIndex == 0) return;
            nowPageIndex--;
            show();
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            if (nowPageIndex >= maxPageIndex) return;
            nowPageIndex++;
            show();
        }

        private void endPage_Click(object sender, RoutedEventArgs e)
        {
            nowPageIndex = maxPageIndex;
            show();
        }

        private void show()
        {
            int i = 1;
            for (; i < items.Count && (i - 1) < showVals.Count; i++)
            {
                items[i].setValues(false, showVals[(nowPageIndex -1) * LineNum + i - 1]);
                items[i].Visibility = Visibility.Visible;
            }
            for (; i < items.Count; i++)
                items[i].Visibility = Visibility.Hidden;
        }
    }
}
