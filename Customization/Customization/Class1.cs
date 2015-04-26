using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.IO;



namespace Customization.Socket
{

    [Guid("CED9A99A-7A61-4322-B61B-2F9DA278E897")]

    public class JavaCommunication 
    {
        public const int CUCUMBER_SIZE = 8192;
        private string ReadInt(TcpClient Client)
        {
            NetworkStream ns = Client.GetStream();
            BinaryReader reader = new BinaryReader(ns);

            int data = reader.ReadInt32();
            Console.WriteLine(data);
            ns.Close();
            return data.ToString();
            
        }

        private string ReadStr(TcpClient Client)
        {        
           NetworkStream ns = Client.GetStream();
           BinaryReader reader = new BinaryReader(ns);

           int length = reader.ReadInt32();
           byte[] nameArray = reader.ReadBytes(length);
           string data = Encoding.UTF8.GetString(nameArray);
           Console.WriteLine(data);
           return data;
        }

        private string ReadObj(TcpClient Client)
        {
            NetworkStream ns = Client.GetStream();
            BinaryReader reader = new BinaryReader(ns);

            int length = reader.ReadInt32();
            byte[] nameArray = reader.ReadBytes(length);
            string filename = Encoding.UTF8.GetString(nameArray);
            Console.WriteLine(filename);

            int contentLength = reader.ReadInt32();
            byte[] bytebuffer = reader.ReadBytes(contentLength);
            FileStream sFile = new FileStream(Environment.CurrentDirectory + "\\" + filename, FileMode.Create);
            sFile.Seek(0, SeekOrigin.Begin);
            sFile.Write(bytebuffer, 0, contentLength);
            sFile.Flush();
            sFile.Close();
            return filename;
            /*
            nameArray = reader.ReadBytes(contentLength);
            String content = Encoding.UTF8.GetString(nameArray);
            byte[] contentbyte = Encoding.UTF8.GetBytes(content);
            Console.WriteLine(content);
            //Console.WriteLine(contentbyte.ToString());

            FileStream sFile = new FileStream(Environment.CurrentDirectory + "\\" + filename, FileMode.Create);
            sFile.Seek(0, SeekOrigin.Begin);
            sFile.Write(contentbyte, 0, contentbyte.Length);

            sFile.Flush();
            sFile.Close();
            return filename;*/
        }

        public string WriteInt(TcpClient Client, int val)
        {
            try
            {
                TcpClient c = Client;
                BinaryWriter bw = new BinaryWriter(c.GetStream());
                bw.Write(IPAddress.HostToNetworkOrder(1));
                bw.Write(IPAddress.HostToNetworkOrder(val));
                bw.Flush();
                bw.Close();
                return "finish";
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return e.ToString();
            }
        }

        public string WriteObj(TcpClient Client, String filePath, String fileName)
        {
            try
            {
                TcpClient c = Client;
                FileStream sFile = new FileStream(filePath + "\\" + fileName, FileMode.Open);
                if (sFile.Length <= CUCUMBER_SIZE)
                {
                    byte[] array = new byte[CUCUMBER_SIZE];
                    sFile.Seek(0, SeekOrigin.Begin);
                    sFile.Read(array, 0, CUCUMBER_SIZE);
                    BinaryWriter bw = new BinaryWriter(c.GetStream());
                    bw.Write(IPAddress.HostToNetworkOrder(-1));
                    bw.Write(IPAddress.HostToNetworkOrder(fileName.Length));
                    bw.Write(Encoding.UTF8.GetBytes(fileName.ToCharArray()));
                    bw.Write(IPAddress.HostToNetworkOrder(sFile.Length));
                    bw.Write(array);
                    bw.Flush();
                    bw.Close();
                    sFile.Flush();
                    sFile.Close();
                    return "finish";
                }
                else 
                {
                    return "TBD";
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return e.ToString();
            }
        }

        public string WriteStr(TcpClient Client, String str)
        {
            try
            {
                TcpClient c = Client;
                byte[] data = Encoding.UTF8.GetBytes(str);
                BinaryWriter bw = new BinaryWriter(c.GetStream());
                bw.Write(IPAddress.HostToNetworkOrder(0));
                bw.Write(IPAddress.HostToNetworkOrder(data.Length));
                bw.Write(data);
                bw.Flush();
                bw.Close();
                return "finish";
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return e.ToString();
            }
        }

        public string Read(TcpClient Client)
        {
            NetworkStream ns = Client.GetStream();
            BinaryReader reader = new BinaryReader(ns);

            int pre = reader.ReadInt32();
            if(pre==-1)
            {
                return ReadObj(Client);  
            }
            if(pre==0)
            {
                return ReadStr(Client);  
            }
            if(pre==1)
            {
                return ReadInt(Client);  
            }
            return "Head code error.";
            
        }
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
    
}
