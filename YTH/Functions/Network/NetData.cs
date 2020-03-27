using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using YTH.Functions;

namespace YTH.Network
{
    class NetData
    {
        public static ThreadProperty loginTP = new ThreadProperty(50, false, true, NetData.DeviceLogin, null);//登录线程信息

        public const string timeOutTag = "TimeOut";//等待获取Device超时标记
        public static string deviceID = null;
        private static string wdno = null;//网点编号
        private static string wdmc = null;//网点名称
        private static string deviceLoginError = "";
        private static bool isTryingLogin = false;

        static string mac = null;
        /// <summary>获取网卡硬件地址</summary>
        public static string GetMacAddress()
        {
            if (mac != null) return mac;
            if (Config.net_dic("deviceMAC") != "")
            {
                mac = Config.net_dic("deviceMAC");
                return mac;
            }
                
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                mac = mac.Replace(':', '-');
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        //设备登陆
        public static void DeviceLogin()
        {
            
            if (isDemo() || isTryingLogin) return;
            isTryingLogin = true;

            string deviceIP = null;
            string deviceMAC = null;
            string hardver = null;
            string softver = null;
            string status = null;
            bool isUseTestData = Config.net_dic("IsUseTestData") == "1";
            if (isUseTestData)
            {
                deviceIP = Config.net_dic("deviceIP");
                deviceMAC = Config.net_dic("deviceMAC");
                hardver = Config.net_dic("hardver");
                softver = Config.net_dic("softver");
                status = Config.net_dic("status");
            }
            else
            {

            }

            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["deviceIP"] = deviceIP;
            kv["deviceMAC"] = deviceMAC;
            kv["hardver"] = hardver;
            kv["softver"] = softver;
            kv["status"] = status;
            string error = null;

            //test
            //return;
            MJson mj = NetHandle.Post(kv, "DeviceLogin", out error);
            if (error != null)
            {
                deviceLoginError = error;
                TipWin.showTip(error, 5000, null);
            }
            else
            {
                deviceLoginError = null;
                deviceID = mj["data"]["deviceID"].ToString();
                wdno = mj["data"]["wdno"].ToString();
                wdmc = mj["data"]["wdmc"].ToString();
            }
            isTryingLogin = false;
            loginTP.resetTime(6000);
            if (deviceID != null && deviceID != "")
            {
                loginTP.stop();
            }
            ////////////[TEST]///////////////////////
            //MJson ret0 = UserLogin("100001", "userName", "password");
            //MJson ret1 = getCPCarMsg("412201199004126586", "张三");
            //UpdateRecord_Test();

            /////////////////////////////////////////
        }
        //查询成品卡库
        public static MJson getCPCarMsg(string idcar, string name)
        {
            NullToEmpty(ref idcar);
            NullToEmpty(ref name);
            if (idcar == "" || name == "")
            {
                Log.AddLog("NetData", "getCPCarMsg：身份证信息为空");
                return ErrorMJ("身份证信息为空");
            }
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["deviceID"] = deviceID;
            kv["idcard"] = idcar;
            kv["name"] = name;
            MJson mj = getResult(kv, "GetCardInfoCP");
            return mj;
        }
        //查询制卡数据
        public static MJson getUserData(string idcar, string name, string qrcode,string bankNo)
        {
            if (isDemo()) return null;
            NullToEmpty(ref idcar);
            NullToEmpty(ref name);
            NullToEmpty(ref qrcode);
            if ((idcar == "" || name == "") && qrcode == "")
            {
                Log.AddLog("NetData", "getUserData：身份证和二维码信息都为空");
                return ErrorMJ("身份证和二维码信息都为空");
            }
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["deviceID"] = deviceID;
            kv["idcard"] = idcar;
            kv["name"] = name;
            kv["qrcode"] = qrcode;
            kv["bankno"] = bankNo;
            kv["cjwdno"] = wdno;
            MJson mj = getResult(kv, "GetUerCardData");
            return mj;
        }
        //上传发卡信息
        private static void UpdateRecord_Test()
        {
            Dictionary<string, string> ins = new Dictionary<string, string>();
            ins["deviceID"] = "000001";
            ins["idcard"] = "412201199004126586";
            ins["name"] = "张三";
            ins["atr"] = "atr";
            ins["ksbm"] = "ksbm";
            ins["batchno"] = "batchno";
            ins["status"] = "status";
            ins["discrip"] = "discrip";
            ins["time"] = "time";
            ins["cardtype"] = "cardtype";
            ins["applytype"] = "applytype";
            ins["signature"] = "signature";
            MJson mj = UpdateRecord(ins);
        }
        public static MJson UpdateRecord(Dictionary<string,string> ins)
        {
            if (isDemo()) return null;
            if (ins == null || ins.Count == 0)
                return ErrorMJ("错误的入参");
            MJson mj = getResult(ins, "UpdateRecord");
            return mj;
        }
        //用户登录
        public static MJson UserLogin(string userID,string userName, string password)
        {
            NullToEmpty(ref userID);
            NullToEmpty(ref userName);
            NullToEmpty(ref password);
            if (isDemo()) return null;
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["userID"] = userID;
            kv["userName"] = userName;
            kv["password"] = password;
            MJson mj = getResult(kv, "UserLogin");
            return mj;
        }
        //查询所有制卡信息
        public static MJson getAllCarMakeCarMsg(string startdate, string enddate, string idno)
        {
            MJson mjError = hadLogin();
            if (mjError != null)
                return mjError;
            NullToEmpty(ref startdate);
            NullToEmpty(ref enddate);
            NullToEmpty(ref idno);
            if (isDemo()) return null;
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["startdate"] = startdate;
            kv["enddate"] = enddate;
            kv["idno"] = idno;
            kv["userID"] = deviceID;
            kv["token"] = "";
            MJson mj = getResult(kv, "GetAllRecord");
            return mj;
        }
        //校验管理员密码
        public static MJson checkPsw(string psw)
        {
            Log.AddLog("TEST", psw);
            DeviceLogin();
            if (deviceLoginError != null)
                return ErrorMJ(deviceLoginError);
            Log.AddLog("TEST", "1");
            //if (mjError != null)
            //{
            //    Log.AddLog("TEST", mjError.error);
            //    return mjError;
            //}
                
            NullToEmpty(ref psw);
            Log.AddLog("TEST", "11");
            if (isDemo()) return null;
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["userID"] = deviceID;
            kv["userName"] = deviceID;
            kv["password"] = psw;
            Log.AddLog("TEST", "2");
            MJson mj = getResult(kv, "UserLogin");
            return mj;
        }
        //回盘
        public static MJson dataReport(Dictionary<string,string> dic)
        {
            if (isDemo()) return null;
            MJson mj = getResult(dic, "DataReport");
            return mj;
        }
        //上传
        public static MJson updateRecord(Dictionary<string,string> dic)
        {
            if (isDemo()) return null;
            dic.Add("deviceID", deviceID);
            MJson mj = getResult(dic, "UpdateRecord");
            return mj;
        }
        //查询成品卡信息
        public static MJson searchCarMsg(string persion, string name)
        {
            NullToEmpty(ref persion);
            NullToEmpty(ref name);
            if (isDemo()) return null;
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["deviceID"] = deviceID;
            kv["idcard"] = persion;
            kv["name"] = name;
            MJson mj = getResult(kv, "GetCardInfoCP");
            return mj;
        }
        //上传成品卡信息
        public static MJson updateCarMsg(string atr,string ksbm,string persionid,string name, string ch)
        {
            NullToEmpty(ref atr);
            NullToEmpty(ref ksbm);
            NullToEmpty(ref persionid);
            NullToEmpty(ref name);
            NullToEmpty(ref ch);
            if (isDemo()) return null;
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv["deviceID"] = deviceID;
            kv["atr"] = atr;
            kv["ksbm"] = ksbm;
            kv["shbzh"] = persionid;
            kv["xm"] = name;
            kv["slotno"] = ch;
            MJson mj = getResult(kv, "UpdateCardInfoCP");
            return mj;
        }

        //其他///////////////////////////////////////
        private static MJson getResult(Dictionary<string,string> kv, string urlKey)
        {
            MJson mjError = hadLogin();
            Log.AddLog("TEST", "3");
            if (mjError != null)
                return mjError;
            string error = null;
            MJson mj = NetHandle.Post(kv, urlKey, out error);
            if (error != null)
                return ErrorMJ(error);
            return mj;
        }

        private static bool isDemo()
        {
            return Config.net_dic("isDemo") == "1";
        }

        private static MJson hadLogin()
        {
            if (deviceID != null && deviceID != "")
                return null;
            string timeTag = CD.timeTag.getTag();
            int t1 = 20;
            int t2 = 0;
            while (timeTag == CD.timeTag.getTag() && deviceLoginError != null && (t2++) < t1)
                Thread.Sleep(500);
            if (timeTag != CD.timeTag.getTag())
                return ErrorMJ(timeOutTag);
            //if (deviceLoginError != null)
            //{
            //    DeviceLogin();
            //    if (deviceLoginError != null)
            //        return ErrorMJ(deviceLoginError);
            //}
            return null;
        }

        private static MJson ErrorMJ(string error)
        {
            return new MJson() { error = error };
        }

        private static void NullToEmpty(ref string str)
        {
            if (str == null)
                str = "";
        }

        public static bool hasLogin()
        {
            return deviceID != null && deviceID != "";
        }
    }
}
