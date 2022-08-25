using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ArduinoCServer
{
    class cServerClass
    {
        public event MessageEventHandler Message;

        public delegate void MessageEventHandler(cServerClass sender, string Data);

        //Server Control
        public IPAddress ServerIP = IPAddress.Parse("192.168.1.201");
        public int ServerPort = 8080;
        public TcpListener myserver;

        public Thread Comthread;
        public bool IsLiserning = true;

        //Clients
        private TcpClient client;
        private StreamReader clientdata;

        public cServerClass()
        {
            myserver = new TcpListener(ServerIP, ServerPort);
            myserver.Start();

            Comthread = new Thread(new ThreadStart(Hearing));
            Comthread.Start();

        }

        private void Hearing()
        {
            while (!IsLiserning == false)
            {
                if (myserver.Pending() ==true)
                {
                    client = myserver.AcceptTcpClient();
                    clientdata = new StreamReader(client.GetStream());
                }

                try
                {
                    Message?.Invoke(this, clientdata.ReadLine());
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(10);

            }
        }

    }
}
