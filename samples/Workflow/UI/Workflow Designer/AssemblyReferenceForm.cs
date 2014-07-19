using System.Windows.Forms;
using System.Workflow.ComponentModel.Compiler;
using System.Reflection;
using System;

namespace WorkflowDesigner
{
    public partial class AssemblyReferenceForm : Form
    {
        private TypeProvider _typeProvider;

        public AssemblyReferenceForm(TypeProvider provider)
        {
            InitializeComponent();
            _typeProvider = provider;
            if (_typeProvider != null)
            {
                PopulateListWithReferences();
            }
        }

        private void PopulateListWithReferences()
        {
            listReferences.Items.Clear();
            foreach (Assembly assembly in _typeProvider.ReferencedAssemblies)
            {
                listReferences.Items.Add(assembly);
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.Filter = "Dll files (*.dll)|*.dll|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.Multiselect = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach (String filename in openFile.FileNames)
                {
                    _typeProvider.AddAssemblyReference(filename);
                }
                PopulateListWithReferences();
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
