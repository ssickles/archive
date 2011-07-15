
/**
 * IApplicationService_GetAuthenticationUnits_ErrorDetailsFault_FaultMessage.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis2 version: 1.5.4  Built on : Dec 19, 2010 (08:18:42 CET)
 */

package test.ws;

public class IApplicationService_GetAuthenticationUnits_ErrorDetailsFault_FaultMessage extends java.lang.Exception{
    
    private test.ws.ApplicationServiceStub.ErrorDetailsE faultMessage;

    
        public IApplicationService_GetAuthenticationUnits_ErrorDetailsFault_FaultMessage() {
            super("IApplicationService_GetAuthenticationUnits_ErrorDetailsFault_FaultMessage");
        }

        public IApplicationService_GetAuthenticationUnits_ErrorDetailsFault_FaultMessage(java.lang.String s) {
           super(s);
        }

        public IApplicationService_GetAuthenticationUnits_ErrorDetailsFault_FaultMessage(java.lang.String s, java.lang.Throwable ex) {
          super(s, ex);
        }

        public IApplicationService_GetAuthenticationUnits_ErrorDetailsFault_FaultMessage(java.lang.Throwable cause) {
            super(cause);
        }
    

    public void setFaultMessage(test.ws.ApplicationServiceStub.ErrorDetailsE msg){
       faultMessage = msg;
    }
    
    public test.ws.ApplicationServiceStub.ErrorDetailsE getFaultMessage(){
       return faultMessage;
    }
}
    