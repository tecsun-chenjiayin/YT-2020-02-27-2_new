using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTH.Controls;

namespace YTH.Functions
{
    //CommonData-常用数据
    class CD
    {
        //一些常用跨类方法////////////////////////////////////
        /// Action
        public static Action returnToMain = null;//返回主页

        /// Set
        public delegate void setValue(object val);

        public static setValue setMainUI = null;//设置主UI
        public static setValue setTopUI = null;
        ////////////////////////////////////////////////////////
                                                                                                                                                                                                                                                 

        //基础路径
        private static string basePath = null;
        public static string getBasePath()
        {
            if (basePath != null) return basePath;
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            basePath = System.IO.Path.GetDirectoryName(asm.Location) + "\\";
            return basePath;
        }

        public static string hidenName(string name)
        {
            string name2 = "";
            if (name == null || name.Length <= 2)
                return name;
            name2 = name[0].ToString();
            int num = 0;
            if (name.Length > 2)
                num = name.Length - 2;
            else
                num = 1;
            for(int i = 0; i < num; i++)
                name2 += "*";
            if (name.Length > 2)
                name2 += name[name.Length - 1].ToString();
            return name2;
        }
        public static string hidenBankNum(string bankid)
        {
            string bankid2 = "";
            if (bankid == null || bankid.Length < 8)
                return bankid;
            bankid2 = bankid.Substring(0, 4);
            int num = bankid.Length - 8;
            for(int i = 0; i < num; i++)
            {
                bankid2 += "*";
            }
            bankid2 += bankid.Substring(bankid.Length - 4, 4);
            return bankid2;
        }

        public static string hidenPhone(string phone)
        {
            if (phone == null || phone.Length <= 2)
                return phone;
            int hidenNum = phone.Length / 2;
            int start = (phone.Length - hidenNum) / 2;
            int end = (phone.Length - hidenNum - start);
            string phone2 = phone.Substring(0, start);
            for (int i = 0; i < hidenNum; i++)
                phone2 += "*";
            phone2 += phone.Substring(phone.Length - end, end);
            return phone2;
        }

        ////常驻线程-UI
        //public static ForeverThread ftUI = new ForeverThread();
        ////常驻线程-数据加载
        //public static ForeverThread ftData = new ForeverThread();
        //公用时间标记
        public static TimeTag timeTag = new TimeTag();
        //业务区1
        public static Business business1 = null;
        //业务区2
        public static Business2 business2 = null;

        //倒计时
        public static CountDownTime countDownTime = null;

        //制卡信息
        public static Dictionary<string, string> makeCarDic = new Dictionary<string, string>();
    }
}
