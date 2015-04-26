package main;

import mix.IniReader;
import server.JavaServer;



public class MainApplication {
	
	public static void main(String[] args) throws Exception{
		
		IniReader reader = new IniReader("resource\\conf.ini"); 
		
		String IPAddress=reader.getValue("Server Info", "IP"); 				
		int Port=Integer.parseInt(reader.getValue("Server Info", "Port")); 
		JavaServer js=new JavaServer(IPAddress,Port);
		js.Init();
	}
	
}