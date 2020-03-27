using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace YTH.Functions
{
    /// <summary>
    /// Json解析-htx
    /// </summary>
    //public class AnalyzeJson
    //{
    //    public string error = null;
    //    string val = "";
    //    StringBuilder key = new StringBuilder();
    //    StringBuilder value = new StringBuilder();
    //    List<AnalyzeJson> array = new List<AnalyzeJson>();
    //    Dictionary<string, AnalyzeJson> dic = new Dictionary<string, AnalyzeJson>();

    //    int big = 0;
    //    int middle = 0;

    //    bool isStart = false;

    //    public AnalyzeJson() { }

    //    public AnalyzeJson(string json)
    //    {
    //        val = json;
    //        if (json == null) return;
    //        //if (json.IndexOf("\\\"") != -1)
    //        //    json = json.Replace("\\\"", "\"");
    //        //if (json.IndexOf("\"{") != -1)
    //        //    json = json.Replace("\"{", "{");
    //        //if (json.IndexOf("\"}") != -1)
    //        //    json = json.Replace("}\"", "}");

    //        try
    //        {
    //            if (json[0] == '[')
    //            {
    //                handleArray(json);
    //                return;
    //            }

    //            int length = json.Length;

    //            for (int i = 0; i < length; i++)
    //            {
    //                bool isNext = false;
    //                //跳过开头
    //                while (i < length && json[i++] != '"') ;
    //                //读名称
    //                while (i < length && json[i] != '"')
    //                    key.Append(json[i++]);
    //                while (i < length && json[i++] != ':') ;
    //                //读内容
    //                while (i < length && json[i] != '"' && json[i] != '[' && json[i] != '{')
    //                {
    //                    //null
    //                    if (json[i] == 'n' &&
    //                        json[i + 1] == 'u' &&
    //                        json[i + 2] == 'l' &&
    //                        json[i + 3] == 'l')
    //                    {
    //                        dic.Add(key.ToString(), null);
    //                        key.Clear();
    //                        value.Clear();
    //                        i += 3;
    //                        isNext = true;
    //                        break;
    //                    }
    //                    //bool
    //                    else if (json[i] == 't' &&
    //                        json[i + 1] == 'r' &&
    //                        json[i + 2] == 'u' &&
    //                        json[i + 3] == 'e')
    //                    {
    //                        dic.Add(key.ToString(), new AnalyzeJson("true"));
    //                        key.Clear();
    //                        value.Clear();
    //                        i += 3;
    //                        isNext = true;
    //                        break;
    //                    }
    //                    //bool
    //                    else if (json[i] == 'f' &&
    //                        json[i + 1] == 'a' &&
    //                        json[i + 2] == 'l' &&
    //                        json[i + 3] == 's' &&
    //                        json[i + 4] == 'e')
    //                    {
    //                        dic.Add(key.ToString(), new AnalyzeJson("false"));
    //                        key.Clear();
    //                        value.Clear();
    //                        i += 4;
    //                        isNext = true;
    //                        break;
    //                    }
    //                    //数值
    //                    if (json[i] >= '0' && json[i] <= '9')
    //                    {
    //                        while (json[i] != ',' && json[i] != '}')
    //                            value.Append(json[i++]);
    //                        i--;
    //                        dic.Add(key.ToString(), new AnalyzeJson(value.ToString().Trim()));
    //                        key.Clear();
    //                        value.Clear();
    //                        isNext = true;
    //                        break;
    //                    }
    //                    ///////////////////////////////////////////////////////////
    //                    i++;
    //                }
    //                if (isNext) continue;
    //                i++;
    //                if (i - 1 > length || i > length) return;
    //                if (json[i - 1] == '"')
    //                {
    //                    int endI = findEnd(json, i - 1);

    //                    //while (i < length && json[i] != '"')
    //                    //    value.Append(json[i++]);
    //                    while(i < endI)
    //                        value.Append(json[i++]);
    //                    dic.Add(key.ToString(), new AnalyzeJson(value.ToString(), true));
    //                    key.Clear();
    //                    value.Clear();
    //                }
    //                else if (json[i - 1] == '{')
    //                {
    //                    value.Append("{");
    //                    while (
    //                        (i < length && json[i] != '}' && isStart == false) ||
    //                        (i < length && json[i] == '}' && isStart == false && big != 0) ||
    //                        (i < length && isStart))
    //                    {
    //                        if (isStart == false && json[i] == '{')
    //                            big++;
    //                        if (isStart == false && json[i] == '}')
    //                            big--;
    //                        value.Append(json[i++]);
    //                    }
    //                    value.Append("}");
    //                    AnalyzeJson mj = new AnalyzeJson(value.ToString());
    //                    value.Clear();
    //                    dic.Add(key.ToString(), mj);
    //                    key.Clear();
    //                    value.Clear();
    //                }
    //                else
    //                {
    //                    value.Append("[");
    //                    while (
    //                        (i < length && json[i] != ']' && isStart == false) ||
    //                        (i < length && json[i] == ']' && isStart == false && middle != 0) ||
    //                        (i < length && isStart))
    //                    {
    //                        if (isStart == false && json[i] == '[')
    //                            middle++;
    //                        if (isStart == false && json[i] == ']')
    //                            middle--;
    //                        value.Append(json[i++]);
    //                    }
    //                    value.Append("]");
    //                    AnalyzeJson mj = new AnalyzeJson(value.ToString());
    //                    value.Clear();
    //                    dic.Add(key.ToString(), mj);
    //                    key.Clear();
    //                    value.Clear();
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            error = e.ToString();
    //        }
    //    }

    //    private int findEnd(string json, int start)
    //    {
    //        int num = 0;
    //        while(start < json.Length)
    //        {
    //            char c = json[start++];
    //            char c2 = json[start];
    //            if(c == '\"' && num == 0)
    //            {
    //                num++;
    //            }
    //            else if(c == '\"' && num == 1)
    //            {
    //                return start - 1;
    //            }
    //            else if(c == '\\' && c2 == '\"')
    //            {
    //                start++;
    //            }
    //        }
    //        return 0;
    //    }
    //    public void handleArray(string json)
    //    {
    //        int i = 0;
    //        int length = json.Length;
    //        while (i < length)
    //        {
    //            while (i < length && json[i++] != '{') ;
    //            value.Append("{");
    //            while (
    //                (i < length && json[i] != '}' && isStart == false) ||
    //                (i < length && json[i] == '}' && isStart == false && big != 0) ||
    //                (i < length && isStart))
    //            {
    //                if (json[i] == '"' && isStart == false)
    //                    isStart = true;
    //                else if (json[i] == '"' && isStart && json[i - 1] != '\\')
    //                    isStart = false;

    //                if (isStart == false && json[i] == '{')
    //                    big++;
    //                if (isStart == false && json[i] == '}')
    //                    big--;
    //                value.Append(json[i++]);
    //            }
    //            value.Append("}");
    //            if (i >= length)
    //            {
    //                value.Clear();
    //                return;
    //            }
    //            array.Add(new AnalyzeJson(value.ToString()));
    //            value.Clear();
    //        }
    //    }

    //    public AnalyzeJson(string value, bool isValue)
    //    {
    //        if (isValue)
    //            val = value;
    //        else
    //        {
    //            val = "";
    //            error = value;
    //        }
    //    }

    //    public AnalyzeJson this[int index]
    //    {
    //        get
    //        {
    //            if (error != null)
    //                return new AnalyzeJson(error, false);
    //            else if (array.Count <= index)
    //                return new AnalyzeJson("##不存在的下标:" + index + "##", false);
    //            else if (array[index] == null)
    //                return new AnalyzeJson(null);
    //            else
    //                return array[index];
    //        }
    //    }
    //    public AnalyzeJson this[string key]
    //    {
    //        get
    //        {
    //            if (error != null)
    //                return new AnalyzeJson(error, false);
    //            else if (dic.ContainsKey(key) == false)
    //                return new AnalyzeJson("##不存在的键:" + key + "##", false);
    //            else if (dic[key] == null)
    //                return new AnalyzeJson(null);
    //            else
    //                return dic[key];
    //        }

    //    }
    //    public int getArrayCount()
    //    {
    //        return array.Count;
    //    }

    //    public override string ToString()
    //    {
    //        if (error != null)
    //            return error;
    //        return val;
    //    }
    //}
}