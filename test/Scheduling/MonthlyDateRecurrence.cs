using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    /// <summary>
    /// Model of a monthly recurrence based on a day in the month.
    /// Each occurance can only happen once a month. It will happen on the day specified.
    /// If the day specified does not exist in the month (e.g. the 30th of February doesn't exist)
    /// the occurance will happen on the last day in the month.
    /// A list of months is provided and the occurance can only happen in these months.
    /// </summary>
    public class MonthlyDateRecurrence: RecurrenceBase
    {
        public int Day { get; set; }
        public List<int> Months { get; set; }
        public override DateTime NextOccurance()
        {
            DateTime start = StartDate.Add(StartTime);
            while (start.CompareTo(DateTime.Now) < 0)
            {

            }
            return start;
        }
    }
}
