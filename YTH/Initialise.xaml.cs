using System;
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

namespace YTH
{
    /// <summary>
    /// Initialise.xaml 的交互逻辑
    /// </summary>
    public partial class Initialise : UserControl
    {
        public Initialise()
        {
            InitializeComponent();
        }

        Action initial_nextStep = null;
        public void initial(Action nextStep)
        {
            initial_nextStep = nextStep;
            new Task(new Action(() =>
            {
                CheckSelf.checkSelf();
                Functions.TH.addOnceUI(initial_nextStep);
            })).Start(); ;
        }
    }
}
