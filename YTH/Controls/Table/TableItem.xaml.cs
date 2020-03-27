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

namespace YTH.Controls.Table
{
    /// <summary>
    /// TableItem.xaml 的交互逻辑
    /// </summary>
    public partial class TableItem : UserControl
    {
        List<TextBlock> vs = new List<TextBlock>();
        public TableItem()
        {
            InitializeComponent();
            vs.Add(v1);
            vs.Add(v2);
            vs.Add(v3);
            vs.Add(v4);
        }

        public void setValues(bool isTopItem, string[] values)
        {
            if (isTopItem == false)
                grid.Background = Brushes.White;
            else
                Line.Visibility = Visibility.Collapsed;
            for (int i = 0; i < vs.Count && i < values.Length; i++)
                vs[i].Text = values[i];
        }
     
    }
}
