//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.IdentityModel.Tokens;

using System.Runtime.Serialization;

using System.Security.Cryptography;

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;

using System.Xml;

using System.Text;


namespace Gudge.Samples.Security.RSTRSTR
{
    // This class is specific to the February 2005 version of WS-Trust
    public class RequestSecurityToken : RequestSecurityTokenBase
    {
        // private members
        private string keyType;     // Tracks the type of the proof key (if any)
        private string requestType; // Tracks the request type (e.g. Issue, Renew, Cancel )        
        private SecurityToken requestorEntropy;
        private SecurityToken proofKey;
        private List<ClaimTypeRequirement> claimReqs;

        // Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RequestSecurityToken() : this(String.Empty, String.Empty, String.Empty, 0, Constants.Trust.KeyTypes.Symmetric, null, null, null, null)
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="context">The value of the wst:RequestSecurityToken/@Context attribute</param>
        /// <param name="tokenType">The content of the wst:RequestSecurityToken/wst:TokenType element</param>
        /// <param name="requestType"></param>
        /// <param name="keySize">The content of the wst:RequestSecurityToken/wst:KeySize element</param>
        /// <param name="keyType"></param>
        /// <param name="proofKey"></param>
        /// <param name="entropy">A SecurityToken representing entropy provided by the requestor in the wst:RequestSecurityToken/wst:Entropy element</param>
        /// <param name="claimTypeRequirements"></param>
        /// <param name="appliesTo">The content of the wst:RequestSecurityToken/wst:KeySize element</param>
        public RequestSecurityToken(string context, string tokenType, string requestType, int keySize, string keyType , SecurityToken proofKey, SecurityToken entropy, IList<ClaimTypeRequirement> claimTypeRequirements, EndpointAddress appliesTo) : base ( context, tokenType,keySize, appliesTo )
        {
            this.keyType = keyType;
            this.proofKey = proofKey;
            this.requestType = requestType;
            this.requestorEntropy = entropy;

            if (claimTypeRequirements != null)
            {
                this.claimReqs = new List<ClaimTypeRequirement>();

                foreach (ClaimTypeRequirement ctr in claimTypeRequirements)
                    this.claimReqs.Add(ctr);
            }
        }

        // public properties
        public string RequestType 
        { 
            get { return requestType; }
            set { requestType = value; }
        }

        public string KeyType
        {
            get { return keyType; }
            set { keyType = value; }
        }

        public SecurityToken ProofKey
        {
            get { return proofKey; }
            set { proofKey = value; }
        }

        public ReadOnlyCollection<ClaimTypeRequirement> ClaimRequirements
        {
            get { return claimReqs != null ? claimReqs.AsReadOnly() : null; }
        }

        /// <summary>
        /// The SecurityToken representing entropy provided by the requestor.
        /// Null if the requestor did not provide entropy
        /// </summary>
        public SecurityToken RequestorEntropy
        {
            get { return requestorEntropy; }
            set { requestorEntropy = value; }
        }

        // public methods
        public bool IsProofKeyAsymmetric()
        {
            return Constants.Trust.KeyTypes.Public == keyType;
        }

        /// <summary>
        /// Reads a wst:RequestSecurityToken element, its attributes and children and 
        /// creates a RequestSecurityToken instance with the appropriate values
        /// </summary>
        /// <param name="xr">An XmlReader positioned on wst:RequestSecurityToken</param>
        /// <returns>A RequestSecurityToken instance, initialized with the data read from the XmlReader</returns>
        public static RequestSecurityToken CreateFrom(XmlReader xr)
        {
            return ProcessRequestSecurityTokenElement(xr);        
        }

        // Methods of BodyWriter
        /// <summary>
        /// Writes out an XML representation of the instance.        
        /// </summary>
        /// <param name="writer">The writer to be used to write out the XML content</param>
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            // Write out the wst:RequestSecurityToken start tag
            writer.WriteStartElement(Constants.Trust.Elements.RequestSecurityToken, Constants.Trust.NamespaceUri);

            // If we have a non-null, non-empty tokenType...
            if (this.tokenType != null && this.tokenType.Length > 0)
            {
                // Write out the wst:TokenType start tag
                writer.WriteStartElement(Constants.Trust.Elements.TokenType, Constants.Trust.NamespaceUri);
                // Write out the tokenType string
                writer.WriteString(this.tokenType);
                writer.WriteEndElement(); // wst:TokenType
            }

