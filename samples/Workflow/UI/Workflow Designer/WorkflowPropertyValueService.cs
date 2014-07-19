using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace WorkflowDesigner
{
    public class WorkflowPropertyValueService: IPropertyValueUIService
    {
        private PropertyValueUIHandler _UIHandler;
        public event EventHandler PropertyUIValueItemsChanged;

        public WorkflowPropertyValueService(IDesignerHost host)
        {

        }

        #region IPropertyValueUIService Members

        public void AddPropertyValueUIHandler(PropertyValueUIHandler newHandler)
        {
            if (newHandler != null)
            {
                _UIHandler += newHandler;
            }
        }

        public PropertyValueUIItem[] GetPropertyUIValueItems(System.ComponentModel.ITypeDescriptorContext context, System.ComponentModel.PropertyDescriptor propDesc)
        {
            PropertyValueUIItem[] result = new PropertyValueUIItem[0];
            if (propDesc == null || _UIHandler == null)
            {
                return result;
            }

            ArrayList propertyItems = new ArrayList();
            _UIHandler(context, propDesc, propertyItems);
            if (propertyItems.Count > 0)
            {
                result = new PropertyValueUIItem[propertyItems.Count];
                propertyItems.CopyTo(result);
            }
            return result;
        }

        public void NotifyPropertyValueUIItemsChanged()
        {
            if (PropertyUIValueItemsChanged != null)
            {
                PropertyUIValueItemsChanged(this, new EventArgs());
            }
        }

        public void RemovePropertyValueUIHandler(PropertyValueUIHandler newHandler)
        {
            if (newHandler != null)
            {
                _UIHandler -= newHandler;
            }
        }

        #endregion
    }
}
