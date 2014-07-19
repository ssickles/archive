using System;
using System.Collections.Generic;
using System.Text;
using Samples.DragDrop;
using System.Windows;
using System.Xml;
using System.Windows.Markup;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Samples.DragDrop
{
	public class DefaultDropTargetAdvisor : IDropTargetAdvisor
	{
		private static DataFormat SupportedFormat = DataFormats.GetDataFormat("Samples");
		private UIElement _targetUI;

		public bool IsValidDataObject(IDataObject obj)
		{
			return obj.GetDataPresent(SupportedFormat.Name);
		}

		public object GetDropData(IDataObject obj)
		{
			string serializedObject = obj.GetData(SupportedFormat.Name) as string;
			if (string.IsNullOrEmpty(serializedObject))
				return null;

			XmlReader reader = XmlReader.Create(new StringReader(serializedObject));
			return XamlReader.Load(reader) as UIElement;
		}

		public void OnDropCompleted(IDataObject obj, Point dropPoint)
		{
			string serializedObject = obj.GetData(SupportedFormat.Name) as string;
			XmlReader reader = XmlReader.Create(new StringReader(serializedObject));
			UIElement elt = XamlReader.Load(reader) as UIElement;

			(TargetUI as Panel).Children.Add(elt);
		}

		public UIElement TargetUI
		{
			get
			{
				return _targetUI;
			}
			set
			{
				_targetUI = value;
			}
		}

		public Point GetDropLocation(Point suggestedPoint)
		{
			return suggestedPoint;
		}

		public UIElement GetVisualFeedback(IDataObject obj)
		{
			UIElement elt = (UIElement)GetDropData(obj);

			//Rectangle rect = new Rectangle();
			//rect.Width = elt.RenderSize.Width;
			//rect.Height = elt.RenderSize.Height;
			//rect.Fill = new VisualBrush(elt);

			return elt;
		}
	}
}
