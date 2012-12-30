using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

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

        private WeekViewModel week;

        public WeekViewModel Week
        {
            get { return week; }
            set
            {
                if (week == value)
                    return;
                week = value;
                NotifyOfPropertyChange(() => Week);
            }
        }

        public CalendarWeekViewModel()
        {
            
            BuildUp();
        }

        private void BuildUp()
        {
            DateTime startOfWeek = DateTime.Today;
            int delta = DayOfWeek.Monday - startOfWeek.DayOfWeek;
            startOfWeek = startOfWeek.AddDays(delta);

            DateTime endOfWeek = startOfWeek.AddDays(7);

            for (int i = 0; i < 7; i++)
            {

            }
        }
    }
}
