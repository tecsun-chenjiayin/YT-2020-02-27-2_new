using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tools
{
    public enum DataStyle
    {
        BOOL = 1,//逻辑值（true 或 false）
        INT = 2,//整数
        DOUBLE = 3,//浮点数
        STR = 4,//字符串
        DATE = 5,//
        ARRAY = 6,//数组（MakeJson[]）
        DIC = 7,//嵌套一个json（MakeJson）
        NULL = 8,//
        NO = 9,
        LONG = 10
    }
    public class MakeJson
    {
        string error = null;
        DataStyle style = DataStyle.NO;
        string str = "";
        bool bValue = false;
        int num1 = 0;
        double num2 = 0;
        long num3 = 0;
        MakeJson[] array = null;
        public Dictionary<string, MakeJson> dic = null;

        public MakeJson(bool val)
        {
            style = DataStyle.BOOL;
            bValue = val;
        }
        public MakeJson(int num)
        {
            style = DataStyle.INT;
            num1 = num;
        }
        public MakeJson(long num)
        {
            style = DataStyle.LONG;
            num3 = num;
        }
        public MakeJson(double num)
        {
            style = DataStyle.DOUBLE;
            num2 = num;
        }
        public MakeJson(string str)
        {
            if (str == null)
                style = DataStyle.NULL;
            else
            {
                style = DataStyle.STR;
                this.str = str;
            }
        }

        public MakeJson()
        {
            style = DataStyle.DIC;
            dic = new Dictionary<string, MakeJson>();
        }

        public void add(string key, object val, DataStyle style)
        {
            try
            {
                switch (style)
                {
                    case DataStyle.BOOL:
                        dic.Add(key, new MakeJson((bool)val));
                        break;
                    case DataStyle.STR:
                    case DataStyle.DATE:
                        {
                            string val2 = val.ToString();
                            if (val2.Contains("\""))
                            {
                                for (int i = val2.Length - 1; i >= 0; i--)
                                {
                                    if ((val2[i] == '\"' && i == 0) || (val2[i] == '\"' && i > 0 && val2[i - 1] != '\\'))
                                        error = "添加了不符合要求的值：" + val2;
                                }
                            }
                            MakeJson j = new MakeJson(val2.ToString());
                            dic.Add(key, j);
                            break;
                        }
                    case DataStyle.INT:
                        dic.Add(key, new MakeJson((int)val));
                        break;
                    case DataStyle.DOUBLE:
                        dic.Add(key, new MakeJson((double)val));
                        break;
                    case DataStyle.ARRAY:
                        {
                            MakeJson j = new MakeJson();
                            j.array = (MakeJson[])val;
                            j.style = style;
                            dic.Add(key, j);
                            break;
                        }
                    case DataStyle.DIC:
                        dic.Add(key, (MakeJson)val);
                        break;
                    case DataStyle.NULL:
                        dic.Add(key, new MakeJson(null));
                        break;
                    case DataStyle.LONG:
                        dic.Add(key, new MakeJson((long)val));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                if (key != null)
                    Log.AddLog("Json", "Key:" + key);
                if (val != null)
                    Log.AddLog("Json", "Val:" + val.ToString());
                Log.AddLog("Json", "josn添加节点异常");
                error = "josn添加节点异常：" + e.ToString();
            }
        }
        public void add(string key, string val, DataStyle style)
        {
            if (style == DataStyle.INT)
                add(key, (object)(int.Parse(val)), style);
            else if (style == DataStyle.LONG)
                add(key, (object)(long.Parse(val)), style);
            else if (style == DataStyle.DOUBLE)
                add(key, (object)(double.Parse(val)), style);
            else
                add(key, (object)val, style);
        }
        public void add(string key, string val)
        {
            add(key, val, DataStyle.STR);
        }
        public void add(string key, int val)
        {
            add(key, val, DataStyle.INT);
        }
        public override string ToString()
        {
            try
            {
                if (error != null)
                    return error;
                else if (style == DataStyle.BOOL)
                {
                    return bValue ? "true" : "false";
                }
                else if (style == DataStyle.DATE)
                {
                    return str;
                }
                else if (style == DataStyle.STR)
                {
                    return "\"" + str + "\"";
                }
                else if (style == DataStyle.INT)
                {
                    return num1.ToString();
                }
                else if (style == DataStyle.LONG)
                {
                    return num3.ToString();
                }
                else if (style == DataStyle.DOUBLE)
                {
                    return num2.ToString();
                }
                else if (style == DataStyle.ARRAY)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[");
                    for (int i = 0; i < array.Length; i++)
                    {
                        sb.Append(array[i].ToString());
                        if (i != array.Length - 1)
                            sb.Append(",");
                    }
                    sb.Append("]");
                    return sb.ToString();
                }
                else if (style == DataStyle.DIC)
                {
                    StringBuilder sb = new StringBuilder();
                    int index = 1;
                    sb.Append("{");
                    foreach (KeyValuePair<string, MakeJson> kv in dic)
                    {
                        sb.Append("\"" + kv.Key + "\":");
                        sb.Append(kv.Value.ToString());
                        if (index != dic.Count)
                            sb.Append(",");
                        index++;
                    }
                    sb.Append("}");
                    return sb.ToString();
                }
                else if (style == DataStyle.NULL)
                    return "null";
                else
                    return "";
            }
            catch (Exception e)
            {
                error = "生成json字符串异常：" + e.ToString();
                return "";
            }
        }
    }
}
