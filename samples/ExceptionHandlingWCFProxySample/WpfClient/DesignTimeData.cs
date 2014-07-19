// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList.WpfClient.ServiceReference1;

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
