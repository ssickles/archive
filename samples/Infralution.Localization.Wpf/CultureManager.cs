//
//      FILE:   CultureManager.cs.
//
// COPYRIGHT:   Copyright 2008 
//              Infralution
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Forms;
using Infralution.Localization.Wpf.Properties;
namespace Infralution.Localization.Wpf
{

    /// <summary>
    /// Provides the ability to change the UICulture for WPF Windows and controls
    /// dynamically.  
    /// </summary>
    /// <remarks>
    /// XAML elements that use the <see cref="ResxExtension"/> are automatically
    /// updated when the <see cref="CultureManager.UICulture"/> property is changed.
    /// </remarks>
    public static class CultureManager
    {
        #region Static Member Variables

        /// <summary>
        /// Current UICulture of the application
        /// </summary>
        private static CultureInfo _uiCulture;

        /// <summary>
        /// The active design time culture selection window (if any)
        /// </summary>
        private static CultureSelectWindow _cultureSelectWindow;

        /// <summary>
        /// The active task bar notify icon for design time culture selection (if any)
        /// </summary>
        private static NotifyIcon _cultureNotifyIcon;

        /// <summary>
        /// Should the <see cref="Thread.CurrentCulture"/> be changed when the
        /// <see cref="UICulture"/> changes.
        /// </summary>
        private static bool _synchronizeThreadCulture = true;

        #endregion

        #region Public Interface

        /// <summary>
        /// Raised when the <see cref="UICulture"/> is changed
        /// </summary>
        /// <remarks>
        /// Since this event is static if the client object does not detach from the event a reference
        /// will be maintained to the client object preventing it from being garbage collected - thus
        /// causing a potential memory leak. 
        /// </remarks>
        public static event EventHandler UICultureChanged;

        /// <summary>
        /// Sets the UICulture for the WPF application and raises the <see cref="UICultureChanged"/>
        /// event causing any XAML elements using the <see cref="ResxExtension"/> to automatically
        /// update
        /// </summary>
        public static CultureInfo UICulture
        {
            get
            {
                if (_uiCulture == null)
                {
                    _uiCulture = Thread.CurrentThread.CurrentUICulture;
                }
                return _uiCulture;
            }
            set
            {
                if (value != UICulture)
                {
                    _uiCulture = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                    if (SynchronizeThreadCulture)
                    {
                        SetThreadCulture(value);
                    }
                    UICultureExtension.UpdateAllTargets();
                    ResxExtension.UpdateAllTargets();
                    if (UICultureChanged != null)
                    {
                        UICultureChanged(null, EventArgs.Empty);
                    }
                    UpdateNotifyIconMenu();
                }
            }
        }

        /// <summary>
        /// If set to true then the <see cref="Thread.CurrentCulture"/> property is changed
        /// to match the current <see cref="UICulture"/>
        /// </summary>
        public static bool SynchronizeThreadCulture
        {
            get { return _synchronizeThreadCulture; }
            set
            {
                _synchronizeThreadCulture = value;
                if (value)
                {
                    SetThreadCulture(UICulture);
                }
            }
        }

        #endregion

        #region Local Methods

        /// <summary>
        /// Set the thread culture to the given culture
        /// </summary>
        /// <param name="value">The culture to set</param>
        /// <remarks>If the culture is neutral then creates a specific culture</remarks>
        private static void SetThreadCulture(CultureInfo value)
        {
            if (value.IsNeutralCulture)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(value.Name);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = value;
            }
        }

        /// <summary>
        /// Show the UICultureSelector to allow selection of the active UI culture
        /// </summary>
        internal static void ShowCultureNotifyIcon()
        {
            if (_cultureNotifyIcon == null)
            {
                ToolStripMenuItem menuItem;

                _cultureNotifyIcon = new NotifyIcon();
                _cultureNotifyIcon.Icon = Resources.UICultureIcon;
                _cultureNotifyIcon.MouseClick += new MouseEventHandler(OnCultureNotifyIconMouseClick);
                _cultureNotifyIcon.MouseDoubleClick += new MouseEventHandler(OnCultureNotifyIconMouseDoubleClick);
                _cultureNotifyIcon.Text = Resources.UICultureSelectText;
                ContextMenuStrip menuStrip = new ContextMenuStrip();

                // separator
                //
                menuStrip.Items.Add(new ToolStripSeparator());

                // add menu to open culture select window
                //
                menuItem = new ToolStripMenuItem(Resources.OtherCulturesMenu);
                menuItem.Click += new EventHandler(OnCultureSelectMenuClick);
                menuStrip.Items.Add(menuItem);

                // add the current UICulture to the menu
                //
                AddUICultureMenuItem(menuStrip);

                _cultureNotifyIcon.ContextMenuStrip = menuStrip;
                _cultureNotifyIcon.Visible = true;

            }
        }

        /// <summary>
        /// Display the CultureSelectWindow to allow the user to select the UICulture
        /// </summary>
        private static void DisplayCultureSelectWindow()
        {
            if (_cultureSelectWindow == null)
            {
                _cultureSelectWindow = new CultureSelectWindow();
                _cultureSelectWindow.Title = _cultureNotifyIcon.Text;
                _cultureSelectWindow.Closed += new EventHandler(OnCultureSelectWindowClosed);
                _cultureSelectWindow.Show();
            }
        }

        /// <summary>
        /// Add a menu item to the NotifyIcon for the current UICulture
        /// </summary>
        /// <param name="menuStrip"></param>
        private static void AddUICultureMenuItem(ContextMenuStrip menuStrip)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem(UICulture.DisplayName);
            menuItem.Checked = true;
            menuItem.CheckOnClick = true;
            menuItem.Tag = UICulture;
            menuItem.CheckedChanged += new EventHandler(OnCultureMenuCheckChanged);
            menuStrip.Items.Insert(menuStrip.Items.Count - 2, menuItem);
        }

        /// <summary>
        /// Update the notify icon menu when the UICulture is changed
        /// </summary>
        private static void UpdateNotifyIconMenu()
        {
            if (_cultureNotifyIcon != null)
            {
                ContextMenuStrip menuStrip = _cultureNotifyIcon.ContextMenuStrip;
                bool cultureItemExists = false;
                foreach (ToolStripItem item in menuStrip.Items)
                {
                    ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                    if (menuItem != null)
                    {
                        menuItem.Checked = (menuItem.Tag == UICulture);
                        if (menuItem.Checked)
                        {
                            cultureItemExists = true;
                        }
                    }
                }
                if (!cultureItemExists)
                {
                    AddUICultureMenuItem(menuStrip);
                }
            }
        }

        /// <summary>
        /// Display the context menu for left clicks (right clicks are handled automatically)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCultureNotifyIconMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _cultureNotifyIcon.ContextMenuStrip.Show(Control.MousePosition);
            }
        }

        /// <summary>
        /// Display the CultureSelectWindow when the user double clicks on the NotifyIcon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCultureNotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
        {
            DisplayCultureSelectWindow();
        }

        /// <summary>
        /// Display the CultureSelectWindow when the user selects the menu option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCultureSelectMenuClick(object sender, EventArgs e)
        {
            DisplayCultureSelectWindow();
        }

        /// <summary>
        /// Handle change of culture via the NotifyIcon menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCultureMenuCheckChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem.Checked)
            {
                UICulture = menuItem.Tag as CultureInfo;
            }
        }

        /// <summary>
        /// Handle close of the culture select window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCultureSelectWindowClosed(object sender, EventArgs e)
        {
            _cultureSelectWindow = null;
        }

        #endregion

    }

}
