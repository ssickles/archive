using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    /// <summary>
    /// Model of a weekday recurrence.
    /// An occurance will happen each weekday.
    /// </summary>
    public class WeekDayRecurrence: RecurrenceBase
    {
        public override DateTime NextOccurance()
        {
            throw new NotImplementedException();
        }
    }
}
