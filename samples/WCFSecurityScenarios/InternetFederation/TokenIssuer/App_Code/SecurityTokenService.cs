//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------
using System;

using System.Collections.Generic;

using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

using Gudge.Samples.Security.RSTRSTR;
using System.Security;

namespace Gudge.Samples.Security.SecurityTokenService
{
    class SecurityTokenService : IWSTrust
    {
        // member variables
        string issuer;
        
        // Public constructors
        public SecurityTokenService() : this("GudgeSTS")
        {            
        }

        public SecurityTokenService( string issuerName)
        {
            issuer = issuerName;
        }

        #region IWSTrust Members

        public Message Issue(Message request)
        {
            try
            {
                Console.WriteLine("Call to IWSTrust::Issue");

                // if request is null, we're toast
                if (request == null)
                    throw new ArgumentNullException("request");

                // Create an RST object from the request message
                Gudge.Samples.Security.RSTRSTR.RequestSecurityToken rst = Gudge.Samples.Security.RSTRSTR.RequestSecurityToken.CreateFrom(request.GetReaderAtBodyContents());

                // Check that is really is an Issue request
                if (rst.RequestType == null || rst.RequestType != Constants.Trust.RequestTypes.Issue)
                    throw new InvalidOperationException(rst.RequestType);
            
                // Create an RSTR object
                Gudge.Samples.Security.RSTRSTR.RequestSecurityTokenResponse rstr = Issue(rst);

                // Create response message
                Message response = Message.CreateMessage(request.Version, Constants.Trust.Actions.IssueReply, rstr);

                // Set RelatesTo of response to message id of request
                response.Headers.RelatesTo = request.Headers.MessageId;

                // Address response to ReplyTo of request
                request.Headers.ReplyTo.ApplyTo(response);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("**** Exception thrown while processing Issue request:");
                Console.WriteLine(e.Message);
                throw;
            }            
        }

        #endregion

        // private methods
        private Gudge.Samples.Security.RSTRSTR.RequestSecurityTokenResponse Issue(Gudge.Samples.Security.RSTRSTR.RequestSecurityToken rst)
        {
            // If rst is null, we're toast
            if (rst == null)
                throw new ArgumentNullException("rst");

            // Create an RSTR object
            Gudge.Samples.Security.RSTRSTR.RequestSecurityTokenResponse rstr = new Gudge.Samples.Security.RSTRSTR.RequestSecurityTokenResponse();

            // Figure out the token type being requested
            string tokenType = GetTokenType(rst);
            Console.WriteLine("Issue: Request for token type {0}", tokenType);

            // Figure out which key we're going to sign the token with...
            SecurityKey signingKey = GetSigningKey();
            SecurityKeyIdentifier signingKeyIdentifier = GetSigningKeyIdentifier();
            SecurityKeyIdentifier proofKeyIdentifier = null;

            if (rst.IsProofKeyAsymmetric())
            {
                Console.WriteLine("Retrieving Asymmetric Proof Key");
                // Asymmetric proof key
                proofKeyIdentifier = GetAsymmetricProofKey(rst);
            }
            else
            {
                // Symmetric proof key
                Console.WriteLine("Constructing Symmetric Proof Key");
                // Construct session key. This is the symmetric key that the client and the service will share. 
                // It actually appears twice in the response message; once for the service and 
                // once for the client. In the former case, it is typically embedded in the issued token, 
                // in the latter case, it is returned in a wst:RequestedProofToken element.
                byte[] sessionKey = GetSessionKey(rst, rstr);
                
                // Get token to use when encrypting key material for the service
                SecurityToken encryptingToken = GetEncryptingToken(rst);

                // Encrypt the session key for the service
                
                byte[] encryptedKey = GetEncryptedKey(encryptingToken, sessionKey, out proofKeyIdentifier);
            }            

            // Issued tokens are valid for 12 hours by default
            DateTime effectiveTime = DateTime.Now;
            DateTime expirationTime = DateTime.Now + new TimeSpan(12, 0, 0);
            SecurityToken st = null;

            switch (tokenType)
            {
                case "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1":
                    st = CreateSAMLToken(effectiveTime, expirationTime, signingKey, signingKeyIdentifier, proofKeyIdentifier, rst.ClaimRequirements );
                    break;
                default:
                    throw new Exception("Unsupported Token Type");
            }

            rstr.RequestedSecurityToken = st;
            rstr.Context = rst.Context;
            rstr.TokenType = tokenType;
            rstr.RequestedAttachedReference = GetInternalSecurityKeyIdentifier(st)[0];
            rstr.RequestedUnattachedReference = GetExternalSecurityKeyIdentifier(st)[0];
            return rstr;
        }

