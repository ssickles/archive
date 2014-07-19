namespace WorkflowDesigner
{
    partial class NewWorkflowForm
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
            this.listWorkflowTypes = new System.Windows.Forms.ListBox();
            this.txtWorkflowName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listWorkflowTypes
            // 
            this.listWorkflowTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.listWorkflowTypes.FormattingEnabled = true;
            this.listWorkflowTypes.Location = new System.Drawing.Point(0, 0);
            this.listWorkflowTypes.Name = "listWorkflowTypes";
            this.listWorkflowTypes.Size = new System.Drawing.Size(467, 160);
            this.listWorkflowTypes.TabIndex = 0;
            this.listWorkflowTypes.SelectedIndexChanged += new System.EventHandler(this.listWorkflowTypes_SelectedIndexChanged);
            // 
            // txtWorkflowName
            // 
            this.txtWorkflowName.Location = new System.Drawing.Point(173, 176);
            this.txtWorkflowName.Name = "txtWorkflowName";
            this.txtWorkflowName.Size = new System.Drawing.Size(200, 20);
            this.txtWorkflowName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "New Workflow Name";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(134, 204);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(117, 23);
            this.btnCreate.TabIndex = 3;
            this.btnCreate.Text = "Create Workflow";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(257, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NewWorkflowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 239);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWorkflowName);
            this.Controls.Add(this.listWorkflowTypes);
            this.Name = "NewWorkflowForm";
            this.Text = "NewWorkflowForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listWorkflowTypes;
        private System.Windows.Forms.TextBox txtWorkflowName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
    }
}