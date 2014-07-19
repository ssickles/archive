using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    public enum WeekNumber
    {
        First,
        Second,
        Third,
        Fourth,
        Last
    }

    /// <summary>
    /// Model of a monthly recurrence based on a day of the week and week number.
    /// Each occurance can only happen once a month. It will happen on the day of the week and week number specified.
    /// A list of months is provided and the occurance can only happen in these months.
    /// </summary>
    public class MonthlyDayRecurrence: RecurrenceBase
    {
        public WeekNumber Week { get; set; }
        public DayOfWeek Day { get; set; }
        public List<int> Months { get; set; }
        public override DateTime NextOccurance()
        {
            throw new NotImplementedException();
        }
    }
}
