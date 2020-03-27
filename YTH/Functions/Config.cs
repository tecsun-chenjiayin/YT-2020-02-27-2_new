using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Reflection;

namespace YTH.Functions
{
    /// <summary>
    /// 配置文件读取
    /// </summary>
    class Config
    {
        const string iniFile = @"ini\Config.ini";//基本配置信息文件
        const string iniNetwork = @"ini\Network.ini";//基本配置信息文件
        Dictionary<string, string> config_dic = new Dictionary<string, string>();
        Dictionary<string, string> network_dic = new Dictionary<string, string>();
        static Config con = null;
        static object locker = new object();//线程锁
        static string basepath = null;
        Config()
        {
            if(basepath== null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                basepath = Path.GetDirectoryName(asm.Location) + "\\";
            }
            iniDic( config_dic, iniFile);
            iniDic(network_dic, iniNetwork);
        }
        void iniDic(Dictionary<string, string> dic, string filePath)
        {
            filePath = basepath + filePath;
            if (File.Exists(filePath))
            {
                string[] values = File.ReadAllLines(filePath, Encoding.Default);
                foreach (string v in values)
                {
                    string[] vs = v.Split('#');
                    string[] vn = vs[0].Split('=');
                    if (vn.Length != 2)
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
        static Config getConfig()
        {
            if (con != null)
                return con;
            lock (locker)
            {
                if (con == null)
                    con = new Config();
            }
            return con;
        }
        /// <summary>
        /// 获取配置文件的信息
        /// </summary>
        /// <param name="key">配置数据的名称</param>
        /// <returns>名称对应的值，不存在就返回null</returns>
        public static string dic(string key)
        {
            if (con == null)
                con = getConfig();
            if (con.config_dic.ContainsKey(key))
                return con.config_dic[key];
            else
                return null;
        }

        public static string net_dic(string key)
        {
            if (con == null)
                con = getConfig();
            if (con.network_dic.ContainsKey(key))
                return con.network_dic[key];
            else
                return null;
        }
    }
}
