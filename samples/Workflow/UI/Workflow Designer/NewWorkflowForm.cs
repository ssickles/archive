using System;
using System.Windows.Forms;
using System.Workflow.Activities;
using System.Workflow.ComponentModel.Compiler;
using System.Reflection;

namespace WorkflowDesigner
{
    public partial class NewWorkflowForm : Form
    {
        private TypeProvider _typeProvider;
        private Type _selectedWorkflowType;
        private String _newWorkflowName; 

        public NewWorkflowForm(TypeProvider provider)
        {
            InitializeComponent();
            _typeProvider = provider;
            if (_typeProvider != null)
            {
                PopulateWorkflowList();
            }

            btnCreate.Enabled = false;
        }

        public Type SelectedWorkflowType
        {
            get { return _selectedWorkflowType; }
        }

        public String NewWorkflowName
        {
            get { return _newWorkflowName; }
        }

        private void PopulateWorkflowList()
        {
            listWorkflowTypes.Items.Clear();

            listWorkflowTypes.Items.Add(typeof(SequentialWorkflowActivity));
            listWorkflowTypes.Items.Add(typeof(StateMachineWorkflowActivity));

            foreach (Assembly assembly in _typeProvider.ReferencedAssemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (typeof(SequentialWorkflowActivity).IsAssignableFrom(type) || typeof(StateMachineWorkflowActivity).IsAssignableFrom(type))
                    {
                        listWorkflowTypes.Items.Add(type);
                    }
                }
            }
        }

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (txtWorkflowName.Text.Trim().Length > 0)
            {
                _newWorkflowName = txtWorkflowName.Text.Trim();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please enter a new workflow name", "Name Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            _selectedWorkflowType = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listWorkflowTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listWorkflowTypes.SelectedIndex >= 0)
            {
                _selectedWorkflowType = listWorkflowTypes.SelectedItem as Type;
                btnCreate.Enabled = true;
            }
        }
    }
}
