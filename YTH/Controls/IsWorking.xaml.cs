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
using System.Windows.Shapes;

namespace YTH.Controls
{
    /// <summary>
    /// IsWorking.xaml 的交互逻辑
    /// </summary>
    public partial class IsWorking : Window
    {
        static IsWorking iw = null;
        public IsWorking()
        {
            InitializeComponent();
            image.init(3, Functions.ImagePaths.getPathByName("白色加载"));
            Topmost = true;
        }

        public static void show()
        {
            if (iw == null)
                iw = new IsWorking();
            iw.Show();
            iw.image.start();
        }

        public static void hiden()
        {
            if (iw == null)
                iw = new IsWorking();
            iw.Hide();
            iw.image.stop();
        }
    }
}
