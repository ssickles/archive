using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    public abstract class RecurrenceBase
    {
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan TimeUntilNextOccurance()
        {
            return NextOccurance().Subtract(DateTime.Now);
        }
        public abstract DateTime NextOccurance();
    }
}
