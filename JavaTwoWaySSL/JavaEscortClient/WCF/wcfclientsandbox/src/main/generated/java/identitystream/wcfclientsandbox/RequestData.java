
package identitystream.wcfclientsandbox;

import java.io.Serializable;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for RequestData complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="RequestData">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="IdentityUid" type="{http://schemas.microsoft.com/2003/10/Serialization/}guid" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "RequestData", namespace = "http://schemas.datacontract.org/2004/07/IdentityStream.Models", propOrder = {
    "identityUid"
})
public class RequestData
    implements Serializable
{

    @XmlElement(name = "IdentityUid")
    protected String identityUid;

    /**
     * Gets the value of the identityUid property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getIdentityUid() {
        return identityUid;
    }

    /**
     * Sets the value of the identityUid property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setIdentityUid(String value) {
        this.identityUid = value;
    }

}
