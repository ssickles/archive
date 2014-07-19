//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

namespace QueuedPublishSubscribe
{
   partial class MyPublisherForm
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
         System.Windows.Forms.Button fireEventButton;
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.m_Event3RadioButton = new System.Windows.Forms.RadioButton();
         this.m_Event2RadioButton = new System.Windows.Forms.RadioButton();
         this.m_Event1RadioButton = new System.Windows.Forms.RadioButton();
         this.m_QueuedCheckBox = new System.Windows.Forms.CheckBox();
         fireEventButton = new System.Windows.Forms.Button();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // fireEventButton
         // 
         fireEventButton.Location = new System.Drawing.Point(138,18);
         fireEventButton.Name = "fireEventButton";
         fireEventButton.Size = new System.Drawing.Size(75,23);
         fireEventButton.TabIndex = 0;
         fireEventButton.Text = "Fire Event";
         fireEventButton.Click += new System.EventHandler(this.OnFireEvent);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.m_Event3RadioButton);
         this.groupBox1.Controls.Add(this.m_Event2RadioButton);
         this.groupBox1.Controls.Add(this.m_Event1RadioButton);
         this.groupBox1.Location = new System.Drawing.Point(12,12);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(104,102);
         this.groupBox1.TabIndex = 1;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Select Event:";
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
         // m_Event1RadioButton
         // 
         this.m_Event1RadioButton.AutoSize = true;
         this.m_Event1RadioButton.Checked = true;
         this.m_Event1RadioButton.Location = new System.Drawing.Point(12,19);
         this.m_Event1RadioButton.Name = "m_Event1RadioButton";
         this.m_Event1RadioButton.Size = new System.Drawing.Size(62,17);
         this.m_Event1RadioButton.TabIndex = 0;
         this.m_Event1RadioButton.TabStop = true;
         this.m_Event1RadioButton.Text = "Event 1";
         this.m_Event1RadioButton.UseVisualStyleBackColor = true;
         // 
         // m_QueuedCheckBox
         // 
         this.m_QueuedCheckBox.AutoSize = true;
         this.m_QueuedCheckBox.Location = new System.Drawing.Point(138,54);
         this.m_QueuedCheckBox.Name = "m_QueuedCheckBox";
         this.m_QueuedCheckBox.Size = new System.Drawing.Size(64,17);
         this.m_QueuedCheckBox.TabIndex = 2;
         this.m_QueuedCheckBox.Text = "Queued";
         this.m_QueuedCheckBox.UseVisualStyleBackColor = true;
         // 
         // MyPublisherForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(228,126);
         this.Controls.Add(this.m_QueuedCheckBox);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(fireEventButton);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "MyPublisherForm";
         this.Text = "Publisher";
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.RadioButton m_Event3RadioButton;
      private System.Windows.Forms.RadioButton m_Event2RadioButton;
      private System.Windows.Forms.RadioButton m_Event1RadioButton;
      private System.Windows.Forms.CheckBox m_QueuedCheckBox;

   }
}

