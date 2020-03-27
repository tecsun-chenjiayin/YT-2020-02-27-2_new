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
using YTH.Functions.ReadCarAndSQCode;
using YTH.GetCar.Self;

namespace YTH.ZhanJiang
{
    /// <summary>
    /// SearchData.xaml 的交互逻辑
    /// </summary>
    public partial class SearchList : UserControl
    {
        public SearchList()
        {
            InitializeComponent();
        }

        static SearchList self = null;
        public static SearchList GetObject()
        {
            if (self == null)
                self = new SearchList();
            return self;
        }

        bool isLoading = false;
        string error = null;
        string timeTag = "";

        //1.入口/读身份证
        public void Goin()
        {
            isLoading = false;
            error = null;
            sp.Visibility = Visibility.Hidden;
            SelectCard.GetObject().BeforeGoin(LoadData);
            SelectCard.GetObject().Goin();
        }
        //2.加载数据
        async private void LoadData()
        {
            string name = "";
            string persionid = "";
            if (SelectCard.isSelectIDCard)
            {
                name = ReadIDCar.name;
                persionid = ReadIDCar.persionid;
            }
            else
            {
                name = B_ReadSSCard.name;
                persionid = B_ReadSSCard.persionid;
            }


            BackExit.setBack(LoadData);
            Business2.Init("医保账户查询");

            Loading.show2("正在查询，请稍候...");
            tools.AnalyzeJson retJson1 = null;
            tools.AnalyzeJson retJson2 = null;
            await TaskMore.Run(new Action(() => {
                retJson1 = getBalance(persionid, name);
                if (retJson1.error == null)
                {
                    retJson2 = getList(persionid, name);
                }
            })).ConfigureAwait(true);
            if(retJson1.error != null)
            {
                ShowTip.show(false, BackExit.Exit, retJson1.error);
            }
            else if(retJson2.error != null)
            {
                ShowTip.show(false, BackExit.Exit, retJson2.error);
            }
            else
            {
                CD.business2.setBusinessValue(this);
                time.reset(true, false);
                string balance = retJson1["data"]["balance"].ToString();
                this.balance.Text = balance;
                setList(retJson2);
            }
        }

        async private void search_Click(object sender, RoutedEventArgs e)
        {
            if (isLoading)
                return;
            isLoading = true;
            sp.Visibility = Visibility.Visible;
            string name = "";
            string persionid = "";
            if (SelectCard.isSelectIDCard)
            {
                name = ReadIDCar.name;
                persionid = ReadIDCar.persionid;
            }
            else
            {
                name = B_ReadSSCard.name;
                persionid = B_ReadSSCard.persionid;
            }
            string timeTag = CD.timeTag.updateTag();
            tools.AnalyzeJson retJson1 = null;
            tools.AnalyzeJson retJson2 = null;
            string[] timeMsg = time.getTime().Split('-');
            int year = int.Parse(timeMsg[0]);
            int month = int.Parse(timeMsg[1]);
            await TaskMore.Run(new Action(() => {
                retJson1 = getBalance(persionid, name);
                if (retJson1.error == null)
                {
                    retJson2 = getList(persionid, name);
                }
            })).ConfigureAwait(true);
            isLoading = false;
            if (CD.timeTag.equal(timeTag) == false)
                return;
            sp.Visibility = Visibility.Hidden;
            if (retJson1.error != null)
            {
                ShowTip.show(false, null, retJson1.error);
            }
            else if (retJson2.error != null)
            {
                ShowTip.show(false, null, retJson2.error);
            }
            else
            {
                CD.business2.setBusinessValue(this);
                string balance = retJson1["data"]["balance"].ToString();
                this.balance.Text = balance;
                setList(retJson2);
            }


        }

        private void setList(tools.AnalyzeJson retJson2)
        {
            table.clear();
            List<Brush> refullColors = new List<Brush>() { Brushes.Black, Brushes.Black, Brushes.Black, Brushes.Green };
            List<double> ratios = new List<double>() { 1, 2, 1, 1.5 };
            List<string> items = new List<string>() { "交易时间", "交易商户", "交易金额（元）", "交易流水号" };
            table.setTopItem(items, ratios, 26, new SolidColorBrush(Color.FromRgb(0x15, 0x4F, 0xC2)), true);
            int count = retJson2["data"]["data"].getArrayCount();
            for (int i = 0; i < count; i++)
            {
                List<string> line = new List<string>();
                line.Add(retJson2["data"]["data"][i]["transDate"].ToString());
                line.Add(retJson2["data"]["data"][i]["merchantName"].ToString());
                line.Add(retJson2["data"]["data"][i]["transAmt"].ToString());
                line.Add(retJson2["data"]["data"][i]["retriRefNo"].ToString());
                table.addTableItem(line, ratios, 23, refullColors, false);
            }

            for (int i = 0; i < 17; i++)
            {
                List<string> line = new List<string>();
                line.Add(DateTime.Now.AddDays(i).ToString("yyyy年MM月dd日"));
                line.Add("一二三四五六七把久是路店");
                line.Add("12.00");
                line.Add("123456789123456789");
                table.addTableItem(line, ratios, 23, refullColors, false);
            }

            table.updateScrollBar();
        }
        private tools.AnalyzeJson getBalance(string persionid, string name)
        {
            MakeJson json1 = new MakeJson();
            json1.add("appid", "zqccb");
            json1.add("appkey", "ccb2293702");
            json1.add("idNo", persionid);
            json1.add("name", name);
            return Post.Post_Json2("getBalance", json1.ToString());
        }
        private tools.AnalyzeJson getList(string persionid, string name)
        {
            MakeJson json2 = new MakeJson();
            json2.add("appid", "zqccb");
            json2.add("appkey", "ccb2293702");
            json2.add("idNo", persionid);
            json2.add("name", name);
            json2.add("stDate", DateTime.Now.AddDays(-31).ToString("yyyyMMdd").ToString());
            json2.add("endDate", DateTime.Now.ToString("yyyyMMdd").ToString());
            json2.add("pageno", "1");
            json2.add("pagesize", 100);
            return Post.Post_Json2("getDetails", json2.ToString());
        }
    }
}
