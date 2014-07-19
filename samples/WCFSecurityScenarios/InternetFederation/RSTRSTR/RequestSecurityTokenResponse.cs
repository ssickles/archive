//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------
using System;

using System.IdentityModel.Tokens;

using System.Security.Cryptography;

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

using System.Xml;

namespace Gudge.Samples.Security.RSTRSTR
{
    public class RequestSecurityTokenResponse : RequestSecurityTokenBase
    {
        // private members
        private SecurityToken requestedSecurityToken;
        private SecurityToken requestedProofToken;
        private SecurityToken issuerEntropy;
        private SecurityKeyIdentifierClause requestedAttachedReference;
        private SecurityKeyIdentifierClause requestedUnattachedReference;
        private bool computeKey;

        // Constructors
        public RequestSecurityTokenResponse()
            : this(String.Empty, String.Empty, 0, null, null, null, false)
        {
        }

        public RequestSecurityTokenResponse(string context, string tokenType, int keySize, EndpointAddress appliesTo, SecurityToken requestedSecurityToken, SecurityToken requestedProofToken, bool computeKey ) : base ( context, tokenType, keySize, appliesTo )
        {
            this.requestedSecurityToken = requestedSecurityToken;
            this.requestedProofToken = requestedProofToken;
            this.computeKey = computeKey;
        }

        // public properties
        public SecurityToken RequestedSecurityToken 
        { 
            get { return requestedSecurityToken; } 
            set { requestedSecurityToken = value; } 
        }

        public SecurityToken RequestedProofToken
        {
            get { return requestedProofToken; }
            set { requestedProofToken = value; }
        }

        public SecurityKeyIdentifierClause RequestedAttachedReference
        {
            get { return requestedAttachedReference; }
            set { requestedAttachedReference = value; }
        }

        public SecurityKeyIdentifierClause RequestedUnattachedReference
        {
            get { return requestedUnattachedReference; }
            set { requestedUnattachedReference = value; }
        }

        public SecurityToken IssuerEntropy
        {
            get { return issuerEntropy; }
            set { issuerEntropy = value; }
        }

        public bool ComputeKey
        {
            get { return computeKey; }
            set { computeKey = value; }
        }

        // public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestorEntropy"></param>
        /// <param name="issuerEntropy"></param>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public byte[] ComputeCombinedKey(byte[] requestorEntropy, byte[] issuerEntropy, int keySize)
        {
            if (keySize < 64 || keySize > 4096)
                throw new ArgumentOutOfRangeException("keySize");

            KeyedHashAlgorithm kha = new HMACSHA1(requestorEntropy, true);

            byte[] key = new byte[keySize / 8]; // Final key
            byte[] a = issuerEntropy; // A(0)
            byte[] b = new byte[kha.HashSize / 8 + a.Length]; // Buffer for A(i) + seed

            for (int i = 0; i < key.Length;)
            {
                // Calculate A(i+1).                
                kha.Initialize();
                a = kha.ComputeHash(a);

                // Calculate A(i) + seed
                a.CopyTo(b, 0);
                issuerEntropy.CopyTo(b, a.Length);
                kha.Initialize();
                byte[] result = kha.ComputeHash(b);

                for (int j = 0; j < result.Length; j++)
                {
                    if (i < key.Length)
                        key[i++] = result[j];
                    else
                        break;
                }
            }

            return key;
        }

        // Methods of BodyWriter
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement(Constants.Trust.Elements.RequestSecurityTokenResponse, Constants.Trust.NamespaceUri);

            if (this.tokenType != null && this.tokenType.Length > 0)
            {
                writer.WriteStartElement(Constants.Trust.Elements.TokenType, Constants.Trust.NamespaceUri);
                writer.WriteString(this.tokenType);
                writer.WriteEndElement(); // wst:TokenType
            }

            WSSecurityTokenSerializer ser = new WSSecurityTokenSerializer();

            if (this.requestedSecurityToken != null)
            {
                writer.WriteStartElement(Constants.Trust.Elements.RequestedSecurityToken, Constants.Trust.NamespaceUri);                
                ser.WriteToken(writer, requestedSecurityToken);                    
                writer.WriteEndElement(); // wst:RequestedSecurityToken
            }

            if ( this.requestedAttachedReference != null )
            {
                writer.WriteStartElement(Constants.Trust.Elements.RequestedAttachedReference, Constants.Trust.NamespaceUri);
                ser.WriteKeyIdentifierClause ( writer, this.requestedAttachedReference );
                writer.WriteEndElement(); // wst:RequestedAttachedReference
            }

            if ( this.requestedUnattachedReference != null )
            {
                writer.WriteStartElement(Constants.Trust.Elements.RequestedUnattachedReference, Constants.Trust.NamespaceUri);
                ser.WriteKeyIdentifierClause ( writer, this.requestedUnattachedReference );
                writer.WriteEndElement(); // wst:RequestedAttachedReference
            }

            if (this.appliesTo != null)
            {
                writer.WriteStartElement(Constants.Policy.Elements.AppliesTo, Constants.Policy.NamespaceUri);
                this.appliesTo.WriteTo(AddressingVersion.WSAddressing10, writer);
                writer.WriteEndElement(); // wsp:AppliesTo
            }

            if (this.requestedProofToken != null)// Issuer entropy; write RPT only
            {
                writer.WriteStartElement(Constants.Trust.Elements.RequestedProofToken, Constants.Trust.NamespaceUri);
                ser.WriteToken(writer, requestedProofToken);
                writer.WriteEndElement(); // wst:RequestedSecurityToken
            }

            if(this.issuerEntropy != null && this.computeKey ) // Combined entropy; write RPT and Entropy
            {
                writer.WriteStartElement(Constants.Trust.Elements.RequestedProofToken, Constants.Trust.NamespaceUri);
                writer.WriteStartElement(Constants.Trust.Elements.ComputedKey, Constants.Trust.NamespaceUri);
                writer.WriteValue(Constants.Trust.ComputedKeyAlgorithms.PSHA1);
                writer.WriteEndElement(); // wst:ComputedKey
                writer.WriteEndElement(); // wst:RequestedSecurityToken

                if (this.issuerEntropy != null)
                {
                    writer.WriteStartElement(Constants.Trust.Elements.Entropy, Constants.Trust.NamespaceUri);                    
                    ser.WriteToken(writer, this.issuerEntropy);
                    writer.WriteEndElement(); // wst:Entropy
                }
            }

            writer.WriteEndElement(); // wst:RequestSecurityTokenResponse
        }
    }
}
