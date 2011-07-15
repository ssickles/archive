using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace CalendarControl
{
    public class CalendarControl : Control
    {
        private DateTime _currentDate;
        private List<CalendarWeek> _weeks;
        private CalendarWeek _week;
        private CalendarDay _day;

        ListView view;

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(CalendarControl), new PropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty CalendarModeProperty =
            DependencyProperty.Register("CalendarMode", typeof(CalendarControlMode), typeof(CalendarControl), new PropertyMetadata(CalendarControlMode.Month));

        static CalendarControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarControl), new FrameworkPropertyMetadata(typeof(CalendarControl)));
        }

        public CalendarControl()
        {
            _currentDate = DateTime.Now;
            InitCalendarDays();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            view = GetTemplateChild("PART_GridDisplay") as ListView;
            view.ItemsSource = _weeks;
        }

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public CalendarControlMode CalendarMode
        {
            get { return (CalendarControlMode)GetValue(CalendarModeProperty); }
            set
            { 
                SetValue(CalendarModeProperty, value);
                InitCalendarDays();
            }
        }

        private void InitCalendarDays()
        {
            switch (CalendarMode)
            {
                case CalendarControlMode.Day:
                    _day = new CalendarDay(_currentDate.Year, _currentDate.Month, _currentDate.Day);
                    break;
                case CalendarControlMode.Week:
                    _week = new CalendarWeek(_currentDate.Year, _currentDate.Month, WeekFromDay(_currentDate.Year, _currentDate.Month, _currentDate.Day));
                    break;
                case CalendarControlMode.Month:
                    int weeksInMonth = WeeksInMonth(_currentDate.Year, _currentDate.Month);
                    _weeks = new List<CalendarWeek>();
                    for (int i = 1; i <= weeksInMonth; i++)
                    {
                        _weeks.Add(new CalendarWeek(_currentDate.Year, _currentDate.Month, i));
                    }
                    break;
            }
        }

        private int WeekFromDay(int Year, int Month, int Day)
        {
            DayOfWeek dayWeek = new DateTime(Year, Month, Day).DayOfWeek;
            DayOfWeek firstDay = new DateTime(Year, Month, 1).DayOfWeek;
            int temp = Day - (1 - (int)firstDay);
            int rem;
            return Math.DivRem(temp, 7, out rem) + 1;
        }

        private int WeeksInMonth(int Year, int Month)
        {
            if (Month == 12)
            {
                Year ++;
                Month = 1;
            }
            else
            {
                Month ++;
            }
            int daysInMonth = new DateTime(Year, Month, 1).AddDays(-1).Day;
            DayOfWeek firstDay = new DateTime(Year, Month - 1, 1).DayOfWeek;
            daysInMonth = daysInMonth - (7 - (int)firstDay);
            int rem;
            int weeks = 1 + Math.DivRem(daysInMonth, 7, out rem);
            if (rem > 0)
            {
                weeks++;
            }
            return weeks;
        }
    }

    public enum CalendarControlMode
    {
        Month,
        Week,
        Day
    }
}
