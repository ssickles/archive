using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    /// <summary>
    /// Model of a daily recurrence.
    /// The property "DaysBetween" will specify how many days should elapse before the next occurance.
    /// </summary>
    public class DailyRecurrence: RecurrenceBase
    {
        public int DaysBetween { get; set; }
        public override DateTime NextOccurance()
        {
            DateTime start = StartDate.Add(StartTime);
            while (start.CompareTo(DateTime.Now) < 0)
            {
                start.AddDays(DaysBetween);
            }
            return start;
        }
    }
}
