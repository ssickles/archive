// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TodoList.WpfClient.ServiceReference1;

namespace TodoList.WpfClient.ServiceReference1 
{
    public partial class TodoItem : object
    {
        static TodoItem()
        {
            PriorityFlags.Add(PriorityFlag.High);
            PriorityFlags.Add(PriorityFlag.Normal);
            PriorityFlags.Add(PriorityFlag.Low);

            StatusFlags.Add(StatusFlag.NotStarted);
            StatusFlags.Add(StatusFlag.InProgress);
            StatusFlags.Add(StatusFlag.Deferred);
            StatusFlags.Add(StatusFlag.WaitingOnSomeoneElse);
            StatusFlags.Add(StatusFlag.Completed);
        }


        public static ObservableCollection<PriorityFlag> PriorityFlags = new ObservableCollection<PriorityFlag>();
        public static ObservableCollection<StatusFlag> StatusFlags = new ObservableCollection<StatusFlag>();

    }
}
