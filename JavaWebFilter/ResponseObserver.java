package IdentityStream.JavaWebFilters;

import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;

public interface ResponseObserver
{
	public void doBeforeResponseStarted(ServletRequest req, ServletResponse resp);
	
	public void doAfterResponseFinished(ServletRequest req, ServletResponse resp);
	
	public void doAfterCharsWritten(char[] charsWritten);
	
	public void doAfterBytesWriten(byte[] bytesWritten);
}