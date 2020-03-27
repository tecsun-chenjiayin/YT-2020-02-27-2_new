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
using System.Windows.Threading;

namespace YTH.Functions
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : UserControl
    {
        bool hasInit = false;
        static Loading load = null;
        public static void init()
        {
            load = new Loading();
        }

        public static void show1(string tip)
        {
            load.Dispatcher.BeginInvoke(DispatcherPriority.Normal,new Action(()=> {
                if (CD.business2 != null)
                    CD.business2.setBusinessValue(null);
                if (CD.business1 != null && CD.business1.Visibility == Visibility.Visible)
                    CD.business1.setBusinessValue(load);
                load.Show(tip); }));
        }
        public static void show2(string tip)
        {
            load.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                if (CD.business1 != null)
                    CD.business1.setBusinessValue(null);
                if (CD.business2 != null && CD.business2.Visibility == Visibility.Visible)
                    CD.business2.setBusinessValue(load);
                load.Show(tip);
            }));
        }
        public static void updateTip(string tip)
        {
            load.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                load.tipValue.Text = tip;
            }));
        }

        public Loading()
        {
            InitializeComponent();
        }

        public void Show(string tip)
        {
            if(!hasInit)
            {
                image.init(3, ImagePaths.getPathByName("加载"));
                hasInit = true;
            }
            tipValue.Text = tip;
            image.start();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(!((bool)e.NewValue))
            {
                image.stop();
            }
        }
    }
}
