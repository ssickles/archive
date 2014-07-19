using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Samples.DragDrop;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;

namespace Samples.DragDrop
{
    public class DefaultDragSourceAdvisor : IDragSourceAdvisor
    {
		private static DataFormat SupportedFormat = DataFormats.GetDataFormat("Samples");
		private UIElement _sourceUI;

		public DragDropEffects SupportedEffects
		{
			get { return DragDropEffects.Copy | DragDropEffects.Move; }
		}

		public UIElement SourceUI
		{
			get
			{
				return _sourceUI;
			}
			set
			{
				_sourceUI = value;
			}
		}

		public DataObject GetDataObject(UIElement draggedElt, Point p)
		{
			string serializedObject = XamlWriter.Save(draggedElt);
			DataObject data = new DataObject(SupportedFormat.Name, serializedObject);

			return data;
		}

    	public UIElement GetVisualFeedback(UIElement draggedElt)
    	{
			Rectangle rect = new Rectangle();
			rect.Width = draggedElt.RenderSize.Width;
			rect.Height = draggedElt.RenderSize.Height;
			rect.Fill = new VisualBrush(draggedElt);
			rect.Opacity = 0.5;
			return rect;
		}

    	public void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects)
		{
			if ((finalEffects & DragDropEffects.Move) == DragDropEffects.Move)
			{
				Panel panel = SourceUI as Panel;
				if (panel != null)
				{
					panel.Children.Remove(draggedElt);
				}
			}
		}


		public bool IsDraggable(UIElement dragElt)
		{
			return (dragElt is Button);
		}
	}
}
