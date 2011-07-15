package IdentityStream.JavaWebFilters;

import java.text.DecimalFormat;
import java.util.concurrent.ConcurrentHashMap;

import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;

public class ResponseSizeMonitorFilter extends ResponseObserverFilter
{
	private final ConcurrentHashMap<Thread, Long> threadToByteCountMap = new ConcurrentHashMap<Thread, Long>();

	public void init(FilterConfig filterConfig) throws ServletException
	{
	}

	public void doBeforeResponseStarted(ServletRequest req, ServletResponse resp)
	{
		threadToByteCountMap.put(Thread.currentThread(), new Long(0));
	}

	public void doAfterResponseFinished(ServletRequest req, ServletResponse resp)
	{
		long numBytesInResponse = threadToByteCountMap.get(Thread.currentThread());
		System.out.println("The response for the requested url (" + getUrl(req) + ") contained about " + new DecimalFormat("###,##0.00").format(numBytesInResponse / 1024.0) + " kilobytes.");

		// remove entry in threadToByteCountMap so that the Long value can be
		// garbage collected
		threadToByteCountMap.remove(Thread.currentThread());
	}

	private String getUrl(ServletRequest req)
	{
		if (req instanceof HttpServletRequest)
		{
			return ((HttpServletRequest) req).getRequestURL().toString();
		}
		else
		{
			return "unknown; non HTTP";
		}
	}

	public void doAfterCharsWritten(char[] charsWritten)
	{
		synchronized (threadToByteCountMap)
		{
			Long currentByteCount = threadToByteCountMap.get(Thread.currentThread());
			threadToByteCountMap.put(Thread.currentThread(), new Long(currentByteCount + (charsWritten.length * 2)));
		}
	}

	public void doAfterBytesWriten(byte[] bytesWritten)
	{
		synchronized (threadToByteCountMap)
		{
			Long currentByteCount = threadToByteCountMap.get(Thread.currentThread());
			threadToByteCountMap.put(Thread.currentThread(), new Long(currentByteCount + bytesWritten.length));
		}
	}

	public void destroy()
	{
	}
}