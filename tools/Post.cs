using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Windows;

namespace tools
{
    public class Post
    {
        public static string TransferEncoding(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            byte[] bytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
            return dstEncoding.GetString(bytes);
        }
        public static string Post_Form(string Url, string jsonParas, out string error)
        {
            error = null;
            try
            {
                Log.AddLog("Post", "URL:" + Url);
                Log.AddLog("Post", "IN:" + jsonParas);
                string strURL = Url;

                //创建一个HTTP请求  
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
                //Post请求方式  
                request.Method = "POST";
                //内容类型
                request.ContentType = "application/x-www-form-urlencoded";
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
                catch (Exception e)
                {
                    writer = null;
                    error = "连接服务器失败:"+ e.ToString();
                    return null;
                }
                //将请求参数写入流
                writer.Write(payload, 0, payload.Length);
                writer.Close();//关闭请求流

                HttpWebResponse response;
                try
                {
                    //获得响应流
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    response = ex.Response as HttpWebResponse;
                    error = "接收服务器数据异常:" + ex.ToString();
                    return null;
                }

                Stream s = response.GetResponseStream();
                StreamReader sRead = new StreamReader(s);
                string postContent = sRead.ReadToEnd();
                Log.AddLog("Post", "Result:" + postContent);
                sRead.Close();
                return postContent;
            }
            catch (Exception e)
            {
                error = e.ToString();
            }
            finally
            {
                if (error != null)
                    Log.AddLog("Post", error);
            }
            return null;
        }
        public static string Post_Json(string strURL, string jsonParas, out string error)
        {
            error = null;
            try
            {
                Log.AddLog("Post", "URL:" + strURL);
                Log.AddLog("Post", "IN:" + jsonParas);

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
                catch (Exception e)
                {
                    writer = null;
                    error = "连接服务器失败:" + e.ToString();
                    return null;
                }
                //将请求参数写入流
                writer.Write(payload, 0, payload.Length);
                writer.Close();//关闭请求流

                HttpWebResponse response;
                try
                {
                    //获得响应流
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    response = ex.Response as HttpWebResponse;
                    error = "接收服务器数据异常!" + ex.ToString();
                    return null;
                }

                Stream s = response.GetResponseStream();
                StreamReader sRead = new StreamReader(s);
                string postContent = sRead.ReadToEnd();
                sRead.Close();
                Log.AddLog("Post", "Result:" + postContent);
                return postContent;
            }
            catch (Exception e)
            {
                error = e.ToString();
            }
            finally
            {
                if (error != null)
                    Log.AddLog("Post", error);
            }
            return null;
        }

    }
}
