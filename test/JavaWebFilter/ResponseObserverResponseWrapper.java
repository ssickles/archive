package IdentityStream.JavaWebFilters;

import java.io.IOException;
import java.io.PrintWriter;

import javax.servlet.ServletOutputStream;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpServletResponseWrapper;

public class ResponseObserverResponseWrapper extends HttpServletResponseWrapper
{
	private final ResponseObserver observer;

	private ResponseObserverPrintWriter writer;
	private ResponseObserverOutputStream stream;

	ResponseObserverResponseWrapper(HttpServletResponse response, ResponseObserver observer)
	{
		super(response);
		this.observer = observer;
	}

	public ServletOutputStream getOutputStream() throws IOException
	{
		if (stream == null)
		{
			stream = new ResponseObserverOutputStream(super.getOutputStream(), observer);
		}

		return stream;
	}

	public PrintWriter getWriter() throws IOException
	{
		if (writer == null)
		{
			writer = new ResponseObserverPrintWriter(super.getWriter(), observer);
		}

		return writer;
	}
}