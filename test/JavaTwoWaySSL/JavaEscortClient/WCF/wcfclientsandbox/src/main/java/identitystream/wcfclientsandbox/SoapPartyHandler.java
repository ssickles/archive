package identitystream.wcfclientsandbox;

import java.util.Set;

import javax.xml.namespace.QName;
import javax.xml.soap.SOAPElement;
import javax.xml.soap.SOAPEnvelope;
import javax.xml.soap.SOAPFactory;
import javax.xml.soap.SOAPHeader;
import javax.xml.ws.handler.MessageContext;
import javax.xml.ws.handler.soap.SOAPHandler;
import javax.xml.ws.handler.soap.SOAPMessageContext;

public class SoapPartyHandler implements SOAPHandler<SOAPMessageContext>  {

	@Override
	public boolean handleMessage(SOAPMessageContext context) {
		Boolean outboundProperty = (Boolean) context.get(MessageContext.MESSAGE_OUTBOUND_PROPERTY); 
	    if (outboundProperty.booleanValue()) {
	        try { 
	            SOAPEnvelope envelope = context.getMessage().getSOAPPart().getEnvelope();
	            SOAPHeader header = envelope.getHeader();
	            SOAPFactory factory = SOAPFactory.newInstance();
	            
	            SOAPElement element = factory.createElement("userId", "ids", "http://www.identitystream.com");
	            element.addTextNode("Custom Header");
	            header.addChildElement(element); 
	
	        } catch (Exception e) { 
	            System.out.println("Exception in handler: " + e); 
	        } 
	    } else { 
	        // inbound 
	    } 
	    return true;
	}

	@Override
	public boolean handleFault(SOAPMessageContext context) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public void close(MessageContext context) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public Set<QName> getHeaders() {
		return null;
	}

}
