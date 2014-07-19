using System;
using System.ComponentModel.Design;
using System.Drawing;

using System.Windows.Forms;
using OpenSpan.Runtime;
using SampleService;

namespace SampleHost
{
	public partial class HostForm : Form, ISampleHostService
	{
		private RuntimeHost mRuntimeHost;
		private ServiceContainer mServiceContainer;

		public HostForm()
		{
			InitializeComponent();

			mServiceContainer = new ServiceContainer();
			mServiceContainer.AddService(typeof(ISampleHostService), this);
		}

		private void btnLaunch_Click(object sender, EventArgs e)
		{
			// Create an instance of the OpenSpan Runtime, passing in the global ServiceContainer
			mRuntimeHost = new RuntimeHost(mServiceContainer);

			// Listen for notification that the project has started
			mRuntimeHost.Started += new ContextEventHandler(RuntimeHost_Started);

			// Listen for notification that the project has stopped
			mRuntimeHost.Stopped += new EventHandler(RuntimeHost_Stopped);

			// Load the SampleSolution from the application directory
			mRuntimeHost.ProjectPath = @"SampleSolution.OpenSpan";

			// Start the OpenSpan Runtime
			mRuntimeHost.Start();
		}

		private void RuntimeHost_Started(object sender, ContextEventArgs e)
		{
			// Make sure update gets put on the UI thread
			if (this.lblStatus.InvokeRequired)
			{
				this.lblStatus.Invoke(new ContextEventHandler(this.OnRuntimeStarted), new object[] { sender, e });
			}
			else
			{
				this.OnRuntimeStarted(sender, e);
			}
		}

		private void RuntimeHost_Stopped(object sender, EventArgs e)
		{
			// Make sure update gets put on the UI thread
			if (this.lblStatus.InvokeRequired)
			{
				this.lblStatus.Invoke(new ContextEventHandler(this.OnRuntimeStopped), new object[] { sender, e });
			}
			else
			{
				this.OnRuntimeStopped(sender, e);
			}
		}

		private void OnRuntimeStarted(object sender, ContextEventArgs e)
		{
			this.lblStatus.Text = "Started";
			this.lblStatus.ForeColor = Color.Green;
		}

		private void OnRuntimeStopped(object sender, EventArgs e)
		{
			this.lblStatus.Text = "Stopped";
			this.lblStatus.ForeColor = Color.Red;
		}

		public string GetSampleText()
		{
			return this.txtSample.Text;
		}

		private void StopRuntime()
		{
			if (this.mRuntimeHost != null)
			{
				RuntimeHost.UnloadRuntime(ref this.mRuntimeHost);
			}
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			StopRuntime();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			StopRuntime();
		}

		private void btnShowMsg_Click(object sender, EventArgs e)
		{
			// Call Entry Point Method
			DialogResult result = (DialogResult)mRuntimeHost.InvokeAutomationMethod(
				"Automator-8CAA0D937D7E37E", "_EntryPointExecute", new object[] { txtSample.Text });

			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				MessageBox.Show("User chose 'Yes'");
			}
			else
			{
				MessageBox.Show("User chose 'No'");
			}
		}

	}
}
