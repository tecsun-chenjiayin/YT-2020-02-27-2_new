using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Management;
using System.Reflection;

namespace YTH.Functions
{
    class Network
    {
        static IniFile iniFile = null;
        static List<uint> tags = new List<uint>();//
        static object locker = new object();
        static TimeSpan t1 = new TimeSpan(-10,0,0,0);//登陆时间
        static string baseHttp = null;
        public static string token = "";
        static string basePath = null;
        public static string getBasePath()
        {
            if (basePath != null) return basePath;
            Assembly asm = Assembly.GetExecutingAssembly();
            basePath = Path.GetDirectoryName(asm.Location) + "\\";
            return basePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style">接口名称</param>
        /// <param name="kv">入参</param>
        /// <param name="error">报错信息</param>
        /// <param name="isGiveUp">是否放弃</param>
        /// <returns></returns>
        public static MJson getNetReturn(string urlM, Dictionary<string, string> inParams, out string error, out bool isGiveUp)
        {
            if (basePath == null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                basePath = Path.GetDirectoryName(asm.Location) + "\\";
            }
            if (iniFile == null)
            {
                iniFile = new IniFile(getBasePath() + "\\ini\\Version.ini");
            }

            error = null;
            isGiveUp = false;
            if (baseHttp == null)
                baseHttp = iniFile.IniReadValue("USER", "ip");
            try
            {
                //登陆
                bool isSucess = checkIn(out error);
                if (isSucess == false)
                    return new MJson("登陆失败", false);

                inParams.Add("channelcode", iniFile.IniReadValue("USER", "CHANNELCODE"));
                inParams.Add("deviceid", GetMacAddress());//ZK123   Config.net_dic("deviceid")
                inParams.Add("tokenid", token);

                //加载数据
                string url = baseHttp + urlM + "?deviceid=" + GetMacAddress() + "&tokenid=" + token;
                Common c = new Common();
                Uri uri = new Uri(url);
                StringBuilder inData = Parameter.get(inParams);

                string jsonStr = c.RetrunJSONValueByHttps(uri, inData, ref error);
                if (error != null)
                {
                    return new MJson(error, false);
                }
                else
                {
                    return new MJson(jsonStr);

                }
            }
            catch (Exception e)
            {
                if (tags.Count >= 1)
                    tags.RemoveAt(0);
                if (error == null)
                    error = e.ToString();
                else
                    error += e.ToString();
                if (error.IndexOf("time out") != -1)
                    error = "请求超时，请重试！";
                return new MJson(error, false);
            }
        }

        public static string save_token = "";

        public static bool checkIn(out string error)
        {
            error = null;
            Parameter.clear();
            Parameter.add("channelcode", iniFile.IniReadValue("USER", "CHANNELCODE"));
            Parameter.add("username", iniFile.IniReadValue("USER", "PROVINCEUSER"));
            Parameter.add("password", iniFile.IniReadValue("USER", "PROVINCEPWD"));

            string url = baseHttp + "/iface/user/checkLogin"; 
            Common c = new Common();
            Uri uri = new Uri(url);
            StringBuilder inData = Parameter.get();
            try
            {
                string jsonStr = c.RetrunJSONValueByHttps(uri, inData, ref error).ToString();
                MJson json = new MJson(jsonStr);

                if (json["statusCode"].ToString() == "200")
                {
                    token = json["data"].ToString();
                    return true;
                }
                else
                {
                    error = json["message"].ToString();
                    return false;
                }
            }
            catch (Exception e)
            {
                error = "登陆失败：请重试！";
                return false;
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
            //return "BFEBFBFF000306A9";
        }
    }

    public class Parameter
    {
        private static StringBuilder data = new StringBuilder();//请求参数
        private static List<KeyValuePair<string, string>> kvs = new List<KeyValuePair<string, string>>();
        public static void clear()
        {
            kvs.Clear();
        }
        public static void add(string key, string value)
        {
            kvs.Add(new KeyValuePair<string, string>(key, value));
        }
        public static StringBuilder get(Dictionary<string, string> inParams)
        {
            kvs.Clear();
            data.Clear();
            foreach (KeyValuePair<string, string> kv in inParams)
                kvs.Add(kv);
            return get();
        }
        public static StringBuilder get()
        {
            if (kvs.Count == 0) return new StringBuilder();
            data.Clear();
            if (kvs.Count >= 1)
                addFirstParameter(kvs[0].Key, kvs[0].Value);
            for (int i = 1; i < kvs.Count; i++)
                addParameter(kvs[i].Key, kvs[i].Value);
            addEndParameter();
            return data;
        }
        //添加请求参数
        private static void addFirstParameter(string name, string value)
        {
            data.Clear();
            data.Append("{\"" + name + "\":\"" + value);
        }
        private static void addParameter(string name, string value)
        {
            data.Append("\",\"" + name + "\":\"" + value);
        }
        private static void addEndParameter()
        {
            data.Append("\"}");
        }
        private static void clearParameter()
        {
            data.Clear();
        }
        private static string getParameter() { return data.ToString(); }
    }

    public class Common
    {
        public Common()
        { }

        /// <summary>
        /// 证书验证总是返回true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }

        /// <summary>
        /// https获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string RetrunJSONValueByHttps(Uri url, StringBuilder data,ref string error)
        {
            Log.AddLog("JSON", url.ToString());
            Log.AddLog("JSON", data.ToString());
            if (error != null) return null;
            try
            {
                //需要验证证书
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
                request.Method = "POST";
                request.Timeout = 60000;//60秒

                request.ContentType = "application/json";
                request.Accept = "application/json";

                byte[] byteData = Encoding.UTF8.GetBytes(data.ToString());
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                    postStream.Flush();
                    postStream.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string outString = reader.ReadToEnd();
                    response.Close();
                    Log.AddLog("JSON", outString);
                    return outString;
                }
            }
            catch (Exception e)
            {
                error = "接口调用异常：" + e.ToString();
                if (error.IndexOf("timed out") != -1)
                    error = "网络超时，请重试。";
                return null;
            }
        }


        /// <summary>        
        /// 将json转换为DataTable        
        /// </summary>        
        /// <param name="strJson">得到的json</param>        
        /// <returns></returns>        
        public DataTable JsonToDataTable(string strJson)
        {
            //  strJson = "{\"statusCode\": \"200\",\"message\": \"查询成功\",\"data\": {\"pageNo\": 1,\"pageSize\": 10,\"count\": 11,\"data\": [{ \"ssq\": \"201609\",\"xz\": \"城镇职工基本医疗保险\",\"jfjs\": \"6000\",\"dwjn\": \"100\",\"grjn\": \"50\",\"jnze\": \"150\" },{ \"ssq\": \"201609\",\"xz\": \"城镇职工基本医疗保险\",\"jfjs\": \"6000\",\"dwjn\": \"100\",\"grjn\": \"50\",\"jnze\": \"150\" }  ] }}";
            try
            {
                //转换json格式   
                string tstJson = strJson.Replace(",\"", "&\"").Replace("\":", "\"#").ToString();
                strJson = strJson.Replace(",\"", "&\"").Replace("\":", "\"#").ToString();

                //针对德生科技公司的json
                //strJson = strJson.Substring(strJson.IndexOf("\"data\""));//从data位置开始取数据

                //取出表名               
                var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
                string strName = rg.Match(strJson).Value;
                DataTable tb = null;
                //去除表名     
                try
                {
                    //有中括号的
                    strJson = strJson.Substring(strJson.IndexOf("[") + 1);
                    strJson = strJson.Substring(0, strJson.IndexOf("]"));
                }
                catch
                {
                    //没有中括号的
                    //data\"#
                    strJson = strJson.Substring(strJson.IndexOf("#{") + 1);
                    strJson = strJson.Substring(0, strJson.IndexOf("}") + 1);
                }
                //获取数据               
                rg = new Regex(@"(?<={)[^}]+(?=})");
                MatchCollection mc = rg.Matches(strJson);

                for (int i = 0; i < mc.Count; i++)
                {
                    string strRow = mc[i].Value;
                    // strRow = HttpUtility.HtmlDecode(strRow);
                    // LogHelper.WriteLog(typeof(MyCommon), string.Format("HttpUtility.HtmlDecode(strRow)   ") + strRow);
                    string[] strRows = strRow.Split('&');

                    //创建表                   
                    if (tb == null)
                    {
                        tb = new DataTable();
                        tb.TableName = strName;
                        foreach (string str in strRows)
                        {
                            var dc = new DataColumn();
                            string[] strCell = str.Split('#');
                            if (strCell[0].Substring(0, 1) == "\"")
                            {
                                int a = strCell[0].Length;
                                dc.ColumnName = strCell[0].Substring(1, a - 2).Trim();
                            }
                            else
                            {
                                dc.ColumnName = strCell[0].Trim();
                            }
                            tb.Columns.Add(dc);
                        }
                        tb.AcceptChanges();
                    }
                    //增加内容                   
                    DataRow dr = tb.NewRow();
                    for (int r = 0; r < strRows.Length; r++)
                    {
                        //strJson = HttpUtility.HtmlEncode(strJson);//System.Net.WebUtility.HtmlEncode(html);  
                        if (strRows[r].Contains("#"))
                            dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                    }
                    tb.Rows.Add(dr);
                    tb.AcceptChanges();
                }

                return tb;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static int getCount(string json)
        {
            Common c = new Common();
            return c.JsonToDataTable(json).Rows.Count;
        }
    }


}
