namespace WorkflowDesignerApp
{
    partial class MainForm
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
            this.designer = new WorkflowDesigner.WorkflowDesigner();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuTopFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenMarkup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewWorkflow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTopReferences = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReferences = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // designer
            // 
            this.designer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.designer.Location = new System.Drawing.Point(0, 24);
            this.designer.Name = "designer";
            this.designer.Size = new System.Drawing.Size(701, 589);
            this.designer.TabIndex = 0;
            this.designer.TypeProvider = null;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTopFile,
            this.menuTopReferences});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(701, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuTopFile
            // 
            this.menuTopFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenMarkup,
            this.menuNewWorkflow,
            this.menuSave,
            this.menuSaveAs});
            this.menuTopFile.Name = "menuTopFile";
            this.menuTopFile.Size = new System.Drawing.Size(35, 20);
            this.menuTopFile.Text = "File";
            // 
            // menuOpenMarkup
            // 
            this.menuOpenMarkup.Name = "menuOpenMarkup";
            this.menuOpenMarkup.Size = new System.Drawing.Size(180, 22);
            this.menuOpenMarkup.Text = "Open Markup File...";
            this.menuOpenMarkup.Click += new System.EventHandler(this.menuOpenMarkup_Click);
            // 
            // menuNewWorkflow
            // 
            this.menuNewWorkflow.Name = "menuNewWorkflow";
            this.menuNewWorkflow.Size = new System.Drawing.Size(180, 22);
            this.menuNewWorkflow.Text = "New Workflow...";
            this.menuNewWorkflow.Click += new System.EventHandler(this.menuNewWorkflow_Click);
            // 
            // menuSave
            // 
            this.menuSave.Name = "menuSave";
            this.menuSave.Size = new System.Drawing.Size(180, 22);
            this.menuSave.Text = "Save";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAs.Text = "Save As...";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // menuTopReferences
            // 
            this.menuTopReferences.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuReferences});
            this.menuTopReferences.Name = "menuTopReferences";
            this.menuTopReferences.Size = new System.Drawing.Size(74, 20);
            this.menuTopReferences.Text = "References";
            // 
            // menuReferences
            // 
            this.menuReferences.Name = "menuReferences";
            this.menuReferences.Size = new System.Drawing.Size(222, 22);
            this.menuReferences.Text = "Add Assembly References...";
            this.menuReferences.Click += new System.EventHandler(this.menuReferences_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 613);
            this.Controls.Add(this.designer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Workflow Designer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WorkflowDesigner.WorkflowDesigner designer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuTopFile;
        private System.Windows.Forms.ToolStripMenuItem menuOpenMarkup;
        private System.Windows.Forms.ToolStripMenuItem menuNewWorkflow;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuTopReferences;
        private System.Windows.Forms.ToolStripMenuItem menuReferences;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
    }
}

