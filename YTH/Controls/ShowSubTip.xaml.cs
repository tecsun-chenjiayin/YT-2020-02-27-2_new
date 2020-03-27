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

namespace YTH.Controls
{
    public enum TIPSTYLE
    {
        NoCar = 0,
        PutCarOutFailed = 1,
        MachineError = 2
    }

    /// <summary>
    /// ShowSubTip.xaml 的交互逻辑
    /// </summary>
    public partial class ShowSubTip : UserControl
    {
        static ShowSubTip sst = null;
        static BitmapImage NoCar = null;
        static BitmapImage PutCarOutFailed = null;
        static BitmapImage MachineError = null;
        public ShowSubTip()
        {
            InitializeComponent();
            NoCar = new BitmapImage(new Uri(@"../Soruce/Images/暂无.png", UriKind.Relative));
            PutCarOutFailed = new BitmapImage(new Uri(@"../Soruce/Images/出卡失败.png", UriKind.Relative));
            MachineError = new BitmapImage(new Uri(@"../Soruce/Images/设备故障.png", UriKind.Relative));
        }

        public static void Show(Business parent, TIPSTYLE tIPSTYLE, string txt)
        {
            TH.addOnceUI(new Action(() =>
            {
                if (sst == null)
                    sst = new ShowSubTip();
                if (tIPSTYLE == TIPSTYLE.MachineError)
                    sst.tipStyle.Source = MachineError;
                else if (tIPSTYLE == TIPSTYLE.NoCar)
                    sst.tipStyle.Source = NoCar;
                else
                    sst.tipStyle.Source = PutCarOutFailed;
                sst.error.Text = txt;

                if (tIPSTYLE != TIPSTYLE.MachineError)
                {
                    parent.showBackAndExitBtn();
                    CD.countDownTime.stop();
                }
                parent.setBusinessValue(sst);
            }));
        }

        public static void Show(Business2 parent, TIPSTYLE tIPSTYLE, string txt)
        {
            TH.addOnceUI(new Action(() =>
            {
                if (sst == null)
                    sst = new ShowSubTip();
                if (tIPSTYLE == TIPSTYLE.MachineError)
                    sst.tipStyle.Source = MachineError;
                else if (tIPSTYLE == TIPSTYLE.NoCar)
                    sst.tipStyle.Source = NoCar;
                else
                    sst.tipStyle.Source = PutCarOutFailed;
                sst.error.Text = txt;

                if (tIPSTYLE != TIPSTYLE.MachineError)
                {
                    parent.showBackExit();
                    CD.countDownTime.stop();
                }
                    
                parent.setBusinessValue(sst);
            }));
        }

        public static void ShowMachineError1()
        {
            TH.addOnceUI(new Action(() =>
            {
                if (CD.business1 == null)
                    return;
                CD.setMainUI(CD.business1);
                CD.countDownTime.stop();
                CD.business1.setTitle("异常");
                CD.business1.hidenBackAndExitBtn();
                Show(CD.business1, TIPSTYLE.MachineError, "设备故障，请联系管理员处理");
            }));
        }

        public static void ShowMachineError2()
        {
            TH.addOnceUI(new Action(() =>
            {
                if (CD.business2 == null)
                    CD.business2 = new Business2();
                CD.countDownTime.stop();
                CD.setMainUI(CD.business2);
                CD.business2.setTitle("异常");
                CD.business2.hidenBackExit();
                Show(CD.business2, TIPSTYLE.MachineError, "设备故障，请联系管理员处理");
            }));
        }
    }
}
