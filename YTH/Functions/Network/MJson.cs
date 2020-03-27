using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTH
{
    /// <summary>
    /// Json解析-htx
    /// </summary>
    class MJson
    {
        public string error = null;
        string val = "";
        StringBuilder key = new StringBuilder();
        StringBuilder value = new StringBuilder();
        List<MJson> array = new List<MJson>();
        Dictionary<string, MJson> dic = new Dictionary<string, MJson>();

        int big = 0;
        int middle = 0;

        bool isStart = false;

        public MJson() { }

        public MJson(string json)
        {
            val = json;
            if (json == null) return;
            try
            {
                if (json[0] == '[')
                {
                    handleArray(json);
                    return;
                }

                int length = json.Length;
                
                for (int i = 0; i < length; i++)
                {
                    bool isNext = false;
                    while (i < length && json[i++] != '"') ;
                    while (i < length && json[i] != '"')
                        key.Append(json[i++]);
                    while (i < length && json[i++] != ':') ;
                    while (i < length && json[i] != '"' && json[i] != '[' && json[i] != '{')
                    {
                        if(json[i] == 'n' &&
                            json[i + 1] == 'u' &&
                            json[i + 2] == 'l' &&
                            json[i + 3] == 'l')
                        {
                            dic.Add(key.ToString(), null);
                            key.Clear();
                            value.Clear();
                            i += 3;
                            isNext = true;
                            break;
                        }
                        if (json[i] >= '0' && json[i] <= '9')
                        {
                            while (json[i] != ',' && json[i] != '}')
                                value.Append(json[i++]);
                            i--;
                            dic.Add(key.ToString(), new MJson(value.ToString().Trim()));
                            key.Clear();
                            value.Clear();
                            isNext = true;
                            break;
                        }
                            ///////////////////////////////////////////////////////////
                        i++;
                    }
                    if (isNext) continue;
                    i++;
                    if (i - 1 > length || i > length) return;
                    if (json[i - 1] == '"')
                    {
                        while (i < length && json[i] != '"')
                            value.Append(json[i++]);
                        dic.Add(key.ToString(), new MJson(value.ToString(), true));
                        key.Clear();
                        value.Clear();
                    }
                    else if(json[i-1] == '{')
                    {
                        value.Append("{");
                        while (
                            (i < length && json[i] != '}' && isStart == false) ||
                            (i < length && json[i] == '}' && isStart == false && big != 0) ||
                            (i < length && isStart))
                        {
                            if (isStart == false && json[i] == '{')
                                big++;
                            if (isStart == false && json[i] == '}')
                                big--;
                            value.Append(json[i++]);
                        }
                        value.Append("}");
                        MJson mj = new MJson(value.ToString());
                        value.Clear();
                        dic.Add(key.ToString(), mj);
                        key.Clear();
                        value.Clear();
                    }
                    else
                    {
                        value.Append("[");
                        while (
                            (i < length && json[i] != ']' && isStart == false) ||
                            (i < length && json[i] == ']' && isStart == false && middle != 0) ||
                            (i < length && isStart))
                        {
                            if (isStart == false && json[i] == '[')
                                middle++;
                            if (isStart == false && json[i] == ']')
                                middle--;
                            value.Append(json[i++]);
                        }
                        value.Append("]");
                        MJson mj = new MJson(value.ToString());
                        value.Clear();
                        dic.Add(key.ToString(), mj);
                        key.Clear();
                        value.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                error = e.ToString();
            }
        }

        public void handleArray(string json)
        {
            int i = 0;
            int length = json.Length;
            while (i < length)
            {
                while (i < length && json[i++] != '{') ;
                value.Append("{");
                while (
                    (i < length && json[i] != '}' && isStart == false) ||
                    (i < length && json[i] == '}' && isStart == false && big != 0) ||
                    (i < length && isStart))
                {
                    if (json[i] == '"' && isStart == false)
                        isStart = true;
                    else if (json[i] == '"' && isStart && json[i - 1] != '\\')
                        isStart = false;

                    if (isStart == false && json[i] == '{')
                        big++;
                    if (isStart == false && json[i] == '}')
                        big--;
                    value.Append(json[i++]);
                }
                value.Append("}");
                if (i >= length)
                {
                    value.Clear();
                    return;
                }
                array.Add(new MJson(value.ToString()));
                value.Clear();
            }
        }

        public MJson(string value, bool isValue)
        {
            if (isValue)
                val = value;
            else
            {
                val = "";
                error = value;
            }
        }

        public MJson this[int index]
        {
            get
            {
                if (error != null)
                    return new MJson(error, false);
                else if (array.Count <= index)
                    return new MJson("##不存在的下标:" + index + "##", false);
                else if (array[index] == null)
                    return new MJson(null);
                else
                    return array[index];
            }
        }
        public MJson this[string key]
        {
            get
            {
                if (error != null)
                    return new MJson(error, false);
                else if (dic.ContainsKey(key) == false)
                    return new MJson("##不存在的键:" + key + "##", false);
                else if(dic[key] == null)
                    return new MJson(null);
                else
                    return dic[key];
            }

        }
        public int getArrayCount()
        {
            return array.Count;
        }

        public override string ToString()
        {
            if (error != null)
                return error;
            return val;
        }
    }
}
