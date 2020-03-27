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
using YTH.Functions;
using YTH.Functions.ReadCarAndSQCode;
namespace YTH
{
    /// <summary>
    /// Business2.xaml 的交互逻辑
    /// </summary>
    public partial class Business2 : UserControl
    {
        public Business2()
        {
            InitializeComponent();
        }

        private void returnToMain_Click(object sender, RoutedEventArgs e)
        {
            BackExit.Exit();
            KeyPad.stop();
            ReadIDCar.stop();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            BackExit.Back();
            ReadIDCar.stop();
            KeyPad.stop();
        }
       
        public void setBusinessValue(UIElement ui)
        {
            bussinessArea.Child = ui;
        }

        public void setTitle(string name)
        {
            title.Text = name;
        }

        public void hidenBackExit()
        {
            exit.Visibility = Visibility.Hidden;
        }
        public void showBackExit()
        {
            exit.Visibility = Visibility.Visible;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
                setBusinessValue(null);
        }

        public void start()
        {
            time.start();
        }
        public void stop()
        {
            time.stop();
        }

        public static void Init(string title)
        {
            if (CD.business2 == null) CD.business2 = new Business2();
            CD.setMainUI(CD.business2);
            CD.business2.start();
            CD.business2.setTitle(title);
        }
    }
}
