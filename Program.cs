using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace ControlSever
{
    class Program
    {
        static void Main(string[] args)
        {
            Listen();           
        }

        static void Listen()
        {

            IPAddress address = GetIP();
            IPEndPoint endPoint = new IPEndPoint(address, 35666);
            
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(5);//open listen port

            Console.WriteLine("Already Listen,Address:{0},Port:{1} ",endPoint.Address,endPoint.Port);

            while(true)
            {
                try
                {
                    Socket client = socket.Accept();
                    Console.WriteLine("Accept a client:" + client.RemoteEndPoint);
                    byte[] buffer = new byte[64];
                    int len = client.Receive(buffer);
                    string msg = Encoding.UTF8.GetString(buffer,0,len);
                    Console.WriteLine("Client say:{0}",msg);

                    /**
                     * 1:shut down
                     * 
                     */             
                    if (msg.Trim().Equals("1"))
                    {
                        Console.WriteLine("Client mean: Shut Down!");
                        Process.Start("shutdown","-s -t 0");
                    }
                }
                catch(Exception e)
                {
                }         
            }
        }

        /// <summary>  
        /// 获取本地网卡地址  
        /// </summary>  
        /// <returns></returns>  
        public static System.Net.IPAddress GetIP()
        {
            System.Net.IPAddress ip = System.Net.IPAddress.Parse("127.0.0.1");

            System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            foreach (System.Net.IPAddress item in host.AddressList)
            {
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return item;
                }
            }
            //如果没有找到可用地址，则用回环地址  
            return ip;
        }  
    }
}

