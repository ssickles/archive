
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Xml;
using System.IO;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Samples.DragDrop
{
	public static class DragDropManager
	{
		class DragInfo
		{
			public DragInfo(IDragSourceAdvisor adv)
			{
				if (adv == null)
					throw new ArgumentNullException();

				SourceAdvisor = adv;
			}
			public IDragSourceAdvisor SourceAdvisor { get; private set; }
			public UIElement DraggedElement { get; set; }
			public bool IsMouseDown { get; set; }
			public Point StartPoint { get; set; }
		}
		static DragInfo DragSource { get; set; } // when dragging from this application
		static DropPreviewAdorner Overlay { get; set; }
		static UIElement UIFeedback { get; set; }

		#region Dependency Properties

		public static readonly DependencyProperty DragSourceAdvisorProperty = DependencyProperty.RegisterAttached(
			"DragSourceAdvisor", 
			typeof(IDragSourceAdvisor), 
			typeof(DragDropManager),
			new FrameworkPropertyMetadata(OnDragSourceAdvisorChanged));

		public static void SetDragSourceAdvisor(UIElement depo, IDragSourceAdvisor advisor)
		{
			depo.SetValue(DragSourceAdvisorProperty, advisor);
		}

		public static IDragSourceAdvisor GetDragSourceAdvisor(UIElement depo)
		{
			return (IDragSourceAdvisor)depo.GetValue(DragSourceAdvisorProperty);
		}

		private static void OnDragSourceAdvisorChanged(DependencyObject depo, DependencyPropertyChangedEventArgs args)
		{
			if (args.NewValue == args.OldValue)
				return;
			UIElement ui = depo as UIElement;
			if (ui == null)
				return;

			if (args.NewValue != null)
			{
				ui.PreviewMouseLeftButtonDown += DragSource_PreviewMouseLeftButtonDown;
				ui.PreviewMouseMove           += DragSource_PreviewMouseMove;
				ui.PreviewMouseLeftButtonUp   += DragSource_PreviewMouseLeftButtonUp;
				ui.PreviewGiveFeedback        += DragSource_PreviewGiveFeedback;
			}
			else
			{
				ui.PreviewMouseLeftButtonDown -= DragSource_PreviewMouseLeftButtonDown;
				ui.PreviewMouseMove           -= DragSource_PreviewMouseMove;
				ui.PreviewMouseLeftButtonUp   -= DragSource_PreviewMouseLeftButtonUp;
				ui.PreviewGiveFeedback        -= DragSource_PreviewGiveFeedback;
			}
		}

		public static readonly DependencyProperty DropTargetAdvisorProperty = DependencyProperty.RegisterAttached(
			"DropTargetAdvisor", 
			typeof(IDropTargetAdvisor), 
			typeof(DragDropManager),
			new FrameworkPropertyMetadata(OnDropTargetAdvisorChanged));

		public static void SetDropTargetAdvisor(FrameworkElement depo, IDropTargetAdvisor advisor)
		{
			depo.SetValue(DropTargetAdvisorProperty, advisor);
		}

		public static IDropTargetAdvisor GetDropTargetAdvisor(FrameworkElement depo)
		{
			return (IDropTargetAdvisor)depo.GetValue(DropTargetAdvisorProperty);
		}

		private static void OnDropTargetAdvisorChanged(DependencyObject depo, DependencyPropertyChangedEventArgs args)
		{
			if (args.NewValue == args.OldValue)
				return;
			UIElement ui = depo as UIElement;
			if (ui == null)
				return;

			if (args.NewValue != null)
			{
				ui.PreviewDragEnter += DropTarget_PreviewDragEnter;
				ui.PreviewDragOver  += DropTarget_PreviewDragOver;
				ui.PreviewDragLeave += DropTarget_PreviewDragLeave;
				ui.PreviewDrop      += DropTarget_PreviewDrop;
				ui.AllowDrop = true;
			}
			else
			{
				ui.PreviewDragEnter -= DropTarget_PreviewDragEnter;
				ui.PreviewDragOver  -= DropTarget_PreviewDragOver;
				ui.PreviewDragLeave -= DropTarget_PreviewDragLeave;
				ui.PreviewDrop      -= DropTarget_PreviewDrop;
				ui.AllowDrop = false;
			}
		}

		#endregion

		#region Drop handling

		static void DropTarget_PreviewDrop(object sender, DragEventArgs e)
		{
			if (!UpdateEffects(sender, e))
				return;

			try
			{
				GetDropTargetAdvisor(sender as FrameworkElement).OnDropCompleted(e.Data, GetOverlaypoint(sender, e));
			}
			finally
			{
				// remove everything
				RemovePreviewAdorner();
				e.Handled = true;
			}
		}

		static void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
		{
			if (!UpdateEffects(sender, e))
				return;

			RemovePreviewAdorner();
			e.Handled = true;
		}

		static void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
		{
			if (!UpdateEffects(sender, e))
				return;

			Overlay.Location = GetOverlaypoint(sender, e);
			e.Handled = true;
		}

		static void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
		{
			if (!UpdateEffects(sender, e))
				return;

			CreatePreviewAdorner(sender, e);
			e.Handled = true;
		}

		static bool UpdateEffects(object sender, DragEventArgs e)
		{
			IDropTargetAdvisor advisor = GetDropTargetAdvisor(sender as FrameworkElement);
			if (!advisor.IsValidDataObject(e.Data))
				return false;

			// TODO: cope with Link, Scroll
			if ((e.AllowedEffects & DragDropEffects.Move) == 0 &&
				(e.AllowedEffects & DragDropEffects.Copy) == 0)
			{
				e.Effects = DragDropEffects.None;
				return false;
			}

			if ((e.AllowedEffects & DragDropEffects.Move) != 0)
			{
				e.Effects = DragDropEffects.Move;
			}

			if ((e.AllowedEffects & DragDropEffects.Copy) != 0)
			{
				if ((e.AllowedEffects & DragDropEffects.Move) == 0 || (e.KeyStates & DragDropKeyStates.ControlKey) != 0)
					e.Effects = DragDropEffects.Copy;
			}

			return true;
		}

		static Point GetOverlaypoint(object sender, DragEventArgs e)
		{
			Point p = e.GetPosition(sender as UIElement);
			p = GetDropTargetAdvisor(sender as FrameworkElement).GetDropLocation(p);
			p = (sender as UIElement).TranslatePoint(p, (UIElement)Overlay.Parent);
			return p;
		}

		static void CreatePreviewAdorner(object sender, DragEventArgs e)
		{
			// Set the Drag source UI
			IDropTargetAdvisor adv = GetDropTargetAdvisor(sender as FrameworkElement);
			adv.TargetUI = sender as UIElement;

			// get the data
			// get the UIFeedback
			UIFeedback = adv.GetVisualFeedback(e.Data);
			DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
			anim.From = 0.25;
			anim.AutoReverse = true;
			anim.RepeatBehavior = RepeatBehavior.Forever;
			UIFeedback.BeginAnimation(UIElement.OpacityProperty, anim);
			UIFeedback.IsHitTestVisible = false;

			// add it to the adornment layer
			AdornerLayer layer = AdornerLayer.GetAdornerLayer(sender as UIElement);
			Overlay = new DropPreviewAdorner(UIFeedback, sender as UIElement, layer);
			layer.Add(Overlay);
			Overlay.Location = GetOverlaypoint(sender, e);
		}

		static void RemovePreviewAdorner()
		{
			if (Overlay == null)
				return;
			Overlay.Layer.Remove(Overlay);
			Overlay = null;
		}

		#endregion

		#region Drag handling

		static void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			IDragSourceAdvisor adv = GetDragSourceAdvisor(sender as UIElement);
			if (!adv.IsDraggable(e.Source as UIElement)) 
				return;

			DragSource = new DragInfo(adv);
			DragSource.DraggedElement = e.Source as UIElement;
			DragSource.StartPoint = e.GetPosition(DragSource.DraggedElement);
			DragSource.IsMouseDown = true;
		}

		static void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DragSource = null;
		}

		static void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (DragSource != null 
				&& DragSource.IsMouseDown 
				&& IsDragGesture(e.GetPosition(DragSource.DraggedElement)))
				DragStarted(sender);
		}

		static void DragSource_PreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			// Can be used to set custom cursors
		}

		static void DragStarted(object uiObject)
		{
			// prepare
			DragSource.IsMouseDown = false;
			Mouse.Capture(DragSource.DraggedElement);

			// gather data
			IDragSourceAdvisor adv = GetDragSourceAdvisor(uiObject as UIElement);
			DataObject data = DragSource.SourceAdvisor.GetDataObject(DragSource.DraggedElement, DragSource.StartPoint);
			DragDropEffects supportedEffects = adv.SupportedEffects;
			adv.SourceUI = uiObject as UIElement;

			// Perform DragDrop
			DragDropEffects effects = System.Windows.DragDrop.DoDragDrop(DragSource.DraggedElement, data, supportedEffects);
			adv.FinishDrag(DragSource.DraggedElement, effects);

			// Clean up
			Mouse.Capture(null);
			DragSource = null;
			adv.SourceUI = null;
		}

		static bool IsDragGesture(Point point)
		{
			bool hGesture = Math.Abs(point.X - DragSource.StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance;
			bool vGesture = Math.Abs(point.Y - DragSource.StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance;

			return (hGesture | vGesture);
		}

		#endregion
	}
}
