using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Workflow.ComponentModel;

namespace WorkflowDesigner
{
    class EventPropertyDescriptor: PropertyDescriptor
    {
        private EventDescriptor _eventDescriptor;
        private IServiceProvider _serviceProvider;
        private DependencyProperty _eventProperty;

        public EventPropertyDescriptor(EventDescriptor eventDesc, IServiceProvider serviceProvider)
            : base(eventDesc)
        {
            _eventDescriptor = eventDesc;
            _serviceProvider = serviceProvider;

            FieldInfo eventFieldInfo = _eventDescriptor.ComponentType.GetField(_eventDescriptor.Name + "Event");
            if (eventFieldInfo != null)
            {
                _eventProperty = eventFieldInfo.GetValue(_eventDescriptor.ComponentType) as DependencyProperty;
            }
        }

        public EventDescriptor EventDescriptor
        {
            get { return _eventDescriptor; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return _eventDescriptor.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return null;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return _eventDescriptor.EventType; }
        }

        public override void ResetValue(object component)
        {
            SetValue(component, null);
        }

        public override void SetValue(object component, object value)
        {
            DependencyObject dependencyObject = component as DependencyObject;
            string eventHandlerName = String.Empty;

            if (dependencyObject == null || _eventProperty == null)
            {
                return;
            }

            string currentHandlerName = String.Empty;
            if (dependencyObject.IsBindingSet(_eventProperty))
            {
                currentHandlerName = dependencyObject.GetBinding(_eventProperty).Path;
            }

            if (eventHandlerName == currentHandlerName)
            {
                return;
            }

            IDesignerHost designerHost = _serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService changeService = _serviceProvider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (changeService != null)
            {
                changeService.OnComponentChanging(component, _eventDescriptor);
            }

            String bindingName = String.Empty;
            if (eventHandlerName == null || eventHandlerName == "[Clear]")
            {
                dependencyObject.RemoveProperty(_eventProperty);
            }
            else
            {
                ActivityBind bind = new ActivityBind(((Activity)designerHost.RootComponent).Name, eventHandlerName);
                dependencyObject.SetBinding(_eventProperty, bind);
            }

            if (changeService != null)
            {
                changeService.OnComponentChanged(component, _eventDescriptor, currentHandlerName, bindingName);
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override TypeConverter Converter
        {
            get
            {
                return new WorkflowEventTypeConverter(_eventDescriptor);
            }
        }
    }

    class WorkflowEventTypeConverter: TypeConverter
    {
        EventDescriptor _eventDescriptor;

        public WorkflowEventTypeConverter(EventDescriptor eventDesc)
        {
            _eventDescriptor = eventDesc;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ICollection compatibleMethods = new ArrayList();
            if (context != null)
            {
                IEventBindingService bindingService = (IEventBindingService)context.GetService(typeof(IEventBindingService));
                if (bindingService != null)
                {
                    compatibleMethods = bindingService.GetCompatibleMethods(_eventDescriptor);
                }
            }
            return new StandardValuesCollection(compatibleMethods);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
