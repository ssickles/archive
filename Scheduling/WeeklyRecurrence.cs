using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    /// <summary>
    /// Model of a weekly recurrence based on days of the week.
    /// The property "WeeksBetween" will specify how many weeks should ellapse until the next occurance.
    /// </summary>
    public class WeeklyRecurrence: RecurrenceBase
    {
        public int WeeksBetween { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public override DateTime NextOccurance()
        {
            throw new NotImplementedException();
        }
    }
}
