package server;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;


public class JavaServer {
	int serverPort;
	String ipAddress; 
	public JavaServer(String ip,int port){
		serverPort=port;
		ipAddress=ip;
	}
	public void Init(){
		   
		try {    
            ServerSocket serverSocket = new ServerSocket(serverPort);    
            while (true) {    
                Socket client = serverSocket.accept(); 
                new HandlerThread(client);    
            }    
        } catch (Exception e) {    
            System.out.println("Server Exception" + e.getMessage());    
        }    
       
	}
	private class HandlerThread implements Runnable {    
        private Socket socket;    
        public HandlerThread(Socket client) {    
            socket = client;    
            new Thread(this).start();    
        }
        public void run() {    
        	
        	try {
        		JavaServerReader jr=new JavaServerReader(socket);
				jr.Read();
			} catch (IOException e) {
				e.printStackTrace();
			}
        }
	}
}
