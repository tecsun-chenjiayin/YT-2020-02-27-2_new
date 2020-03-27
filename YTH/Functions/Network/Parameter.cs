using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTH.Network
{

    public class Parameter
    {
        private static StringBuilder data = new StringBuilder();//请求参数
        private static List<KeyValuePair<string, string>> kvs = new List<KeyValuePair<string, string>>();
        public static void clear()
        {
            kvs.Clear();
        }
        public static void add(string key, string value)
        {
            kvs.Add(new KeyValuePair<string, string>(key, value));
        }
        public static StringBuilder get(Dictionary<string, string> inParams)
        {
            kvs.Clear();
            data.Clear();
            foreach (KeyValuePair<string, string> kv in inParams)
                kvs.Add(kv);
            return get();
        }
        public static StringBuilder get()
        {
            if (kvs.Count == 0) return new StringBuilder();
            data.Clear();
            if (kvs.Count >= 1)
                addFirstParameter(kvs[0].Key, kvs[0].Value);
            for (int i = 1; i < kvs.Count; i++)
                addParameter(kvs[i].Key, kvs[i].Value);
            addEndParameter();


            return data;
        }
        //添加请求参数
        private static void addFirstParameter(string name, string value)
        {
            bool isInt = false;
            if (value.IndexOf("@_") == 0)
            {
                isInt = true;
                value = value.Substring(2, value.Length - 2);
            }
                
            data.Clear();
            if(value.IndexOf('[') != 0 && !isInt)
                data.Append("{\"" + name + "\":\"" + value + "\"");
            else
                data.Append("{\"" + name + "\":" + value);
        }
        private static void addParameter(string name, string value)
        {
            bool isInt = false;
            if (value.IndexOf("@_") == 0)
            {
                isInt = true;
                value = value.Substring(2, value.Length - 2);
            }
            if (value.IndexOf('[') != 0 && !isInt)
                data.Append(",\"" + name + "\":\"" + value + "\"");
            else
                data.Append(",\"" + name + "\":" + value);
        }
        private static void addEndParameter()
        {
            data.Append("}");
        }
        private static void clearParameter()
        {
            data.Clear();
        }
        private static string getParameter() { return data.ToString(); }
    }

}
