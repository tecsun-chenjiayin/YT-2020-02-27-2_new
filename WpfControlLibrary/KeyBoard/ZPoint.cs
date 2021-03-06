﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlLibrary.KeyBoard
{
    class ZPoint
    {
        static Dictionary<char, ZPoint> dic0 = new Dictionary<char, ZPoint>();
        static ZPoint()
        {

            string path = System.IO.Directory.GetCurrentDirectory() + @"\WpfControlLibrary.data";
            string[] lines = File.ReadAllLines(path, Encoding.Default);
            foreach (string line in lines)
            {
                string[] datas = line.Split('=');
                int i = 0;
                ZPoint p = null;
                foreach (char c in datas[0])
                {
                    if (i == 0)
                    {
                        if (dic0.ContainsKey(c) == false)
                        {
                            dic0.Add(c, new ZPoint());
                            dic0[c].key = c;
                        }
                        p = dic0[c];
                    }
                    else
                    {
                        if (p.dic.ContainsKey(c) == false)
                        {
                            p.dic.Add(c, new ZPoint());
                            p.key = c;
                        }
                        p = p.dic[c];
                    }
                    i++;
                    if (i == datas[0].Length)
                        p.values.Add(datas[1][0]);
                }
            }
        }
        public static List<char> getValues(string pinyin)
        {
            if (pinyin.Length == 0)
                return new List<char>();
            pinyin = pinyin.ToLower();
            ZPoint p = null;
            foreach (char c in pinyin)
            {
                if (p == null)
                {
                    if (dic0.ContainsKey(c) == false)
                        return new List<char>();
                    p = dic0[c];
                    continue;
                }
                if (p.dic.ContainsKey(c))
                    p = p.dic[c];
            }
            Dictionary<char, char> cs = new Dictionary<char, char>();
            getValues(p, cs);
            List<char> cs2 = new List<char>();
            foreach (KeyValuePair<char, char> kv in cs)
            {
                cs2.Add(kv.Value);
            }
            return cs2;

        }

        static void getValues(ZPoint p, Dictionary<char, char> cs)
        {
            foreach (char c in p.values)
            {
                if (cs.ContainsKey(c) == false)
                    cs.Add(c, c);
            }
            if (p.dic.Count != 0)
            {
                foreach (KeyValuePair<char, ZPoint> kv in p.dic)
                {
                    getValues(kv.Value, cs);
                }
            }
        }

        public char key = '0';
        public Dictionary<char, ZPoint> dic = new Dictionary<char, ZPoint>();
        public List<char> values = new List<char>();
    }
}
