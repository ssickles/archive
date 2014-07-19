package IdentityStream.JavaWebFilters;

import java.io.PrintWriter;

public class ResponseObserverPrintWriter extends PrintWriter
{
	private final ResponseObserver observer;
	
	ResponseObserverPrintWriter(PrintWriter writer, ResponseObserver observer)
	{
		super(writer);
		this.observer = observer;
	}
	
	public void write(int c)
	{
		super.write(c);
		
		char[] charsWritten = new char[1];
		charsWritten[0] = (char) c;
		observer.doAfterCharsWritten(charsWritten);
	}

	public void write(char[] buf, int off, int len)
	{
		System.out.println("Print writer 1");
		
		super.write(buf, off, len);
		
		char[] charsWritten = new char[len];
		for (int i = 0; i < len; i++) {
			charsWritten[i] = buf[off + i];
		}
		observer.doAfterCharsWritten(charsWritten);
	}

	public void write(char[] buf)
	{
		System.out.println("Print writer 2");

		super.write(buf);
		
		observer.doAfterCharsWritten(buf);
	}

	public void write(String s, int off, int len)
	{
		System.out.println("Print writer 3");
		if (s.contains("value=\"AUTHORISER\""))
		{
			System.out.println(s);
			s = s.replaceAll("value=\"AUTHORISER\"", "value=\"SCOTT\"");
		}
		
		super.write(s, off, len);
		
		observer.doAfterCharsWritten(s.substring(off, off + len).toCharArray());
	}

	public void write(String s)
	{
		System.out.println("Print writer 4");
		if (s.contains("value=\"AUTHORISER\""))
		{
			System.out.println(s);
			s = s.replaceAll("value=\"AUTHORISER\"", "value=\"SCOTT\"");
		}
		
		super.write(s);
		
		observer.doAfterCharsWritten(s.toCharArray());
	}
}