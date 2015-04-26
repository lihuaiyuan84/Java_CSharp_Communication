using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

using Ini;

namespace SocketClientC
{
    class Program
    {
        static void Main(string[] args)
        {
            SendMessage();
        }
        static void SendMessage()
        {
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\param.ini");
            int port = Int32.Parse(ini.IniReadValue("Config", "Port"));
            string host = ini.IniReadValue("Config", "IP");
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            int intValue = Int32.Parse(ini.IniReadValue("Data", "SendIntValue"));
            string strValue = ini.IniReadValue("Data", "SendStringValue");
            string strObjectName = ini.IniReadValue("Data", "SendObjectName");

            TcpClient client = new TcpClient();
            client.Connect(ipe);
            NetworkStream ns = client.GetStream();
            BinaryWriter bw = new BinaryWriter(ns);
            bw.Write(1);
            bw.Write(intValue);

            byte[] byteStrValue = Encoding.UTF8.GetBytes(strValue);
            bw.Write(-2);
            bw.Write(byteStrValue.Length);
            bw.Write(byteStrValue);

            byte[] byteObejctName = Encoding.UTF8.GetBytes(strObjectName);
            bw.Write(-1);
            bw.Write(byteObejctName.Length);
            bw.Write(byteObejctName);

            

            FileStream sFile = new FileStream(Environment.CurrentDirectory + "\\resource\\" + strObjectName, FileMode.Open);
            byte[] array = new byte[sFile.Length];
            sFile.Seek(0, SeekOrigin.Begin);
            sFile.Read(array, 0, Convert.ToInt16(sFile.Length));
            bw.Write(Convert.ToInt32(sFile.Length));
            bw.Write(array);

     

            sFile.Flush();
            sFile.Close();

            client.Client.Shutdown(SocketShutdown.Both);
            ns.Close();


        }
    }
}
