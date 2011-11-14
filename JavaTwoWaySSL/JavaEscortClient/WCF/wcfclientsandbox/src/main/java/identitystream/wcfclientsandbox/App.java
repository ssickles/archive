package identitystream.wcfclientsandbox;

import java.io.File;
import java.io.IOException;
import java.net.URL;
//import java.security.PrivateKey;
//import java.security.cert.Certificate;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.ArrayList;
import java.util.List;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;
import javax.security.auth.callback.Callback;
import javax.security.auth.callback.UnsupportedCallbackException;
import javax.xml.namespace.QName;
import javax.xml.ws.handler.Handler;
import javax.xml.ws.handler.HandlerResolver;
import javax.xml.ws.handler.PortInfo;
import javax.xml.ws.soap.AddressingFeature;

import com.sun.xml.ws.api.security.CallbackHandlerFeature;
//import com.sun.xml.wss.impl.callback.PasswordCallback;
import com.sun.xml.wss.impl.callback.SignatureKeyCallback;
//import com.sun.xml.wss.impl.callback.SignatureKeyCallback.Request;
import com.sun.xml.wss.impl.callback.TimestampValidationCallback;
//import com.sun.xml.wss.impl.callback.UsernameCallback;
//import com.sun.xml.wss.impl.callback.TimestampValidationCallback.Request;
import com.sun.xml.wss.impl.callback.TimestampValidationCallback.TimestampValidationException;


public class App implements javax.security.auth.callback.CallbackHandler
{
	private static CertificateFinder _CertFinder;
    public static void main( String[] args ) throws Throwable
    {
    	System.out.println ( System.getProperty ( "sun.boot.class.path" ) ) ;
    	System.out.println ( System.getProperty ( "java.class.path" ) ) ;
    	System.out.println ( System.getProperty("java.endorsed.dirs"));
    	//Class <?> czz = Class.forName("javax.xml.ws.WebFault") ;
    	App app = new App();
    	app.run(); // -Xbootclasspath:C:/Users/Scott/.m2/repository/org/glassfish/metro/webservices-api/2.1/webservices-api-2.1.jar
    }

    public void run() throws Throwable {
    	//Class <?> czz = Class.forName ( "com.sun.xml.internal.ws.api.message.Packet" ) ;
    	//System.out.println ( czz.getProtectionDomain().getCodeSource().getLocation() ) ;
        //System.setProperty((new StringBuilder()).append(HttpTransportPipe.class.getName()).append(".dump").toString(), "true");
    	
    	_CertFinder = new CertificateFinder();
    	_CertFinder.loadKeyStore(System.getProperty("user.home") + File.separator + "Desktop" + File.separator + "TwoWaySSL" + File.separator + "sickles.p12", "PKCS12", "pa$$w0rd");
        
    	SSLContext sc = SSLContext.getInstance("SSL");
        sc.init(null, getTrustManager(), new java.security.SecureRandom());
        HttpsURLConnection.setDefaultSSLSocketFactory(sc.getSocketFactory());
        HttpsURLConnection.setDefaultHostnameVerifier(getHostnameVerifier());
    	
        //SecuredApplicationServiceShell2 service = new SecuredApplicationServiceShell2(new URL("https://localhost/ServicesHost/securedApplicationService2.svc?wsdl"), new QName("http://tempuri.org/", "SecuredApplicationServiceShell2"));
        EscortService service = new EscortService(new URL("https://localhost/EscortService/escort.svc?wsdl"), new QName("http://tempuri.org/", "EscortService"));
        service.setHandlerResolver(new HandlerResolver() {
			@SuppressWarnings("rawtypes")
			@Override
			public List<Handler> getHandlerChain(PortInfo portInfo) {
				List<Handler> handlers = new ArrayList<Handler>();
				handlers.add(new SoapPartyHandler());
				return handlers;
			}
        });
        AddressingFeature wsAddressing = new AddressingFeature(true, true);
        CallbackHandlerFeature callbackHandler = new CallbackHandlerFeature(this);
        //ISecuredApplicationService endpoint = service.getSecuredApplicationServiceShell2Endpoint(wsAddressing, callbackHandler);
        IEscortService endpoint = service.getEscortServiceEndpoint(wsAddressing, callbackHandler);
        
        /*
        try {
        	ArrayOfCountryData result = endpoint.getCountries(new RequestData());
        	System.out.println(String.format("%s members in my group.", result.toString()));
        } catch (Exception e) {
        	e.printStackTrace();
        }
        */

        try {
        	if (endpoint.escort(1))
        		System.out.println("You are a slut.");
        	else
        		System.out.println("You are not a slut");
        	if (endpoint.escort(2))
        		System.out.println("You are a slut.");
        	else
        		System.out.println("You are not a slut");
        } catch (Exception e) {
        	e.printStackTrace();
        }
    }
    
	static TrustManager[] getTrustManager() {
    	return new TrustManager[] {
    			new X509TrustManager() {
    				
    				public X509Certificate[] getAcceptedIssuers() {
    					return null;
    				}

					public void checkClientTrusted(java.security.cert.X509Certificate[] arg0, String arg1) 
						throws CertificateException {
						
					}

					public void checkServerTrusted(java.security.cert.X509Certificate[] arg0, String arg1) 
						throws CertificateException {

					}
    			}
    	};
    }
    
    static HostnameVerifier getHostnameVerifier() {
    	return new HostnameVerifier() {
			public boolean verify(String arg0, SSLSession arg1) {
				return arg1.getPeerHost().equalsIgnoreCase(arg0);
			}
    	};
    }


    public void handle(Callback[] callbacks) throws IOException,
			UnsupportedCallbackException {
    	if (callbacks == null) { 
    		return;
    	}
    	for ( Callback c : callbacks ) {
    		
    		if (c instanceof SignatureKeyCallback) {
    			SignatureKeyCallback.DefaultPrivKeyCertRequest request = (SignatureKeyCallback.DefaultPrivKeyCertRequest)((SignatureKeyCallback)c).getRequest();
    			request.setX509Certificate((X509Certificate)_CertFinder.getCertificateByAlias("sickles"));
    			request.setPrivateKey(_CertFinder.getPrivateKeyByAlias("sickles"));
    		}
    		if (c instanceof TimestampValidationCallback) {
    			((TimestampValidationCallback) c).setValidator(new TimestampValidationCallback.TimestampValidator() {	
					public void validate(com.sun.xml.wss.impl.callback.TimestampValidationCallback.Request request) throws TimestampValidationException {
						//((TimestampValidationCallback.UTCTimestampRequest)request).
					}
				});
    		}
    	}
    }
}
