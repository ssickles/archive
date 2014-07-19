//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

namespace QueuedPublishSubscribe
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
         this.SuspendLayout();
         // 
         // HostForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(228,126);
         this.MaximizeBox = false;
         this.Name = "HostForm";
         this.Text = "Persistent Subscriber Host";
         this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
         this.ResumeLayout(false);

      }

      #endregion


   }
}

