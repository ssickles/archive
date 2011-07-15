using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarControl
{
    public class CalendarWeek
    {
        private int _year;
        private int _month;
        private int _week;
        private List<CalendarDay> _days;

        public CalendarWeek(int Year, int Month, int WeekNumber)
        {
            _year = Year;
            _month = Month;
            _week = WeekNumber;

            _days = new List<CalendarDay>();

            DateTime startDate = new DateTime(Year, Month, 1);
            DayOfWeek firstDay = startDate.DayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                DateTime newDate = startDate.AddDays(((WeekNumber - 1) * 7) - (int)firstDay + i);
                CalendarDay day = new CalendarDay(newDate.Year, newDate.Month, newDate.Day);
                _days.Add(day);
            }
        }

        public int Year
        {
            get { return _year; }
        }

        public int Month
        {
            get { return _month; }
        }

        public int Week
        {
            get { return _week; }
        }

        public List<CalendarDay> Days
        {
            get { return _days; }
        }
    }
}
