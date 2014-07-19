using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;

namespace WorkflowDesigner
{
    public partial class WorkflowDesigner : UserControl
    {
        private WorkflowLoader _wfLoader = new WorkflowLoader();
        private WorkflowView _wfView;
        private Control _toolboxControl;

        private DesignSurface _designSurface;
        private TypeProvider _typeProvider;

        public WorkflowDesigner()
        {
            InitializeComponent();
        }

        public Control ToolboxControl
        {
            get { return _toolboxControl; }
        }

        public TypeProvider TypeProvider
        {
            get { return _typeProvider; }
            set
            {
                _typeProvider = value;
                _wfLoader.TypeProvider = _typeProvider;
            }
        }

        public Boolean LoadWorkflow(String markupFileName)
        {
            ClearWorkflow();

            _designSurface = new DesignSurface();
            _wfLoader.MarkupFileName = markupFileName;
            _wfLoader.NewWorkflowType = null;

            return CommonWorkflowLoading();
        }

        public Boolean CreateNewWorkflow(Type workflowType, String newWorkflowName)
        {
            ClearWorkflow();

            _designSurface = new DesignSurface();
            _wfLoader.MarkupFileName = String.Empty;
            _wfLoader.NewWorkflowType = workflowType;
            _wfLoader.NewWorkflowName = newWorkflowName;

            return CommonWorkflowLoading();
        }

        private bool CommonWorkflowLoading()
        {
            Boolean result = false;

            _designSurface.BeginLoad(_wfLoader);

            IDesignerHost designer = _designSurface.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designer == null || designer.RootComponent == null)
            {
                return false;
            }

            IRootDesigner rootDesigner = designer.GetDesigner(designer.RootComponent) as IRootDesigner;
            if (rootDesigner != null)
            {
                SuspendLayout();

                _wfView = rootDesigner.GetView(ViewTechnology.Default) as WorkflowView;
                splitContainer1.Panel2.Controls.Add(_wfView);
                _wfView.Dock = DockStyle.Fill;
                _wfView.Focus();

                propertyGrid1.Site = designer.RootComponent.Site;

                IToolboxService toolboxService = designer.GetService(typeof(IToolboxService)) as IToolboxService;
                if (toolboxService != null)
                {
                    if (toolboxService is Control)
                    {
                        _toolboxControl = (Control)toolboxService;
                        splitContainer2.Panel1.Controls.Add(_toolboxControl);
                    }
                }

                ISelectionService selectionService = ((IServiceProvider)_wfView).GetService(typeof(ISelectionService)) as ISelectionService;
                if (selectionService != null)
                {
                    selectionService.SelectionChanged += new EventHandler(selectionService_SelectionChanged);
                }

                ResumeLayout();
                result = true;
            }
            return result;
        }

        void selectionService_SelectionChanged(object sender, EventArgs e)
        {
            ISelectionService selectionService = ((IServiceProvider)_wfView).GetService(typeof(ISelectionService)) as ISelectionService;
            if (selectionService != null)
            {
                propertyGrid1.SelectedObject = new ArrayList(selectionService.GetSelectedComponents()).ToArray();
            }
        }

        public Boolean SaveWorkflow(String markupFileName)
        {
            _wfLoader.MarkupFileName = markupFileName;
            _designSurface.Flush();
            return true;
        }

        private void ClearWorkflow()
        {
            if (_designSurface != null)
            {
                IDesignerHost designer = _designSurface.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (designer != null)
                {
                    if (designer.Container.Components.Count > 0)
                    {
                        _wfLoader.RemoveFromDesigner(designer, designer.RootComponent as Activity);
                    }
                }

                _designSurface.Dispose();
                _designSurface = null;
            }

            if (_wfView != null)
            {
                ISelectionService selectionService = ((IServiceProvider)_wfView).GetService(typeof(ISelectionService)) as ISelectionService;
                if (selectionService != null)
                {
                    selectionService.SelectionChanged -= new EventHandler(selectionService_SelectionChanged);
                }

                Controls.Remove(_wfView);
                _wfView.Dispose();
                _wfView = null;
            }

            if (_toolboxControl != null)
            {
                Controls.Remove(_toolboxControl);
            }
        }
    }
}
