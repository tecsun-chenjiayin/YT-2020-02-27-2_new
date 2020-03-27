using System;
using System.Collections.Generic;
using System.IO;
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

namespace YTH.BuKa
{
    /// <summary>
    /// CheckPersionData.xaml 的交互逻辑
    /// </summary>
    public partial class CheckPersionData : UserControl
    {
        public CheckPersionData()
        {
            InitializeComponent();
        }

        Action nextStep = null;

        public void Goin(string persionid,string name,string pic, Action nextStep)
        {
            //string p = "pic" + DateTime.Now.ToString("yyyyMMddHHmmss");
            //string er = "";
            //Base64ToJpg(pic, p, ref er);
            CD.business1.showBackAndExitBtn();
            BackExit.LetNextClickToMain();
            CD.business1.setBusinessValue(this);
            this.nextStep = nextStep;
            this.persionid.Text = persionid;
            this.name.Text = name;
            try
            {
                if(pic == null || pic == "")
                {
                    this.pic.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.pic.Source = TImage.Base64ToImageSource(pic);
                    this.pic.Visibility = Visibility.Visible;
                }
            }
            catch(Exception e)
            {
                Log.AddLog("ERROR", "照片base64数据解析失败：" + e.ToString() + "   照片数据:" + pic);
                this.pic.Visibility = Visibility.Hidden;
            }
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (nextStep != null)
                nextStep();
        }

        public static int Base64ToJpg(string base64string, string photoPath, ref string strErrorMsg)//base64字符串， 输出照片路径
        {
            int iRet = 0;
            strErrorMsg = string.Empty;
            byte[] arr = null;
            System.Drawing.Bitmap bMap = null;

            try
            {
                arr = Convert.FromBase64String(base64string.Replace("\\n", "").Replace("\\r", ""));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                strErrorMsg = ex.Message;
                return -1;
            }

            using (MemoryStream ms = new MemoryStream(arr))
            {
                try
                {
                    bMap = new System.Drawing.Bitmap(ms);
                    if (!File.Exists(photoPath))
                    {
                        if (arr[0] == 0xff && arr[1] == 0xd8)
                        {
                            photoPath += ".jpeg";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        else if (arr[0] == 0x42 && arr[1] == 0x4d)
                        {
                            photoPath += ".bmp";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Bmp);
                        }
                        else if (arr[0] == 0x89 && arr[1] == 0x50)
                        {
                            photoPath += ".png";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        else if ((arr[0] == 0x4d && arr[1] == 0x4d) || (arr[0] == 0x49 && arr[1] == 0x49))
                        {
                            photoPath += ".tiff";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Tiff);
                        }
                        //if (base64string.Substring(0, 4).Equals("/9j/"))
                        //{
                        //    bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //}
                        //else
                        //{
                        //    bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Bmp);
                        //}
                    }
                    else
                    {
                        File.Delete(photoPath);
                        if (arr[0] == 0xff && arr[1] == 0xd8)
                        {
                            photoPath += ".jpeg";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        else if (arr[0] == 0x42 && arr[1] == 0x4d)
                        {
                            photoPath += ".bmp";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Bmp);
                        }
                        else if (arr[0] == 0x89 && arr[1] == 0x50)
                        {
                            photoPath += ".png";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        else if ((arr[0] == 0x4d && arr[1] == 0x4d) || (arr[0] == 0x49 && arr[1] == 0x49))
                        {
                            photoPath += ".tiff";
                            bMap.Save(photoPath, System.Drawing.Imaging.ImageFormat.Tiff);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    strErrorMsg = ex.Message;
                    return -1;
                }
                finally
                {
                    bMap.Dispose();
                    ms.Close();
                    ms.Dispose();
                }
            }
            return iRet;
        }

    }
}
