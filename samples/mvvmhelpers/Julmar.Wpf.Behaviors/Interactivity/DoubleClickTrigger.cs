using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace JulMar.Windows.Interactivity
{
    /// <summary>
    /// This is a Blend/VS.NET behavior which drives a DoubleClick event to 
    /// some interactive action
    /// </summary>
    public class DoubleClickTrigger : TriggerBase<UIElement>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseDown += HandlePreviewMouseLeftButtonDown;
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
            this.AssociatedObject.PreviewMouseDown -= HandlePreviewMouseLeftButtonDown;
        }

        /// <summary>
        /// This handles the UIElement.LeftButtonDown event to test for a double-click event.
        /// </summary>
        /// <param name="sender">UIElement</param>
        /// <param name="mouseEventArgs">EventArgs</param>
        private void HandlePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.ClickCount == 2)
            {
                this.InvokeActions(mouseEventArgs);
            }
        }
    }
}
