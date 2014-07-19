using System;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Compiler;
using WorkflowDesigner;

namespace WorkflowDesignerApp
{
    public partial class MainForm : Form
    {
        private String _loadedMarkupFile = String.Empty;
        private TypeProvider _typeProvider;

        public MainForm()
        {
            InitializeComponent();
            _typeProvider = new TypeProvider(new ServiceContainer());
            designer.TypeProvider = _typeProvider;
        }

        private void menuOpenMarkup_Click(object sender, EventArgs e)
        {
            SetApplicationTitle(null);
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.Filter = "xoml files (*.xoml)|*.xoml|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                String fileName = openFile.FileName;
                try
                {
                    if (designer.LoadWorkflow(fileName))
                    {
                        _loadedMarkupFile = fileName;
                        SetApplicationTitle(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Unable to load markup file", "Error loading markup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(String.Format("Exception loading workflow: {0}", exception.Message), "Exception in LoadWorkflow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuNewWorkflow_Click(object sender, EventArgs e)
        {
            NewWorkflowForm form = new NewWorkflowForm(_typeProvider);
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (designer.CreateNewWorkflow(form.SelectedWorkflowType, form.NewWorkflowName))
                    {
                        _loadedMarkupFile = form.NewWorkflowName + ".xoml";
                        SetApplicationTitle(_loadedMarkupFile);

                        menuSaveAs_Click(this, new EventArgs());
                    }
                    else
                    {
                        MessageBox.Show("Unable to create new workflow", "Error creating workflow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(String.Format("Exception creating workflow: {0}", exception.Message), "Exception in CreateNewWorkflow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_loadedMarkupFile))
            {
                SaveWorkflowDefinition(_loadedMarkupFile);
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = Environment.CurrentDirectory;
            saveFile.Filter = "xoml files (*.xoml)|*.xoml|All Files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                if (SaveWorkflowDefinition(saveFile.FileName))
                {
                    SetApplicationTitle(saveFile.FileName);
                }
                else
                {
                    SetApplicationTitle(null);
                }
            }
        }

        private void menuReferences_Click(object sender, EventArgs e)
        {
            AssemblyReferenceForm form = new AssemblyReferenceForm(_typeProvider);
            form.ShowDialog();

            if (designer.ToolboxControl != null)
            {
                ((IToolboxService)designer.ToolboxControl).Refresh();
            }
        }

        private void SetApplicationTitle(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                this.Text = "Custom Workflow Designer";
            }
            else
            {
                this.Text = String.Format("Custom Workflow Designer: {0}", Path.GetFileName(fileName));
            }
        }

        private bool SaveWorkflowDefinition(string fileName)
        {
            Boolean result = false;

            try
            {
                if (designer.SaveWorkflow(fileName))
                {
                    _loadedMarkupFile = fileName;
                    result = true;
                }
                else
                {
                    MessageBox.Show("Unable to save markup file", "Error saving markup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(String.Format("Exception saving workflow: {0}", exception.Message), "Exception in SaveWorkflowDefinition", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
