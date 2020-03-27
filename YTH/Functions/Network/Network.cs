using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using YTH.Functions;

namespace YTH
{
    class NetHandle
    {
        public static StringBuilder data = new StringBuilder();//请求参数
        //添加请求参数
        public static void AddFirstParameter(string name, string value)
        {
            data.Clear();
            data.Append("{\"" + name + "\":\"" + value);
        }
        public static void addParameter(string name, string value)
        {
            data.Append("\",\"" + name + "\":\"" + value);
        }
        public static void addParameterEnd()
        {
            data.Append("\"}");
        }
        public static void clearParameter()
        {
            data.Clear();
        }
        public static string ggetParameter() { return data.ToString(); } 
        //获取网络数据-方式2
        public static MJson Post(Dictionary<string,string> pairs, string key, out string error)
        {
            Log.AddLog("TEST", "4");
            string url = Config.net_dic("basePath") + Config.net_dic(key);
            clearParameter();
            bool isFirst = true;
            foreach (KeyValuePair<string,string> kv in pairs)
            {
                if (isFirst)
                {
                    AddFirstParameter(kv.Key, kv.Value);
                    isFirst = false;
                }
                else
                    addParameter(kv.Key, kv.Value);
            }
            addParameterEnd();
            string jsonParas = data.ToString();
            data.Clear();

            Log.AddLog("Post", "URL:" + url);
            Log.AddLog("Post", "Ins:" + jsonParas);
            error = null;
            try
            {
                string strURL = url;

                //创建一个HTTP请求  
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
                //Post请求方式  
                request.Method = "POST";
                //内容类型
                request.ContentType = "application/json";

                //设置参数，并进行URL编码 

                string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas);   

                byte[] payload;
                //将Json字符串转化为字节  
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                //设置请求的ContentLength   
                request.ContentLength = payload.Length;
                //发送请求，获得请求流 

                Stream writer;
                try
                {
                    writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
                }
                catch (Exception)
                {
                    writer = null;
                    Log.AddLog("Post", "Ret:连接服务器失败!");
                    error = "连接服务器失败";
                    return null;
                }
                //将请求参数写入流
                writer.Write(payload, 0, payload.Length);
                writer.Close();//关闭请求流
                //String strValue = "";//strValue为http响应所返回的字符流
                HttpWebResponse response;
                try
                {
                    //获得响应流
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    response = ex.Response as HttpWebResponse;
                }
                Stream s = response.GetResponseStream();
                StreamReader sRead = new StreamReader(s);
                string postContent = sRead.ReadToEnd();
                sRead.Close();
                Log.AddLog("Post", "Ret:" + postContent);
                MJson json = new MJson(postContent);
                if (json.error != null)
                    error = json.error;
                else if (json["result"].ToString() != "0")
                    error = json["message"].ToString();
                return json;
            }
            catch (Exception e)
            {
                error = e.ToString();
                Log.AddLog("Post", error);
            }
            return null;
        }
    }
}
