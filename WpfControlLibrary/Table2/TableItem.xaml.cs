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

namespace WpfControlLibrary
{
    /// <summary>
    /// MTable1.xaml 的交互逻辑
    /// </summary>
    public partial class TabelItem : UserControl
    {
        List<TextBlock> tbs = new List<TextBlock>();
        List<Brush> colors = null;
        int fontsize = 20;
        bool isbord = false;
        int columnNum = 0;
        List<double> ratios = null;
        public TabelItem()
        {
            InitializeComponent();    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnNum">列数</param>
        /// <param name="colors">每列的颜色</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="isbord">是不是粗体</param>
        /// <param name="ratios">列宽比例</param>
        public void setProperty(int columnNum, List<Brush> colors, int fontsize, bool isbord, List<double> ratios, double height)
        {
            if (this.columnNum != columnNum)
            {
                this.columnNum = columnNum;
                grid.ColumnDefinitions.Clear();
                grid.Children.Clear();


                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
                for (int i = 0; i < columnNum; i++)
                {
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(ratios[i], GridUnitType.Star);
                    grid.ColumnDefinitions.Add(column);
                }
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
                for (int i = 1; i <= columnNum; i++)
                {
                    TextBlock tb = new TextBlock();
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    if (i == 1)
                        tb.HorizontalAlignment = HorizontalAlignment.Left;
                    else if (i == columnNum)
                        tb.HorizontalAlignment = HorizontalAlignment.Right;
                    else
                        tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Foreground = colors[i - 1];
                    tb.FontSize = fontsize;
                    if (isbord)
                        tb.FontWeight = FontWeights.Bold;

                    grid.Children.Add(tb);
                    Grid.SetColumn(tb, i);
                    tbs.Add(tb);
                }
            }
            else
            {
                for(int i = 0; i < ratios.Count; i++)
                {
                    if (ratios[i] != this.ratios[i])
                        grid.ColumnDefinitions[i].Width = new GridLength(ratios[i], GridUnitType.Star);
                    if (this.fontsize != fontsize)
                        tbs[i].FontSize = fontsize;
                    if(this.isbord != isbord)
                    {
                        if(isbord)
                            tbs[i].FontWeight = FontWeights.Bold;
                        else
                            tbs[i].FontWeight = FontWeights.Normal;
                    }      
                }
            }
            this.colors = colors;
            this.fontsize = fontsize;
            this.isbord = isbord;
            this.ratios = ratios;
            if (this.MinHeight != height)
                this.MinHeight = height;
        }


        public void setText(List<string> vals)
        {
            for (int i = 0; i < vals.Count && i < tbs.Count; i++)
                tbs[i].Text = vals[i];
        }


    }
}
