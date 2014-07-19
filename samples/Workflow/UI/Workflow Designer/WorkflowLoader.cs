using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;

namespace WorkflowDesigner
{
    public class WorkflowLoader: WorkflowDesignerLoader
    {
        private String _markupFileName = String.Empty;
        private Type _newWorkflowType;
        //private IToolboxService _toolboxService;
        private TypeProvider _typeProvider;
        private String _newWorkflowName = String.Empty;

        public String MarkupFileName 
        {
            get { return _markupFileName; }
            set { _markupFileName = value; }
        }

        public Type NewWorkflowType
        {
            get { return _newWorkflowType; }
            set { _newWorkflowType = value; }
        }

        public String NewWorkflowName
        {
            get { return _newWorkflowName; }
            set { _newWorkflowName = value; }
        }

        public TypeProvider TypeProvider
        {
            get { return _typeProvider; }
            set { _typeProvider = value; }
        }

        public override string FileName
        {
            get { return _markupFileName; }
        }

        public override TextReader GetFileReader(string filePath)
        {
            return null;
        }

        public override TextWriter GetFileWriter(string filePath)
        {
            return null;
        }

        protected override void Initialize()
        {
            base.Initialize();

            IDesignerHost host = LoaderHost;
            if (host != null)
            {
                host.AddService(typeof(IMenuCommandService), new WorkflowMenuService(host));
                host.AddService(typeof(ITypeProvider), _typeProvider, true);
                host.AddService(typeof(IToolboxService), new WorkflowToolboxService(host));
                host.AddService(typeof(IPropertyValueUIService), new WorkflowPropertyValueService(host));
                host.AddService(typeof(IEventBindingService), new WorkflowEventBindingService(host));
            }
        }

        protected override void PerformLoad(IDesignerSerializationManager serializationManager)
        {
            base.PerformLoad(serializationManager);
            Activity workflow = null;

            if (!String.IsNullOrEmpty(MarkupFileName))
            {
                workflow = DeserializeFromMarkup(MarkupFileName);
            }
            else if (NewWorkflowType != null)
            {
                workflow = CreateNewWorkflow(NewWorkflowType, NewWorkflowName);
            }

            if (workflow != null)
            {
                IDesignerHost designer = (IDesignerHost)GetService(typeof(IDesignerHost));
                AddWorkflowToDesigner(designer, workflow);
                designer.Activate();
            }
        }

        private Activity DeserializeFromMarkup(String fileName)
        {
            Activity workflow = null;

            //construct a serialization manager.
            DesignerSerializationManager dsm
                = new DesignerSerializationManager();
            using (dsm.CreateSession())
            {
                using (XmlReader xmlReader
                    = XmlReader.Create(fileName))
                {
                    //deserialize the workflow from the XmlReader
                    WorkflowMarkupSerializer markupSerializer
                        = new WorkflowMarkupSerializer();
                    workflow = markupSerializer.Deserialize(
                        dsm, xmlReader) as Activity;

                    if (dsm.Errors.Count > 0)
                    {
                        WorkflowMarkupSerializationException error
                            = dsm.Errors[0]
                              as WorkflowMarkupSerializationException;
                        throw error;
                    }
                }

                //deserialize a .rules file is one exists
                String rulesFileName = GetRulesFileName(fileName);
                if (File.Exists(rulesFileName))
                {
                    //read the .rules file
                    using (XmlReader xmlReader
                        = XmlReader.Create(rulesFileName))
                    {
                        WorkflowMarkupSerializer markupSerializer
                            = new WorkflowMarkupSerializer();
                        //deserialize the rule definitions
                        RuleDefinitions ruleDefinitions
                            = markupSerializer.Deserialize(dsm, xmlReader)
                                as RuleDefinitions;
                        if (ruleDefinitions != null)
                        {
                            //add the rules definitions to the workflow
                            workflow.SetValue(
                                RuleDefinitions.RuleDefinitionsProperty,
                                ruleDefinitions);
                        }
                    }
                }
            }
            return workflow;
        }

        private static String GetRulesFileName(String fileName)
        {
            String rulesFileName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".rules");
            return rulesFileName;
        }

        private Activity CreateNewWorkflow(Type workflowType, String newWorkflowName)
        {
            Activity workflow = null;

            ConstructorInfo cstr = workflowType.GetConstructor(Type.EmptyTypes);
            if (cstr != null)
            {
                workflow = cstr.Invoke(new object[] { }) as Activity;
                workflow.Name = newWorkflowName;
                _markupFileName = newWorkflowName + ".xoml";
            }
            return workflow;
        }

        private void AddWorkflowToDesigner(IDesignerHost designer, Activity workflow)
        {
            designer.Container.Add(workflow, workflow.QualifiedName);

            if (workflow is CompositeActivity)
            {
                List<Activity> children = new List<Activity>();
                GetChildActivities(workflow as CompositeActivity, children);
                foreach (Activity child in children)
                {
                    designer.Container.Add(child, child.QualifiedName);
                }
            }
        }

        private void GetChildActivities(CompositeActivity compositeActivity, List<Activity> children)
        {
            foreach (Activity activity in compositeActivity.Activities)
            {
                children.Add(activity);
                if (activity is CompositeActivity)
                {
                    GetChildActivities(activity as CompositeActivity, children);
                }
            }
        }

        public void RemoveFromDesigner(IDesignerHost designer, Activity workflow)
        {
            if (workflow != null)
            {
                designer.DestroyComponent(workflow);
                if (workflow is CompositeActivity)
                {
                    List<Activity> children = new List<Activity>();
                    GetChildActivities(workflow as CompositeActivity, children);
                    foreach (Activity child in children)
                    {
                        designer.DestroyComponent(child);
                    }
                }
            }
        }

        public override void Flush()
        {
            PerformFlush(null);
        }

        protected override void PerformFlush(IDesignerSerializationManager serializationManager)
        {
            base.PerformFlush(serializationManager);

            IDesignerHost designer = (IDesignerHost)GetService(typeof(IDesignerHost));

            Activity workflow = designer.RootComponent as Activity;
            if (workflow != null)
            {
                SerializeToMarkup(workflow, _markupFileName);
            }
        }

        private void SerializeToMarkup(Activity workflow, string fileName)
        {
            workflow.SetValue(WorkflowMarkupSerializer.XClassProperty, null);

            using (XmlWriter xmlWriter = XmlWriter.Create(fileName))
            {
                WorkflowMarkupSerializer markupSerializer = new WorkflowMarkupSerializer();
                markupSerializer.Serialize(xmlWriter, workflow);
            }

            RuleDefinitions ruleDefinitions = workflow.GetValue(RuleDefinitions.RuleDefinitionsProperty) as RuleDefinitions;
            if (ruleDefinitions != null)
            {
                if (ruleDefinitions.Conditions.Count > 0 || ruleDefinitions.RuleSets.Count > 0)
                {
                    String rulesFileName = GetRulesFileName(fileName);
                    using (XmlWriter xmlWriter = XmlWriter.Create(rulesFileName))
                    {
                        WorkflowMarkupSerializer markupSerializer = new WorkflowMarkupSerializer();
                        markupSerializer.Serialize(xmlWriter, ruleDefinitions);
                    }
                }
            }
        }
    }
}
