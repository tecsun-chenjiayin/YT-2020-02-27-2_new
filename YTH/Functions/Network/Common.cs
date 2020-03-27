using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace YTH.Network
{
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
        public string RetrunJSONValueByHttps(Uri url, StringBuilder data, ref string error)
        {
            error = null;
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

    }

}
