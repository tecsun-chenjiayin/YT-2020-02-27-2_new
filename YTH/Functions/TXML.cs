using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTH.Functions
{
    //非通用
    class TXML
    {
        private string src = null;
        public string error = null;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        public TXML(string data)
        {
            src = data;
            data = data.Replace("<root>", "");
            data = data.Replace("</root>", "");
            data = data.Replace(">", "<");
            string[] datas = data.Split('<');
            string k = null;
            string v = null;
            for (int i = 0; i < datas.Length; i++)
            {
                if (i % 4 == 1)
                    k = datas[i];
                else if (i % 4 == 2)
                    v = datas[i];
                if (k != null && v != null)
                {
                    if (dic.ContainsKey(k))
                    {
                        error = "数据项重复！";
                        return;
                    }
                    dic.Add(k, v);
                    k = v = null;
                }
            }
        }

        public string get(string key)
        {
            if (dic.ContainsKey(key))
                return dic[key];
            return "";
        }

    }
}
