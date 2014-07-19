using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace WorkflowDesigner
{
    public class WorkflowEventBindingService: IEventBindingService
    {
        private IServiceProvider _serviceProvider;

        public WorkflowEventBindingService(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        #region IEventBindingService Members

        public string CreateUniqueMethodName(IComponent component, EventDescriptor e)
        {
            return String.Empty;
        }

        public ICollection GetCompatibleMethods(EventDescriptor e)
        {
            List<String> compatibleMethods = new List<string>();

            IDesignerHost designerHost = _serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designerHost == null || designerHost.RootComponent == null)
            {
                return compatibleMethods;
            }

            EventInfo eventInfo = e.ComponentType.GetEvent(e.Name);
            ParameterInfo[] eventParameters = null;
            if (eventInfo != null)
            {
                MethodInfo invokeMethod = eventInfo.EventHandlerType.GetMethod("Invoke");
                if (invokeMethod != null)
                {
                    eventParameters = invokeMethod.GetParameters();
                }
            }

            if (eventParameters != null)
            {
                Type rootType = designerHost.RootComponent.GetType();
                MethodInfo[] methods = rootType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                foreach (MethodInfo method in methods)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == eventParameters.Length)
                    {
                        if (IsCandidateMethod(eventParameters, parameters))
                        {
                            compatibleMethods.Add(method.Name);
                        }
                    }
                }

                compatibleMethods.Add("[Clear]");
            }

            return compatibleMethods;
        }

        private bool IsCandidateMethod(ParameterInfo[] eventParameters, ParameterInfo[] parameters)
        {
            Boolean isCandidate = true;
            for (Int32 i = 0; i < eventParameters.Length; i++)
            {
                if (!eventParameters[i].ParameterType.IsAssignableFrom(parameters[i].ParameterType))
                {
                    isCandidate = false;
                    break;
                }
            }
            return isCandidate;
        }

        public EventDescriptor GetEvent(PropertyDescriptor property)
        {
            if (property is EventPropertyDescriptor)
            {
                return ((EventPropertyDescriptor)property).EventDescriptor;
            }
            else
            {
                return null;
            }
        }

        public PropertyDescriptorCollection GetEventProperties(EventDescriptorCollection events)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
            foreach (EventDescriptor eventDesc in events)
            {
                properties.Add(new EventPropertyDescriptor(eventDesc, _serviceProvider));
            }

            PropertyDescriptorCollection propertiesCollection = new PropertyDescriptorCollection(properties.ToArray(), true);
            return propertiesCollection;
        }

        public PropertyDescriptor GetEventProperty(EventDescriptor e)
        {
            return new EventPropertyDescriptor(e, _serviceProvider);
        }

        public bool ShowCode(IComponent component, EventDescriptor e)
        {
            return false;
        }

        public bool ShowCode(int lineNumber)
        {
            return false;
        }

        public bool ShowCode()
        {
            return false;
        }

        #endregion
    }
}
