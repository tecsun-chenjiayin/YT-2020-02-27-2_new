using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows;
using WH_NEW;

namespace tools
{
    public class BitmapImageUtil
    {
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
        private static MemoryStream globalMemoryStream = new System.IO.MemoryStream();
        public static BitmapSource getBitMapSourceFromSnapScreen()
        {
            Bitmap bitmap = GetScreenSnapshot();
            IntPtr intPtrl = bitmap.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(intPtrl, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(intPtrl);
            return bitmapSource;
        }
        public static BitmapSource getBitMapSourceFromBitmap(Bitmap bitmap)
        {
            IntPtr intPtrl = bitmap.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(intPtrl, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(intPtrl);
            return bitmapSource;
        }
        public static BitmapImage getBitmapImageFromSnapScreen()
        {
            Bitmap bitmap = GetScreenSnapshot();
            bitmap.Save(globalMemoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bitmapBytes = globalMemoryStream.GetBuffer();  //byte[]   bytes=   ms.ToArray();  
            //ms.Close(); 
            bitmap.Dispose();
            // Init bitmap
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(bitmapBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }

        private static int num = 1;
        //单独调用这个方法即可截图
        public static Bitmap GetScreenSnapshot()
        {
            System.Drawing.Rectangle rc = System.Windows.Forms.SystemInformation.VirtualScreen;
            var bitmap = new Bitmap(1280, 1024);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(1280,1024), CopyPixelOperation.SourceCopy);
            }
            string path2 = System.DateTime.Now.ToString("yyyy年MM月dd日");
            string path1 = "截图\\" + path2 + "\\" + num + ".jpg";
            if (Directory.Exists("截图") == false)
                Directory.CreateDirectory("截图");
            if (Directory.Exists("截图\\" + path2) == false)
                Directory.CreateDirectory("截图\\" + path2);

            bitmap.Save(path1);
            num++;
            return bitmap;
        }

        private static System.Drawing.Bitmap GlobalBitmap;
        public static void save()
        {
            System.Drawing.Image CatchedBmp = new System.Drawing.Bitmap(1280, 1024);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(CatchedBmp); //创建图片画布 

            ////目标范围
            System.Drawing.Rectangle desiRectangle = new System.Drawing.Rectangle(0, 0, 1280, 1024);

            ////源范围
            System.Drawing.Rectangle sourceRectangle = new System.Drawing.Rectangle(0, 0, 1280, 1024);

            g.DrawImage(GlobalBitmap, desiRectangle, sourceRectangle, System.Drawing.GraphicsUnit.Pixel);

            //保存到剪贴板
            System.Drawing.Bitmap map = (System.Drawing.Bitmap)CatchedBmp;
            BitmapSource source = BitmapImageUtil.getBitMapSourceFromBitmap(map);
            Clipboard.SetImage(source);
            g.Dispose();
            CatchedBmp.Dispose();
            GlobalBitmap.Dispose();
        }
    }
}