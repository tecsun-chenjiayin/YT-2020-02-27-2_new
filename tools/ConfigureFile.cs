using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Reflection;
//配置文件读取类
namespace WH_NEW
{
    class Config
    {
        static string basePath = null;
        static Dictionary<string, Dictionary<string, string>> dics = new Dictionary<string, Dictionary<string, string>>();
        public static bool IniConfi(string iniFileName)
        {
            if (basePath == null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                basePath = Path.GetDirectoryName(asm.Location) + "\\";
            }
            string iniPath = basePath + "ini\\" + iniFileName;
            if (File.Exists(iniPath) == false)
                return false;
            if (dics.ContainsKey(iniFileName))
                return false;
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                iniDic(dic, iniPath);
                dics.Add(iniFileName, dic);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string IniConfi(string[] iniFileNames)
        {
            string errorIni = "";
            foreach(string name in iniFileNames)
            {
                if (IniConfi(name) == false)
                    errorIni += "-" + name;
            }
            if (errorIni == "")
                return null;
            else
                return errorIni;
        }
        public static string getValue(string iniFileName, string key)
        {
            if (dics.ContainsKey(iniFileName))
                if (dics[iniFileName].ContainsKey(key))
                    return dics[iniFileName][key];
            return null;
        }
        static void iniDic(Dictionary<string, string> dic, string filePath)
        {
            string path = basePath + filePath;
            if (File.Exists(path))
            {
                string[] values = File.ReadAllLines(path, Encoding.Default);
                foreach (string v in values)
                {
                    string[] vs = v.Split('#');
                    string[] vn = vs[0].Split('=');
                    if(vn.Length != 2)
                        continue;
                    string key = vn[0];
                    string value = "";
                    if (vn[1].Length > 0)
                        for (int k = vn[1].Length - 1; k >= 0; k--)
                            if (vn[1][k] != ' ' && vn[1][k] != '\t')
                            {
                                value = vn[1].Substring(0, k + 1);
                                break;
                            }
                    dic.Add(key, value);
                }
            }
        }
    }
}
