//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using ServiceModelEx;

namespace PersistentSubscriptionManager
{
   public partial class ManagerForm : Form
   {
      PersistentSubscriptionServiceClient m_Proxy = new PersistentSubscriptionServiceClient();

      string m_ContractNamespace;
      string m_ContractName;

      public ManagerForm()
      {
         InitializeComponent();

         LookupSubscription(this,EventArgs.Empty);
         RefreshGrid();
      }
      void LookupSubscription(object sender,EventArgs e)
      {
         string[] contracts = MetadataHelper.GetContracts<NetMsmqBinding>(m_MEXAddressTextBox.Text);
         if(contracts.Length == 0)
         {
            return;
         }
         m_ContractComboBox.Items.Clear();
         
         foreach(string contract in contracts)
         {
            m_ContractComboBox.Items.Add(contract.Split(' ')[1]);//Drop namespace
         }
         m_ContractName = contracts[0].Split(' ')[1];//Pick first one
         m_ContractNamespace = contracts[0].Split(' ')[0];//Pick first one

         m_ContractComboBox.Text = m_ContractName;
      }
      void OnContractChanged(object sender,EventArgs e)
      {
         string[] operations = MetadataHelper.GetOperations(m_MEXAddressTextBox.Text,m_ContractNamespace,m_ContractName);
         m_EventTypeComboBox.Items.Clear();
         m_EventTypeComboBox.Items.AddRange(operations);
         m_EventTypeComboBox.Text = operations[0];
         m_QueuedServiceAddress.Text = MetadataHelper.GetAddresses<NetMsmqBinding>(m_MEXAddressTextBox.Text,m_ContractNamespace,m_ContractName)[0];      
      }
      void OnClosing(object sender,FormClosingEventArgs e)
      {
         m_Proxy.Close();
      }

      void OnSubscribe(object sender,EventArgs e)
      {
         m_Proxy.PersistSubscribe(m_QueuedServiceAddress.Text,m_ContractComboBox.Text,m_EventTypeComboBox.Text);
         RefreshGrid();
      }
      void OnUnsubscribe(object sender,EventArgs e)
      {
         if(m_SubscriptionsGrid.CurrentRow == null)
         {
            return;
         }
         string address = (string)m_SubscriptionsGrid.CurrentRow.Cells[0].Value;
         string contract = (string)m_SubscriptionsGrid.CurrentRow.Cells[1].Value;
         string eventType = (string)m_SubscriptionsGrid.CurrentRow.Cells[2].Value;

         m_Proxy.PersistUnsubscribe(address,contract,eventType);
         
         RefreshGrid();
      }
      void RefreshGrid()
      {
         m_SubscriptionsBindingSource.DataSource = m_Proxy.GetAllSubscribers();
         ResizeGrid(m_SubscriptionsGrid);
      }

      void OnLoad(object sender,EventArgs e)
      {
         RefreshGrid();
      }
      static void ResizeGrid(DataGridView grid)
      {
         for(int i = 0; i < grid.ColumnCount; i++)
         {
            grid.AutoResizeColumn(i,DataGridViewAutoSizeColumnMode.AllCells);
         }
      }
   }
}