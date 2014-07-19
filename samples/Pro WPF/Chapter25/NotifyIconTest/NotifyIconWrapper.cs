using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NotifyIconTest
{
    public partial class NotifyIconWrapper : Component
    {

        public NotifyIconWrapper()
        {
            InitializeComponent();

            // Attach event handlers.
            cmdClose.Click += cmdClose_Click;
            cmdShowWindow.Click += cmdShowWindow_Click;
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) components.Dispose();
        }

        // Use just one instance of this window.
        // The window is typed as an object because that avoids a designer error
        // that would otherwise occur due to a limitation of Visual Studio's WPF support.
        private object win = new Window1();
        private void cmdShowWindow_Click(object sender, EventArgs e)
        {
            Window1 win = (Window1)this.win;
            // Show the window (and bring it to the forefront if it's already visible).
            win.ShowInTaskbar = true;
            if (win.WindowState == System.Windows.WindowState.Minimized) win.WindowState = System.Windows.WindowState.Normal;
            win.Show();
            win.Activate();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyIconWrapper));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdShowWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdClose = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdShowWindow,
            this.cmdClose});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 48);
            // 
            // cmdShowWindow
            // 
            this.cmdShowWindow.Name = "cmdShowWindow";
            this.cmdShowWindow.Size = new System.Drawing.Size(150, 22);
            this.cmdShowWindow.Text = "Show Window";
            // 
            // cmdClose
            // 
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(150, 22);
            this.cmdClose.Text = "Close";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "NotifyIcon Test";
            this.notifyIcon1.Visible = true;
            this.contextMenuStrip1.ResumeLayout(false);

        }
    }
}
