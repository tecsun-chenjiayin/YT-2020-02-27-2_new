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

namespace YTH.ManagementCar
{
    /// <summary>
    /// StopAddCar.xaml 的交互逻辑
    /// </summary>
    public partial class StopAddCar : UserControl
    {
        static BitmapImage successICO = null;
        static BitmapImage failedICO = null;
        static StopAddCar sac = null;
        public StopAddCar()
        {
            InitializeComponent();
        }

        //public static void show(int successNum, int failedNum, int yzk, int sbk, int jjk, int xyk, bool haveError, StopStyle ss, string describe)
        //{
        //    TH.addOnceUI(new Action(() => {
        //        if(successICO == null)
        //        {
        //            successICO = new BitmapImage(new Uri(@"../Soruce/Images/成功.png", UriKind.Relative));
        //            failedICO = new BitmapImage(new Uri(@"../Soruce/Images/失败.png", UriKind.Relative));
        //        }
        //        if (sac == null)
        //            sac = new StopAddCar();
        //        sac.success.Text = successNum.ToString() + "张";
        //        sac.failed.Text = failedNum.ToString() + "张";
        //        sac.yzk.Text = yzk.ToString() + "张";
        //        sac.sbk.Text = sbk.ToString() + "张";
        //        sac.jjk.Text = jjk.ToString() + "张";
        //        sac.xyk.Text = xyk.ToString() + "张";
        //        if (haveError)
        //            sac.ico.Source = failedICO;
        //        else
        //            sac.ico.Source = successICO;
        //        sac.describe.Text = describe;
        //        CD.business2.setBusinessValue(sac);
        //        if(ss != StopStyle.MachineError)
        //        {
        //            CD.business2.showBackExit();
        //            Status.isWorking = false;
        //        }
                    
        //    }));
        //}
    }
}
