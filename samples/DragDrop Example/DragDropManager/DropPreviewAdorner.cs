using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Samples.DragDrop
{
    public class DropPreviewAdorner : Adorner
    {
		ContentPresenter presenter;
		Point location;

		public DropPreviewAdorner(object feedbackUI, UIElement adornedElt, AdornerLayer layer)
			: base(adornedElt)
        {
			Layer = layer;

			presenter = new ContentPresenter();
			presenter.Content = feedbackUI;
			presenter.IsHitTestVisible = false;
        }

        private void UpdatePosition()
        {
			if (Layer != null)
				Layer.Update(AdornedElement);
        }

		public Point Location
		{
			get { return location; }
			set
			{
				location = value;
				UpdatePosition();
			}
		}

		public AdornerLayer Layer { get; private set; }

        protected override Size MeasureOverride(Size constraint)
        {
			presenter.Measure(constraint);
			return presenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
			presenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) { return presenter; }

        protected override int VisualChildrenCount { get { return 1; } }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
			return new TranslateTransform(location.X, location.Y);
        }
    }
}
