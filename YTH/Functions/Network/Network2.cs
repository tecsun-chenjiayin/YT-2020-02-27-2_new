using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using YTH.Functions;

namespace YTH.Network
{
    //吉林接口
    class Network2
    {
        static TimeSpan t1 = new TimeSpan(-10, 0, 0, 0);//登陆时间
        private static string token = null;
        public static bool checkIn(out string error)
        {
            error = null;

            Parameter.clear();
            Parameter.add("channelcode", Config.net_dic("channelcode"));
            Parameter.add("username", Config.net_dic("username"));
            Parameter.add("password", Config.net_dic("password"));

            string url = Config.net_dic("httpBase") + Config.net_dic("checkLogin");
            Common c = new Common();
            Uri uri = new Uri(url);
            StringBuilder inData = Parameter.get();
            try
            {
                Log.AddLog("POST", "URL:" + url);
                Log.AddLog("POST", "ARG:" + inData.ToString());
                string jsonStr = c.RetrunJSONValueByHttps(uri, inData, ref error);
                if(error != null)
                    jsonStr = c.RetrunJSONValueByHttps(uri, inData, ref error);
                if (error != null)
                {
                    error = "POST Error:" + error;
                    Log.AddLog("POST", error);
                    token = null;
                    return false;
                }
                else if (jsonStr == null)
                {
                    error = "ReturnError:接口返回了null";
                    Log.AddLog("POST", error);
                    token = null;
                    return false;
                }
                else
                {
                    Log.AddLog("POST", "Result:" + jsonStr);
                    MJson mJson = new MJson(jsonStr);
                    if (mJson["statusCode"].ToString() == "200")
                    {
                        token = mJson["data"].ToString();
                        t1 = new TimeSpan(DateTime.Now.Ticks);
                        return true;
                    }
                    else
                    {
                        error = mJson["message"].ToString();
                        token = null;
                        return false;
                    }
                }   
            }
            catch (Exception e)
            {
                if(error == null)
                {
                    error = "POST Throw Error:" + e.ToString();
                    Log.AddLog("POST", error);
                }
                token = null;
                return false;
            }
        }

        public static MJson Post(Dictionary<string, string> dic, string netKey)
        {
            string error = "";
            TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);
            if (token == null || t2.Subtract(t1).Duration().TotalMinutes >= 25)
            {
                if (checkIn(out error) == false)
                    return new MJson(error, false);
            }
            dic.Add("channelcode", Config.net_dic("channelcode"));
            dic.Add("deviceid", GetMacAddress());
            dic.Add("tokenid", token);

            try
            {
                Parameter.clear();
                foreach (KeyValuePair<string, string> kv in dic)
                {
                    Parameter.add(kv.Key, kv.Value);
                }
                string url = Config.net_dic("httpBase") + Config.net_dic(netKey) + "?deviceid=" + GetMacAddress() + "&tokenid=" + token;
                Common c = new Common();
                Uri uri = new Uri(url);
                StringBuilder inData = Parameter.get();
                Log.AddLog("POST", "URL:" + url);
                Log.AddLog("POST", "ARG:" + inData.ToString());
                string jsonStr = c.RetrunJSONValueByHttps(uri, inData, ref error);
                if(error != null)
                    jsonStr = c.RetrunJSONValueByHttps(uri, inData, ref error);
                if (error != null)
                {
                    error = "POST Error:" + error;
                    Log.AddLog("POST", error);
                    return new MJson(error, false);
                }
                else if (jsonStr == null)
                {
                    error = "ReturnError:接口返回了null";
                    Log.AddLog("POST", error);
                    return new MJson(error, false);
                }
                else
                {
                    Log.AddLog("POST", "Result:" + jsonStr);
                    return new MJson(jsonStr);
                }
            }
            catch (Exception e)
            {
                error = "POST Throw Error:" + e.ToString();
                Log.AddLog("POST", error);
                return new MJson(error, false);
            }
        }


        static string mac = null;
        /// <summary>获取网卡硬件地址</summary>
        public static string GetMacAddress()
        {
            if (mac != null) return mac;
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
    }
}
