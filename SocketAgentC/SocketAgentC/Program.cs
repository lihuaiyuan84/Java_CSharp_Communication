using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.IO;

using Customization.Socket;

using Ini;

namespace SocketAgentC
{
    class Program
    {
        static void Main(string[] args)
        {                     
            ReceiveMessage();
        }

        static void ReceiveMessage()
        {
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\param.ini");
            int port = Int32.Parse(ini.IniReadValue("Config", "PORT"));
            string host = ini.IniReadValue("Config", "IP");
            string filename = ini.IniReadValue("Config", "DOCUMENT");
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);
            TcpListener listener = new TcpListener(ipe);
            listener.Start();

            while (true)
            {
                using (TcpClient client = listener.AcceptTcpClient())
                {
                    try
                    {
                        ComReadTesting(client, filename);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
            }
        }

        static void ComReadTesting(TcpClient client, String objectname)
        {
            JavaCommunication jc = new Customization.Socket.JavaCommunication();
            jc.Read(client);     
        }
    }
}


/*
         //Function Read is used to transfer int or string data.
         static void Read(TcpClient client)
         {
             Console.WriteLine("Got connection: {0}", DateTime.Now);
             NetworkStream ns = client.GetStream();
             BinaryReader reader = new BinaryReader(ns);

             Employee emp = new Employee();
             // first read the Id
             emp.Id = reader.ReadInt32();

             // length of first name in bytes.
             int length = reader.ReadInt32();

             // read the name bytes into the byte array.
             // recall that java side is writing two bytes for every character.
             byte[] nameArray = reader.ReadBytes(length);
             emp.FirstName = Encoding.UTF8.GetString(nameArray);

             // last name
             length = reader.ReadInt32();
             nameArray = reader.ReadBytes(length);
             emp.LastName = Encoding.UTF8.GetString(nameArray);

             // salary
             emp.Salary = reader.ReadInt32();

             Console.WriteLine(emp.Id);
             Console.WriteLine(emp.FirstName);
             Console.WriteLine(emp.LastName);
             Console.WriteLine(emp.Salary);

             System.Threading.Thread.Sleep(5);

             Console.WriteLine("Writing data...");
             // now reflect back the same structure.
             BinaryWriter bw = new BinaryWriter(ns);

             bw.Write(emp.Id);
             byte[] data = Encoding.UTF8.GetBytes(emp.FirstName);
             bw.Write(data.Length);
             bw.Write(data);

             data = Encoding.UTF8.GetBytes(emp.LastName);
             bw.Write(data.Length);
             bw.Write(data);

             bw.Write(emp.Salary);

             Console.WriteLine("Writing data...DONE");

             client.Client.Shutdown(SocketShutdown.Both);
             ns.Close();
         }

         //ObjectRead is used to transfer a binary object
         static void ObjectRead(TcpClient client, String objectname)
         {
             //WriteObj(client, Environment.CurrentDirectory, objectname);
             /*
             byte[] array = new byte[256];

             FileStream sFile = new FileStream(Environment.CurrentDirectory + "\\" + objectname, FileMode.Open);

             if (sFile.Length <= 256)
             {
                 sFile.Seek(0, SeekOrigin.Begin);
                 sFile.Read(array, 0, 256);
                 NetworkStream ns = client.GetStream(); ;
                 BinaryWriter bw = new BinaryWriter(ns);
                 bw.Write(IPAddress.HostToNetworkOrder(-1));
                 bw.Write(IPAddress.HostToNetworkOrder(objectname.Length));
                 bw.Write(Encoding.UTF8.GetBytes(objectname.ToCharArray()));
                 bw.Write(IPAddress.HostToNetworkOrder(sFile.Length));
                 bw.Write(array);

                 client.Client.Shutdown(SocketShutdown.Both);
                 ns.Close();

                 sFile.Flush();
                 sFile.Close();
             }
             else
             {
                 int loop = (int)sFile.Length/256;
                 NetworkStream ns = client.GetStream(); ;
                 for (int i = 0;i<=loop; i++)
                 {
                     sFile.Seek(0, SeekOrigin.Begin);
                     sFile.Read(array, 0, 256);
                    
                     BinaryWriter bw = new BinaryWriter(ns);
                     bw.Write(IPAddress.HostToNetworkOrder(i));
                     bw.Write(IPAddress.HostToNetworkOrder(objectname.Length));
                     bw.Write(Encoding.UTF8.GetBytes(objectname.ToCharArray()));
                     bw.Write(IPAddress.HostToNetworkOrder(sFile.Length));
                     bw.Write(array);

                    
                 }
                 client.Client.Shutdown(SocketShutdown.Both);
                 ns.Close();

                 sFile.Flush();
                 sFile.Close();
             }

           
           
             Console.WriteLine("Got connection: {0}", DateTime.Now);
             NetworkStream ns = client.GetStream();
             BinaryReader reader = new BinaryReader(ns);

             Employee emp = new Employee();
             // first read the Id
             emp.Id = IPAddress.NetworkToHostOrder(reader.ReadInt32());

             // length of first name in bytes.
             int length = IPAddress.NetworkToHostOrder(reader.ReadInt32());

             // read the name bytes into the byte array.
             // recall that java side is writing two bytes for every character.
             byte[] nameArray = reader.ReadBytes(length);
             emp.FirstName = Encoding.UTF8.GetString(nameArray);

             // last name
             length = IPAddress.NetworkToHostOrder(reader.ReadInt32());
             nameArray = reader.ReadBytes(length);
             emp.LastName = Encoding.UTF8.GetString(nameArray);

             // salary
             emp.Salary = IPAddress.NetworkToHostOrder(reader.ReadInt32());

             Console.WriteLine(emp.Id);
             Console.WriteLine(emp.FirstName);
             Console.WriteLine(emp.LastName);
             Console.WriteLine(emp.Salary);

             System.Threading.Thread.Sleep(5);

             Console.WriteLine("Writing data...");
             // now reflect back the same structure.
             BinaryWriter bw = new BinaryWriter(ns);

             bw.Write(IPAddress.HostToNetworkOrder(emp.Id));
             byte[] data = Encoding.UTF8.GetBytes(emp.FirstName);
             bw.Write(IPAddress.HostToNetworkOrder(data.Length));
             bw.Write(data);

             data = Encoding.UTF8.GetBytes(emp.LastName);
             bw.Write(IPAddress.HostToNetworkOrder(data.Length));
             bw.Write(data);

             bw.Write(IPAddress.HostToNetworkOrder(emp.Salary));

             Console.WriteLine("Writing data...DONE");

             client.Client.Shutdown(SocketShutdown.Both);
             ns.Close();
             */

/*
        static void cRead(TcpClient Client)
        {
            NetworkStream ns = Client.GetStream();
            BinaryReader reader = new BinaryReader(ns);

            int data = reader.ReadInt32();
            Console.WriteLine(data);
            data = reader.ReadInt32();
            Console.WriteLine(data);
            ns.Close();
        }
*/
/*
Console.WriteLine(jc.Read(client));
Console.WriteLine(jc.Read(client));
Console.WriteLine(jc.Read(client));
Console.WriteLine(jc.Read(client));
Console.WriteLine(jc.WriteInt(client,99));
Console.WriteLine(jc.WriteStr(client,"test"));
Console.WriteLine(jc.WriteObj(client, Environment.CurrentDirectory, objectname));*/