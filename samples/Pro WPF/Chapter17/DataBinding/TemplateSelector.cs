using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;
using StoreDatabase;

namespace DataBinding
{
    public class ProductByCategoryTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Product product = (Product)item;
            Window window = Application.Current.MainWindow;
            
            if (product.CategoryName == "Travel")
            {
                return (DataTemplate)window.FindResource("TravelProductTemplate");
            }            
            else
            {
                return (DataTemplate)window.FindResource("DefaultProductTemplate");
            }
        }
    }

    public class SingleCriteriaHighlightTemplateSelector : DataTemplateSelector
    {
        private DataTemplate defaultTemplate;
        public DataTemplate DefaultTemplate
        {
            get { return defaultTemplate; }
            set { defaultTemplate = value; }
        }

        private DataTemplate highlightTemplate;
        public DataTemplate HighlightTemplate
        {
            get { return highlightTemplate; }
            set { highlightTemplate = value; }
        }

        private string propertyToEvaluate;
        public string PropertyToEvaluate
        {
            get { return propertyToEvaluate; }
            set { propertyToEvaluate = value; }
        }

        private string propertyValueToHighlight;
        public string PropertyValueToHighlight
        {
            get { return propertyValueToHighlight; }
            set { propertyValueToHighlight = value; }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Product product = (Product)item;

            Type type = product.GetType();
            PropertyInfo property = type.GetProperty(PropertyToEvaluate);
            if (property.GetValue(product, null).ToString() == PropertyValueToHighlight)
            {
                return HighlightTemplate;
            }
            else
            {
                return DefaultTemplate;
            }
        }

    }
}
