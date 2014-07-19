using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Xml.Schema;
using ServiceDescription = System.Web.Services.Description.ServiceDescription;

namespace WcfBehaviorExtensionSandbox
{
    public class FlatWsdlExportEndpointBehavior : IWsdlExportExtension, IEndpointBehavior
    {
        #region IWsdlExportExtension Members

        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            XmlSchemaSet schemaSet = exporter.GeneratedXmlSchemas;

            foreach (ServiceDescription wsdl in exporter.GeneratedWsdlDocuments)
            {
                List<XmlSchema> importsList = new List<XmlSchema>();

                foreach (XmlSchema schema in wsdl.Types.Schemas)
                {
                    AddImportedSchemas(schema, schemaSet, importsList);
                }

                wsdl.Types.Schemas.Clear();

                foreach (XmlSchema schema in importsList)
                {
                    //RemoveXsdImports(schema);
                    wsdl.Types.Schemas.Add(schema);
                }
            }
        }

        #endregion

        private void AddImportedSchemas(XmlSchema schema, XmlSchemaSet schemaSet, List<XmlSchema> importsList)
        {
            foreach (XmlSchemaImport import in schema.Includes)
            {
                ICollection realSchemas = schemaSet.Schemas(import.Namespace);
                foreach (XmlSchema ixsd in realSchemas)
                {
                    if (!importsList.Contains(ixsd))
                    {
                        importsList.Add(ixsd);
                        AddImportedSchemas(ixsd, schemaSet, importsList);
                    }
                }
            }
        }

        private void RemoveXsdImports(XmlSchema schema)
        {
            for (int i = 0; i < schema.Includes.Count; i++)
            {
                if (schema.Includes[i] is XmlSchemaImport)
                    schema.Includes.RemoveAt(i--);
            }
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
