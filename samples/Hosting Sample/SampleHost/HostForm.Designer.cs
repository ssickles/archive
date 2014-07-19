namespace SampleHost
{
	partial class HostForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnLaunch = new System.Windows.Forms.Button();
			this.txtSample = new System.Windows.Forms.TextBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnShowMsg = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnLaunch
			// 
			this.btnLaunch.Location = new System.Drawing.Point(13, 13);
			this.btnLaunch.Name = "btnLaunch";
			this.btnLaunch.Size = new System.Drawing.Size(88, 23);
			this.btnLaunch.TabIndex = 0;
			this.btnLaunch.Text = "Launch Project";
			this.btnLaunch.UseVisualStyleBackColor = true;
			this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
			// 
			// txtSample
			// 
			this.txtSample.Location = new System.Drawing.Point(12, 108);
			this.txtSample.Name = "txtSample";
			this.txtSample.Size = new System.Drawing.Size(100, 20);
			this.txtSample.TabIndex = 1;
			this.txtSample.Text = "Hello World!";
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(13, 42);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(88, 23);
			this.btnStop.TabIndex = 2;
			this.btnStop.Text = "Stop Project";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 92);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Sample Text";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(9, 185);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "Status:";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus.Location = new System.Drawing.Point(75, 185);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(0, 20);
			this.lblStatus.TabIndex = 6;
			// 
			// btnShowMsg
			// 
			this.btnShowMsg.Location = new System.Drawing.Point(153, 105);
			this.btnShowMsg.Name = "btnShowMsg";
			this.btnShowMsg.Size = new System.Drawing.Size(97, 23);
			this.btnShowMsg.TabIndex = 7;
			this.btnShowMsg.Text = "Show Message";
			this.btnShowMsg.UseVisualStyleBackColor = true;
			this.btnShowMsg.Click += new System.EventHandler(this.btnShowMsg_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(307, 276);
			this.Controls.Add(this.btnShowMsg);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.txtSample);
			this.Controls.Add(this.btnLaunch);
			this.Name = "Form1";
			this.Text = "Host Form";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnLaunch;
		private System.Windows.Forms.TextBox txtSample;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btnShowMsg;
	}
}

