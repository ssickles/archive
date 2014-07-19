using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using Samples.DragDrop;

namespace Samples.DragDrop
{
	public class CanvasDragDropAdvisor : IDragSourceAdvisor, IDropTargetAdvisor
	{
		private UIElement _sourceAndTargetElt;
		private UIElement _draggedElt;
		private string _serializedElt;

		#region IDragSourceAdvisor Members

		public UIElement SourceUI
		{
			get { return _sourceAndTargetElt; }
			set { _sourceAndTargetElt = value; }
		}

		public DragDropEffects SupportedEffects
		{
			get { return DragDropEffects.Copy | DragDropEffects.Move; }
		}

		public DataObject GetDataObject(UIElement draggedElt, Point p)
		{
			_draggedElt = draggedElt;

			_serializedElt = XamlWriter.Save(_draggedElt);
			DataObject obj = new DataObject("CanvasExample", _serializedElt);

			return obj;
		}

		public UIElement GetVisualFeedback(UIElement draggedElt)
		{
			Rectangle rect = new Rectangle();
			rect.Width = draggedElt.RenderSize.Width;
			rect.Height = draggedElt.RenderSize.Height;
			rect.Fill = new VisualBrush(draggedElt);

			return rect;
		}

		public void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects)
		{
			if ((finalEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
			{
				XmlReader reader = XmlReader.Create(new StringReader(_serializedElt));
				UIElement elt = XamlReader.Load(reader) as UIElement;

				(_sourceAndTargetElt as Canvas).Children.Add(elt);
			}
		}

		public bool IsDraggable(UIElement dragElt)
		{
			return (!(dragElt is Canvas));
		}

		#endregion

		#region IDropTargetAdvisor Members

		public UIElement TargetUI
		{
			get { return _sourceAndTargetElt; }
			set { _sourceAndTargetElt = value; }
		}

		public object GetDropData(IDataObject obj)
		{
			return obj.GetData("CanvasExample");
		}

		public bool IsValidDataObject(IDataObject obj)
		{
			return (obj.GetDataPresent("CanvasExample"));
		}

		public void OnDropCompleted(IDataObject obj, Point dropPoint)
		{
			Canvas canvas = _sourceAndTargetElt as Canvas;
			Canvas.SetLeft(_draggedElt, dropPoint.X);
			Canvas.SetTop(_draggedElt, dropPoint.Y);
		}

		public Point GetDropLocation(Point suggestedPoint)
		{
			return suggestedPoint;
		}

		public UIElement GetVisualFeedback(IDataObject obj)
		{
			XmlReader reader = XmlReader.Create(new StringReader(_serializedElt));
			UIElement elt = XamlReader.Load(reader) as UIElement;

			Rectangle rect = new Rectangle();
			rect.Width = elt.RenderSize.Width;
			rect.Height = elt.RenderSize.Height;
			rect.Fill = new VisualBrush(elt);

			return rect;
		}

		#endregion
	}
}