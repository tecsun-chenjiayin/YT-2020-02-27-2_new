using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using YTH.BuKa;
using YTH.Controls;
using YTH.Controls.InputPersion;
using YTH.Controls.SelectTimeCtls;
using YTH.Functions;
using YTH.Functions.MSDLL;
using YTH.Functions.ReadCarAndSQCode;
using YTH.GetCar;
using YTH.LingKa;
using YTH.ManagementCar;
using YTH.Network;
using YTH.ZhanJiang;
using YTH.ZhanJiang.BuKa;

namespace YTH
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

      

        public MainWindow()
        {
            //string error = null;
            //var data = WeiWang.getZKData("441781199302143817", out error);
            //MessageBox.Show("");
            //    CheckPersionData cpd = new CheckPersionData();
            //cpd.Goin(data[0]["社会保障号码"], data[0]["姓名"], data[0]["印刷相片"], null);
            //string tt = File.ReadAllText("2.txt", Encoding.Default);
            //tools.AnalyzeJson aj = new tools.AnalyzeJson(tt);
            //string dm = aj["data"]["dwmc"].ToString();
            //int a = 0;
            //string msg = WeiWang.getAC01("441202199703050512", "罗志豪");
            //MessageBox.Show(msg);

            //List<string> printDatas = new List<string>();
            //printDatas.Add("        肇庆市社会保障卡业务回执单");
            //printDatas.Add("业务类型：自助领卡");
            //printDatas.Add("交易流水号：" );
            //printDatas.Add("终端名称：" );
            //printDatas.Add("终端编号：");
            //printDatas.Add("所属区域：");
            //printDatas.Add("所属网点：");
            //printDatas.Add("网点编号：");
            //printDatas.Add("交易时间：");
            //printDatas.Add("交易结果：领卡成功");
            //printDatas.Add("卡号：");
            //printDatas.Add("领卡人：");
            //Print.print(printDatas);
            //ReadIDCar.persionid = "441225198703040437";
            //string msg = WeiWang.iWrite();
            //MessageBox.Show(msg);
            //YTH.NHNet.Network3.test();
            //YTH.NHNet.Network3.test_add_card();
            //YTH.NHNet.Network3.test_lingka();

            //while(true)
            //{
            //    Network3
            //}
            //读卡信息
            //while (true)
            //{
            //    string readCarError2 = null;
            //    string atr2 = null;
            //    //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
            //    //0         1       2         3               4         5         6     7             8     9     10    11      12
            //    string readCarData2 = MS2.readCar(out readCarError2, out atr2);
            //    if (readCarError2 != null)
            //    {
            //        int b = 0;
            //    }
            //}


            //int a = 00;

            //NHNet.Network3.test();////ID="1" space="09" height="65" data="6228 791910000639416 " y="305" x="80" font="1" name="" variable="False"/>

            //IntPtr handle = MsTerm_OpenDevice("127.0.0.1", 6200);
            //int ret1 = MsTerm_putCardIn(handle, 5, 49);
            //int ret2 = MsTerm_bulgeCardFace(handle, "10.0|36.5|1|622823 8888888888888", 0, 30);

            //                MS2.open();
            //MS2.iTranseDevputCardIn(4);
            //int rr = MS2.iTranseDevbulgeCardFaceM(MS2.handle, "1", "", "6228 888888888888888 ", 2, 30);
            ////                                                      888888888888888
            //string error = null;
            //YTH.BuKa.WeiWang.getZKData("445202198406110610", out error);
            //if (error == null)
            //    MessageBox.Show("Success");
            //else
            //    MessageBox.Show(error);
            //MS2.open();
            //string ssss;
            //string aaa = MS2.readBankCarNum(out ssss);

            //copyFiles();
            //MS2.open();
            //MS2.reset();

            //string error111 = null;
            //List<Dictionary<string, string>> aa = WeiWang.getZKData("441781199302143817", out error111);
            //string aabb = "0";
            //string error1 = null;
            //SubThreadMakeCar.readBankCarNum(out error1);
            //////4、调用维望接口
            //string[] persionDatas = new string[5];
            //persionDatas[0] = Config.dic("WIP");
            //persionDatas[1] = Config.dic("userid");
            //persionDatas[2] = Config.dic("password");
            //persionDatas[3] = Config.dic("cityCode");
            //persionDatas[4] = "441781199302143817";
            ////4、调用维望接口 依次为姓名、社会保障号码、性别、发卡日期、印刷相片、条码、有效日期
            //string name = Config.dic("pName");
            //string pPersion = Config.dic("pPersionID");
            //string Sex = Config.dic("pSex");
            //string date1 = Config.dic("pDate1");
            //string picture = Config.dic("pPicture");
            //string code = Config.dic("pCode");
            //string date2 = Config.dic("pDate2");
            ////string test = "宋体|8.5|320|180,宋体|8.5|320|230,宋体|8.5|320|280,宋体|8.5|320|330,46|127|234|300,,宋体|8.5|320|380";
            ////ret = WeiWang.iWrite(persionDatas, test);
            //string ret = WeiWang.iWrite(persionDatas, string.Format("{0},{1},{2},{3},{4},{5},{6}", name, pPersion, Sex, date1, picture, "", date2));

            //SubThreadMakeCar.Goin("441781199302143817");



            //bool hasCarIn = MS2.hasCarIn();


            //MS2.open();
            //MS2.iTranseDevputCardIn(4);
            //int isSuccess = 0;
            //string read = MS2.OcrGetData(out isSuccess);


            //string error09;//3B6D00000087CF20018649618C009773F5
            //string atr = MS2.getART(out error09);
            //int s = 0;
            //MS2.open();
            //MS2.iTranseDevCardToStore(atr, ref s);


            //int carInSlotNo = 0;
            //string atr = null;
            //string dderror = null;
            //string ddd = MS2.readCar(out dderror, out atr);
            //string carInError2 = MS2.iTranseDevCardToStore(atr, ref carInSlotNo);


            ////4、调用维望接口
            //string name = Config.dic("pName");
            //string pPersion = Config.dic("pPersionID");
            //string pDate = Config.dic("pDate");
            //string pPicture = Config.dic("pPicture");
            //string pSsse = Config.dic("pSsse");
            //string ret = WeiWang.iWrite(persionDatas, string.Format("{0},{1},{4},{2},{3}", name, pPersion, pDate, pPicture, pSsse));


            /*
            string[] persionDatas = new string[5];
            persionDatas[0] = Config.dic("WIP");
            persionDatas[1] = Config.dic("userid");
            persionDatas[2] = Config.dic("password");
            persionDatas[3] = Config.dic("cityCode");
            persionDatas[4] = "441781199302143817";
            MS2.open();
            MS2.iTranseDevputCardIn(4);

            //4、调用维望接口 依次为姓名、社会保障号码、性别、发卡日期、印刷相片、条码、有效日期
            string name = Config.dic("pName");
            string pPersion = Config.dic("pPersionID");
            string Sex = Config.dic("pSex");
            string date1 = Config.dic("pDate1");
            string picture = Config.dic("pPicture");
            string code = Config.dic("pCode");
            string date2 = Config.dic("pDate2");
       
            string ret = WeiWang.iWrite(persionDatas, string.Format("{0},{1},{2},{3},{4},{5},{6}", name, pPersion, Sex, date1, picture, "", date2));
            MS2.putCarOut();
            */

            string ddd = YTH.Functions.Network.GetMacAddress();

            InitializeComponent();

            // 设置全屏
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.Topmost = true;
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            TH.mainUI = this;
            TipWin.init();
            TipWinB1.init();
            Loading.init();
            //mainGrid = mainPage;

            CD.setMainUI = setMainUI;
            CD.setTopUI = setTopUI;
            CD.returnToMain = returnToMain;
            //CD.countDownTime = time;
            BackExit.setBack(returnToMain);

            Loaded += HadLoaded;
        }

        #region 文件操作
        private void copyFiles()
        {

            List<string> files = new List<string>();
            addFilePathsToList("Sys32", files);
            foreach (string file in files)
            {
                try
                {
                    File.Copy(file, @"C:\Windows\System32\" + System.IO.Path.GetFileName(file), true);
                }
                catch (Exception e)
                {
                    //MessageBox.Show("文件拷贝操作失败，程序可能不能正常使用，请确保是以管理员权限打开程序，错误信息：" + e.ToString());
                }
            }

        }

        public static void addFilePathsToList(string folder, List<string> list)
        {
            //检测当前文件夹下的文件
            string[] filePaths = Directory.GetFiles(folder);
            if (filePaths.Length > 0)
                foreach (string fpath in filePaths)
                    list.Add(fpath);

            //检测当前文件夹下的子文件夹
            string[] directoriesPaths = Directory.GetDirectories(folder);
            if (directoriesPaths.Length > 0)
                foreach (string dpath in directoriesPaths)
                    addFilePathsToList(dpath, list);//递归
        }
        #endregion

        #region 程序初始化
        //刚加载完成界面
        private void HadLoaded(object sender, RoutedEventArgs args)
        {

        }



        #endregion
     

        ///////////////////设置方法///////////////////////
        public void setMainUI(object ui)
        {
            UIElement uii = (UIElement)ui;
            mainArea.Child = uii;
            clearTopUI();
        }
        public void setTopUI(object ui)
        {
            UIElement uii = (UIElement)ui;
            topArea.Child = uii;
        }
        public void clearTopUI()
        {
            topArea.Child = null;
        }
        ///////////////////功能方法///////////////////////
        public void returnToMain()
        {
            try {
                clearTopUI();
                mainArea.Child = mainGrid;
                BackExit.setBack(returnToMain);
            }
            catch
            {
                TH.addOnceUI(returnToMain);
            }
        }


        /////////////////// 其  他 ///////////////////////
        ///监听窗口关闭，以关闭还在运行的子线程
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StatusData.WindowAboutClose();
            TipWin.close();
            TipWinB1.close();
            SelectTimeWin.CloseAll();
            InputPersionidWin.CloseAll();
            Environment.Exit(0);
        }

        MainList ml = null;
        private void MoreFunction_Click(object sender, RoutedEventArgs e)
        {
            if (ml == null)
                ml = new MainList();
            ml.Goin();
        }

        private void Xinbanka_Click(object sender, RoutedEventArgs e)
        {
            newCard.GetObject().Goin();
        }
        private void Buka_Click(object sender, RoutedEventArgs e)
        {
            YTH.ZhanJiang.buka.GetObject().Goin();
        }
        private void Huanka_Click(object sender, RoutedEventArgs e)
        {
            ShowTip.show(false, null, "功能尚未开放，敬请期待");
            //YTH.ZhanJiang.huanka.GetObject().Goin();
        }

        ManagementList management = null;
        int downNum = 0;
        System.DateTime dTime = System.DateTime.Now;
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.DateTime dTime2 = System.DateTime.Now;
            int s = dTime2.Second - dTime.Second;
            if (s >= 0 && s <= 3)
                downNum++;
            else
                downNum = 0;
            dTime = dTime2;
            if (downNum >= 3)
            {
                downNum = 0;
                if (management == null)
                    management = new ManagementList();
                //CheckPassword.Goin(management.Goin);

                Business2.Init("管理员功能");
                CheckPassword3.GetObject().Goin(management.Goin, false, "请输入管理员账号密码");
            }
        }

        private void Lingka_Click(object sender, RoutedEventArgs e)
        {
            //ZhaoQing.LinagKa.GetObject().Goin();
            ShowTip.show(false, null, "功能尚未开放，敬请期待");
        }

        private void Jindu_Click(object sender, RoutedEventArgs e)
        {
            ZhanJiang.MakeCardProcess.GetObject().Goin();
        }

        private void Message_Click(object sender, RoutedEventArgs e)
        {
            SearchData.GetObject().Goin();
        }

        private void YbBalance_Click(object sender, RoutedEventArgs e)
        {
            SearchList.GetObject().Goin();
        }

        private void JrBalance_Click(object sender, RoutedEventArgs e)
        {
            ShowTip.show(false, null, "功能尚未开放，敬请期待");
        }

        private void ChangePsw_Click(object sender, RoutedEventArgs e)
        {
            ChangePsw.GetObject().Goin();
        }

        async private void PutCardOut_Click(object sender, RoutedEventArgs e)
        {
            await TaskMore.Run(new Action(() => {
                if (B_ReadSSCard.putCardOut())
                    ShowTip.show(true, null, "请取走您的社保卡！");
                else
                    ShowTip.show(false, null, "出卡失败");
            }));

        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CountDownTime2.updateTime();
        }


        //private void addCar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Status.isWorking) return;
        //    if (mcp == null)
        //        mcp = new Main_ManagementCarPath();
        //    mcp.Goin();
        //}

        //Main_ManagementCarPath mcp = null;
        //private void logoBtn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (Status.isWorking && BackExit.isOnMain()) return;
        //    if (mcp == null)
        //        mcp = new Main_ManagementCarPath();
        //    CheckPassword.Goin(mcp.Goin);
        //}
    }
}
