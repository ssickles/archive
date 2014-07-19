//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

namespace Subscriber
{
   partial class MyTransientSubscriber
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise,false.</param>
      protected override void Dispose(bool disposing)
      {
         if(disposing && (components != null))
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
         System.Windows.Forms.Button m_SubscribeButtton;
         this.m_UnsubscribeButton = new System.Windows.Forms.Button();
         this.m_AllEventsRadioButton = new System.Windows.Forms.RadioButton();
         this.m_Event3RadioButton = new System.Windows.Forms.RadioButton();
         this.m_Event2RadioButton = new System.Windows.Forms.RadioButton();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.m_Event1RadioButton = new System.Windows.Forms.RadioButton();
         m_SubscribeButtton = new System.Windows.Forms.Button();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // m_SubscribeButtton
         // 
         m_SubscribeButtton.Location = new System.Drawing.Point(138,18);
         m_SubscribeButtton.Name = "m_SubscribeButtton";
         m_SubscribeButtton.Size = new System.Drawing.Size(75,23);
         m_SubscribeButtton.TabIndex = 2;
         m_SubscribeButtton.Text = "Subscribe";
         m_SubscribeButtton.UseVisualStyleBackColor = true;
         m_SubscribeButtton.Click += new System.EventHandler(this.OnSubscribe);
         // 
         // m_UnsubscribeButton
         // 
         this.m_UnsubscribeButton.Location = new System.Drawing.Point(138,51);
         this.m_UnsubscribeButton.Name = "m_UnsubscribeButton";
         this.m_UnsubscribeButton.Size = new System.Drawing.Size(75,23);
         this.m_UnsubscribeButton.TabIndex = 1;
         this.m_UnsubscribeButton.Text = "Unsubscribe";
         this.m_UnsubscribeButton.UseVisualStyleBackColor = true;
         this.m_UnsubscribeButton.Click += new System.EventHandler(this.OnUnsubscribe);
         // 
         // m_AllEventsRadioButton
         // 
         this.m_AllEventsRadioButton.AutoSize = true;
         this.m_AllEventsRadioButton.Checked = true;
         this.m_AllEventsRadioButton.Location = new System.Drawing.Point(12,88);
         this.m_AllEventsRadioButton.Name = "m_AllEventsRadioButton";
         this.m_AllEventsRadioButton.Size = new System.Drawing.Size(36,17);
         this.m_AllEventsRadioButton.TabIndex = 3;
         this.m_AllEventsRadioButton.TabStop = true;
         this.m_AllEventsRadioButton.Text = "All";
         this.m_AllEventsRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_Event3RadioButton
         // 
         this.m_Event3RadioButton.AutoSize = true;
         this.m_Event3RadioButton.Location = new System.Drawing.Point(12,65);
         this.m_Event3RadioButton.Name = "m_Event3RadioButton";
         this.m_Event3RadioButton.Size = new System.Drawing.Size(62,17);
         this.m_Event3RadioButton.TabIndex = 2;
         this.m_Event3RadioButton.TabStop = true;
         this.m_Event3RadioButton.Text = "Event 3";
         this.m_Event3RadioButton.UseVisualStyleBackColor = true;
         // 
         // m_Event2RadioButton
         // 
         this.m_Event2RadioButton.AutoSize = true;
         this.m_Event2RadioButton.Location = new System.Drawing.Point(12,42);
         this.m_Event2RadioButton.Name = "m_Event2RadioButton";
         this.m_Event2RadioButton.Size = new System.Drawing.Size(62,17);
         this.m_Event2RadioButton.TabIndex = 1;
         this.m_Event2RadioButton.Text = "Event 2";
         this.m_Event2RadioButton.UseVisualStyleBackColor = true;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.m_AllEventsRadioButton);
         this.groupBox1.Controls.Add(this.m_Event3RadioButton);
         this.groupBox1.Controls.Add(this.m_Event2RadioButton);
         this.groupBox1.Controls.Add(this.m_Event1RadioButton);
         this.groupBox1.Location = new System.Drawing.Point(12,12);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(104,114);
         this.groupBox1.TabIndex = 3;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Select Event:";
         // 
         // m_Event1RadioButton
         // 
         this.m_Event1RadioButton.AutoSize = true;
         this.m_Event1RadioButton.Location = new System.Drawing.Point(12,19);
         this.m_Event1RadioButton.Name = "m_Event1RadioButton";
         this.m_Event1RadioButton.Size = new System.Drawing.Size(62,17);
         this.m_Event1RadioButton.TabIndex = 0;
         this.m_Event1RadioButton.Text = "Event 1";
         this.m_Event1RadioButton.UseVisualStyleBackColor = true;
         // 
         // MyTransientSubscriber
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(233,138);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(m_SubscribeButtton);
         this.Controls.Add(this.m_UnsubscribeButton);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "MyTransientSubscriber";
         this.Text = "Transient Subscriber";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button m_UnsubscribeButton;
      private System.Windows.Forms.RadioButton m_AllEventsRadioButton;
      private System.Windows.Forms.RadioButton m_Event3RadioButton;
      private System.Windows.Forms.RadioButton m_Event2RadioButton;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.RadioButton m_Event1RadioButton;
   }
}

