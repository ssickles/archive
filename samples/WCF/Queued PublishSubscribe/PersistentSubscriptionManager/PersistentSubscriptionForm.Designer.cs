//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

namespace PersistentSubscriptionManager
{
   partial class ManagerForm
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
         this.components = new System.ComponentModel.Container();
         System.Windows.Forms.Button unsubscribeButton;
         System.Windows.Forms.Button subscribeButton;
         System.Windows.Forms.GroupBox subscriptionsGroupBox;
         System.Windows.Forms.GroupBox newSubscriptionGroupBox;
         System.Windows.Forms.Button lookupButton;
         System.Windows.Forms.Label eventLabel;
         System.Windows.Forms.Label queuedServiceAddressLabel;
         System.Windows.Forms.Label contractLabel;
         System.Windows.Forms.Label mexAddressLabel;
         this.m_SubscriptionsGrid = new System.Windows.Forms.DataGridView();
         this.m_AddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_EventsContractDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_EventOperationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_SubscriptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.m_EventTypeComboBox = new System.Windows.Forms.ComboBox();
         this.m_ContractComboBox = new System.Windows.Forms.ComboBox();
         this.m_QueuedServiceAddress = new System.Windows.Forms.TextBox();
         this.m_MEXAddressTextBox = new System.Windows.Forms.TextBox();
         unsubscribeButton = new System.Windows.Forms.Button();
         subscribeButton = new System.Windows.Forms.Button();
         subscriptionsGroupBox = new System.Windows.Forms.GroupBox();
         newSubscriptionGroupBox = new System.Windows.Forms.GroupBox();
         lookupButton = new System.Windows.Forms.Button();
         eventLabel = new System.Windows.Forms.Label();
         queuedServiceAddressLabel = new System.Windows.Forms.Label();
         contractLabel = new System.Windows.Forms.Label();
         mexAddressLabel = new System.Windows.Forms.Label();
         subscriptionsGroupBox.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.m_SubscriptionsGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.m_SubscriptionsBindingSource)).BeginInit();
         newSubscriptionGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // unsubscribeButton
         // 
         unsubscribeButton.Location = new System.Drawing.Point(513, 34);
         unsubscribeButton.Name = "unsubscribeButton";
         unsubscribeButton.Size = new System.Drawing.Size(75, 23);
         unsubscribeButton.TabIndex = 2;
         unsubscribeButton.Text = "Unsubscribe";
         unsubscribeButton.UseVisualStyleBackColor = true;
         unsubscribeButton.Click += new System.EventHandler(this.OnUnsubscribe);
         // 
         // subscribeButton
         // 
         subscribeButton.Location = new System.Drawing.Point(512, 87);
         subscribeButton.Name = "subscribeButton";
         subscribeButton.Size = new System.Drawing.Size(75, 23);
         subscribeButton.TabIndex = 3;
         subscribeButton.Text = "Subscribe";
         subscribeButton.UseVisualStyleBackColor = true;
         subscribeButton.Click += new System.EventHandler(this.OnSubscribe);
         // 
         // subscriptionsGroupBox
         // 
         subscriptionsGroupBox.Controls.Add(this.m_SubscriptionsGrid);
         subscriptionsGroupBox.Controls.Add(unsubscribeButton);
         subscriptionsGroupBox.Location = new System.Drawing.Point(12, 136);
         subscriptionsGroupBox.Name = "subscriptionsGroupBox";
         subscriptionsGroupBox.Size = new System.Drawing.Size(594, 169);
         subscriptionsGroupBox.TabIndex = 4;
         subscriptionsGroupBox.TabStop = false;
         subscriptionsGroupBox.Text = "Available Subscriptions:";
         // 
         // m_SubscriptionsGrid
         // 
         this.m_SubscriptionsGrid.AutoGenerateColumns = false;
         this.m_SubscriptionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.m_SubscriptionsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_AddressDataGridViewTextBoxColumn,
            this.m_EventsContractDataGridViewTextBoxColumn,
            this.m_EventOperationDataGridViewTextBoxColumn});
         this.m_SubscriptionsGrid.DataSource = this.m_SubscriptionsBindingSource;
         this.m_SubscriptionsGrid.Location = new System.Drawing.Point(7, 34);
         this.m_SubscriptionsGrid.Name = "m_SubscriptionsGrid";
         this.m_SubscriptionsGrid.Size = new System.Drawing.Size(496, 129);
         this.m_SubscriptionsGrid.TabIndex = 0;
         // 
         // m_AddressDataGridViewTextBoxColumn
         // 
         this.m_AddressDataGridViewTextBoxColumn.DataPropertyName = "Address";
         this.m_AddressDataGridViewTextBoxColumn.HeaderText = "Address";
         this.m_AddressDataGridViewTextBoxColumn.Name = "m_AddressDataGridViewTextBoxColumn";
         // 
         // m_EventsContractDataGridViewTextBoxColumn
         // 
         this.m_EventsContractDataGridViewTextBoxColumn.DataPropertyName = "EventsContract";
         this.m_EventsContractDataGridViewTextBoxColumn.HeaderText = "EventsContract";
         this.m_EventsContractDataGridViewTextBoxColumn.Name = "m_EventsContractDataGridViewTextBoxColumn";
         // 
         // m_EventOperationDataGridViewTextBoxColumn
         // 
         this.m_EventOperationDataGridViewTextBoxColumn.DataPropertyName = "EventOperation";
         this.m_EventOperationDataGridViewTextBoxColumn.HeaderText = "EventOperation";
         this.m_EventOperationDataGridViewTextBoxColumn.Name = "m_EventOperationDataGridViewTextBoxColumn";
         // 
         // m_SubscriptionsBindingSource
         // 
         this.m_SubscriptionsBindingSource.DataSource = typeof(PersistentSubscription[]);
         // 
         // newSubscriptionGroupBox
         // 
         newSubscriptionGroupBox.Controls.Add(this.m_EventTypeComboBox);
         newSubscriptionGroupBox.Controls.Add(this.m_ContractComboBox);
         newSubscriptionGroupBox.Controls.Add(lookupButton);
         newSubscriptionGroupBox.Controls.Add(subscribeButton);
         newSubscriptionGroupBox.Controls.Add(this.m_QueuedServiceAddress);
         newSubscriptionGroupBox.Controls.Add(this.m_MEXAddressTextBox);
         newSubscriptionGroupBox.Controls.Add(eventLabel);
         newSubscriptionGroupBox.Controls.Add(queuedServiceAddressLabel);
         newSubscriptionGroupBox.Controls.Add(contractLabel);
         newSubscriptionGroupBox.Controls.Add(mexAddressLabel);
         newSubscriptionGroupBox.Location = new System.Drawing.Point(12, 12);
         newSubscriptionGroupBox.Name = "newSubscriptionGroupBox";
         newSubscriptionGroupBox.Size = new System.Drawing.Size(593, 118);
         newSubscriptionGroupBox.TabIndex = 5;
         newSubscriptionGroupBox.TabStop = false;
         newSubscriptionGroupBox.Text = "New Subscription:";
         // 
         // m_EventTypeComboBox
         // 
         this.m_EventTypeComboBox.Location = new System.Drawing.Point(402, 43);
         this.m_EventTypeComboBox.Name = "m_EventTypeComboBox";
         this.m_EventTypeComboBox.Size = new System.Drawing.Size(100, 21);
         this.m_EventTypeComboBox.TabIndex = 5;
         // 
         // m_ContractComboBox
         // 
         this.m_ContractComboBox.Location = new System.Drawing.Point(296, 43);
         this.m_ContractComboBox.Name = "m_ContractComboBox";
         this.m_ContractComboBox.Size = new System.Drawing.Size(100, 21);
         this.m_ContractComboBox.TabIndex = 4;
         this.m_ContractComboBox.TextChanged += new System.EventHandler(this.OnContractChanged);
         // 
         // lookupButton
         // 
         lookupButton.Location = new System.Drawing.Point(512, 43);
         lookupButton.Name = "lookupButton";
         lookupButton.Size = new System.Drawing.Size(75, 23);
         lookupButton.TabIndex = 3;
         lookupButton.Text = "Lookup";
         lookupButton.UseVisualStyleBackColor = true;
         lookupButton.Click += new System.EventHandler(this.LookupSubscription);
         // 
         // m_QueuedServiceAddress
         // 
         this.m_QueuedServiceAddress.Location = new System.Drawing.Point(6, 89);
         this.m_QueuedServiceAddress.Name = "m_QueuedServiceAddress";
         this.m_QueuedServiceAddress.Size = new System.Drawing.Size(496, 20);
         this.m_QueuedServiceAddress.TabIndex = 3;
         // 
         // m_MEXAddressTextBox
         // 
         this.m_MEXAddressTextBox.Location = new System.Drawing.Point(6, 43);
         this.m_MEXAddressTextBox.Name = "m_MEXAddressTextBox";
         this.m_MEXAddressTextBox.Size = new System.Drawing.Size(284, 20);
         this.m_MEXAddressTextBox.TabIndex = 3;
         this.m_MEXAddressTextBox.Text = "http://localhost:7000/";
         // 
         // eventLabel
         // 
         eventLabel.AutoSize = true;
         eventLabel.Location = new System.Drawing.Point(399, 27);
         eventLabel.Name = "eventLabel";
         eventLabel.Size = new System.Drawing.Size(38, 13);
         eventLabel.TabIndex = 2;
         eventLabel.Text = "Event:";
         // 
         // queuedServiceAddressLabel
         // 
         queuedServiceAddressLabel.AutoSize = true;
         queuedServiceAddressLabel.Location = new System.Drawing.Point(9, 73);
         queuedServiceAddressLabel.Name = "queuedServiceAddressLabel";
         queuedServiceAddressLabel.Size = new System.Drawing.Size(128, 13);
         queuedServiceAddressLabel.TabIndex = 0;
         queuedServiceAddressLabel.Text = "Queued Service Address:";
         // 
         // contractLabel
         // 
         contractLabel.AutoSize = true;
         contractLabel.Location = new System.Drawing.Point(293, 27);
         contractLabel.Name = "contractLabel";
         contractLabel.Size = new System.Drawing.Size(91, 13);
         contractLabel.TabIndex = 1;
         contractLabel.Text = "Queued Contract:";
         // 
         // mexAddressLabel
         // 
         mexAddressLabel.AutoSize = true;
         mexAddressLabel.Location = new System.Drawing.Point(6, 27);
         mexAddressLabel.Name = "mexAddressLabel";
         mexAddressLabel.Size = new System.Drawing.Size(74, 13);
         mexAddressLabel.TabIndex = 0;
         mexAddressLabel.Text = "MEX Address:";
         // 
         // ManagerForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(614, 317);
         this.Controls.Add(newSubscriptionGroupBox);
         this.Controls.Add(subscriptionsGroupBox);
         this.Name = "ManagerForm";
         this.Text = "Queued Persistent Subscription Manager";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
         this.Load += new System.EventHandler(this.OnLoad);
         subscriptionsGroupBox.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.m_SubscriptionsGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.m_SubscriptionsBindingSource)).EndInit();
         newSubscriptionGroupBox.ResumeLayout(false);
         newSubscriptionGroupBox.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView m_SubscriptionsGrid;
      private System.Windows.Forms.ComboBox m_EventTypeComboBox;
      private System.Windows.Forms.ComboBox m_ContractComboBox;
      private System.Windows.Forms.TextBox m_MEXAddressTextBox;
      private System.Windows.Forms.BindingSource m_SubscriptionsBindingSource;
      private System.Windows.Forms.TextBox m_QueuedServiceAddress;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_AddressDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_EventsContractDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_EventOperationDataGridViewTextBoxColumn;

   }
}

