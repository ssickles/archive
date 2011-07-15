using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ChatService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ChatService : IChat
    {
        public delegate void MessagePostedEventHandler(object sender, MessagePostedEventArgs e);
        public static event MessagePostedEventHandler MessagePosted;

        Guid clientId = Guid.Empty;
        IChatClient callback = null;
        MessagePostedEventHandler messagePostedHandler = null;

        //Clients call this service operation to subscribe.
        //A price change event handler is registered for this client instance.
        public void Subscribe(Guid ClientId)
        {
            clientId = ClientId;
            callback = OperationContext.Current.GetCallbackChannel<IChatClient>();
            messagePostedHandler = new MessagePostedEventHandler(OnMessagePosted);
            MessagePosted += messagePostedHandler;
        }

        //Clients call this service operation to unsubscribe.
        //The previous price change event handler is deregistered.
        public void Unsubscribe()
        {
            MessagePosted -= messagePostedHandler;
        }

        //Information source clients call this service operation to report a price change.
        //A price change event is raised. The price change event handlers for each subscriber will execute.
        public void PostMessage(string Message)
        {
            if (MessagePosted != null)
                MessagePosted(this, new MessagePostedEventArgs(Message));
        }

        //This event handler runs when a PriceChange event is raised.
        //The client's PriceChange service operation is invoked to provide notification about the price change.
        public void OnMessagePosted(object sender, MessagePostedEventArgs e)
        {
            callback.MessagePosted(e.Message);
        }
    }

    public class MessagePostedEventArgs : EventArgs
    {
        public MessagePostedEventArgs(string Message)
        {
            this.Message = Message;
        }

        public string Message;
    }
}
