using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;

namespace WorkflowDesigner
{
    public class WorkflowToolboxService: UserControl, IToolboxService
    {
        private ListBox _activitiesList;
        private List<Type> _standardActivities = new List<Type>();
        private IServiceProvider _serviceProvider;

        public WorkflowToolboxService(IServiceProvider provider)
        {
            _serviceProvider = provider;
            Dock = DockStyle.Fill;

            _activitiesList = new ListBox();
            _activitiesList.Dock = DockStyle.Fill;
            _activitiesList.ItemHeight = 23;
            _activitiesList.DrawMode = DrawMode.OwnerDrawFixed;
            _activitiesList.DrawItem += new DrawItemEventHandler(ActivitiesList_DrawItem);
            _activitiesList.MouseMove += new MouseEventHandler(ActivitiesList_MouseMove);

            Controls.Add(_activitiesList);

            _standardActivities.Add(typeof(CallExternalMethodActivity));
            _standardActivities.Add(typeof(CancellationHandlerActivity));
            _standardActivities.Add(typeof(CodeActivity));
            _standardActivities.Add(typeof(CompensatableSequenceActivity));
            _standardActivities.Add(typeof(CompensatableTransactionScopeActivity));
            _standardActivities.Add(typeof(CompensateActivity));
            _standardActivities.Add(typeof(ConditionedActivityGroup));
            _standardActivities.Add(typeof(DelayActivity));
            _standardActivities.Add(typeof(EventDrivenActivity));
            _standardActivities.Add(typeof(EventHandlersActivity));
            _standardActivities.Add(typeof(EventHandlingScopeActivity));
            _standardActivities.Add(typeof(FaultHandlerActivity));
            _standardActivities.Add(typeof(FaultHandlersActivity));
            _standardActivities.Add(typeof(HandleExternalEventActivity));
            _standardActivities.Add(typeof(IfElseActivity));
            _standardActivities.Add(typeof(InvokeWebServiceActivity));
            _standardActivities.Add(typeof(InvokeWorkflowActivity));
            _standardActivities.Add(typeof(ListenActivity));
            _standardActivities.Add(typeof(ParallelActivity));
            _standardActivities.Add(typeof(PolicyActivity));
            _standardActivities.Add(typeof(ReplicatorActivity));
            _standardActivities.Add(typeof(SequenceActivity));
            _standardActivities.Add(typeof(SetStateActivity));
            _standardActivities.Add(typeof(StateActivity));
            _standardActivities.Add(typeof(StateFinalizationActivity));
            _standardActivities.Add(typeof(StateInitializationActivity));
            _standardActivities.Add(typeof(SuspendActivity));
            _standardActivities.Add(typeof(SynchronizationScopeActivity));
            _standardActivities.Add(typeof(TerminateActivity));
            _standardActivities.Add(typeof(ThrowActivity));
            _standardActivities.Add(typeof(TransactionScopeActivity));
            _standardActivities.Add(typeof(WebServiceFaultActivity));
            _standardActivities.Add(typeof(WebServiceInputActivity));
            _standardActivities.Add(typeof(WebServiceOutputActivity));
            _standardActivities.Add(typeof(WhileActivity));

            CreateToolboxItems(_activitiesList, _standardActivities);
        }

        private void CreateToolboxItems(ListBox _activitiesList, List<Type> _standardActivities)
        {
            _activitiesList.Items.Clear();

            LoadReferenceTypes(_activitiesList);
            foreach (Type activityType in _standardActivities)
            {
                ToolboxItem item = CreateItemForActivityType(activityType);
                if (item != null)
                {
                    _activitiesList.Items.Add(item);
                }
            }
        }

        private ToolboxItem CreateItemForActivityType(Type activityType)
        {
            ToolboxItem result = null;
            ToolboxItemAttribute toolboxAttribute = null;
            foreach (Attribute attribute in activityType.GetCustomAttributes(typeof(ToolboxItemAttribute), true))
            {
                if (attribute is ToolboxItemAttribute)
                {
                    toolboxAttribute = (ToolboxItemAttribute)attribute;
                    break;
                }
            }

            if (toolboxAttribute != null)
            {
                if (toolboxAttribute.ToolboxItemType != null)
                {
                    ConstructorInfo constructor = toolboxAttribute.ToolboxItemType.GetConstructor(new Type[] { typeof(Type) });
                    if (constructor != null)
                    {
                        result = constructor.Invoke(new Object[] { activityType }) as ToolboxItem;
                    }
                }
            }
            else
            {
                result = new ToolboxItem(activityType);
            }
            return result;
        }

