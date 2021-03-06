
package identitystream.wcfclientsandbox;

import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebResult;
import javax.jws.WebService;
import javax.xml.bind.annotation.XmlSeeAlso;
import javax.xml.ws.RequestWrapper;
import javax.xml.ws.ResponseWrapper;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.1.7-b01-
 * Generated source version: 2.1
 * 
 */
@WebService(name = "ISecuredApplicationService", targetNamespace = "http://tempuri.org/")
@XmlSeeAlso({
    ObjectFactory.class
})
public interface ISecuredApplicationService {


    /**
     * 
     * @param requestData
     * @return
     *     returns identitystream.wcfclientsandbox.ArrayOfCountryData
     */
    @WebMethod(operationName = "GetCountries", action = "http://tempuri.org/ISecuredApplicationService/GetCountries")
    @WebResult(name = "GetCountriesResult", targetNamespace = "http://tempuri.org/")
    @RequestWrapper(localName = "GetCountries", targetNamespace = "http://tempuri.org/", className = "identitystream.wcfclientsandbox.GetCountries")
    @ResponseWrapper(localName = "GetCountriesResponse", targetNamespace = "http://tempuri.org/", className = "identitystream.wcfclientsandbox.GetCountriesResponse")
    public ArrayOfCountryData getCountries(
        @WebParam(name = "requestData", targetNamespace = "http://tempuri.org/")
        RequestData requestData);

}
