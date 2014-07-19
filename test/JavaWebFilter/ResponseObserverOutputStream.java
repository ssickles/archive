package IdentityStream.JavaWebFilters;

import java.io.IOException;

import javax.servlet.ServletOutputStream;

public class ResponseObserverOutputStream extends ServletOutputStream
{
	private final ServletOutputStream servletOutputStream;
	private final ResponseObserver observer;

	public ResponseObserverOutputStream(ServletOutputStream servletOutputStream, ResponseObserver observer)
	{
		this.servletOutputStream = servletOutputStream;
		this.observer = observer;
	}

	@Override
	public void print(boolean b) throws IOException
	{
		servletOutputStream.print(b);
		
		observer.doAfterBytesWriten(String.valueOf(b).getBytes());
	}

	@Override
	public void print(char c) throws IOException
	{
		servletOutputStream.print(c);
		
		observer.doAfterBytesWriten(String.valueOf(c).getBytes());
	}

	@Override
	public void print(double d) throws IOException
	{
		servletOutputStream.print(d);
		
		observer.doAfterBytesWriten(String.valueOf(d).getBytes());
	}

	@Override
	public void print(float f) throws IOException
	{
		servletOutputStream.print(f);
		
		observer.doAfterBytesWriten(String.valueOf(f).getBytes());
	}

	@Override
	public void print(int i) throws IOException
	{
		servletOutputStream.print(i);
		
		observer.doAfterBytesWriten(String.valueOf(i).getBytes());
	}

	@Override
	public void print(long l) throws IOException
	{
		servletOutputStream.print(l);
		
		observer.doAfterBytesWriten(String.valueOf(l).getBytes());
	}

	@Override
	public void print(String s) throws IOException
	{
		servletOutputStream.print(s);
		
		observer.doAfterBytesWriten(s.getBytes());
	}

	@Override
	public void println() throws IOException
	{
		servletOutputStream.println();
		
		observer.doAfterBytesWriten("\r\n".getBytes());
	}

	@Override
	public void println(boolean b) throws IOException
	{
		print(b);
		println();
	}

	@Override
	public void println(char c) throws IOException
	{
		print(c);
		println();
	}

	@Override
	public void println(double d) throws IOException
	{
		print(d);
		println();
	}

	@Override
	public void println(float f) throws IOException
	{
		print(f);
		println();
	}

	@Override
	public void println(int i) throws IOException
	{
		print(i);
		println();
	}

	@Override
	public void println(long l) throws IOException
	{
		print(l);
		println();
	}

	@Override
	public void println(String s) throws IOException
	{
		print(s);
		println();
	}

	@Override
	public void write(byte[] b, int off, int len) throws IOException
	{
		System.out.println("Write to outputstream 2.");

		servletOutputStream.write(b, off, len);
		
		byte[] bytesWritten = new byte[len];
		for (int i = 0; i < len; i++) {
			bytesWritten[i] = b[off + i];
		}
		observer.doAfterBytesWriten(bytesWritten);
	}

	@Override
	public void write(byte[] b) throws IOException
	{
		String temp = b.toString();
		System.out.println("Write to outputstream.");
		if (temp.contains("AUTHORISER"))
			System.out.println("Found AUTHORISER");
			temp = temp.replaceAll("AUTHORISER", "SCOTT");

		servletOutputStream.write(temp.getBytes());
		
		System.out.println(temp);
		observer.doAfterBytesWriten(temp.getBytes());
	}

	@Override
	public void write(int b) throws IOException
	{
		servletOutputStream.write(b);
		
		byte[] bytesWritten = new byte[1];
		bytesWritten[0] = (byte) b;
		observer.doAfterBytesWriten(bytesWritten);
	}

	@Override
	public void close() throws IOException
	{
		servletOutputStream.close();
	}

	@Override
	public void flush() throws IOException
	{
		servletOutputStream.flush();
	}
}