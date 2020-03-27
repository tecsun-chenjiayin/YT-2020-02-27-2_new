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

namespace YTH.ZhanJiang
{
    /// <summary>
    /// SearchData.xaml 的交互逻辑
    /// </summary>
    public partial class SearchData : UserControl
    {
        static SearchData self = null;
        public static SearchData GetObject()
        {
            if (self == null)
                self = new SearchData();
            return self;
        }

        string timeTag = "";
        List<string> names = new List<string>();
        List<string> keys = new List<string>();
        List<TextBlock> ctls = new List<TextBlock>();
        public SearchData()
        {
            InitializeComponent();
            foreach(UIElement ui in sp.Children)
            {
                TextBlock tb = ui as TextBlock;
                ctls.Add(tb);
                names.Add(tb.Text.Substring(0,tb.Text.IndexOf("：") + 1));
                keys.Add(tb.Text.Substring(names[names.Count - 1].Length, tb.Text.Length - names[names.Count - 1].Length));
            }
        }

        //1.入口/读身份证
        public void Goin()
        {
            timeTag = CD.timeTag.updateTag();
            SelectCard.GetObject().BeforeGoin(LoadDataAsync);
            SelectCard.GetObject().Goin();
        }
        /*
                         <TextBlock>姓　　名：name</TextBlock>
                <TextBlock>性　　别：sex</TextBlock>
                <TextBlock>民　　族：nation</TextBlock>
                <TextBlock>出生日期：birthday</TextBlock>
                <TextBlock>身份证号：shbzh</TextBlock>
                <TextBlock>社保卡号：sbkh</TextBlock>
                <TextBlock>户口性质：registtype</TextBlock>
                <TextBlock>电　　话：telno</TextBlock>
                <TextBlock>手　　机：mobile</TextBlock>
                <TextBlock>通讯地址：mailaddr</TextBlock>
                <TextBlock>邮　　编：zipcode</TextBlock>
                <TextBlock>电子邮箱：email</TextBlock>
                <TextBlock>单位名称：dwmc</TextBlock>
             */
        async private void LoadDataAsync()
        {
            string name = "";
            string persionid = "";
            if(SelectCard.isSelectIDCard)
            {
                name = ReadIDCar.name;
                persionid = ReadIDCar.persionid;
            }
            else
            {
                name = B_ReadSSCard.name;
                persionid = B_ReadSSCard.persionid;
            }

            BackExit.setBack(LoadDataAsync);
            Business2.Init("卡信息查询");

            Loading.show2("正在查询，请稍候...");
            List<Dictionary<string, string>> zkData = null;
            string error = null;
            await TaskMore.Run(new Action(() =>
            {
                if (error == null)
                    zkData = WeiWang.getZKData(persionid, name, out error);
            })).ConfigureAwait(true);
            if (error != null)
            {
                ShowTip.show(false, BackExit.Exit, error);
                return;
            }
            if (zkData[0]["ERR"] != "OK")
            {
                ShowTip.show(false, BackExit.Exit, zkData[0]["ERR"]);
                return;
            }
            try
            {
                for (int i = 0; i < keys.Count; i++)
                {
                   // string value = retJson["data"][keys[i]].ToString();
                    switch(keys[i])
                    {
                        case "name":
                            keys[i] = zkData[0]["AAC003"];
                            break;
                        case "sex":
                            {
                                if (zkData[0]["AAC004"] == "1" || zkData[0]["AAC004"] == "5")
                                    keys[i] = "男";
                                else if (zkData[0]["AAC004"] == "2" || zkData[0]["AAC004"] == "6")
                                    keys[i] = "女";
                                else
                                    keys[i] = "未知";
                                break;
                            }

                        case "nation":
                            keys[i] = ReadIDCar.GetNationName(zkData[0]["AAC005"]);
                            break;
                        case "birthday":
                            keys[i] =zkData[0]["AAC006"];
                            break;
                        case "shbzh":
                            keys[i] = zkData[0]["AAC002"].Substring(0, 5) + "***********" + zkData[0]["AAC002"].Substring(zkData[0]["AAC002"].Length - 2, 2);
                            break;
                        case "sbkh":
                            keys[i] = zkData[0]["AAZ500"].Substring(0,2) + "*******" + zkData[0]["AAZ500"].Substring(zkData[0]["AAZ500"].Length - 2, 2);
                            break;
                        case "registtype":
                            keys[i] =zkData[0]["AAC009"];
                            break;

                        case "telno":
                            keys[i] =zkData[0]["AAE005"];
                            break;
                        case "mobile":
                            keys[i] = zkData[0]["MOBILE"];
                            break; 
                        case "mailaddr":
                            keys[i] =zkData[0]["AAE006"];
                            break;
                        case "zipcode":
                            keys[i] = zkData[0]["AAE007"];
                            break;
                        case "email":
                            keys[i] = zkData[0]["EMAIL"];
                            break;
                        case "dwmc":
                            keys[i] =zkData[0]["AAB004"];
                            break;
                    }
                    ctls[i].Text = names[i] + keys[i];
                }
                pic.Source = TImage.Base64ToImageSource(zkData[0]["PHOTO"].ToString());
                CD.business2.setBusinessValue(this);
            }
            catch (Exception e)
            {
                Log.AddLog("Search", e.ToString());
                ShowTip.show(false, BackExit.Exit, "数据解析异常");
            }
        }
    }
}
