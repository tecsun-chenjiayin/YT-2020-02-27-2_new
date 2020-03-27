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

namespace YTH.BuKa
{
    /// <summary>
    /// SelectCard.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCard : UserControl
    {
        public static bool isSelectIDCard = true;

        public SelectCard()
        {
            InitializeComponent();
        }

        static SelectCard self = null;
        public static SelectCard GetObject()
        {
            if (self == null)
                self = new SelectCard();
            return self;
        }

        Action nextStep = null;
        public void BeforeGoin(Action action)
        {
            nextStep = action;
        }
        public void Goin()
        {
            //BackExit.setBack(Goin);
            CD.setTopUI(this);
            if(B_ReadSSCard.persionid != "" && B_ReadSSCard.persionid != null)
            {
                isSelectIDCard = false;
                nextStep();
                return;
            }
            time.start();
        }

        private void Idcard_Click(object sender, RoutedEventArgs e)
        {
            isSelectIDCard = true;
            B_ReadIDCar readIDCar = B_ReadIDCar.getObject();
            readIDCar.BeforeGoin(nextStep);
            readIDCar.Goin();
        }

        private void Sscard_Click(object sender, RoutedEventArgs e)
        {
            isSelectIDCard = false;
            B_ReadSSCard.GetObject().BeforeGoin(nextStep);
            B_ReadSSCard.GetObject().Goin();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CD.setTopUI(null);
        }
    }
}
