using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace tools
{
    /// <summary>写日志类</summary>
    public class Log
    {
        static object locker = new object();
        static Dictionary<string, string> style_path = new Dictionary<string, string>();
        static Dictionary<string, int> style_index = new Dictionary<string, int>();
        static Dictionary<string, StreamWriter> sws = new Dictionary<string, StreamWriter>();
        static string basePath = null;

        public static string getBasePath()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string basePath = Path.GetDirectoryName(asm.Location) + "\\";
            return basePath;
        }
        public static void clearLogMsg()
        {
            style_path.Clear();
            style_index.Clear();
        }

        public static void AddLog(string style, string log)
        {
            lock (locker)
            {
                if (basePath == null)
                {
                    Assembly asm = Assembly.GetExecutingAssembly();
                    basePath = Path.GetDirectoryName(asm.Location) + "\\";
                }
                //长期打开文件的写日志方式////////////////////////////////////////////////////////////////
                //if (Directory.Exists("日志") == false)
                //    Directory.CreateDirectory("日志");

                //string file_path = "";
                //if (style_path.ContainsKey(style))
                //    file_path = style_path[style];
                //else
                //{
                //    file_path = basePath +  @"日志\" + style + "\\" + style + " " + getTime() + ".txt";
                //    style_path[style] = file_path;
                //    style_index[style] = 1;
                //    string dir = Path.GetDirectoryName(file_path);
                //    if (Directory.Exists(dir) == false)
                //        Directory.CreateDirectory(dir);

                //    FileStream fs = File.Open(file_path, FileMode.Append);
                //    StreamWriter sw1 = new StreamWriter(fs);
                //    sws.Add(file_path, sw1);
                //}
                //StreamWriter sw = sws[file_path];//new StreamWriter(fs);
                //sw.WriteLine(style_index[style].ToString("D5") + " " + getTime2() + " " + log);
                //sw.WriteLine("  ");
                //sw.Flush();
                //style_index[style]++;
                //写完即关闭文件的写日志方式/////////////////////////////////////////////////////
                if (Directory.Exists("日志") == false)
                    Directory.CreateDirectory("日志");

                string file_path = "";
                if (style_path.ContainsKey(style))
                    file_path = style_path[style];
                else
                {
                    file_path = basePath + @"日志\" + style + "\\" + style + " " + GetTime() + ".txt";
                    style_path[style] = file_path;
                    style_index[style] = 1;
                }

                string dir = Path.GetDirectoryName(file_path);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                using (FileStream fs = File.Open(file_path, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(style_index[style].ToString("D5") + " " + GetTime2() + " " + log);
                        sw.WriteLine("  ");
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                    }
                }
                style_index[style]++;
            }
        }

        static string fileName = null;
        public static void AddLog(string log)
        {
            lock (locker)
            {
                if (basePath == null)
                {
                    Assembly asm = Assembly.GetExecutingAssembly();
                    basePath = Path.GetDirectoryName(asm.Location) + "\\";
                    fileName = basePath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }

                //写完即关闭文件的写日志方式/////////////////////////////////////////////////////
                if (Directory.Exists("日志") == false)
                    Directory.CreateDirectory("日志");

                string file_path = fileName;
                using (FileStream fs = File.Open(file_path, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(GetTime3() + "  " + log);
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                    }
                }
            }
        }
        /// <summary>获取时间：年月日时分秒</summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        public static string GetTime2()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string GetTime3()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");
        }
    }
}
