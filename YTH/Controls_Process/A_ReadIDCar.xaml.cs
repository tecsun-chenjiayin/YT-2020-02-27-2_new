using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YTH.BuKa;
using YTH.Controls;
using YTH.Functions;
using YTH.Functions.ReadCarAndSQCode;

namespace YTH.GetCar.Self
{
    /// <summary>
    /// A_ReadIDCar.xaml 的交互逻辑  YTH.GetCar.Self.Self
    /// </summary>
    public partial class A_ReadIDCar : UserControl
    {
        Action nextStep = null;
        string timeTag = "";
        static A_ReadIDCar obj = null;
        public static A_ReadIDCar getObject()
        {
            if (obj == null)
                obj = new A_ReadIDCar();
            return obj;
        }


        public A_ReadIDCar()
        {
            InitializeComponent();
           // readCarGif.setBitmap(new System.Drawing.Bitmap(ImagePaths.getPathByName("读取身份证")), 724,482);
        }
        //0.入口
        public void Goin(Action nextStep)
        {
            timeTag = CD.timeTag.getTag();
            str.Clear();
            //inputTB.Text = "";
            //tip.Visibility = Visibility.Visible;
            this.nextStep = nextStep;   
            ReadIDCar.readCar(afterReadCar, this);
        }
        //2.读卡操作结束后显示提示取回身份证信息
        private void afterReadCar()
        {
            if (CD.timeTag.equal(timeTag) == false)
                return;
            //ReadIDCar.persionid = "450422198502152718";
            if (ReadIDCar.error == null)
            {
                //List<Dictionary<string, string>> zkData = null;
                //string error = null;
                //zkData = WeiWang.getAZ03(ReadIDCar.persionid, ReadIDCar.name, out error);
                //if (error != null)
                //{
                //    ShowTip.show(false, BackExit.Exit, error);
                //    return;
                //}
                //if (zkData[0]["ERR"] == "OK")
                //{
                //    ShowTip.show(false, BackExit.Exit, "已经申领过制卡，不能重复申领！");
                //    return;
                //}
                TipWinB1.showTip("信息读取成功，请取回您的身份证", 3000, nextStep);
            }
              
            else if (CD.timeTag.equal(timeTag))
                TipWin.showTip(ReadIDCar.error, 5000, BackExit.Exit);
        }
        //其他-点击返回或强制关闭窗口的时候执行
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if (!((bool)e.NewValue))
            //{
            //    readCarGif.StopAnimate();
            //}
        }

        private void inputTB_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        StringBuilder str = new StringBuilder();

      
    //    private void WrapPanel_Click(object sender, RoutedEventArgs e)
    //    {
    //        string val = inputTB.Text;
    //        FButton btn = (FButton)e.Source;
    //        string tag = btn.Tag.ToString();
    //        if (tag == "Num")
    //        {
    //            if (val.Length < 18)
    //                str.Append(btn.Content.ToString());
    //        }
    //        else if (tag == "Ok")
    //        {
    //            if (inputTB.Text.Length != 18)
    //            {
    //                TipWinB1.showTip("请输入18位的身份证号！", 3000, null);
    //                return;
    //            }
    //            ReadIDCar.persionid = inputTB.Text;
    //            //增加制卡进度查询

    //            //List<Dictionary<string, string>> zkData = null;
    //            //string error = null;
    //            //    zkData = WeiWang.getAZ03(ReadIDCar.persionid, ReadIDCar.name, out error);
    //            //if (error != null)
    //            //{
    //            //    ShowTip.show(false, BackExit.Exit, error);
    //            //    return;
    //            //}
    //            //if (zkData[0]["ERR"] == "OK")
    //            //{
    //            //    ShowTip.show(false, BackExit.Exit,"已经申领过制卡，不能重复申领！");
    //            //    return;
    //            //}

    //            inputTB.Text = "";
    //            tip.Visibility = Visibility.Visible;
    //            if (nextStep != null)
    //                nextStep();
    //            return;
    //        }
    //        else if (tag == "Delete")
    //        {
    //            if (val.Length > 0)
    //                str.Remove(val.Length - 1, 1);
    //        }
    //        inputTB.Text = str.ToString();

    //        if (inputTB.Text != "")
    //            tip.Visibility = Visibility.Hidden;
    //        else
    //            tip.Visibility = Visibility.Visible;
    //    }
    }
}
