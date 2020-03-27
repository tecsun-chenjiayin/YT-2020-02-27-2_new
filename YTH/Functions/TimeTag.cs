using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTH.Functions
{
    /// <summary>
    /// 时间标记
    /// </summary>
    class TimeTag
    {
        private string tag = "";
        public string updateTag()
        {
            tag = GetTime();
            return tag;
        }
        public string getTag()
        {
            return tag;
        }
        public bool equal(string timeTag)
        {
            return tag == timeTag;
        }

        public static string GetTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HHmmss");
        }

        public static string GetTime2()
        {
            return DateTime.Now.ToString(@"yyyy.MM.dd dddd");// + DateTime.Now.ToString("dddd");//2017-05-02星期二
        }

    }
}
