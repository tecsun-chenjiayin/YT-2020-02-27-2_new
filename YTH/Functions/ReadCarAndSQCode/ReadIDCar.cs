using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace YTH.Functions.ReadCarAndSQCode
{
    class ReadIDCar
    {
        static ThreadProperty readCarThread = new ThreadProperty(500, false, false, read, null);
        static ThreadProperty uiThread = null;
        static int readTime = 0;
        const int maxReadTime = 10;

        public static string name = null;
        public static string persionid = null;
        public static string startDate = null;
        public static string endDate = null;
        public static string sexCode = "0";
        public static string sex = "";
        public static string birthday = "";
        public static string nationCode = "";
        public static string address = "";
        public static string base64 = "";

        public static string error = null;
        private static string startTag = null;
        public static string photoSavePath2 = null;
        public static string photoSavePath = null;
        public static StringBuilder pOutInfo = new StringBuilder(1024);
        public static void readCar(Action nextStep, UIElement ui)
        {
            if (photoSavePath2 == null)
            {
                photoSavePath2 = "D:";
                photoSavePath = "D:\\" + "zp.bmp";
            }
                
            name = null;
            persionid = null;
            error = null;
            startTag = CD.timeTag.getTag();
            readTime = 0;
            readCarThread.resetTime(200);
            readCarThread.start();

            if(uiThread == null)
                uiThread = new ThreadProperty(50, true, false, nextStep, ui);
            uiThread.srcUI = ui;
            uiThread.action = nextStep;
        }

        public static void stop()
        {
            if (readCarThread != null)
                readCarThread.stop();
        }
        private static void read()
        {
   
            address = "";
            nationCode = "";
            //readCarThread.resetTime(2000);
            try
            {
                int ret = -1;
                string tag = CD.timeTag.getTag();
                //Thread.Sleep(3000);//测试
                name = null;
                persionid = null;
                if (Config.dic("UseTestPersionalData") == "1")
                {
                    name = Config.dic("Name");
                    persionid = Config.dic("IDCar");
                    ret = 0;
                }
                else
                {
                    readTime++;
                    pOutInfo.Clear();
                    Log.AddLog("读身份证", "Ins:" + photoSavePath2);
                    ret = iReadMultiCard(photoSavePath, pOutInfo);
                    Log.AddLog("读身份证", "Out:ret:" + ret + " msg:" + pOutInfo.ToString());
                    //姓名|性别|民族|出生日期|地址|身份证号码|发行机关|有效开始日期|有效截止日期|照片文件存放路径
                    //姓名|性别|民族|出生日期|地址|身份证号码|发行机关|有效开始日期|有效截止日期|照片文件存放路径|
                    //0   |1   |2   |3       |4   |5         |6       |7           |8           |9               |10 
                    if (ret == 0)
                    {
                        string[] array = pOutInfo.ToString().Split('|');
                        name = array[0];
                        persionid = array[5];
                        startDate = array[7];
                        endDate = array[8];
                        sex = array[1];
                        if (sex == "男")
                            sexCode = "1";
                        else
                            sexCode = "2";
                        birthday = array[3];
                        error = null;
                        nationCode = GetNationCode(array[2]);
                        address = array[4];
                    }
                    else if (ret == 1)
                    {
                        //英文名|中文名|性别|国籍或所在区域代码|出生日期|永久居住证号码|有效开始日期|有效截止日期|当次申请受理机关代码|证件版本号|照片文件存放路径
                        //0     |1     |2   |3                 |4       |5             |6           |7           |8                   |9         |10

                        string[] array = pOutInfo.ToString().Split('|');
                        name = array[1];
                        persionid = array[5];
                        startDate = array[6];
                        endDate = array[7];
                        sex = array[2];
                        if (sex == "男")
                            sexCode = "1";
                        else
                            sexCode = "2";
                        birthday = array[4];
                        error = null;
                    }
                    else if (ret == 2)
                    {
                        //姓名|性别|出生日期|地址|身份证号码|发行机关|有效开始日期|有效截止日期|通行证号码|签发次数|照片文件存放路径
                        //0   |1   |2       |3   |4         |5       |6           |7           |8         |9       |10  
                        string[] array = pOutInfo.ToString().Split('|');
                        name = array[0];
                        persionid = array[4];
                        startDate = array[6];
                        endDate = array[7];
                        sex = array[1];
                        if (sex == "男")
                            sexCode = "1";
                        else
                            sexCode = "2";
                        birthday = array[2];
                        address = array[3];
                        error = null;
                    }
                    if (readTime >= maxReadTime && (ret < 0 || ret > 2))
                    {
                        error = "ret:" + ret + "  msg:" + pOutInfo.ToString();
                    }
                }

                if (tag != CD.timeTag.getTag())
                {
                    Log.AddLog("读身份证", "tag:" + CD.timeTag.getTag() + "   oldTag:" + tag);
                    if (tag == startTag)
                        readCarThread.stop();
                    return;
                }
                if (ret == 0 || ret == 1 || ret == 2)
                {
                    base64 = ImgToBase64String(photoSavePath);
                    Log.AddLog("读身份证", "goto next step");
                    readCarThread.stop();
                    uiThread.start();
                }  
            }
            catch (Exception e)
            {
                error = e.ToString();

            }
        }
        public static string getSubCardID()
        {
            //44178***********17
            if (persionid != null && persionid != "" && persionid.Length >= 18)
                return persionid.Substring(0, 5) + "***********" + persionid.Substring(16, 2);
            else
                return "";
        }

        //图片转为base64编码的字符串
        public static string ImgToBase64String(string Imagefilename)
        {
            Bitmap im0 = new Bitmap(Imagefilename);
            Bitmap im = KiResizeImage(im0, 358, 441);
            //转成jpg
            var eps = new EncoderParameters(1);
            var ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
            eps.Param[0] = ep;
            var jpsEncodeer = GetEncoder(ImageFormat.Jpeg);
            //保存图片
            string imgurl = @"D:\photo.jpg";
            im.Save(imgurl, jpsEncodeer, eps);
            //释放资源
            im.Dispose();
            ep.Dispose();
            eps.Dispose();

            byte[] array = File.ReadAllBytes(imgurl);
            return Convert.ToBase64String(array);
        }
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, newW, newH), new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }

        const string dll = "TSCardDriver.dll";//C:\Windows\System32\TSCardDriver.dll
        //二代证
        [DllImport(dll, EntryPoint = "iReadSFZ", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadSFZ(string pPhotoSavePath, StringBuilder pOutInfo);

        [DllImport(dll, EntryPoint = "iOpenIDReader", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iOpenIDReader(int port);

        [DllImport(dll, EntryPoint = "iReadIDCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadIDCard(string pPhotoSavePath, StringBuilder pOutInfo);

        [DllImport(dll, EntryPoint = "iCloseIDReader", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iCloseIDReader();

        //
        [DllImport(dll, EntryPoint = "iReadHKMCTW", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadHKMCTW(string pPhotoSavePath, StringBuilder pOutInfo);

        [DllImport(dll, EntryPoint = "iReadMultiCard", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int iReadMultiCard(string pPhotoSavePath, StringBuilder pOutInfo);


        static Dictionary<string, string> lstnation = null;
        public static string GetNationCode(string nationName)
        {
            //脑残的写法
            if (lstnation == null)
            {
                lstnation = new Dictionary<string, string>();
                lstnation.Add("57", "穿青人");
                lstnation.Add("01", "汉");
                lstnation.Add("02", "蒙古");
                lstnation.Add("03", "回");
                lstnation.Add("04", "藏");
                lstnation.Add("05", "维吾尔");
                lstnation.Add("06", "苗");
                lstnation.Add("07", "彝");
                lstnation.Add("08", "壮");
                lstnation.Add("09", "布依");
                lstnation.Add("10", "朝鲜");
                lstnation.Add("11", "满");
                lstnation.Add("12", "侗");
                lstnation.Add("13", "瑶");
                lstnation.Add("14", "白");
                lstnation.Add("15", "土家");
                lstnation.Add("16", "哈尼");
                lstnation.Add("17", "哈萨克");
                lstnation.Add("18", "傣");
                lstnation.Add("19", "黎");
                lstnation.Add("20", "僳僳");
                lstnation.Add("21", "佤");
                lstnation.Add("22", "畲");
                lstnation.Add("23", "高山");
                lstnation.Add("24", "拉祜");
                lstnation.Add("25", "水");
                lstnation.Add("26", "东乡");
                lstnation.Add("27", "纳西");
                lstnation.Add("28", "景颇");
                lstnation.Add("29", "柯尔克孜");
                lstnation.Add("30", "土");
                lstnation.Add("31", "达斡尔");
                lstnation.Add("32", "仫佬");
                lstnation.Add("33", "羌");
                lstnation.Add("34", "布朗");
                lstnation.Add("35", "撒拉");
                lstnation.Add("36", "毛难");
                lstnation.Add("37", "仡佬");
                lstnation.Add("38", "锡伯");
                lstnation.Add("39", "阿昌");
                lstnation.Add("40", "普米");
                lstnation.Add("41", "塔吉克");
                lstnation.Add("42", "怒");
                lstnation.Add("43", "乌孜别克");
                lstnation.Add("44", "俄罗斯");
                lstnation.Add("45", "鄂温克");
                lstnation.Add("46", "德昂");
                lstnation.Add("47", "保安");
                lstnation.Add("48", "裕固");
                lstnation.Add("49", "京");
                lstnation.Add("50", "塔塔尔");
                lstnation.Add("51", "独龙");
                lstnation.Add("52", "鄂伦春");
                lstnation.Add("53", "赫哲");
                lstnation.Add("54", "门巴");
                lstnation.Add("55", "珞巴");
                lstnation.Add("56", "基诺");
            }

            foreach (var item in lstnation)
            {
                if (item.Value.Contains(nationName))
                {
                    return item.Key;
                }
            }
            return string.Empty;
        }
        public static string GetNationName(string code)
        {
            if (lstnation == null)
            {
                lstnation = new Dictionary<string, string>();
                lstnation.Add("57", "穿青人");
                lstnation.Add("01", "汉");
                lstnation.Add("02", "蒙古");
                lstnation.Add("03", "回");
                lstnation.Add("04", "藏");
                lstnation.Add("05", "维吾尔");
                lstnation.Add("06", "苗");
                lstnation.Add("07", "彝");
                lstnation.Add("08", "壮");
                lstnation.Add("09", "布依");
                lstnation.Add("10", "朝鲜");
                lstnation.Add("11", "满");
                lstnation.Add("12", "侗");
                lstnation.Add("13", "瑶");
                lstnation.Add("14", "白");
                lstnation.Add("15", "土家");
                lstnation.Add("16", "哈尼");
                lstnation.Add("17", "哈萨克");
                lstnation.Add("18", "傣");
                lstnation.Add("19", "黎");
                lstnation.Add("20", "僳僳");
                lstnation.Add("21", "佤");
                lstnation.Add("22", "畲");
                lstnation.Add("23", "高山");
                lstnation.Add("24", "拉祜");
                lstnation.Add("25", "水");
                lstnation.Add("26", "东乡");
                lstnation.Add("27", "纳西");
                lstnation.Add("28", "景颇");
                lstnation.Add("29", "柯尔克孜");
                lstnation.Add("30", "土");
                lstnation.Add("31", "达斡尔");
                lstnation.Add("32", "仫佬");
                lstnation.Add("33", "羌");
                lstnation.Add("34", "布朗");
                lstnation.Add("35", "撒拉");
                lstnation.Add("36", "毛难");
                lstnation.Add("37", "仡佬");
                lstnation.Add("38", "锡伯");
                lstnation.Add("39", "阿昌");
                lstnation.Add("40", "普米");
                lstnation.Add("41", "塔吉克");
                lstnation.Add("42", "怒");
                lstnation.Add("43", "乌孜别克");
                lstnation.Add("44", "俄罗斯");
                lstnation.Add("45", "鄂温克");
                lstnation.Add("46", "德昂");
                lstnation.Add("47", "保安");
                lstnation.Add("48", "裕固");
                lstnation.Add("49", "京");
                lstnation.Add("50", "塔塔尔");
                lstnation.Add("51", "独龙");
                lstnation.Add("52", "鄂伦春");
                lstnation.Add("53", "赫哲");
                lstnation.Add("54", "门巴");
                lstnation.Add("55", "珞巴");
                lstnation.Add("56", "基诺");
            }
            if (lstnation.ContainsKey(code))
                return lstnation[code];
            else
                return code;
        }
    }
}
