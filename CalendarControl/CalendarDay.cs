using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarControl
{
    public class CalendarDay
    {
        private int _year;
        private int _month;
        private int _day;

        public CalendarDay(int Year, int Month, int Day)
        {
            _year = Year;
            _month = Month;
            _day = Day;
        }

        public int Year
        {
            get { return _year; }
        }

        public int Month
        {
            get { return _month; }
        }

        public int Day
        {
            get { return _day; }
        }
    }
}
