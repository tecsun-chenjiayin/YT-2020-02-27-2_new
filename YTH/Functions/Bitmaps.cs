using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace YTH.Functions
{
    class Bitmaps
    {
        static Dictionary<string, BitmapImage> dic = new Dictionary<string, BitmapImage>();
        static Dictionary<string, System.Drawing.Bitmap> dic2 = new Dictionary<string, System.Drawing.Bitmap>();
        static Dictionary<string, string> dic_path = null;
        public static BitmapImage getBitmapImage(string key)
        {
            if(dic_path == null)
            {
                dic_path = new Dictionary<string, string>();
                string path = CD.getBasePath() + @"Soruce\Images";
                List<string> list = new List<string>();
                addFilePathsToList(path, list);
                foreach (string str in list)
                {
                    dic_path.Add(Path.GetFileNameWithoutExtension(str), str);
                }
            }
            if (dic.ContainsKey(key))
                return dic[key];
            else if (dic_path.ContainsKey(key))
            {
                BitmapImage b = new BitmapImage(new Uri(dic_path[key]));
                dic.Add(key, b);
                return b;
            }
            else
                MessageBox.Show("试图获取不存在的图片：" + key);
            return null;
        }
        public static System.Drawing.Bitmap getBitmap(string key)
        {
            if (dic_path == null)
            {
                dic_path = new Dictionary<string, string>();
                string path = CD.getBasePath() + @"Images";
                List<string> list = new List<string>();
                addFilePathsToList(path, list);
                foreach (string str in list)
                {
                    dic_path.Add(Path.GetFileNameWithoutExtension(str), str);
                }
            }
            if (dic2.ContainsKey(key))
                return dic2[key];
            else if (dic_path.ContainsKey(key))
            {
                System.Drawing.Bitmap b = new System.Drawing.Bitmap(dic_path[key]);
                dic2.Add(key, b);
                return b;
            }
            else
                MessageBox.Show("试图获取不存在的图片：" + key);
            return null;
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
}
