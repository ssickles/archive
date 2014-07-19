using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace WickedSick.CommonComponents.Toolbox
{
    public enum SysTrayActions
    {
        OpenContextMenu,
        ShowWindow,
        HideWindow,
        CustomAction
    }

    public enum SysTraySource
    {
        LeftClick,
        RightClick,
        DoubleClick,
        Startup
    }

    public class SysTrayActionEventArgs: EventArgs
    {
        private SysTraySource _source;

        public SysTrayActionEventArgs(SysTraySource Source)
        {
            _source = Source;
        }

        public SysTraySource Source
        {
            get { return _source; }
        }
    }

    public class SysTray: IDisposable
    {
        public delegate void CustomActionHandler(object sender, SysTrayActionEventArgs SysTrayActionArgs);
        public event CustomActionHandler CustomAction;

        private NotifyIcon _notify;
        private Window _window;
        private System.Windows.Controls.ContextMenu _contextMenu;
        private SysTrayActions _leftClick;
        private SysTrayActions _rightClick;
        private SysTrayActions _doubleClick;
        private SysTrayActions _startup;

        public SysTray(Window Window, Icon Icon, System.Windows.Controls.ContextMenu Menu)
        {
            _window = Window;
            _contextMenu = Menu;
            _leftClick = SysTrayActions.ShowWindow;
            _rightClick = SysTrayActions.OpenContextMenu;
            _doubleClick = SysTrayActions.ShowWindow;
            _startup = SysTrayActions.ShowWindow;
            _notify = new NotifyIcon();
            _notify.Icon = Icon;
            _notify.Visible = true;
            _notify.MouseClick += new MouseEventHandler(_notify_MouseClick);
            _notify.DoubleClick += new EventHandler(_notify_DoubleClick);

            TakeAction(SysTraySource.Startup, _startup);
        }

        public SysTrayActions LeftClick
        {
            get { return _leftClick; }
            set { _leftClick = value; }
        }

        public SysTrayActions RightClick
        {
            get { return _rightClick; }
            set { _rightClick = value; }
        }

        public SysTrayActions DoubleClick
        {
            get { return _doubleClick; }
            set { _doubleClick = value; }
        }

        public SysTrayActions Startup
        {
            get { return _startup; }
            set { _startup = value; }
        }

        private void OnCustomAction(SysTraySource Source)
        {
            CustomActionHandler _eventObj = CustomAction;
            if (_eventObj != null)
            {
                _eventObj(this, new SysTrayActionEventArgs(Source));
            }
        }

        private void TakeAction(SysTraySource Source, SysTrayActions Action)
        {
            switch (Action)
            {
                case SysTrayActions.HideWindow:
                    HideWindow();
                    break;
                case SysTrayActions.OpenContextMenu:
                    _contextMenu.IsOpen = true;
                    break;
                case SysTrayActions.ShowWindow:
                    ShowWindow();
                    break;
                case SysTrayActions.CustomAction:
                    OnCustomAction(Source);
                    break;
            }
        }

        private void _notify_DoubleClick(object sender, EventArgs e)
        {
            TakeAction(SysTraySource.DoubleClick, _doubleClick);
        }

        private void _notify_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TakeAction(SysTraySource.RightClick, _rightClick);
            }
            if (e.Button == MouseButtons.Left)
            {
                TakeAction(SysTraySource.LeftClick, _leftClick);
            }
        }

        public void ShowWindow()
        {
            _window.Show();
            _window.Activate();
            _window.ShowInTaskbar = true;
            _window.WindowState = WindowState.Normal;
        }

        public void HideWindow()
        {
            _window.Hide();
            _window.ShowInTaskbar = false;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _notify.Visible = false;
            _notify.Dispose();
        }

        #endregion
    }
}
