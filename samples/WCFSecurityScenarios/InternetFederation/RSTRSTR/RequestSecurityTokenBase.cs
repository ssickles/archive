//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------
using System;

using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Gudge.Samples.Security.RSTRSTR
{
    public abstract class RequestSecurityTokenBase : BodyWriter
    {
        // private members
        protected string context;
        protected string tokenType;
        protected int keySize;
        protected EndpointAddress appliesTo;
        
        // Constructors
        protected RequestSecurityTokenBase() : this(String.Empty,String.Empty,0, null)
        {
        }

        protected RequestSecurityTokenBase(string context, string tokenType, int keySize, EndpointAddress appliesTo )
            : base(true)
        {
            this.context = context;
            this.tokenType = tokenType;
            this.keySize = keySize;
            this.appliesTo = appliesTo;
        }

        // public properties
        public string Context
        {
            get { return context; }
            set { context = value; }
        }

        public string TokenType 
        { 
            get { return tokenType; }
            set { tokenType = value; }
        }

        public int KeySize
        {
            get { return keySize; }
            set { keySize = value; }
        }

        public EndpointAddress AppliesTo
        {
            get { return appliesTo; }
            set { appliesTo = value; }
        }
    }
}
