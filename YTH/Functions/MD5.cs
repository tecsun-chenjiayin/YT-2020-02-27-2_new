using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTH.NHNet
{
    class MD5
    {
        ///MD5加密
        public static string GetMD5String(string input)
        {
            //1.创建一个md5对象
            System.Security.Cryptography.MD5 md5Obj = System.Security.Cryptography.MD5.Create();
            //1.1把字符串转换为byte[]
            byte[] buffer = System.Text.Encoding.Default.GetBytes(input);

            //2.通过md5对象计算给定值的md5
            byte[] md5Buffer = md5Obj.ComputeHash(buffer);
            //把byte[]数组转换为字符串
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Buffer.Length; i++)
            {
                sb.Append(md5Buffer[i].ToString("x2"));
            }
            //3.释放资源
            md5Obj.Clear();
            // return sb.ToString();
            return BitConverter.ToString(md5Buffer).Replace("-", "").ToLower();
        }
    }
}
