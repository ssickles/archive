//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------
using System;

using System.ServiceModel;
using System.ServiceModel.Channels;

using Gudge.Samples.Security.RSTRSTR;

namespace Gudge.Samples.Security.SecurityTokenService
{
    [ServiceContract]
    interface IWSTrust
    {
        [OperationContract(Action = Constants.Trust.Actions.Issue, ReplyAction = Constants.Trust.Actions.IssueReply)]
        Message Issue(Message request);
    }
}
