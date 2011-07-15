package IdentityStream.JavaWebFilters;

import javax.servlet.*;
import javax.servlet.http.HttpServletResponse;

import java.io.*;
import java.util.Enumeration;

public final class T24Web implements Filter, ResponseObserver 
{	   
	public static String _securedTransactions;
	public static String _securityResponse;
	public static String _securityResponseFailure;
	private String _secureOverrides;
	
	public void init(FilterConfig filterConfig) throws ServletException
	{
		if (_securedTransactions == null)
		{
			String transFile = filterConfig.getInitParameter("TransactionsFile");
			if (transFile == null)
			{
				System.err.println("TransactionsFile init parameter not found.");
				return;
			}
			try
			{
				FileReader reader = new FileReader(transFile);
				_securedTransactions = readFileIntoString(reader).toUpperCase();
				System.out.println("Secured Transaction List loaded: " + _securedTransactions);
			}
			catch (FileNotFoundException ex)
			{
				System.err.println("Unable to find the file containing the list of biometrically secured transactions. " + ex.toString());
				throw new ServletException(ex);
			}
			catch (IOException ex)
			{
				System.err.println("Unable to read the list of biometrically secured transactions from file. " + ex.toString());
				throw new ServletException(ex);
			}
		}
		
		if (_securityResponse == null)
		{
			String responseFile = filterConfig.getInitParameter("IdentityStreamSecurityForm");
			if (responseFile == null)
			{
				System.err.println("IdentityStreamSecurityForm init parameter not found.");
				return;
			}
			try
			{
				FileReader reader = new FileReader(responseFile);
				_securityResponse = readFileIntoString(reader);
				System.out.println("Security Response page loaded.");
			}
			catch (FileNotFoundException ex)
			{
				System.err.println("Unable to find the security response web form. " + ex.toString());
				throw new ServletException(ex);
			}
			catch (IOException ex)
			{
				System.err.println("Unable to read the security response web form from file. " + ex.toString());
				throw new ServletException(ex);
			}
		}

		if (_securityResponseFailure == null)
		{
			String responseFailureFile = filterConfig.getInitParameter("IdentityStreamSecurityFailure");
			if (responseFailureFile == null)
			{
				System.err.println("IdentityStreamSecurityFailure init parameter not found.");
				return;
			}
			try
			{
				FileReader reader = new FileReader(responseFailureFile);
				_securityResponseFailure = readFileIntoString(reader);
				System.out.println("Security Response failure page loaded.");
			}
			catch (FileNotFoundException ex)
			{
				System.err.println("Unable to find the security response failure. " + ex.toString());
				throw new ServletException(ex);
			}
			catch (IOException ex)
			{
				System.err.println("Unable to read the security response failure from file. " + ex.toString());
				throw new ServletException(ex);
			}
		}
		
		_secureOverrides = filterConfig.getInitParameter("SecureOverrides").toUpperCase();
}
	
	private static String readFileIntoString(FileReader fr) throws IOException
	{
		BufferedReader br = new BufferedReader(fr);
		String line;
		StringBuffer result = new StringBuffer();
		while ((line = br.readLine()) != null)
		{
			result.append(line);
		}
		return result.toString();
	}
	
	public void destroy() 
	{

	}
	   
	public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException 
	{
		//read the necessary parameters from the request object
		//if the parameters don't exist, the returned value will be null
		String transactionId = request.getParameter("transactionId");

		String application = request.getParameter("application");
		String version = request.getParameter("version");
		String ofsFunction = request.getParameter("ofsFunction");
		
		String idsKey = request.getParameter("identitystream_key");
		String overrides = request.getParameter("overridesPresent");
		
		//CATCH BIOMETRIC SECURITY SUCCESS/FAILURE
		//transactionId is shared among the Temenos submission and IdentityStream submission
		//the existance of idsKey lets us know it is an IdentityStream submission
		if (transactionId != null && idsKey != null)
		{
			System.out.println("Biometric security submission.");
			if(idsKey.equals(generateKey()))
			{
				//biometric security passed
				System.out.println("Biometric security passed.");
				ResponseObserverResponseWrapper wrappedResponse = new ResponseObserverResponseWrapper((HttpServletResponse) response, this);
				chain.doFilter(request, wrappedResponse);
				response = wrappedResponse;
				return;
			}
			else
			{
				//biometric security failed
				OutputStream out = response.getOutputStream();
				out.write(_securityResponseFailure.getBytes());
				out.close();
				System.out.println("Biometric security failed.");
				return;
			}
		}

		//INTERCEPT TRANSACTION SUBMISSIONS
		//checking for the existance of transactionId and ofsOperation will tell us whether a transaction is being submitted
		//if the parameters transactionId and ofsOperation (case-sensitive) do not exist, "getParameter" will return null
		if (transactionId != null && ofsFunction != null && application != null && version != null)
		{
			//check to see if a transaction has been begun and whether the transaction has been submitted for processing
			if (transactionId.length() > 0 && ofsFunction.toUpperCase().equals("I"))
			{
				//check to see if it is an override
				//check to see if we should intercept overrides as well
				if (overrides == null || _secureOverrides.equals("Y"))
				{
					System.out.println("Original post of the transaction.");
					
					//build the full submitted transaction name
					String transaction = application.concat(version).toUpperCase();
					if (_securedTransactions.contains(transaction))
					{
						//respond to the request with the IdentityStream authentication page
						String responseString = _securityResponse.replace("%TRANSACTIONNAME%", transaction);
	
						//write all of the original parameters back to the response so that the biometric security submission contains these inputs
						responseString = responseString.replace("%ORIGINALPOST%", writeParameters(request));
	
						//write the response and close the response stream
						ServletOutputStream out = response.getOutputStream();
						out.write(responseString.getBytes());
						out.close();
						return;
					}
				}
				else
				{
					System.out.println("Override submission.");
				}
			}
		}
		
		//we should reach this code for any servlet requests that are not for biometrically secured transactions
		//this line allows the request to be processed normally
		chain.doFilter(request, response);
	}
	
	private String generateKey() 
	{
		return new String("YES");
	}

	@SuppressWarnings("unchecked")
	private String writeParameters(ServletRequest request)
	   {
		   try
		   {
			   Enumeration paramNames = request.getParameterNames();
			   StringBuffer result = new StringBuffer();
			   while(paramNames.hasMoreElements())
			   {
				   String paramName = (String)paramNames.nextElement();
				   String values[] = request.getParameterValues(paramName);
				   for (int i = 0; i < values.length; i++)
				   {
					  result.append("<input type='hidden' name='" + paramName + "' id='" + paramName + "' value='" + values[i] + "'/>");
				   }
			   }
			   return result.toString();
		   }
		   catch (Exception ex)
		   {
			   System.out.println("Unable to write the hidden inputs back to the IdentityStream security response. " + ex.toString());
		   }
		   return null;
	   }

	public void doAfterBytesWriten(byte[] bytesWritten) {
		// TODO Auto-generated method stub
		
	}

	public void doAfterCharsWritten(char[] charsWritten) {
		// TODO Auto-generated method stub
		
	}

	public void doAfterResponseFinished(ServletRequest req, ServletResponse resp) {
		// TODO Auto-generated method stub
		
	}

	public void doBeforeResponseStarted(ServletRequest req, ServletResponse resp) {
		// TODO Auto-generated method stub
		
	}
}