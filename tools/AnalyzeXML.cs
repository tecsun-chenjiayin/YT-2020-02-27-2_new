using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tools
{
    /// <summary>
    /// 解析XML 2019/10/22 17:38
    /// </summary>
    public class AnalyzeXML
    {
        Dictionary<string, List<AnalyzeXML>> dic = new Dictionary<string, List<AnalyzeXML>>();
        public Dictionary<string, string> xmlProperty = new Dictionary<string, string>();
        public string error = null;
        public string key = null;
        public string value = null;
        private AnalyzeXML(string txt, int nouse)
        {
            int end = txt.Length;
            int index = 0;
            while (index < end)
            {
                char c = txt[index++];
                if (c == '<')
                    break;
            }
            StringBuilder key_ = new StringBuilder();
            StringBuilder property_ = new StringBuilder();
            bool isEmpty = false;
            int tag = 0;
            while (index < end)
            {
                char c = txt[index++];
                if (c == '/')
                {
                    tag++;
                    continue;
                }
                else if (isEmpty && c != '>' && c != '/')
                {
                    isEmpty = false;
                    property_.Append(c);
                    bool dn = false;
                    while (index < end)
                    {
                        char c2 = txt[index++];
                        if (c2 == '\"')
                        {
                            dn = !dn;
                        }
                        if ((dn == false && c2 != '/' && c2 != '>') || dn)
                            property_.Append(c2);
                        if (dn == false && (c2 == '/' || c2 == '>'))
                            break;
                    }

                    bool isSuccess = analyzeProperty(property_.ToString());
                    property_.Clear();
                    if (isSuccess == false)
                        return;
                    if (txt[index] == '/')
                        tag++;
                    if (index == txt.Length)
                        break;
                    index--;
                    continue;
                }
                else if (c == ' ')
                {
                    isEmpty = true;
                    continue;
                }
                else if (tag == 0 && c == '>')
                {
                    key = key_.ToString().Trim();
                    int eindex = findEndIndex(key, txt);// txt.LastIndexOf(keyEnd);
                    value = txt.Substring(index, eindex - index);
                    break;
                }
                else if (tag == 1 && c == '>')
                {
                    key = key_.ToString().Trim();
                    dic.Add(key_.ToString().Trim(), null);
                    break;
                }


                key_.Append(c);
            }
            key = key_.ToString();
            if (value == null && tag == 0)
            {
                error = "XML解析失败：" + key_.ToString();
                return;
            }
            else if (value == null && tag == 1)
                return;
            int a1 = value.LastIndexOf("&>");
            int a2 = value.LastIndexOf(">");
            int b1 = value.IndexOf("&<");
            int b2 = value.IndexOf("<");
            if (a2 != -1 && b2 != -1)
            {
                if (a2 > a1)
                {
                    if ((b1 != -1 && b2 < b1) || b1 == -1)
                    {
                        List<AnalyzeXML> aaa = analyzeSub(value);
                        foreach (AnalyzeXML a in aaa)
                        {
                            if (dic.ContainsKey(a.key) == false)
                            {
                                dic.Add(a.key, new List<AnalyzeXML>());
                            }
                            dic[a.key].Add(a);
                        }
                    }
                }
            }

            if (value.IndexOf("&<") != -1 || value.IndexOf("&>") != -1)
            {
                value = value.Replace("&<", "<").Replace("&>", ">");
            }
            if (value.IndexOf("&lt;") != -1)
                value = value.Replace("&lt;", "<");
            if (value.IndexOf("&gt;") != -1)
                value = value.Replace("&gt;", ">");
            if (value.IndexOf("&amp;") != -1)
                value = value.Replace("&amp;", "&");
            if (value.IndexOf("&apos;") != -1)
                value = value.Replace("&apos;", "'");
            if (value.IndexOf("&quot;") != -1)
                value = value.Replace("&quot;", "\"");
            value = value.Trim();
        }
        private bool analyzeProperty(string str)
        {
            bool isKey = true;
            int dn = 0;
            int kn = 0;
            int vn = 0;
            StringBuilder key = new StringBuilder();
            StringBuilder value = new StringBuilder();
            foreach (char c in str)
            {
                if (c == '=')
                {
                    isKey = false;
                    continue;
                }
                if (c == '\"')
                    dn++;
                if (kn == 0 && c <= ' ')
                    continue;
                else if (isKey == false && vn == 0 && c < ' ')
                    continue;
                if (isKey)
                {
                    key.Append(c);
                    kn++;
                }
                else
                {
                    value.Append(c);
                    vn++;
                }
                if (dn == 2)
                {
                    kn = 0;
                    vn = 0;
                    dn = 0;
                    isKey = true;
                    string key2 = key.ToString();
                    string value2 = value.ToString();
                    if (value2[0] == '"' && value2[value2.Length - 1] == '"')
                        value2 = value2.Substring(1, value2.Length - 2);

                    if (xmlProperty.ContainsKey(key2))
                    {
                        error = "属性重复";
                        return false;
                    }
                    else
                    {
                        xmlProperty.Add(key2, value2);
                        key.Clear();
                        value.Clear();
                    }
                }

            }
            string key3 = key.ToString().Trim();
            string value3 = value.ToString().Trim();
            if (key3 != "" || value3 != "")
            {
                error = "属性异常";
                return false;
            }
            return true;
        }
        private int findEndIndex(string tag, string txt)
        {
            string tag1 = "<" + tag;
            string tag2 = "</" + tag;
            int num = 0;
            int i = 0;
            int e = txt.Length;
            bool hasFindFirst = false;
            while (i < e)
            {
                int i1 = i;
                int k1 = 0;
                for (; k1 < tag1.Length; k1++, i1++)
                {
                    if (txt[i1] != tag1[k1])
                        break;
                }
                if (k1 == tag1.Length)
                {
                    num++;
                    hasFindFirst = true;
                }
                int i2 = i;
                int k2 = 0;
                for (; k2 < tag2.Length; k2++, i2++)
                {
                    if (txt[i2] != tag2[k2])
                        break;
                }
                if (k2 == tag2.Length)
                    num--;

                if (hasFindFirst && num == 0)
                    break;
                i++;
            }
            return i;
        }

        public AnalyzeXML(string txt)
        {
            if (check1(txt) == false)
                return;
            if (check2(txt) == false)
                return;
            List<AnalyzeXML> xmls = analyzeSub(txt);
            foreach (AnalyzeXML xml in xmls)
            {
                if (dic.ContainsKey(xml.key) == false)
                {
                    dic.Add(xml.key, new List<AnalyzeXML>());
                }

                dic[xml.key].Add(xml);
            }
        }

        private List<AnalyzeXML> analyzeSub(string txt)
        {
            List<AnalyzeXML> aaa = new List<AnalyzeXML>();
            int a1 = txt.LastIndexOf("&>");
            int a2 = txt.LastIndexOf(">");
            int b1 = txt.IndexOf("&<");
            int b2 = txt.IndexOf("<");
            if (a2 != -1 && b2 != -1)
            {
                if (a2 > a1)
                {
                    if ((b1 != -1 && b2 < b1) || b1 == -1)
                    {
                        AnalyzeXML a = new AnalyzeXML(txt, 0);
                        if (a.error != null)
                        {
                            error = a.error;
                            return aaa;
                        }
                        else
                            aaa.Add(a);
                        string keyEnd = "</" + a.key;
                        int end = txt.Length;
                        int start = txt.IndexOf(keyEnd) + keyEnd.Length;
                        while (start < end)
                        {
                            if (txt[start++] == '>')
                                break;
                        }
                        if (start < end)
                        {
                            string txt2 = txt.Substring(start, end - start);
                            List<AnalyzeXML> aaa2 = analyzeSub(txt2);
                            foreach (AnalyzeXML A in aaa2)
                            {
                                aaa.Add(A);
                                if (A.error != null)
                                    error = A.error;
                            }
                        }
                    }
                }
            }
            return aaa;
        }

        public List<AnalyzeXML> this[string key]
        {
            get
            {
                if (dic.ContainsKey(key))
                    return dic[key];
                else
                    return null;
            }
        }
        public override string ToString()
        {
            return value;
        }

        private bool check1(string value)
        {
            int a = 0;
            int b = 0;
            bool isValue = false;
            for (int i = 0; i < value.Length; i++)
            {
                char c1 = value[i];
                if (c1 == '"')
                    isValue = !isValue;
                if (isValue == false && c1 == '<')
                    a++;
                if (isValue == false && c1 == '>')
                    b++;
            }
            bool s = (isValue == false) && a == b;
            if (s == false)
                error = "数据格式异常";
            return s;
        }
        private bool check2(string value)
        {
            char c1 = ' ';
            char c2 = ' ';
            int tn = 0;
            int end = value.Length - 2;
            bool isValue = false;
            bool isIn = false;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (c == '"')
                {
                    isValue = !isValue;
                    continue;
                }
                if (isValue)
                    continue;
                if (c == '<')
                {
                    isIn = true;
                    tn = 0;
                }
                if (c == '>')
                    isIn = false;
                if (c <= ' ' && c1 <= ' ' && i != 0)
                    c1 = value[i - 1];
                if (c <= ' ' && c1 > ' ' && i <= end && value[i + 1] != ' ')
                    c2 = value[i + 1];
                if (c1 > ' ' && c2 > ' ' && isIn)
                {
                    if ((c1 == '"' && c2 != '"' && c2 != '=') ||
                        (c1 != '"' && c1 != '=' && c2 == '=') ||
                        (c1 == '=' && c2 == '"') ||
                        (c1 == '>' && c2 == '<'))
                    {
                        c1 = ' ';
                        c2 = ' ';

                    }
                    else if (c1 != '"' && c1 != '=' && c2 != '"' && c2 != '=')
                    {
                        if (tn == 0)
                        {
                            c1 = ' ';
                            c2 = ' ';
                        }
                        else
                        {
                            error = "数据格式异常，位置：" + i;
                            return false;
                        }
                        tn++;
                    }

                }
            }
            if (c1 > ' ' && c1 != '>' && c2 <= ' ')
            {
                error = "数据格式异常，位置：结尾附近";
                return false;
            }
            return true;
        }
    }

    public class AnalyzeXML_TEST
    {
        public static void test()
        {
            AnalyzeXML xml_base = new AnalyzeXML(File.ReadAllText("base.txt", Encoding.Default));
            AnalyzeXML xml_yin = new AnalyzeXML(File.ReadAllText("yin.txt", Encoding.Default));
            AnalyzeXML xml_yangw = new AnalyzeXML(File.ReadAllText("yangw.txt", Encoding.Default));
            AnalyzeXML xml_yangy = new AnalyzeXML(File.ReadAllText("yangy.txt", Encoding.Default));

            string txt2 = File.ReadAllText("测试数据3.txt", Encoding.Default);
            AnalyzeXML xml2 = new AnalyzeXML(txt2);
            if (xml2.error != null)
                Console.WriteLine(xml2.error);
            else
            {

                Console.WriteLine(xml2["NewDataSet"][0]["Table"][0]["PatientID"][0].ToString());

            }
        }
    }
}
