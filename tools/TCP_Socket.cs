using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace tools
{
    //客户端
    class TCP_Socket
    {
        Socket socketSend = null;
        string connect_error = null;
        string send_error = null;
        string recive_error = null;
        string recive_msg = null;
        int tag_base = 0;
        int tag_send = 0;
        int tag_recv = 0;
        int tag_conn = 0;
        string ip_str = "";
        string port_str = "";

        public TCP_Socket(string ip, string port)
        {
            ip_str = ip;
            port_str = port;
        }

        //连接
        private string connect()
        {
            if (socketSend != null && send_error == null)
                return null;
            send_error = null;
            tag_conn = tag_base;
            try
            {
                if (socketSend != null)
                    socketSend.Close();

                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(ip_str);
                socketSend.Connect(ip, int.Parse(port_str));
                //Thread Receive = new Thread(new ThreadStart(recive));
                //Receive.IsBackground = true;
                //Receive.Start();
                //Console.WriteLine("Connect Success!");
                Console.WriteLine("Connect Success");
                return null;
            }
            catch (Exception e)
            {
                connect_error = "连接服务端出错：" + e.ToString();
                return connect_error;
            }
        }

        //发送
        public string send(string txt)
        {
            Console.WriteLine("Send:" + txt);
            tag_base++;
            tag_send = tag_base;
            recive_msg = null;
            try
            {
                if (socketSend == null || send_error != null || recive_error != null)
                    connect();
                byte[] buffer = Encoding.Default.GetBytes(txt);
                int receive = socketSend.Send(buffer);
                Console.WriteLine("发送成功");
                return null;
            }
            catch (Exception e)
            {
                send_error = "发送消息出错：" + e.ToString();
                Console.WriteLine(send_error);
                return send_error;
            }
        }
        static object obj = new object();
        StringBuilder ret = new StringBuilder();
        //接收
        public string recive2(out string error)
        {
            lock (obj)
            {
                error = null;
                recive_msg = null;
                ret.Clear();
                try
                {
                    byte[] buffer = new byte[102400];
                    //实际接收到的字节数
                    int r = socketSend.Receive(buffer);
                    if (r > 0)
                    {//}@@
                        recive_msg = Encoding.Default.GetString(buffer, 0, r);
                        Console.WriteLine("Recive:" + recive_msg);
                        return recive_msg;
                    }
                    else
                    {
                        error = "接收到空的消息";
                        Console.WriteLine("Recive:" + error);
                    }
                    return "";
                }
                catch (Exception e)
                {
                    error = "接收服务端消息出错：" + e.ToString();
                    Console.WriteLine(error);
                    return null;
                }
            }
        }
        //断开
        public void close()
        {
            if (socketSend != null)
                socketSend.Close();
            socketSend = null;
        }

        //接收-阻塞-废弃
        private void recive()
        {
            try
            {
                int tag_conn_ = tag_conn;
                while (tag_conn_ == tag_conn)
                {
                    byte[] buffer = new byte[2048];
                    //实际接收到的字节数
                    int r = socketSend.Receive(buffer);
                    if (r > 0)
                    {
                        recive_msg = Encoding.Default.GetString(buffer, 1, r - 1);

                        //return;
                    }
                    else
                        recive_msg = null;
                    recive_error = null;
                }
            }
            catch (Exception e)
            {
                send_error = "接收服务端发送的消息出错：" + e.ToString();
            }
        }
        //获取接收的数据-废弃
        public string getRecive(out string error)
        {
            if (connect_error != null)
                error = connect_error;
            else if (send_error != null)
                error = send_error;
            else
                error = recive_error;
            string re = recive_msg;
            recive_msg = null;
            return re;
        }

    }
}
