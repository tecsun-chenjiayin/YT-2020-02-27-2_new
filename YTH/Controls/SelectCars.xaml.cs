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

namespace YTH.LingKa
{
    /// <summary>
    /// SelectCars.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCars : UserControl
    {
        public SelectCars()
        {
            InitializeComponent();
            table.setTitle("序号,姓名,身份证号,待领卡类型,银行卡号,社保卡号");
            //List<string[]> datas = new List<string[]>();
            //datas.Add(new string[] { "1", "测试姓名", "441781199321458658", "社保卡", "622848******8426", "GZ123456789" });
            //datas.Add(new string[] { "1", "测试姓名", "441781199321458658", "社保卡", "622848******8426", "GZ123456789" });
            //datas.Add(new string[] { "1", "测试姓名", "441781199321458658", "社保卡", "622848******8426", "GZ123456789" });
            //datas.Add(new string[] { "1", "测试姓名", "441781199321458658", "社保卡", "622848******8426", "GZ123456789" });
            //datas.Add(new string[] { "1", "测试姓名", "441781199321458658", "社保卡", "622848******8426", "GZ123456789" });
            //table.setList(datas);
        }

        Action okk0 = null;
        public void setSelectOK(Action okk1)
        {
            okk0 = okk1;
        }
        public void setTable(List<string[]> datas)
        {
            table.setList(datas);
        }

        public List<int> getSelectItems()
        {
            return table.getSelectItems();
        }

        //private void ok_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    okk0();
        //}

        private void TXButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> datas = getSelectItems();
            if (datas == null || datas.Count == 0)
                return;
            CD.business1.hidenBackAndExitBtn();
            okk0();
        }
    }
}