            // If we have a non-null, non-empty requestType...
            if (this.requestType != null && this.requestType.Length > 0)
            {
                // Write out the wst:RequestType start tag
                writer.WriteStartElement(Constants.Trust.Elements.RequestType, Constants.Trust.NamespaceUri);
                // Write out the requestType string
                writer.WriteString(this.requestType);
                writer.WriteEndElement(); // wst:RequestType
            }

            // If we have a non-null appliesTo
            if (this.appliesTo != null)
            {
                // Write out the wsp:AppliesTo start tag
                writer.WriteStartElement(Constants.Policy.Elements.AppliesTo, Constants.Policy.NamespaceUri);
                // Write the appliesTo in WS-Addressing 1.0 format
                this.appliesTo.WriteTo(AddressingVersion.WSAddressing10, writer);
                writer.WriteEndElement(); // wsp:AppliesTo
            }

            if (this.requestorEntropy!=null)
            {
                writer.WriteStartElement(Constants.Trust.Elements.Entropy, Constants.Trust.NamespaceUri);
                BinarySecretSecurityToken bsst = this.requestorEntropy as BinarySecretSecurityToken;
                if (bsst!=null)
                {
                    writer.WriteStartElement(Constants.Trust.Elements.BinarySecret, Constants.Trust.NamespaceUri);
                    byte[] key = bsst.GetKeyBytes();
                    writer.WriteBase64(key, 0, key.Length);
                    writer.WriteEndElement(); // wst:BinarySecret
                }
                writer.WriteEndElement(); // wst:Entropy
            }

            if (this.keyType != null && this.keyType.Length > 0)
            {
                writer.WriteStartElement(Constants.Trust.Elements.KeyType, Constants.Trust.NamespaceUri);
                writer.WriteString(this.keyType);
                writer.WriteEndElement(); // wst:KeyType
            }

            if (this.keySize > 0 )
            {
                writer.WriteStartElement(Constants.Trust.Elements.KeySize, Constants.Trust.NamespaceUri);
                writer.WriteValue(this.keySize);
                writer.WriteEndElement(); // wst:KeySize
            }

            if (this.proofKey != null && this.IsProofKeyAsymmetric())
            {
                writer.WriteStartElement(Constants.Trust.Elements.UseKey, Constants.Trust.NamespaceUri);
                writer.WriteStartElement(Constants.XmlDSig.Elements.KeyInfo, Constants.XmlDSig.NamespaceUri);
                writer.WriteStartElement(Constants.XmlDSig.Elements.KeyValue, Constants.XmlDSig.NamespaceUri);
                RsaSecurityToken rsa = proofKey as RsaSecurityToken;

                if (rsa != null)
                {
                    RSAParameters p = rsa.Rsa.ExportParameters(false);
                    writer.WriteStartElement(Constants.XmlDSig.Elements.RsaKeyValue, Constants.XmlDSig.NamespaceUri);
                    writer.WriteStartElement(Constants.XmlDSig.Elements.Modulus, Constants.XmlDSig.NamespaceUri);
                    byte[] modulus = p.Modulus;
                    writer.WriteBase64(modulus, 0, modulus.Length);
                    writer.WriteEndElement(); // ds:Modulus
                    writer.WriteStartElement(Constants.XmlDSig.Elements.Exponent, Constants.XmlDSig.NamespaceUri);
                    byte[] exp = p.Exponent;
                    writer.WriteBase64(exp, 0, exp.Length);
                    writer.WriteEndElement(); // ds:Exponent
                    writer.WriteEndElement(); // ds:RsaKeyValue
                }

                writer.WriteEndElement(); // ds:KeyValue
                writer.WriteEndElement(); // ds:KeyInfo
                writer.WriteEndElement(); // wst:UseKey
            }

