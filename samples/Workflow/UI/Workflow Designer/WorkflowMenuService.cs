using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace WorkflowDesigner
{
    public class WorkflowMenuService: MenuCommandService
    {
        public WorkflowMenuService(IServiceProvider provider): base(provider)
        {

        }

        public override void ShowContextMenu(CommandID menuID, int x, int y)
        {
            base.ShowContextMenu(menuID, x, y);

            if (menuID == WorkflowMenuCommands.SelectionMenu)
            {
                ContextMenu contextMenu = new ContextMenu();
                foreach (DesignerVerb verb in Verbs)
                {
                    MenuItem menuItem = new MenuItem(verb.Text, new EventHandler(OnMenuClicked));
                    menuItem.Tag = verb;
                    contextMenu.MenuItems.Add(menuItem);
                }

                foreach (MenuItem menu in BuildItemsForSeletion())
                {
                    contextMenu.MenuItems.Add(menu);
                }

                WorkflowView workflowView = GetService(typeof(WorkflowView)) as WorkflowView;
                if (workflowView != null)
                {
                    contextMenu.Show(workflowView, workflowView.PointToClient(new Point(x, y)));
                }
            }
        }

        private IList<MenuItem> BuildItemsForSeletion()
        {
            List<MenuItem> items = new List<MenuItem>();

            Boolean isActivity = false;
            Boolean isComposite = false;
            ISelectionService selectionService = GetService(typeof(ISelectionService)) as ISelectionService;

            if (selectionService != null)
            {
                ICollection selectedObjects = selectionService.GetSelectedComponents();
                if (selectedObjects.Count > 1)
                {
                    isActivity = true;
                    foreach (Object selection in selectedObjects)
                    {
                        if (!(selection is Activity))
                        {
                            isActivity = false;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (Object selection in selectedObjects)
                    {
                        isComposite = (selection is CompositeActivity);
                        isActivity = (selection is Activity);
                    }
                }
            }

            if (isActivity)
            {
                Dictionary<CommandID, String> commands = new Dictionary<CommandID, string>();
                commands.Add(WorkflowMenuCommands.Copy, "Copy");
                commands.Add(WorkflowMenuCommands.Cut, "Cut");
                commands.Add(WorkflowMenuCommands.Paste, "Paste");
                commands.Add(WorkflowMenuCommands.Delete, "Delete");

                if (isComposite)
                {
                    commands.Add(WorkflowMenuCommands.Collapse, "Collapse");
                    commands.Add(WorkflowMenuCommands.Expand, "Expand");
                }

                items.Add(new MenuItem("-"));

                foreach (KeyValuePair<CommandID, String> pair in commands)
                {
                    MenuCommand command = FindCommand(pair.Key);
                    if (command != null)
                    {
                        MenuItem menuItem = new MenuItem(pair.Value, new EventHandler(OnMenuClicked));
                        menuItem.Tag = command;
                        items.Add(menuItem);
                    }
                }
            }
            return items;
        }

        private void OnMenuClicked(Object sender, EventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem menu = sender as MenuItem;
                if ((menu != null) && (menu.Tag is MenuCommand))
                {
                    ((MenuCommand)menu.Tag).Invoke();
                }
            }
        }
    }
}