        // This method determines the token type for the service the issued token is intended for.
        private string GetTokenType(Gudge.Samples.Security.RSTRSTR.RequestSecurityToken rst)
        {
            // If rst is null, we're toast
            if (rst == null)
                throw new ArgumentNullException("rst");

            // Set tokenType to null
            string tokenType = null;

            // If there is no service URI or we don't have a tokenType registered for that service URI...
            tokenType = rst.TokenType;
            
            // return the tokenType
            return tokenType;
        }

        private byte[] GetSenderEntropy(Gudge.Samples.Security.RSTRSTR.RequestSecurityToken rst)
        {
            // If rst is null, we're toast
            if (rst == null)
                throw new ArgumentNullException("rst");

            SecurityToken senderEntropyToken = rst.RequestorEntropy;
            byte[] senderEntropy = null;

            if (senderEntropyToken != null)
            {
                BinarySecretSecurityToken bsst = senderEntropyToken as BinarySecretSecurityToken;

                if (bsst != null)
                    senderEntropy = bsst.GetKeyBytes();
            }

            return senderEntropy;
        }

        private byte[] GetIssuerEntropy(int keySize)
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] issuerEntropy = new byte[keySize / 8];
            random.GetNonZeroBytes(issuerEntropy);
            return issuerEntropy;
        }

        private byte[] GetSessionKey(Gudge.Samples.Security.RSTRSTR.RequestSecurityToken rst, Gudge.Samples.Security.RSTRSTR.RequestSecurityTokenResponse rstr)
        {
            // If rst is null, we're toast
            if (rst == null)
                throw new ArgumentNullException("rst");

            // If rstr is null, we're toast
            if (rstr == null)
                throw new ArgumentNullException("rstr");

            // Figure out the keySize
            int keySize = 256;

            if (rst.KeySize != 0)
                keySize = rst.KeySize;

            Console.WriteLine("Proof key size {0}", keySize);

            // Figure out whether we're using Combined or Issuer entropy.
            byte[] sessionKey = null;
            byte[] senderEntropy = GetSenderEntropy(rst);
            byte[] issuerEntropy = GetIssuerEntropy(keySize);
            
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

            if (senderEntropy != null)
            {
                // Combined entropy
                Console.WriteLine("Combined Entropy");                
                sessionKey = rstr.ComputeCombinedKey(senderEntropy, issuerEntropy, keySize);
                rstr.IssuerEntropy = new BinarySecretSecurityToken ( issuerEntropy );
                rstr.ComputeKey = true;
            }
            else
            {
                // Issuer-only entropy
                Console.WriteLine("Issuer-only entropy");
                sessionKey = issuerEntropy;
                rstr.RequestedProofToken = new BinarySecretSecurityToken(sessionKey);
            }

            rstr.KeySize = keySize;
            return sessionKey;
        }

        // This method retrieves the key that the STS should sign the issued token with in order 
        // for the service the issued token is intended for to trust that token
        private SecurityKey GetSigningKey()
        {
            // Find the signing cert
            SecurityToken token = GetSigningToken();

            if (token != null)
                // return the zeroth key
                return token.SecurityKeys[0];
            else
                return null; 
        }

        // This method determines the security token that contains the key material that 
        // the STS should sign the issued token with in order 
        // for the service the issued token is intended for to trust that token        
        private SecurityToken GetSigningToken()
        {
            // Set signingCert to null
            SecurityToken signingToken = null;

            // ... Open the LocalMachine store in My for read-only access
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            // Find the "STS" cert ...
            foreach (X509Certificate2 cert in store.Certificates)
                if (cert.SubjectName.Name == "CN=IPKey")
                {
                    // ... and set signingCert to that cert
                    signingToken = new X509SecurityToken(cert);
                    break;
                }

            // Don't forget to close the store
            store.Close();

            // return the token
            return signingToken;
        }

        // This method determines the security token that contains the key material that 
        // the STS should encrypt a session key with in order 
        // for the service the issued token is intended for to be able to extract that session key
        private SecurityToken GetEncryptingToken(Gudge.Samples.Security.RSTRSTR.RequestSecurityToken rst)
        {
            // If rst is null, we're toast
            if (rst == null)
                throw new ArgumentNullException("rst");

            // Set encryptingToken to null
            SecurityToken encryptingToken = null;

            // ... Open the LocalMachine store in My for read-only access
            X509Store store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            // Find the "localhost" cert ...
            foreach (X509Certificate2 cert in store.Certificates)
                if (cert.SubjectName.Name == "CN=RPKey")
                {
                    // ... and set encryptingToken to that cert
                    encryptingToken = new X509SecurityToken(cert);
                    break;
                }

            // Don't forget to close the store
            store.Close();

            // return the token
            return encryptingToken;
        }


        // This method returns the security token that contains the key material that should be used to 
        // encrypt the session that will be used by the client when talking to the service endpoint
        // This method is called when creating the RequestedProofToken
        private SecurityToken GetClientKeyWrapToken()
        {
            // Get the current OperationContext
            OperationContext oc = OperationContext.Current;

            // set token to null
            SecurityToken token = null;

            // If there is an initiator token...
            if (oc.IncomingMessageProperties.Security.InitiatorToken != null)
            {
                Console.WriteLine("GetClientKeyWrapToken returning InitiatorToken");
                // .. Extract it
                token = oc.IncomingMessageProperties.Security.InitiatorToken.SecurityToken;
            }
            // ... otherwise, if these is a protection token...
            else if (oc.IncomingMessageProperties.Security.ProtectionToken != null)
            {
                Console.WriteLine("GetClientKeyWrapToken returning ProtectionToken");
                // ... extract that instread
                token = oc.IncomingMessageProperties.Security.ProtectionToken.SecurityToken;
            }
            // ... otherwise return null
            else
            {
                Console.WriteLine("GetClientKeyWrapToken returning null");
            }

            // return the token
            return token;
        }

        // This method returns a security key identifier that can be used to refer to the provided security token when 
        // the provided token appears in the message
        private SecurityKeyIdentifier GetInternalSecurityKeyIdentifier(SecurityToken t)
        {
            // If t is null, we're toast
            if ( t == null )
                throw new ArgumentNullException ( "t" );

            // Set skiClause to null
            SecurityKeyIdentifierClause skiClause = null;

            // Try for a local id reference first...
            if (t.CanCreateKeyIdentifierClause<LocalIdKeyIdentifierClause>())
            {
                Console.WriteLine("GetInternalSecurityKeyIdentifier using LocalIdKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<LocalIdKeyIdentifierClause>();
                
                // return a SecurityKeyIdentifier 
                return new SecurityKeyIdentifier(skiClause);
            }
            else 
                return GetExternalSecurityKeyIdentifier ( t );
            
            
        }

        // This method returns a security key identifier that can be used to refer to the provided security token when 
        // the provided token does not appear in the message
        private SecurityKeyIdentifier GetExternalSecurityKeyIdentifier(SecurityToken t)
        {
            // If t is null, we're toast
            if (t == null)
                throw new ArgumentNullException("t");

            // Set skiClause to null
            SecurityKeyIdentifierClause skiClause = null;

            // Try for an encrypted key reference first...
            if (t.CanCreateKeyIdentifierClause<EncryptedKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using EncryptedKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<EncryptedKeyIdentifierClause>();
            }
            // ... kerb token reference next...
            else if (t.CanCreateKeyIdentifierClause<KerberosTicketHashKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using KerberosTicketHashKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<KerberosTicketHashKeyIdentifierClause>();
            }
            // ... X509 thumbprint next...
            else if (t.CanCreateKeyIdentifierClause<X509ThumbprintKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using X509ThumbprintKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<X509ThumbprintKeyIdentifierClause>();
            }
            // ... X509 raw reference next...
            else if (t.CanCreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using X509RawDataKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>();
            }
            // ... X509 SKI next...
            else if (t.CanCreateKeyIdentifierClause<X509SubjectKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using X509SubjectKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<X509SubjectKeyIdentifierClause>();
            }
            // ... try for a binary secret...
            else if (t.CanCreateKeyIdentifierClause<BinarySecretKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using BinarySecretKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<BinarySecretKeyIdentifierClause>();
            }
            // ... then a X509IssuerSerial reference ...
            else if (t.CanCreateKeyIdentifierClause<X509IssuerSerialKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using X509IssuerSerialKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<X509IssuerSerialKeyIdentifierClause>();
            }
            // ... then a SAML assertion reference...
            else if (t.CanCreateKeyIdentifierClause<SamlAssertionKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using SamlAssertionKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<SamlAssertionKeyIdentifierClause>();
            }
            
            // ... then an RSA key reference...
            else if (t.CanCreateKeyIdentifierClause<RsaKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using RsaKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<RsaKeyIdentifierClause>();
            }
            // ... then a key name reference...
            else if (t.CanCreateKeyIdentifierClause<KeyNameIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using KeyNameIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<KeyNameIdentifierClause>();
            }
            // ... and finally an SCT reference...
            else if (t.CanCreateKeyIdentifierClause<SecurityContextKeyIdentifierClause>())
            {
                Console.WriteLine("GetExternalSecurityKeyIdentifier using SecurityContextKeyIdentifierClause");
                skiClause = t.CreateKeyIdentifierClause<SecurityContextKeyIdentifierClause>();
            }            

            // return a SecurityKeyIdentifier
            return new SecurityKeyIdentifier(skiClause);
        }

        // This method returns a security token to be used as the requested proof token portion of the RSTR
        // The key parameter is the proof key that will be shared by the client and the actual service
        private SecurityToken GetRequestedProofToken(byte[] key)
        {
            // If key is null, we're toast
            if (key == null)
                throw new ArgumentNullException("key");

            // Get the security token that will be used to encrypt the provided key
            SecurityToken wrappingToken = GetClientKeyWrapToken();

            // Get the encryptionAlgorithm to use
            // TODO: Fix this to not rely on first key
            // TODO: Honour any algorithm requests put into the RST by the client
            string keywrapAlgorithm = GetKeyWrapAlgorithm(wrappingToken.SecurityKeys[0]); ;

            // Get the SecurityKeyIdentifier for the wrapping token
            SecurityKeyIdentifier wrappingTokenReference = GetInternalSecurityKeyIdentifier(wrappingToken);

            // Create a new Wrapped token and return it
            return new WrappedKeySecurityToken("_" + Guid.NewGuid().ToString(), key, keywrapAlgorithm, wrappingToken, wrappingTokenReference);
        }

        private string GetKeyWrapAlgorithm(SecurityKey key)
        {
            // If key is null, we're toast
            if (key == null)
                throw new ArgumentNullException("key");

            // Set keywrapAlgorithm to null
            string keywrapAlgorithm = null;

            // If the security key supports RsaOaep then use that ...
            if (key.IsSupportedAlgorithm(SecurityAlgorithms.RsaOaepKeyWrap))
                keywrapAlgorithm = SecurityAlgorithms.RsaOaepKeyWrap;
            // ... otherwise if it supports RSA15 use that ...
            else if (key.IsSupportedAlgorithm(SecurityAlgorithms.RsaV15KeyWrap))
                keywrapAlgorithm = SecurityAlgorithms.RsaV15KeyWrap;
            // ... otherwise if it supports AES256 use that ...
            else if (key.IsSupportedAlgorithm(SecurityAlgorithms.Aes256KeyWrap))
                keywrapAlgorithm = SecurityAlgorithms.Aes256KeyWrap;
            // ... otherwise if it supports AES192 use that ...
            else if (key.IsSupportedAlgorithm(SecurityAlgorithms.Aes192KeyWrap))
                keywrapAlgorithm = SecurityAlgorithms.Aes192KeyWrap;
            // ... otherwise if it supports AES128 use that ...
            else if (key.IsSupportedAlgorithm(SecurityAlgorithms.Aes128KeyWrap))
                keywrapAlgorithm = SecurityAlgorithms.Aes128KeyWrap;

            return keywrapAlgorithm;
        }

        private string GetSignatureAlgorithm(SecurityKey key)
        {
            // If key is null, we're toast
            if (key == null)
                throw new ArgumentNullException("key");

            // Set signatureAlgorithm to null
            string signatureAlgorithm = null;

            // If the security key supports RsaSha1 then use that ...
            if (key.IsSupportedAlgorithm(SecurityAlgorithms.RsaSha1Signature))
                signatureAlgorithm = SecurityAlgorithms.RsaSha1Signature;
            // ... otherwise if it supports HMACSha1 use that ...
            else if (key.IsSupportedAlgorithm(SecurityAlgorithms.HmacSha1Signature))
                signatureAlgorithm = SecurityAlgorithms.HmacSha1Signature;

            return signatureAlgorithm;
        }

        private string GetEncryptionAlgorithm(SecurityKey key)
        {
            // If key is null, we're toast
            if (key == null)
                throw new ArgumentNullException("key");

            // Set encryptionAlgorithm to null
            string encryptionAlgorithm = null;

            // If the security key supports AES256 use that ...
            if (key.IsSupportedAlgorithm(SecurityAlgorithms.Aes256Encryption))
                encryptionAlgorithm = SecurityAlgorithms.Aes256Encryption;
            // ... otherwise if it supports AES192 use that ...
            else if (key.IsSupportedAlgorithm(SecurityAlgorithms.Aes192Encryption))
                encryptionAlgorithm = SecurityAlgorithms.Aes192Encryption;
            // ... otherwise if it supports AES128 use that ...
            else if (key.IsSupportedAlgorithm(SecurityAlgorithms.Aes128Encryption))
                encryptionAlgorithm = SecurityAlgorithms.Aes128Encryption;

            return encryptionAlgorithm;
        }

        // This method returns a SecurityKeyIdentifier for the key the STS will use to sign the issued token 
        private SecurityKeyIdentifier GetSigningKeyIdentifier()
        {
            // Get the token that the STS will sign the token with
            SecurityToken signingToken = GetSigningToken();

            // return a SecurityKeyIdentifier for the token
            return GetExternalSecurityKeyIdentifier(signingToken);
        }

        // This method encrypts the provided key using the key material associated with the cert
        // returned by DetermineEncryptingCert
        private byte[] GetEncryptedKey(SecurityToken encryptingToken, byte[] key, out SecurityKeyIdentifier ski)
        {
            // If encryptingToken is null, we're toast
            if (encryptingToken == null)
                throw new ArgumentNullException("encryptingToken");

            // If key is null, we're toast
            if (key == null)
                throw new ArgumentNullException("key");

            // Get the zeroth security key
            SecurityKey encryptingKey = encryptingToken.SecurityKeys[0];

            // Get the encryption algorithm to use
            string keywrapAlgorithm = GetKeyWrapAlgorithm(encryptingKey);
            
            // encrypt the passed in key 
            byte[] encryptedKey = encryptingKey.EncryptKey ( keywrapAlgorithm, key );

            // get a key identifier for the encrypting key
            SecurityKeyIdentifier eki = GetExternalSecurityKeyIdentifier(encryptingToken);

            // return the proof key identifier
            ski = GetProofKeyIdentifier ( encryptedKey, keywrapAlgorithm, eki );

            // return the encrypted key
            return encryptedKey;
        }

        private SecurityKeyIdentifier GetProofKeyIdentifier(byte[] key, string algorithm, SecurityKeyIdentifier eki )
        {
            // If key is null, we're toast
            if (key == null)
                throw new ArgumentNullException("key");

            // return a SecurityKeyIdentifier
            return new SecurityKeyIdentifier(new EncryptedKeyIdentifierClause(key, algorithm, eki));
        }

        private SecurityKeyIdentifier GetAsymmetricProofKey(Gudge.Samples.Security.RSTRSTR.RequestSecurityToken rst)
        {
            return GetInternalSecurityKeyIdentifier(rst.ProofKey);
        }

        private SecurityToken CreateSAMLToken(DateTime validFrom, DateTime validTo, SecurityKey signingKey, SecurityKeyIdentifier signingKeyIdentifier, SecurityKeyIdentifier proofKeyIdentifier, IList<ClaimTypeRequirement> claimReqs )
        {
            // Create list of confirmation strings
            List<string> confirmations = new List<string>();

            // Add holder-of-key string to list of confirmation strings
            confirmations.Add(SamlConstants.HolderOfKey);

            // Create SAML subject statement based on issuer member variable, confirmation string collection 
            // local variable and proof key identifier parameter
            SamlSubject subject = new SamlSubject(null, null, issuer, confirmations, null, proofKeyIdentifier);

            // Create a list of SAML attributes
            List<SamlAttribute> attributes = new List<SamlAttribute>();

            // Get the claimset we want to place into the SAML assertion
            ClaimSet cs = GetClaimSet(claimReqs);

            // Iterate through the claims and add a SamlAttribute for each claim
            // Note that GetClaimSet call above returns a claimset that only contains PossessProperty claims
            foreach (Claim c in cs)
                attributes.Add(new SamlAttribute(c));

            // Create list of SAML statements
            List<SamlStatement> statements = new List<SamlStatement>();

            // Add a SAML attribute statement to the list of statements. Attribute statement is based on 
            // subject statement and SAML attributes resulting from claims
            statements.Add(new SamlAttributeStatement(subject, attributes));

            // Create a valid from/until condition
            SamlConditions conditions = new SamlConditions(validFrom, validTo);
            
            // Create the SAML assertion
            SamlAssertion assertion = new SamlAssertion("_" + Guid.NewGuid().ToString(), issuer, validFrom, conditions, null, statements);

            // Set the signing credentials for the SAML assertion
            string signatureAlgorithm = GetSignatureAlgorithm(signingKey);
            assertion.SigningCredentials = new SigningCredentials(signingKey, signatureAlgorithm, SecurityAlgorithms.Sha1Digest, signingKeyIdentifier);

            SamlSecurityToken token = new SamlSecurityToken(assertion);
            Console.WriteLine("token.SecurityKeys.Count: {0}", token.SecurityKeys.Count);
            return token;
        }

        // Return a ClaimSet to be serialized into an issued SAML token
        private ClaimSet GetClaimSet(IList<ClaimTypeRequirement> claimReqs)
        {
            // Create an empty list for claims
            List<Claim> claims = new List<Claim>();
            ClaimSet issuedClaims = null;

            // Iterate through all the ClaimSets in the current AuthorizationContext
            foreach (ClaimSet cs in OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            {
                // If the issuer of the ClaimSet is this STS...
                if ( cs.Issuer.ContainsClaim ( Claim.CreateNameClaim("http://www.thatindigogirl.com/samples/2006/07/issuer")))
                {
                    issuedClaims = cs;
                }
            }

            if (claimReqs != null)
            {
                foreach (ClaimTypeRequirement ctr in claimReqs)
                {
                    if (Constants.IdentityModel.ClaimTypes.PPI == ctr.ClaimType)
                        claims.Add(new Claim(ctr.ClaimType, Guid.NewGuid().ToString(), Rights.PossessProperty));
                }
            }

            // all all issued claims
            foreach (Claim c in issuedClaims)
                claims.Add(c);
                
            // Create a new ClaimSet based on the claims list and return that ClaimSet
            return new DefaultClaimSet ( DefaultClaimSet.System, claims );
        }
    }
}
