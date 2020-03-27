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
using YTH.BuKa;
using YTH.Functions;
using YTH.Functions.MSDLL;
using YTH.Functions.ReadCarAndSQCode;
using YTH.GetCar.Self;

namespace YTH.ZhanJiang
{
    /// <summary>
    /// MakeCardProcess.xaml 的交互逻辑
    /// </summary>
    public partial class MakeCardProcess : UserControl
    {
        static MakeCardProcess self = null;
        public static MakeCardProcess GetObject()
        {
            if (self == null)
                self = new MakeCardProcess();
            return self;
        }

        static SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(0x03, 0xAB, 0xDC));//#FF1294FF
        int index = -1;
        ImageSource bluePoint = null;
        ImageSource grayPoint = null;
        ImageSource blueLine = null;
        ImageSource grayLine = null;
        List<Image> points = new List<Image>();
        List<Image> lines = new List<Image>();
        List<TextBlock> texts = new List<TextBlock>();
        List<TextBlock> times = new List<TextBlock>();
        List<TextBlock> scripts = new List<TextBlock>();
        string timeTag = "";
        public MakeCardProcess()
        {
            InitializeComponent();
            bluePoint = p1.Source;
            blueLine = L1.Source;
            grayPoint = p3.Source;
            grayLine = L3.Source;

            times.Add(m1);
            times.Add(m2);
            times.Add(m3);
            times.Add(m4);
            times.Add(m5);
            times.Add(m6);
            times.Add(m7);

            points.Add(p1);
            points.Add(p2);
            points.Add(p3);
            points.Add(p4);
            points.Add(p5);
            points.Add(p6);
            points.Add(p7);

            lines.Add(L1);
            lines.Add(L2);
            lines.Add(L3);
            lines.Add(L4);
            lines.Add(L5);
            lines.Add(L6);

            texts.Add(t1);
            texts.Add(t2);
            texts.Add(t3);
            texts.Add(t4);
            texts.Add(t5);
            texts.Add(t6);
            texts.Add(t7);

            scripts.Add(T1);
            scripts.Add(T2);
            scripts.Add(T3);
            scripts.Add(T4);
            scripts.Add(T5);
            scripts.Add(T6);
            scripts.Add(T7);
        }
        //1.入口/读身份证
        public void Goin()
        {
           

            //测试====
            //index = 3;
            //if (CD.business2 == null) CD.business2 = new Business2();
            //CD.setMainUI(CD.business2);
            //showResult();
            //return;


            B_ReadIDCar readIDCar = B_ReadIDCar.getObject();
            readIDCar.BeforeGoin(LoadData, true);
            readIDCar.Goin();
        }
        //2.加载数据
        async private void LoadData()
        {
            BackExit.setBack(LoadData);
            index = 0;
            
            timeTag = CD.timeTag.updateTag();
            if (CD.business2 == null) CD.business2 = new Business2();
            CD.setMainUI(CD.business2);
            CD.business2.start();

            Loading.show2("正在查询，请稍候...");
            tools.AnalyzeJson retJson = null;
            await TaskMore.Run(new Action(() => {
                MakeJson json = new MakeJson();
                json.add("fkcs", Config.dic("cityCode"));
                json.add("shbzh", ReadIDCar.persionid);
                json.add("xm", ReadIDCar.name);
                //ppid String  是 由肇庆市社保局提供
                //appkey String  是 由肇庆市社保局提供
                retJson = Post.Post_Json("getCardProgress", json.ToString());
            })).ConfigureAwait(true);
            if (retJson.error != null)
                ShowTip.show(false, BackExit.Exit, retJson.error);
            else
            {
                if(retJson["data"]["err"].ToString() != "OK")
                {
                    ShowTip.show(false, BackExit.Exit, retJson["data"]["err"].ToString());
                    return;
                }
                if (retJson["data"]["validtag"].ToString().Trim().IndexOf("-") == 0)
                {
                    error.Text = "制卡失败：" + retJson["data"]["remarks"].ToString();
                }
                else
                    error.Text = "";

                times[0].Text = handleTimeStr(retJson["data"]["applytime"].ToString());
                times[1].Text = handleTimeStr(retJson["data"]["banktime"].ToString());
                times[2].Text = handleTimeStr(retJson["data"]["kszkhpsj"].ToString());
                times[3].Text = handleTimeStr(retJson["data"]["provincetime"].ToString());
                times[4].Text = handleTimeStr(retJson["data"]["citytime"].ToString());
                times[5].Text = handleTimeStr(retJson["data"]["gettime"].ToString());
                times[6].Text = handleTimeStr(retJson["data"]["yhjhtime"].ToString());

                if(times[0].Text == "")
                {
                    CD.business2.setBusinessValue(MakeCardProcess2.GetObject());
                    return;
                }

                if (times[4].Text != "")
                    T52.Text = "(" + retJson["data"]["lkjgmc"].ToString() + ")";
                else
                    T52.Text = "";

                index = 0;
                for(int i = 0; i < times.Count; i++)
                {
                    if (times[i].Text == "")
                        break;
                    index++;
                }

                showResult();
            }
        }
        //3.显示结果
        private void showResult()
        {
            CD.business2.setBusinessValue(this);
            name.Text = ReadIDCar.name;
            persionid.Text = ReadIDCar.getSubCardID();
            int i;
            for (i = 0; i < index; i++)
            {
                points[i].Source = bluePoint;
                if(i < 6)
                    lines[i].Source = blueLine;
                texts[i].Foreground = blue;
                scripts[i].Visibility = Visibility.Visible;
                status.Text = texts[i].Text;
            }
            for(; i < points.Count; i++)
            {
                points[i].Source = grayPoint;
                if(i < lines.Count)
                    lines[i].Source = grayLine;
                texts[i].Foreground = Brushes.Black;
                scripts[i].Visibility = Visibility.Hidden;
            }
            if(index - 1 >= 0)
                status.Text = texts[index - 1].Text;
            if (index < 5)
                T52.Text = "";
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackExit.Back();
            CD.business2.stop();
        }

        private string handleTimeStr(string time)
        {
            //20151127163442
            if (time != null && time.IndexOf("-") == -1)
            {
                time = time.Trim();
                if (time != "" && time.Length >= 8)
                    return string.Format("{0}.{1}.{2}", time.Substring(0, 4), time.Substring(4, 2), time.Substring(6, 2));
            }
            return "";
        }
    }
}
