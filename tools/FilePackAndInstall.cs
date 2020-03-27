using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tools
{
    //文件打包和分解
    public class FilePackAndInstall
    {
        const int startLength = 1;//500k
        static byte[] normal = new byte[1] { 0 };
        static byte[] middle = new byte[1] { (byte)('|') };
        static byte[] end = new byte[1] { (byte)('@') };
        static FileStream fs = null;
        static int fileLen = 0;
        /// <summary>
        /// 文件打包
        /// </summary>
        /// <param name="DirectoryName">待打包文件目录</param>
        /// <param name="decFileName">打包结果文件命名</param>
        public static int packFiles(string DirectoryName, string decFileName)
        {
            fileLen = 0;
            Console.WriteLine("正在扫描文件...");
            List<string> files1 = new List<string>();
            List<string> files2 = new List<string>();
            addFilePathsToList(DirectoryName, files1);
            for (int i = 0; i < files1.Count; i++)
            {
                files2.Add(files1[i].Replace("待打包程序文件\\", ""));
            }
            Console.WriteLine("目标文件" + files1.Count + "个");

            Console.WriteLine("正在生成中间文件...");

            fs = new FileStream(decFileName, FileMode.Create);
            for (int i = 0; i < files1.Count; i++)
            {
                Console.WriteLine("正在打包文件：" + files2[i]);
                //文件路径
                string path1 = files2[i];
                byte[] path2 = getBytes(path1);
                write2(path2);
                //文件内容
                byte[] datas = File.ReadAllBytes(files1[i]);
                write2(datas);
            }
            write(end);
            fs.Close();
            return fileLen;
        }
        /// <summary>
        /// 文件分离
        /// </summary>
        /// <param name="srcFileName">待分离文件</param>
        /// <param name="decPath">分离文件保存路径，完整路径</param>
        public static void installer(string srcFileName, string decPath)
        {
            Console.WriteLine("\n");
            if (decPath[decPath.Length - 1] != '\\')
                decPath += "\\";

            byte[] src = File.ReadAllBytes(srcFileName);
            int index = startLength;
            StringBuilder length = new StringBuilder();

            int n = 0;

            //分离文件
            byte[] fileName_ = new byte[500];
            while (index < src.Length && src[index] != '@')
            {
                //文件名长度
                StringBuilder length_ = new StringBuilder();
                while (index < src.Length && src[index] != '|')
                {
                    length_.Append((char)src[index++]);
                }
                int len = int.Parse(length_.ToString());
                index++;
                //文件名
                for (int i = 0; i < 500; i++)
                    fileName_[i] = 0;
                n = 0;
                string fileName = "";
                while (index < src.Length && src[index] != '|')
                {
                    fileName_[n++] = src[index++];
                }
                fileName = Encoding.Default.GetString(fileName_).Replace("\0", "");
                index++;
                //内容长度
                StringBuilder dataLength_ = new StringBuilder();
                while (index < src.Length && src[index] != '|')
                {
                    dataLength_.Append((char)src[index++]);
                }
                int dataLen = int.Parse(dataLength_.ToString());
                index++;
                //内容
                //byte[] datas = new byte[dataLen];
                string p = decPath + fileName;
                doDirectoryExit(p);
                Console.WriteLine("分离:" + fileName);
                FileStream saveFile = new FileStream(p, FileMode.Create);
                saveFile.Write(src, index, dataLen);
                saveFile.Close();

                index += dataLen + 1;
            }
            Console.WriteLine("\n分离完成!");
        }
        private static void doDirectoryExit(string path)
        {
            string[] paths = path.Split('\\');
            string p = "";
            int end = paths.Length - 1;
            for (int i = 0; i < end; i++)
            {
                p += paths[i] + '\\';
                if (Directory.Exists(p) == false)
                    Directory.CreateDirectory(p);
            }
        }
        private static void write(byte[] datas)
        {
            fileLen += datas.Length;
            fs.Write(datas, 0, datas.Length);
        }
        private static void write2(byte[] datas)
        {
            write(middle);
            byte[] length = getBytes(datas.Length.ToString());
            write(length);
            write(middle);
            write(datas);
            fileLen += (1 + length.Length + 1 + datas.Length);
        }
        private static byte[] getBytes(string str)
        {
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            return byteArray;
        }
        private static void addFilePathsToList(string folder, List<string> list)
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
    }
}
