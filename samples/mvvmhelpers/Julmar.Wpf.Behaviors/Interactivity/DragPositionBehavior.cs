using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace JulMar.Windows.Interactivity
{
    /// <summary>
    /// This Blend behavior provides positional translation for UIElements through a 
    /// RenderTransform using Drag/Drop semantics.
    /// </summary>
        
    public class DragPositionBehavior : Behavior<UIElement>
    {
        #region IsEnabledProperty
        /// <summary>
        /// This property allows the behavior to be used as a traditional
        /// attached property behavior.
        /// </summary>
        public static DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(DragPositionBehavior),
                new FrameworkPropertyMetadata(false, OnIsEnabledChanged));

        /// <summary>
        /// Returns whether DragPositionBehavior is enabled via attached property
        /// </summary>
        /// <param name="uie">Element</param>
        /// <returns>True/False</returns>
        public static bool GetIsEnabled(DependencyObject uie)
        {
            return (bool)uie.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// Adds DragPositionBehavior to an element
        /// </summary>
        /// <param name="uie">Element to apply</param>
        /// <param name="value">True/False</param>
        public static void SetIsEnabled(DependencyObject uie, bool value)
        {
            uie.SetValue(IsEnabledProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = dpo as UIElement;
            if (uie != null)
            {
                var behColl = Interaction.GetBehaviors(uie);
                var existingBehavior = behColl.FirstOrDefault(b => b.GetType() == typeof(DragPositionBehavior)) as DragPositionBehavior;
                if ((bool)e.NewValue == false && existingBehavior != null)
                {
                    behColl.Remove(existingBehavior);
                }
                else if ((bool)e.NewValue == true && existingBehavior == null)
                {
                    behColl.Add(new DragPositionBehavior());
                }
            }
        }
        #endregion

        /// <summary>
        /// This class encapsulates the drag data + logic for a given element.
        /// It saves memory space as it is only allocated while the object is
        /// being dragged around.
        /// </summary>
        internal class UIElementMouseDrag
        {
            private Point _startPos;
            private TranslateTransform _translatePos;

            public void OnMouseDown(UIElement uie, MouseButtonEventArgs e)
            {
                _startPos = e.GetPosition(null);
                
                uie.CaptureMouse();
                uie.MouseUp += OnMouseUp;
                uie.MouseMove += OnMouseMove;
            }

            private void OnMouseUp(object sender, MouseButtonEventArgs e)
            {
                UIElement uie = (UIElement) sender;
                uie.MouseMove -= OnMouseMove;
                uie.MouseUp -= OnMouseUp;
                uie.ReleaseMouseCapture();
            }

            public void OnMouseMove(object sender, MouseEventArgs e)
            {
                UIElement uie = (UIElement) sender;

                Point currentPosition = e.GetPosition(null);

                if (_translatePos != null)
                {
                    _translatePos.X = currentPosition.X - _startPos.X;
                    _translatePos.Y = currentPosition.Y - _startPos.Y;
                }
                else
                {
                    _translatePos = new TranslateTransform { X = currentPosition.X - _startPos.X, Y = currentPosition.Y - _startPos.Y };

                    // Replace existing transform if it exists.
                    TransformGroup transformGroup = new TransformGroup();
                    if (uie.RenderTransform != null)
                        transformGroup.Children.Add(uie.RenderTransform);
                    transformGroup.Children.Add(_translatePos);
                    uie.RenderTransform = transformGroup;
                }
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseDown += OnMouseDown;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseDown -= OnMouseDown;
        }

        /// <summary>
        /// Handles the MouseDown event
        /// </summary>
        /// <param name="sender">UIElement</param>
        /// <param name="e">Mouse eventargs</param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && 
                e.LeftButton == MouseButtonState.Pressed)
            {
                UIElement uie = (UIElement)sender;
                var currentDragInfo = new UIElementMouseDrag();
                currentDragInfo.OnMouseDown(uie, e);
            }
        }
    }
}