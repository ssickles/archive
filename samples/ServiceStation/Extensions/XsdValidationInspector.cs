using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;

namespace Extensions
{
    public class XsdValidationInspector : IDispatchMessageInspector
    {
        XmlSchemaSet schemas;

        public XsdValidationInspector(XmlSchemaSet schemas) { this.schemas = schemas; }

        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(
            ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            MessageBuffer buffer = request.CreateBufferedCopy(int.MaxValue);

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(schemas);
                settings.ValidationType = ValidationType.Schema;
                
                Message msgToValidate = buffer.CreateMessage();
                XmlReader reader = XmlReader.Create(
                    msgToValidate.GetReaderAtBodyContents().ReadSubtree(), settings);

                while (reader.Read()) ; // do nothing, just validate

                request = buffer.CreateMessage();
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion
    }
}
