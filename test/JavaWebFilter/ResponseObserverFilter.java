package IdentityStream.JavaWebFilters;

import java.io.IOException;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletResponse;

/*
 * This filter can be used to monitor attributes of an HTTPServletResponse. 
 */

public abstract class ResponseObserverFilter implements Filter, ResponseObserver
{
	public void doFilter(ServletRequest req, ServletResponse resp, FilterChain chain) throws IOException, ServletException
	{
		doBeforeResponseStarted(req, resp);
		
		ResponseObserverResponseWrapper wrappedResponse = new ResponseObserverResponseWrapper((HttpServletResponse) resp, this);
		chain.doFilter(req, wrappedResponse);
		
		doAfterResponseFinished(req, resp);
	}	
}