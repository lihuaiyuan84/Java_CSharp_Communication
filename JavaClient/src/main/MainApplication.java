package main;

import java.io.FileInputStream;

import mix.IniReader;
import client.JavaClient;



public class MainApplication {
	
	public static void main(String[] args) throws Exception{
		
		IniReader reader = new IniReader("resource\\conf.ini"); 
		
		String IPAddress=reader.getValue("Server Info", "IP"); 		
		String SendObjectName="resource\\"+reader.getValue("Client Info", "SendObjectName"); 		
		String SendStringValue=reader.getValue("Client Info", "SendStringValue"); 	
		
		int SendIntValue=Integer.parseInt(reader.getValue("Client Info", "SendIntValue")); 
		int Port=Integer.parseInt(reader.getValue("Server Info", "Port")); 
		
		System.out.println("Reading File <<< "+ SendObjectName + " >>> ");
		FileInputStream fis=new FileInputStream(SendObjectName);
		
		JavaClient jc1,jc2,jc3;
		jc1=new JavaClient(Port,IPAddress);		
		jc1.Send(SendIntValue);
		jc2=new JavaClient(Port,IPAddress);
		jc2.Send(SendStringValue);
		jc3=new JavaClient(Port,IPAddress);
		jc3.Send(fis,SendObjectName);
	}
	
}