            if (this.claimReqs != null && this.claimReqs.Count > 0)
            {
                writer.WriteStartElement(Constants.Trust.Elements.Claims, Constants.Trust.NamespaceUri);

                foreach (ClaimTypeRequirement ctr in this.claimReqs)
                {
                    writer.WriteStartElement(Constants.IdentityModel.Elements.Claim, Constants.IdentityModel.NamespaceUri);
                    writer.WriteAttributeString(Constants.IdentityModel.Attributes.Uri, ctr.ClaimType);

                    if ( ctr.IsOptional )
                        writer.WriteAttributeString(Constants.IdentityModel.Attributes.Optional, "true");

                    writer.WriteEndElement(); // wsid:Claim
                }

                writer.WriteEndElement(); // wst:Claims
            }

            writer.WriteEndElement(); // wst:RequestSecurityToken
        }


        // private methods

        /// <summary>
        /// Reads the wst:RequestSecurityToken element
        /// </summary>
        /// <param name="xr">An XmlReader, positioned on the start tag of wst:RequestSecurityToken</param>
        /// <returns>A RequestSecurityToken instance, initialized with the data read from the XmlReader</returns>
        private static RequestSecurityToken ProcessRequestSecurityTokenElement(XmlReader xr)
        {
            // If provided XmlReader is null, throw an exception
            if (xr == null)
                throw new ArgumentNullException("xr");

            // If the wst:RequestSecurityToken element is empty, then throw an exception.
            if (xr.IsEmptyElement)
                throw new ArgumentException("wst:RequestSecurityToken element was empty. Unable to create RequestSecurityToken object");

            // Store the initial depth so we can exit this function when we reach the corresponding end-tag
            int initialDepth = xr.Depth;

            // Extract the @Context attribute value.                           
            string context = xr.GetAttribute(Constants.Trust.Attributes.Context, String.Empty);
            
            string tokenType = String.Empty;
            string requestType = String.Empty;
            int keySize = 0;
            string keyType = Constants.Trust.KeyTypes.Symmetric;            
            EndpointAddress appliesTo = null;
            SecurityToken entropy = null;
            SecurityToken proofKey = null;
            IList<ClaimTypeRequirement> claimReqs = null;

            // Enter a read loop...
            while (xr.Read())
            {                
                // Process element start tags
                if (XmlNodeType.Element == xr.NodeType)
                {
                    // Process WS-Trust elements
                    if (Constants.Trust.NamespaceUri == xr.NamespaceURI)
                    {
                        if (Constants.Trust.Elements.RequestType == xr.LocalName &&
                             !xr.IsEmptyElement)
                        {
                            xr.Read();
                            requestType = xr.ReadContentAsString();
                        }
                        else if (Constants.Trust.Elements.TokenType == xr.LocalName &&
                                 !xr.IsEmptyElement)
                        {
                            xr.Read();
                            tokenType = xr.ReadContentAsString();
                        }
                        else if (Constants.Trust.Elements.KeySize == xr.LocalName &&
                                 !xr.IsEmptyElement)
                        {
                            xr.Read();
                            keySize = xr.ReadContentAsInt();                            
                        }
                        else if (Constants.Trust.Elements.KeyType == xr.LocalName &&
                                 !xr.IsEmptyElement)
                        {
                            xr.Read();
                            keyType = xr.ReadContentAsString();
                        }
                        else if (Constants.Trust.Elements.UseKey == xr.LocalName &&
                                 !xr.IsEmptyElement)
                        {
                            proofKey = ProcessUseKeyElement(xr);
                        }
                        else if (Constants.Trust.Elements.Entropy == xr.LocalName &&
                                 !xr.IsEmptyElement)
                        {
                            entropy = ProcessEntropyElement(xr);
                        }
                        else if (Constants.Trust.Elements.Claims == xr.LocalName &&
                                 !xr.IsEmptyElement)
                        {
                            claimReqs = ProcessClaimsElement(xr);
                        }
                        else
                        {
                            Console.WriteLine("Not processing element: {0}:{1}", xr.NamespaceURI, xr.LocalName);
                        }
                    }
                    // Process WS-Policy elements
                    else if (Constants.Policy.NamespaceUri == xr.NamespaceURI)
                    {
                        if (Constants.Policy.Elements.AppliesTo == xr.LocalName &&
                            !xr.IsEmptyElement)
                        {
                            appliesTo = ProcessAppliesToElement(xr);
                        }
                        else
                        {
                            Console.WriteLine("Not processing element: {0}:{1}", xr.NamespaceURI, xr.LocalName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not processing element: {0}:{1}", xr.NamespaceURI, xr.LocalName);
                    }
                }

                // Look for the end-tag corresponding to the start-tag the reader was positioned 
                // on when the method was called
                if (Constants.Trust.Elements.RequestSecurityToken == xr.LocalName &&
                    Constants.Trust.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType )
                    break;
            }

            // Construct a new RequestSecurityToken based on the values read and return it
            return new RequestSecurityToken(context, tokenType, requestType, keySize, keyType, proofKey, entropy, claimReqs, appliesTo);
        }

        // This method assumes that the XmlReader is positioned on the start tag of wst:UseKey
        private static SecurityToken ProcessUseKeyElement(XmlReader xr)
        {
            int initialDepth = xr.Depth;

            if (xr.IsEmptyElement)
                throw new Exception("wst:UseKey element was empty. Unable to create SecurityToken object");

            SecurityToken st = null;

            while (xr.Read())
            {
                
                if (Constants.XmlDSig.Elements.KeyInfo == xr.LocalName &&
                         Constants.XmlDSig.NamespaceUri == xr.NamespaceURI &&
                         !xr.IsEmptyElement &&
                         XmlNodeType.Element == xr.NodeType)
                {
                    st = ProcessKeyInfoElement ( xr );
                }

                if (Constants.Trust.Elements.UseKey == xr.LocalName &&
                    Constants.Trust.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType)
                    break;
            }

            return st;
        }

        // This method assumes that the XmlReader is positioned on the start tag of ds:KeyInfo
        private static SecurityToken ProcessKeyInfoElement ( XmlReader xr )
        {
            int initialDepth = xr.Depth;

            if (xr.IsEmptyElement)
                throw new Exception("ds:KeyInfo element was empty. Unable to create SecurityToken object");

            SecurityToken st = null;

            while (xr.Read())
            {
                
                if (Constants.XmlDSig.Elements.KeyValue == xr.LocalName &&
                         Constants.XmlDSig.NamespaceUri == xr.NamespaceURI &&
                         !xr.IsEmptyElement &&
                         XmlNodeType.Element == xr.NodeType)
                {
                    st = ProcessKeyValueElement(xr);
                }

                if (Constants.XmlDSig.Elements.KeyInfo == xr.LocalName &&
                    Constants.XmlDSig.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType)
                    break;
            }

            return st;
        }

        // This method assumes that the XmlReader is positioned on the start tag of ds:KeyValue
        private static SecurityToken ProcessKeyValueElement(XmlReader xr)
        {
            int initialDepth = xr.Depth;

            if (xr.IsEmptyElement)
                throw new Exception("ds:KeyValue element was empty. Unable to create SecurityToken object");

            SecurityToken st = null;

            while (xr.Read())
            {
                
                if (Constants.XmlDSig.Elements.RsaKeyValue == xr.LocalName &&
                         Constants.XmlDSig.NamespaceUri == xr.NamespaceURI &&
                         !xr.IsEmptyElement &&
                         XmlNodeType.Element == xr.NodeType)
                {
                    st = ProcessRsaKeyValueElement(xr);
                }

                if (Constants.XmlDSig.Elements.KeyValue == xr.LocalName &&
                    Constants.XmlDSig.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType)
                    break;
            }

            return st;
        }

        // This method assumes that the XmlReader is positioned on the start tag of ds:RsaKeyValue
        private static SecurityToken ProcessRsaKeyValueElement(XmlReader xr)
        {
            int initialDepth = xr.Depth;

            if (xr.IsEmptyElement)
                throw new Exception("ds:RsaKeyValue element was empty. Unable to create SecurityToken object");

            SecurityToken st = null;
            StringBuilder rsaXmlString = new StringBuilder();
            rsaXmlString.Append("<RSAKeyValue>");

            while (xr.Read())
            {                
                // Process element start tags
                if (XmlNodeType.Element == xr.NodeType)
                {
                    // Process XMLDSIG elements
                    if (Constants.XmlDSig.NamespaceUri == xr.NamespaceURI)
                    {
                        if (Constants.XmlDSig.Elements.Modulus == xr.LocalName &&
                            !xr.IsEmptyElement)
                        {
                            rsaXmlString.Append("<Modulus>");
                            xr.Read();
                            rsaXmlString.Append(xr.ReadContentAsString());
                            rsaXmlString.Append("</Modulus>");
                        }
                        else if (Constants.XmlDSig.Elements.Exponent == xr.LocalName &&
                                 !xr.IsEmptyElement)
                        {
                            rsaXmlString.Append("<Exponent>");
                            xr.Read();
                            rsaXmlString.Append(xr.ReadContentAsString());
                            rsaXmlString.Append("</Exponent>");
                        }
                        else
                        {
                            Console.WriteLine("Not processing element: {0}:{1}", xr.NamespaceURI, xr.LocalName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not processing element: {0}:{1}", xr.NamespaceURI, xr.LocalName);
                    }
                }

                if (Constants.XmlDSig.Elements.RsaKeyValue == xr.LocalName &&
                    Constants.XmlDSig.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType)
                    break;
            }

            rsaXmlString.Append("</RSAKeyValue>");
            RSA key = RSA.Create();
            key.FromXmlString(rsaXmlString.ToString());
            st = new RsaSecurityToken(key);

            return st;
        }

        /// <summary>
        /// Reads a wst:Entropy element and constructs a SecurityToken
        /// Assumes that the provided entropy will never be more than 1Kb in size
        /// </summary>
        /// <param name="xr">An XmlReader positioned on the start tag of wst:Entropy</param>
        /// <returns>A SecurityToken containing the entropy value</returns>
        private static SecurityToken ProcessEntropyElement(XmlReader xr)
        {
            // If provided XmlReader is null, throw an exception
            if (xr == null)
                throw new ArgumentNullException("xr");

            // If the wst:Entropy element is empty, then throw an exception.
            if (xr.IsEmptyElement)
                throw new ArgumentException("wst:Entropy element was empty. Unable to create SecurityToken object");

            // Store the initial depth so we can exit this function when we reach the corresponding end-tag            
            int initialDepth = xr.Depth;

            // Set our return value to null
            SecurityToken st = null;

            // Enter a read loop...
            while (xr.Read())
            {
                // Look for a non-empty wst:BinarySecret element
                if (Constants.Trust.Elements.BinarySecret == xr.LocalName &&
                         Constants.Trust.NamespaceUri == xr.NamespaceURI &&
                         !xr.IsEmptyElement &&
                         XmlNodeType.Element == xr.NodeType)
                {
                    // Allocate a 1024 byte buffer for the entropy
                    byte[] temp = new byte[1024];

                    // Move reader to content of wst:BinarySecret element...
                    xr.Read();

                    // ...and read that content as base64. Store the actual number of bytes we get.                    
                    int nBytes = xr.ReadContentAsBase64(temp, 0, temp.Length);

                    // Allocate a new array of the correct size to hold the provided entropy
                    byte[] entropy = new byte[nBytes];

                    // Copy the entropy from the temporary array into the new array.
                    for (int i = 0; i < nBytes; i++)
                        entropy[i] = temp[i];

                    // Create new BinarySecretSecurityToken from the provided entropy
                    st = new BinarySecretSecurityToken(entropy);
                }

                // Look for the end-tag corresponding to the start-tag the reader was positioned 
                // on when the method was called. When we find it, break out of the read loop.
                if (Constants.Trust.Elements.Entropy == xr.LocalName &&
                    Constants.Trust.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType)
                    break;
            }

            return st;
        }

        private static IList<ClaimTypeRequirement> ProcessClaimsElement(XmlReader xr)
        {
            // If provided XmlReader is null, throw an exception
            if (xr == null)
                throw new ArgumentNullException("xr");

            // If the wst:Claims element is empty, then throw an exception.
            if (xr.IsEmptyElement)
                throw new ArgumentException("wst:Claims element was empty. Unable to create claim requirement list");

            // Store the initial depth so we can exit this function when we reach the corresponding end-tag            
            int initialDepth = xr.Depth;

            // Set our return value to null
            List<ClaimTypeRequirement> claimReqs = new List<ClaimTypeRequirement>();

            // Enter a read loop...
            while (xr.Read())
            {
                // Look for wsid:Claim elements
                if (Constants.IdentityModel.Elements.Claim == xr.LocalName &&
                         Constants.IdentityModel.NamespaceUri == xr.NamespaceURI &&
                         !xr.IsEmptyElement &&
                         XmlNodeType.Element == xr.NodeType)
                {
                    string claimType = xr.GetAttribute(Constants.IdentityModel.Attributes.Uri);
                    Console.WriteLine("ClaimType: {0}", claimType);
                    if ( claimType != null )
                    {
                        bool bOptional = false;
                        string optional = xr.GetAttribute(Constants.IdentityModel.Attributes.Optional);

                        if ( optional != null )
                        {
                            if ( "true" == optional || "True" == optional )
                                bOptional = true;
                        }

                        claimReqs.Add(new ClaimTypeRequirement(claimType, bOptional));
                    }
                }

                // Look for the end-tag corresponding to the start-tag the reader was positioned 
                // on when the method was called. When we find it, break out of the read loop.
                if (Constants.Trust.Elements.Claims == xr.LocalName &&
                    Constants.Trust.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType)
                    break;
            }

            return claimReqs;
        }

        /// <summary>
        /// Reads a wsp:AppliesTo element
        /// </summary>
        /// <param name="xr">An XmlReader positioned on the start tag of wsp:AppliesTo</param>
        /// <returns>An EndpointAddress</returns>
        private static EndpointAddress ProcessAppliesToElement(XmlReader xr)
        {
            // If provided XmlReader is null, throw an exception
            if (xr == null)
                throw new ArgumentNullException("xr");

            // If the wsp:AppliesTo element is empty, then throw an exception.
            if (xr.IsEmptyElement)
                throw new ArgumentException("wsp:AppliesTo element was empty. Unable to create EndpointAddress object");

            // Store the initial depth so we can exit this function when we reach the corresponding end-tag
            int initialDepth = xr.Depth;

            // Set our return value to null
            EndpointAddress ea = null;

            // Enter a read loop...
            while (xr.Read())
            {
                // Look for a WS-Addressing 1.0 Endpoint Reference...
                if (Constants.Addressing.Elements.EndpointReference == xr.LocalName &&
                         Constants.Addressing.NamespaceUri == xr.NamespaceURI &&
                         !xr.IsEmptyElement &&
                         XmlNodeType.Element == xr.NodeType)
                {
                    // Create a DataContractSerializer for an EndpointAddress10
                    DataContractSerializer dcs = new DataContractSerializer(typeof(EndpointAddress10));
                    // Read the EndpointAddress10 from the DataContractSerializer
                    EndpointAddress10 ea10 = (EndpointAddress10)dcs.ReadObject(xr, false);
                    // Convert the EndpointAddress10 into an EndpointAddress
                    ea = ea10.ToEndpointAddress();
                }
                // Look for a WS-Addressing 2004/08 Endpoint Reference...
                else if (Constants.Addressing.Elements.EndpointReference == xr.LocalName &&
                         Constants.Addressing.NamespaceUriAugust2004 == xr.NamespaceURI &&
                         !xr.IsEmptyElement &&
                         XmlNodeType.Element == xr.NodeType)
                {
                    // Create a DataContractSerializer for an EndpointAddressAugust2004
                    DataContractSerializer dcs = new DataContractSerializer(typeof(EndpointAddressAugust2004));
                    // Read the EndpointAddressAugust2004 from the DataContractSerializer
                    EndpointAddressAugust2004 eaAugust2004 = (EndpointAddressAugust2004)dcs.ReadObject(xr, false);
                    // Convert the EndpointAddressAugust2004 into an EndpointAddress
                    ea = eaAugust2004.ToEndpointAddress();
                }

                // Look for the end-tag corresponding to the start-tag the reader was positioned 
                // on when the method was called. When we find it, break out of the read loop.
                if (Constants.Policy.Elements.AppliesTo == xr.LocalName &&
                    Constants.Policy.NamespaceUri == xr.NamespaceURI &&
                    xr.Depth == initialDepth &&
                    XmlNodeType.EndElement == xr.NodeType)
                    break;
            }

            // Return the EndpointAddress
            return ea;
        }
    }
}
