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

namespace YTH.BuKa
{
    /// <summary>
    /// B_ReadIDCar.xaml 的交互逻辑
    /// </summary>
    public partial class B_ReadIDCar : UserControl
    {
        public B_ReadIDCar()
        {
            InitializeComponent();
        }
        string timeTag = null;
        Action nextStep = null;
        static B_ReadIDCar b_ReadIDCar = null;
        public static B_ReadIDCar getObject()
        {
            if (b_ReadIDCar == null)
                b_ReadIDCar = new B_ReadIDCar();
            return b_ReadIDCar;
        }

        public void BeforeGoin(Action action, bool canInput = false)
        {
            nextStep = action;
            if (!canInput)
                input.Visibility = Visibility.Hidden;
            else
                input.Visibility = Visibility.Visible;
        }
        public void Goin()
        {
            //BackExit.setBack(Goin);
            timeTag = CD.timeTag.updateTag();
            CD.setTopUI(this);
            time.start();
            ReadIDCar.readCar(nextStep, this);
            //KeyPad.startInput(input, Delete, clear, OK, Back_, this);
        }

        private void NextSetp()
        {
            if (timeTag == CD.timeTag.getTag())
                nextStep();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CD.timeTag.updateTag();
            CD.setTopUI(null);
        }

        private void input_Click(object sender, RoutedEventArgs e)
        {
            InputPersionMsg.Goin(nextStep);
        }
    }
}
