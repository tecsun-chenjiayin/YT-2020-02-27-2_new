﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlLibrary.KeyBoard
{
    /// <summary>
    /// KeyButton.xaml 的交互逻辑
    /// </summary>
    public partial class KeyButton : UserControl
    {
        public KeyButton()
        {
            InitializeComponent();
        }

        static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(KeyButton), new PropertyMetadata("A"));
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        public double fontSize { get; set; }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
        }
    }
}
