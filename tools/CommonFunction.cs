using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace tools
{
    public class CommonFunction
    {
        /*
            文件转Base64字符串：   FileToBase64
            转码：                 GB2312ToUTF8
            图片分辨率调整：       KiResizeImage
            BMP图片转JPG图片：     BmpToJpg
            MD5加密：              GetMD5String
         */

        public static string FileToBase64(string filePath)
        {
            try
            {
                byte[] array = File.ReadAllBytes(filePath);
                return Convert.ToBase64String(array);
            }
            catch { return null; }
        }
        public static string GB2312ToUTF8(string str)
        {
            try
            {
                Encoding uft8 = Encoding.GetEncoding(65001);
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] temp = gb2312.GetBytes(str);
                byte[] temp1 = Encoding.Convert(gb2312, uft8, temp);
                string result = uft8.GetString(temp1);
                return result;
            }
            catch
            {
                return null;
            }
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

        public static void BmpToJpg(string srcPath, string decPath)
        {
            Bitmap im = new Bitmap(srcPath);
            //转成jpg
            var eps = new EncoderParameters(1);
            var ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
            eps.Param[0] = ep;
            var jpsEncodeer = GetEncoder(ImageFormat.Jpeg);
            //保存图片
            im.Save(decPath, jpsEncodeer, eps);
            //释放资源
            im.Dispose();
            ep.Dispose();
            eps.Dispose();
        }
        public static void BmpToJpg(Bitmap srcBitmap, string decPath)
        {
            //转成jpg
            var eps = new EncoderParameters(1);
            var ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
            eps.Param[0] = ep;
            var jpsEncodeer = GetEncoder(ImageFormat.Jpeg);
            //保存图片
            srcBitmap.Save(decPath, jpsEncodeer, eps);
            //释放资源
            srcBitmap.Dispose();
            ep.Dispose();
            eps.Dispose();
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }
        public static string GetMD5String(string input)
        {
            //1.创建一个md5对象
            System.Security.Cryptography.MD5 md5Obj = System.Security.Cryptography.MD5.Create();
            //1.1把字符串转换为byte[]
            byte[] buffer = System.Text.Encoding.Default.GetBytes(input);

            //2.通过md5对象计算给定值的md5
            byte[] md5Buffer = md5Obj.ComputeHash(buffer);
            //把byte[]数组转换为字符串
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Buffer.Length; i++)
            {
                sb.Append(md5Buffer[i].ToString("x2"));
            }
            //3.释放资源
            md5Obj.Clear();
            // return sb.ToString();
            return BitConverter.ToString(md5Buffer).Replace("-", "").ToLower();
        }


    }
}
