//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace ServiceModelEx
{
   public static class MetadataHelper
   {
      public static ServiceEndpointCollection GetEndpoints(string mexAddress)
      {
         if(String.IsNullOrEmpty(mexAddress))
         {
            Debug.Assert(false,"Empty address");
            return null;
         }
         if(mexAddress.EndsWith("?wsdl") == false)
         {
            mexAddress += "?wsdl";
         }

         BasicHttpBinding binding = new BasicHttpBinding();
         binding.MaxReceivedMessageSize *= 4;
         MetadataExchangeClient MEXClient = new MetadataExchangeClient(binding);

         MetadataSet metadata =  MEXClient.GetMetadata(new Uri(mexAddress),MetadataExchangeClientMode.HttpGet);
         MetadataImporter importer = new WsdlImporter(metadata);
         return importer.ImportAllEndpoints();
      }
      public static Type GetCallbackContract(string mexAddress,Type contractType)
      {
         if(contractType.IsInterface == false)
         {
            Debug.Assert(false,contractType + " is not an interface");
            return null;
         }

         object[] attributes = contractType.GetCustomAttributes(typeof(ServiceContractAttribute),false);
         if(attributes.Length == 0)
         {
            Debug.Assert(false,"Interface " + contractType + " does not have the ServiceContractAttribute");
            return null;
         }
         ServiceContractAttribute attribute = attributes[0] as ServiceContractAttribute;
         if(attribute.Name == null)
         {
            attribute.Name = contractType.ToString();
         }
         if(attribute.Namespace == null)
         {
            attribute.Namespace = "http://tempuri.org/";
         }
         return GetCallbackContract(mexAddress,attribute.Namespace,attribute.Name);
      }

      public static Type GetCallbackContract(string mexAddress,string contractNamespace,string contractName)
      {
         if(String.IsNullOrEmpty(contractNamespace))
         {
            Debug.Assert(false,"Empty namespace");
            return null;
         }
         if(String.IsNullOrEmpty(contractName))
         {
            Debug.Assert(false,"Empty name");
            return null;
         }
         try
         {
            ServiceEndpointCollection endpoints = GetEndpoints(mexAddress);
            foreach(ServiceEndpoint endpoint in endpoints)
            {
               if(endpoint.Contract.Namespace == contractNamespace && endpoint.Contract.Name == contractName)
               {
                  return endpoint.Contract.CallbackContractType;
               }
            }
         }
         catch
         {}
         return null;
      }
      public static bool QueryContract(string mexAddress,Type contractType)
      {
         if(contractType.IsInterface == false)
         {
            Debug.Assert(false,contractType + " is not an interface");
            return false;
         }

         object[] attributes = contractType.GetCustomAttributes(typeof(ServiceContractAttribute),false);
         if(attributes.Length == 0)
         {
            Debug.Assert(false,"Interface " + contractType + " does not have the ServiceContractAttribute");
            return false;
         }
         ServiceContractAttribute attribute = attributes[0] as ServiceContractAttribute;
         if(attribute.Name == null)
         {
            attribute.Name = contractType.ToString();
         }
         if(attribute.Namespace == null)
         {
            attribute.Namespace = "http://tempuri.org/";
         }
         return QueryContract(mexAddress,attribute.Namespace,attribute.Name);
      }
      public static bool QueryContract(string mexAddress,string contractNamespace,string contractName)
      {
         if(String.IsNullOrEmpty(contractNamespace))
         {
            Debug.Assert(false,"Empty namespace");
            return false;
         }
         if(String.IsNullOrEmpty(contractName))
         {
            Debug.Assert(false,"Empty name");
            return false;
         }
         try
         {
            ServiceEndpointCollection endpoints = GetEndpoints(mexAddress);
            foreach(ServiceEndpoint endpoint in endpoints)
            {
               if(endpoint.Contract.Namespace == contractNamespace && endpoint.Contract.Name == contractName)
               {
                  return true;
               }
            }
         }
         
         catch
         {}
         return false;
      }
      public static string[] GetContracts(string mexAddress)
      {
         return GetContracts<Binding>(mexAddress);
      }
      public static string[] GetContracts<B>(string mexAddress) where B : Binding
      {
         ServiceEndpointCollection endpoints = GetEndpoints(mexAddress);

         List<string> contracts = new List<string>();
         string contract;
         foreach(ServiceEndpoint endpoint in endpoints)
         {
            if(typeof(B).IsInstanceOfType(endpoint.Binding))
            {
               contract = endpoint.Contract.Namespace + " " + endpoint.Contract.Name;

               if(contracts.Contains(contract) == false)
               {
                  contracts.Add(contract);
               }
            }
         }
         return contracts.ToArray();
      }
      public static string[] GetAddresses(string mexAddress,Type contractType) 
      {
         if(contractType.IsInterface == false)
         {
            Debug.Assert(false,contractType + " is not an interface");
            return new string[] { };
         }

         object[] attributes = contractType.GetCustomAttributes(typeof(ServiceContractAttribute),false);
         if(attributes.Length == 0)
         {
            Debug.Assert(false,"Interface " + contractType + " does not have the ServiceContractAttribute");
            return new string[] { };
         }
         ServiceContractAttribute attribute = attributes[0] as ServiceContractAttribute;
         if(attribute.Name == null)
         {
            attribute.Name = contractType.ToString();
         }
         if(attribute.Namespace == null)
         {
            attribute.Namespace = "http://tempuri.org/";
         }
         return GetAddresses(mexAddress,attribute.Namespace,attribute.Name);
      }
      public static string[] GetAddresses(string mexAddress,string contractNamespace,string contractName)
      {
         ServiceEndpointCollection endpoints = GetEndpoints(mexAddress);

         List<string> addresses = new List<string>();

         foreach(ServiceEndpoint endpoint in endpoints)
         {
            if(endpoint.Contract.Namespace == contractNamespace && endpoint.Contract.Name == contractName)
            {
               Debug.Assert(addresses.Contains(endpoint.Address.Uri.AbsoluteUri) == false);
               addresses.Add(endpoint.Address.Uri.AbsoluteUri);
            }
         }
         return addresses.ToArray();
      }
      public static string[] GetAddresses<B>(string mexAddress,Type contractType) where B : Binding
      {
         if(contractType.IsInterface == false)
         {
            Debug.Assert(false,contractType + " is not an interface");
            return new string[]{};
         }

         object[] attributes = contractType.GetCustomAttributes(typeof(ServiceContractAttribute),false);
         if(attributes.Length == 0)
         {
            Debug.Assert(false,"Interface " + contractType + " does not have the ServiceContractAttribute");
            return new string[]{};
         }
         ServiceContractAttribute attribute = attributes[0] as ServiceContractAttribute;
         if(attribute.Name == null)
         {
            attribute.Name = contractType.ToString();
         }
         if(attribute.Namespace == null)
         {
            attribute.Namespace = "http://tempuri.org/";
         }
         return GetAddresses<B>(mexAddress,attribute.Namespace,attribute.Name);
      }
      public static string[] GetAddresses<B>(string mexAddress,string contractNamespace,string contractName) where B : Binding
      {
         ServiceEndpointCollection endpoints = GetEndpoints(mexAddress);

         List<string> addresses = new List<string>();

         foreach(ServiceEndpoint endpoint in endpoints)
         {
            if(typeof(B).IsInstanceOfType(endpoint.Binding))
            {
               if(endpoint.Contract.Namespace == contractNamespace && endpoint.Contract.Name == contractName) 
               {
                  Debug.Assert(addresses.Contains(endpoint.Address.Uri.AbsoluteUri) == false);
                  addresses.Add(endpoint.Address.Uri.AbsoluteUri);
               }
            }
         }
         return addresses.ToArray();
      }
      public static string[] GetOperations(string mexAddress,string contractNamespace,string contractName)
      {
         ServiceEndpointCollection endpoints = GetEndpoints(mexAddress);

         List<string> operations = new List<string>();

         foreach(ServiceEndpoint endpoint in endpoints)
         {
            if(endpoint.Contract.Namespace == contractNamespace && endpoint.Contract.Name == contractName)
            {
               foreach(OperationDescription operation in endpoint.Contract.Operations)
               {
                  Debug.Assert(operations.Contains(operation.Name) == false);
                  operations.Add(operation.Name);
               }
               break;
            }
         }
         return operations.ToArray();
      }
   
      public static Binding GetBinding(string address)
      {
         if(String.IsNullOrEmpty(address))
         {
            Debug.Assert(false,"Empty address");
            return null;
         }
         string baseAddress = GetBaseAddress(address) + "?wsdl";

         ServiceEndpointCollection endpoints = GetEndpoints(address);

         foreach(ServiceEndpoint endpoint in endpoints)
         {
            if(endpoint.Address.Uri.AbsoluteUri == address)
            {
               return endpoint.Binding;
            }
         }
         return null;
      }

      static string GetBaseAddress(string address)
      {
         string[] segments = address.Split('/');
         return  segments[0] + segments[1] + segments[2] + "/";
      }
   }
}
