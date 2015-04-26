package server;

import java.io.DataInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.CharBuffer;
import java.nio.charset.Charset;
import java.nio.charset.CharsetDecoder;
import java.nio.charset.CharsetEncoder;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class JavaServerReader {
	public final static int PROTOCOL_INT_CODE=1;
	public final static int PROTOCOL_STR_CODE=-2;
	public final static int PROTOCOL_OBJ_CODE=-1;
	public final static int PROTOCOL_IRQ_CODE=0;
	private Socket socket;    
	Charset charset = Charset.forName("UTF-8");
	CharsetDecoder decoder = charset.newDecoder();
	CharsetEncoder encoder = charset.newEncoder();
	private DataInputStream dis;
	private InputStream iStream;

	public JavaServerReader(Socket s) throws IOException{
		socket=s;
		dis=new DataInputStream(iStream);
		iStream = socket.getInputStream();
	}
	public void Read() throws IOException{
		
		int db = iStream.read();
		
		ByteBuffer readBuffer = ByteBuffer.allocate(16384);
		readBuffer.order(ByteOrder.LITTLE_ENDIAN);

		   while(db != -1)
		   {
		    readBuffer.put((byte)db);
		    db = iStream.read();
		   }
		   readBuffer.flip();
		   
		   List<Integer> flagSet=new ArrayList<Integer>();
		   flagSet=Arrays.asList(-1,-2,1,0);
		   int flag=readBuffer.getInt();
		   
		   while(flagSet.contains(flag)&&readBuffer.remaining()!=0)
		   {	   
			   switch(flag) {
			   	case PROTOCOL_INT_CODE:
			   		ReadInt(readBuffer);
			   		if(readBuffer.remaining()!=0)
			   		{
			   			flag=readBuffer.getInt();
			   		}
			   		break;
			   	case PROTOCOL_STR_CODE:
			   		ReadStr(readBuffer);
			   		if(readBuffer.remaining()!=0)
			   		{
			   			flag=readBuffer.getInt();
			   		}
			   		break;
			   	case PROTOCOL_OBJ_CODE:
			   		ReadObj(readBuffer);
			   		if(readBuffer.remaining()!=0)
			   		{
			   			flag=readBuffer.getInt();
			   		}
			   		break;
			   }
			   
		   }
	}
	private void ReadInt(ByteBuffer buf) {		
		int receiveIntValue=buf.getInt();
		System.out.println(receiveIntValue);
	}
	private void ReadStr(ByteBuffer buf) throws IOException {
		int receiveStrLen=buf.getInt();
		
		byte [] stringBuffer = new byte[receiveStrLen];
		buf.get(stringBuffer);
		System.out.println(decoder.decode(ByteBuffer.wrap(stringBuffer)).toString());
	
	}
	private void ReadObj(ByteBuffer buf) throws IOException {
		
		
		
		int receiveNameLen=buf.getInt();
		
		byte [] objNameBuffer = new byte[receiveNameLen];
		buf.get(objNameBuffer);
		String filename=decoder.decode(ByteBuffer.wrap(objNameBuffer)).toString();
		System.out.println(filename);
		int receiveObjectLen=buf.getInt();
		
		byte [] objBuffer = new byte[receiveObjectLen];
		buf.get(objBuffer);
		//System.out.println(decoder.decode(ByteBuffer.wrap(objBuffer)).toString());
		
		//CharBuffer nameBuffer = CharBuffer.wrap(decoder.decode(ByteBuffer.wrap(objBuffer)));
		//ByteBuffer nbBuffer = encoder.encode(nameBuffer);
		FileOutputStream fos=new FileOutputStream("resource\\"+filename);
		
		fos.write(objBuffer);
		//fos.write(nbBuffer.array());;
		fos.flush();
		fos.close();
	}
}