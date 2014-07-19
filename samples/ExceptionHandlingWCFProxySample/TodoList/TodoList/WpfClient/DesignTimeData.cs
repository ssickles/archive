using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace TodoList.WpfClient.DesignTime
{
    class Data
    {
        public static List<TodoItem> DesignTimeData
        {
            get
            {
                Random rnd = new Random();

                List<TodoItem> data = new List<TodoItem>();
                for (int x = 0; x < 50; x++)
                {
                    data.Add(new TodoItem
                                 {
                                     Title = string.Format("Design Time Item #{0}", x),
                                     ID = x.ToString(),
                                     CreationDate = DateTime.Today.AddDays(-x),
                                     Status = StatusFlag.InProgress,
                                     DueDate = DateTime.Today.AddDays(x),
                                     Priority = (PriorityFlag) rnd.Next(0, 3),
                                     Description = string.Format("blah {0} blah {0} blah {0} blah {0} blah {0} blah", x),
                                     PercentComplete = rnd.NextDouble()*100.00
                                    
                    });
                }
                return data;
            }
        }
    }
}