        private void LoadReferenceTypes(ListBox _activitiesList)
        {
            ITypeProvider typeProvider = _serviceProvider.GetService(typeof(ITypeProvider)) as ITypeProvider;
            if (typeProvider == null)
            {
                return;
            }

            foreach (Assembly assembly in typeProvider.ReferencedAssemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (typeof(Activity).IsAssignableFrom(type))
                    {
                        ToolboxItem item = CreateItemForActivityType(type);
                        if (item != null)
                        {
                            _activitiesList.Items.Add(item);
                        }
                    }
                }
            }
        }

        void ActivitiesList_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_activitiesList.SelectedItem is ActivityToolboxItem)
                {
                    ActivityToolboxItem selectedItem = _activitiesList.SelectedItem as ActivityToolboxItem;
                    IDataObject dataObject = SerializeToolboxItem(selectedItem) as IDataObject;
                    DoDragDrop(dataObject, DragDropEffects.All);
                }
            }
        }

        void ActivitiesList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            ActivityToolboxItem item = ((ListBox)sender).Items[e.Index] as ActivityToolboxItem;
            if (item != null)
            {
                Graphics graphics = e.Graphics;
                graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                if (e.State == DrawItemState.Selected)
                {
                    Rectangle rect = e.Bounds;
                    rect.Width -= 2;
                    rect.Height -= 2;
                    graphics.DrawRectangle(SystemPens.ActiveBorder, rect);
                }

                Int32 bitmapWidth = 0;
                if (item.Bitmap != null)
                {
                    graphics.DrawImage(item.Bitmap, e.Bounds.X + 2, e.Bounds.Y + 2, item.Bitmap.Width, item.Bitmap.Height);
                    bitmapWidth = item.Bitmap.Width;
                }

                graphics.DrawString(item.DisplayName, e.Font, SystemBrushes.ControlText, e.Bounds.X + bitmapWidth + 2, e.Bounds.Y + 2);
            }
        }

        public new void Refresh()
        {
            CreateToolboxItems(_activitiesList, _standardActivities);
        }

        #region IToolboxService Members

        public void AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public void AddCreator(ToolboxItemCreatorCallback creator, string format)
        {
            throw new NotImplementedException();
        }

        public void AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public void AddLinkedToolboxItem(ToolboxItem toolboxItem, IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public void AddToolboxItem(ToolboxItem toolboxItem, string category)
        {
            throw new NotImplementedException();
        }

        public void AddToolboxItem(ToolboxItem toolboxItem)
        {
            throw new NotImplementedException();
        }

        public CategoryNameCollection CategoryNames
        {
            get { return new CategoryNameCollection(new String[] { "WindowsWorkflow" }); }
        }

        public ToolboxItem DeserializeToolboxItem(object serializedObject, IDesignerHost host)
        {
            ToolboxItem result = null;
            if (serializedObject is IDataObject)
            {
                result = ((IDataObject)serializedObject).GetData(typeof(ToolboxItem)) as ToolboxItem;
            }
            return result;
        }

        public ToolboxItem DeserializeToolboxItem(object serializedObject)
        {
            return DeserializeToolboxItem(serializedObject, null);
        }

        public ToolboxItem GetSelectedToolboxItem(IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public ToolboxItem GetSelectedToolboxItem()
        {
            throw new NotImplementedException();
        }

        public ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public ToolboxItemCollection GetToolboxItems(string category)
        {
            throw new NotImplementedException();
        }

        public ToolboxItemCollection GetToolboxItems(IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public ToolboxItemCollection GetToolboxItems()
        {
            throw new NotImplementedException();
        }

        public bool IsSupported(object serializedObject, ICollection filterAttributes)
        {
            return true;
        }

        public bool IsSupported(object serializedObject, IDesignerHost host)
        {
            return true;
        }

        public bool IsToolboxItem(object serializedObject, IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public bool IsToolboxItem(object serializedObject)
        {
            throw new NotImplementedException();
        }

        public void RemoveCreator(string format, IDesignerHost host)
        {
            throw new NotImplementedException();
        }

        public void RemoveCreator(string format)
        {
            throw new NotImplementedException();
        }

        public void RemoveToolboxItem(ToolboxItem toolboxItem, string category)
        {
            throw new NotImplementedException();
        }

        public void RemoveToolboxItem(ToolboxItem toolboxItem)
        {
            throw new NotImplementedException();
        }

        public string SelectedCategory
        {
            get
            {
                return "WindowsWorkflow";
            }
            set
            {

            }
        }

        public void SelectedToolboxItemUsed()
        {
            throw new NotImplementedException();
        }

        public object SerializeToolboxItem(ToolboxItem toolboxItem)
        {
            DataObject dataObject = new DataObject();
            dataObject.SetData(typeof(ToolboxItem), toolboxItem);
            return dataObject;
        }

        public bool SetCursor()
        {
            return false;
        }

        public void SetSelectedToolboxItem(ToolboxItem toolboxItem)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
