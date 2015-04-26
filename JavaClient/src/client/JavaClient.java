package client;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.CharBuffer;
import java.nio.charset.Charset;
import java.nio.charset.CharsetEncoder;


public class JavaClient {
	
	public final static int PROTOCOL_INT_CODE=1;
	public final static int PROTOCOL_STR_CODE=0;
	public final static int PROTOCOL_OBJ_CODE=-1;
	
	public static Socket client;
	public static OutputStream oStream;
	
	public JavaClient(int port,String ip) throws Exception, IOException {
		client= new Socket(ip, port);;
		oStream = client.getOutputStream();
	}
	
	public void Send(FileInputStream is,String filename) throws IOException{		
		
		Charset charset = Charset.forName("UTF-8");
		CharsetEncoder encoder = charset.newEncoder();
		CharBuffer nameBuffer = CharBuffer.wrap(filename.toCharArray());
		ByteBuffer objectname,buffer;
		objectname = encoder.encode(nameBuffer);
		int length=is.available();
		byte[] bs = new byte[length];
		is.read(bs);

		buffer = GetBuffer();		
		wrapper(buffer,objectname,bs); 	  	
	  	SendBuffer(buffer);
	  	
		client.close();
		System.out.println("Send finish!");
	}
	public void Send(String value) throws IOException {		
		Charset charset = Charset.forName("UTF-8");
		CharsetEncoder encoder = charset.newEncoder();
		CharBuffer nameBuffer = CharBuffer.wrap(value.toCharArray());
		ByteBuffer nbBuffer,buffer;
		
		nbBuffer=encoder.encode(nameBuffer);		
		buffer = GetBuffer(); 	
		
		wrapper(buffer,nbBuffer); 	  	
	  	SendBuffer(buffer);

	}
	public void Send(int value) throws IOException {
  	  	ByteBuffer buffer = GetBuffer(); 
  	  	
  	  	wrapper(buffer,value);	  	
	  	SendBuffer(buffer);
	  	
	}
	
	
	private static ByteBuffer GetBuffer() {
		byte [] underlyingBuffer = new byte[1024];
  	  	ByteBuffer buf = ByteBuffer.wrap(underlyingBuffer);
  	  	buf.order(ByteOrder.LITTLE_ENDIAN);
		return buf;
	}
	
	private static void SendBuffer(ByteBuffer buffer) throws IOException {
		int remaining = buffer.remaining();
  		while(remaining > 0)
  		{	  			
  			oStream.write(buffer.get());
  			-- remaining;
  		}  		
  		oStream.flush();
  		oStream.close();
  		client.close();
	}
	private void wrapper(final ByteBuffer buf, final int value){
		buf.putInt(PROTOCOL_INT_CODE);
		buf.putInt(value);
		buf.flip();
	}	
	private void wrapper(final ByteBuffer buf, final ByteBuffer bufvalue){
		buf.putInt(PROTOCOL_STR_CODE);
		buf.putInt(bufvalue.limit());
		buf.put(bufvalue);
		buf.flip();
	}
	private void wrapper(final ByteBuffer buf, final ByteBuffer objectname,final byte[] bs){
		buf.putInt(PROTOCOL_OBJ_CODE);
		buf.putInt(objectname.limit());
		buf.put(objectname);
		buf.putInt(bs.length);
		buf.put(bs);
		buf.flip();
	}
}

