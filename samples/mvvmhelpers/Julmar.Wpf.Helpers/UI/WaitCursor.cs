using System;
using System.Windows.Input;

namespace JulMar.Windows.UI
{
    /// <summary>
    /// This class implements a disposable WaitCursor to show an hourglass while
    /// some long-running event occurs.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    /// using (new WaitCursor())
    /// {
    ///    .. Do work here ..
    /// }
    /// 
    /// ]]>
    /// </example>
    public class WaitCursor : IDisposable
    {
        private readonly Cursor _oldCursor;

        /// <summary>
        /// Constructor
        /// </summary>
        public WaitCursor()
        {
            _oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        /// <summary>
        /// Returns the cursor to the default state.
        /// </summary>
        public void Dispose()
        {
            Mouse.OverrideCursor = _oldCursor;
        }
    }
}