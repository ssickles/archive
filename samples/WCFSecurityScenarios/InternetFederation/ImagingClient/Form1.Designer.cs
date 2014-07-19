namespace ImagingClient
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.labFilename = new System.Windows.Forms.Label();
            this.dlg = new System.Windows.Forms.OpenFileDialog();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.pic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(504, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 77;
            this.button1.Text = "Upload File";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(17, 22);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(57, 24);
            this.Label3.TabIndex = 75;
            this.Label3.Text = "Filename:";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labFilename
            // 
            this.labFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labFilename.ForeColor = System.Drawing.Color.Brown;
            this.labFilename.Location = new System.Drawing.Point(83, 22);
            this.labFilename.Name = "labFilename";
            this.labFilename.Size = new System.Drawing.Size(416, 64);
            this.labFilename.TabIndex = 73;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(504, 22);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(112, 23);
            this.cmdBrowse.TabIndex = 76;
            this.cmdBrowse.Text = "Browse...";
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // pic
            // 
            this.pic.Location = new System.Drawing.Point(12, 80);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(604, 377);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 78;
            this.pic.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 469);
            this.Controls.Add(this.pic);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.labFilename);
            this.Controls.Add(this.cmdBrowse);
            this.Name = "Form1";
            this.Text = "Imaging Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label labFilename;
        internal System.Windows.Forms.OpenFileDialog dlg;
        internal System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.PictureBox pic;
    }
}

