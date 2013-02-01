using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace octocal.UI.Calendar.ViewModels
{
    public class DateTimeViewModel : PropertyChangedBase
    {
        private BindableCollection<int> days;

        public BindableCollection<int> Days
        {
            get { return days; }
            set
            {
                if (days == value)
                    return;
                days = value;
                NotifyOfPropertyChange(() => Days);
            }
        }

        private Dictionary<int, string> months;

        public Dictionary<int, string> Months
        {
            get { return months; }
            set
            {
                if (months == value)
                    return;
                months = value;
                NotifyOfPropertyChange(() => Months);
            }
        }

        private int selectedDay;

        public int SelectedDay
        {
            get { return selectedDay; }
            set
            {
                if (selectedDay == value)
                    return;
                selectedDay = value;
                NotifyOfPropertyChange(() => SelectedDay);
            }
        }

        private BindableCollection<int> years;

        public BindableCollection<int> Years
        {
            get { return years; }
            set
            {
                if (years == value)
                    return;
                years = value;
                NotifyOfPropertyChange(() => Years);
            }
        }

        private int selectedYear;

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (selectedYear == value)
                    return;
                selectedYear = value;
                NotifyOfPropertyChange(() => SelectedYear);
            }
        }

        private KeyValuePair<int, string> selectedMonth;

        public KeyValuePair<int, string> SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                if (Equals(selectedMonth, value))
                    return;
                selectedMonth = value;
                NotifyOfPropertyChange(() => SelectedMonth);
                OnMonthsChanged();
            }
        }

        private BindableCollection<int> hours;

        public BindableCollection<int> Hours
        {
            get { return hours; }
            set
            {
                if (hours == value)
                    return;
                hours = value;
                NotifyOfPropertyChange(() => Hours);
            }
        }

        private BindableCollection<int> minutes;

        public BindableCollection<int> Minutes
        {
            get { return minutes; }
            set
            {
                if (minutes == value)
                    return;
                minutes = value;
                NotifyOfPropertyChange(() => Minutes);
            }
        }

        private int selectedHour;

        public int SelectedHour
        {
            get { return selectedHour; }
            set
            {
                if (selectedHour == value)
                    return;
                selectedHour = value;
                NotifyOfPropertyChange(() => SelectedHour);
            }
        }

        private int selectedMinute;

        public int SelectedMinute
        {
            get { return selectedMinute; }
            set
            {
                if (selectedMinute == value)
                    return;
                selectedMinute = value;
                NotifyOfPropertyChange(() => SelectedMinute);
            }
        }

        public DateTime DateTime
        {
            get
            {
                return new DateTime(SelectedYear, SelectedMonth.Key, SelectedDay, SelectedHour, SelectedMinute, 0);
            }
        }

        public DateTimeViewModel()
        {
            Days = new BindableCollection<int>();
            Months = new Dictionary<int, string>();
            Years = new BindableCollection<int>();
            Hours = new BindableCollection<int>();
            Minutes = new BindableCollection<int>();


            for (var i = 0; i < 24; i++)
                Hours.Add(i + 1);
            for (var i = 0; i < 60; i++)
                Minutes.Add(i);
            for (var i = DateTime.Now.Year - 20; i < DateTime.Now.Year + 20; i++)
                Years.Add(i);

            SelectedYear = DateTime.Now.Year;

            OnMonthsChanged();

            SelectedDay = DateTime.Now.Day;

            SelectedMonth = Months.Single(c => c.Key == DateTime.Now.Month);

            SelectedHour = DateTime.Now.Hour;
            SelectedMinute = DateTime.Now.Minute;
        }

        public DateTimeViewModel(DateTime date)
            : this()
        {
            this.SelectedDay = date.Day;
            this.SelectedMonth = Months.Single(c => c.Key == date.Month);
            this.SelectedYear = date.Year;
            this.SelectedHour = date.Hour;
            this.SelectedMinute = date.Minute;
        }

        private void OnMonthsChanged()
        {
            Months.Clear();
            for (var i = 0; i < 12; i++)
            {
                var dayCount = DateTime.DaysInMonth(SelectedYear, i + 1);

                Days.Clear();
                for (var x = 0; x < dayCount; x++)
                    Days.Add(x + 1);

                Months.Add(i + 1, DateTimeFormatInfo.CurrentInfo.GetMonthName(i + 1));
            }
            NotifyOfPropertyChange(() => SelectedDay);
        }
    }
}
