
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace tools
{
    public class ConfigureFile2
    {
        static string basePath = null;
        Dictionary<string, Dictionary<string, string>> dics = new Dictionary<string, Dictionary<string, string>>();
        public ConfigureFile2(string fileName)
        {
            if (basePath == null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                basePath = Path.GetDirectoryName(asm.Location) + "\\";
            }
            string iniPath = basePath + "ini\\" + fileName;
            if (File.Exists(iniPath))
            {
                string baseKey = "normal";
                dics.Add(baseKey, new Dictionary<string, string>());
                string[] values = File.ReadAllLines(iniPath, Encoding.Default);
                foreach (string v in values)
                {
                    string v_ = v.Trim();
                    if (v_.StartsWith("[") && v_.EndsWith("]"))
                    {
                        baseKey = v_.Substring(1, v_.Length - 2);
                        dics.Add(baseKey, new Dictionary<string, string>());
                    }
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
                    dics[baseKey].Add(key, value);
                }
            }
        }
        public string GetValue(string key1, string key2)
        {
            if (dics.ContainsKey(key1))
                if (dics[key1].ContainsKey(key2))
                    return dics[key1][key2];
                else
                    return null;
            else
                return null;
        }

        public string this[string key1, string key2]
        {
            get
            {
                if (dics.ContainsKey(key1))
                    if (dics[key1].ContainsKey(key2))
                        return dics[key1][key2];
                    else
                        return null;
                else
                    return null;
            }
        }
    }
}