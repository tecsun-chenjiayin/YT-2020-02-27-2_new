using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace YTH.Functions
{
    class ImagePaths
    {
        private static Dictionary<string, string> kvs = new Dictionary<string, string>();
        private static bool isInit = false;
        private static void init()
        {
            isInit = true;
            string path = CD.getBasePath() + @"Soruce\Images";
            List<string> list = new List<string>();
            addFilePathsToList(path, list);
            foreach(string str in list)
            {
                kvs.Add(Path.GetFileNameWithoutExtension(str), str);
            }
        }
        public static string getPathByName(string name)
        {
            if (isInit == false)
                init();
            if (name == null)
                MessageBox.Show("getPathByName里传入了空值");
            else if (kvs.ContainsKey(name) == false)
                MessageBox.Show("getPathByName传入了不存在的图片名");
            else
                return kvs[name];
            return "";
        }
        public static void addFilePathsToList(string folder, List<string> list)
        {
            //检测当前文件夹下的文件
            string[] filePaths = System.IO.Directory.GetFiles(folder);
            if (filePaths.Length > 0)
                foreach (string fpath in filePaths)
                    list.Add(fpath);

            //检测当前文件夹下的子文件夹
            string[] directoriesPaths = System.IO.Directory.GetDirectories(folder);
            if (directoriesPaths.Length > 0)
                foreach (string dpath in directoriesPaths)
                    addFilePathsToList(dpath, list);//递归
        }
    }

    class TImage
    {
        static string[] paths = new string[] {"img1.jpg", "img2.jpg" };
        static int index = 0;
        public static System.Windows.Media.Imaging.BitmapSource Base64ToImageSource(string base64)
        {
            byte[] ImgByte = Convert.FromBase64String(base64);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(ImgByte);
            System.Drawing.Image images = System.Drawing.Image.FromStream(ms);
            string fileIMG = Path.Combine("D:\\", paths[index]);
            images.Save(fileIMG, System.Drawing.Imaging.ImageFormat.Jpeg);
            images.Dispose();
            ms.Dispose();
            BitmapImage b = new BitmapImage(new Uri(@"D:\" + paths[index]));
            index = (index + 1) % 2;
            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(base64);

            //IntPtr hBitmap = bmp.GetHbitmap();

            //System.Windows.Media.Imaging.BitmapSource bitmapSource =
            //                 System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //                 hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty,
            //                 System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return b;
        }
    }
}
