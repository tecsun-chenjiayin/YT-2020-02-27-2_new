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
using YTH.Functions.MSDLL;

namespace YTH.GetCar
{
    /// <summary>
    /// I_GetSSCar.xaml 的交互逻辑
    /// </summary>
    public partial class I_GetSSCar_old : UserControl
    {
        static I_GetSSCar_old obj = null;
        public static I_GetSSCar_old getObject()
        {
            if (obj == null)
                obj = new I_GetSSCar_old();
            return obj;
        }

        string timeTag = "";
        public I_GetSSCar_old()
        {
            InitializeComponent();

            if (checkTP == null)
                checkTP = new ThreadProperty(200, false, false, checkHasCarIn, null);
            checkTP.start();
        }

        ThreadProperty checkTP = null;
        private void checkHasCarIn()
        {
            if (Visibility != Visibility.Visible)
            {
                checkTP.stop();
                return;
            }
            bool hasCarIn = MS2.IfHasCarIn();
            if (Visibility != Visibility.Visible)
            {
                checkTP.stop();
                return;
            }
            if (hasCarIn == false)
            {
                if(nextStep == null)
                    TH.addOnceUI(BackExit.Exit);
                else
                    TH.addOnceUI(nextStep);
                checkTP.stop();
            }
        }

        public void Goin()
        {
            nextStep = null;
            MS2.Open();
            timeTag = CD.timeTag.getTag();
            BackExit.LetNextClickToMain();
            CD.business1.setBusinessValue(this);
            checkTP.start();
        }

        Action nextStep = null;
        public void Goin(Action action)
        {
            nextStep = action;
            MS2.Open();
            timeTag = CD.timeTag.getTag();
            CD.business1.setBusinessValue(this);
            checkTP.start();
        }
    }
}
