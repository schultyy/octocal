using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Castle.Windsor;

namespace octocal.UI.Calendar.ViewModels
{
    public class CalendarWeekViewModel : Screen
    {
        public override string DisplayName
        {
            get
            {
                return "Week view";
            }
            set
            {
                base.DisplayName = value;
            }
        }

        private BindableCollection<DayViewModel> days;

        public BindableCollection<DayViewModel> Days
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

        public CalendarWeekViewModel()
        {
            Days = new BindableCollection<DayViewModel>();
            BuildUp();
        }

        private void BuildUp()
        {
            DateTime startOfWeek = DateTime.Today;
            int delta = DayOfWeek.Monday - startOfWeek.DayOfWeek;
            startOfWeek = startOfWeek.AddDays(delta);

            DateTime endOfWeek = startOfWeek.AddDays(7);

            DateTime currentDay = startOfWeek;
            while (currentDay < endOfWeek)
            {
                Days.Add(new DayViewModel { Date = currentDay });
                currentDay = currentDay.AddDays(1);
            }
        }
    }
}
