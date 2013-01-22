using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using octocal.Domain;

namespace octocal.UI.Calendar.ViewModels
{
    public class DayTileViewModel : Screen
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date == value)
                    return;
                date = value;
                NotifyOfPropertyChange(() => Date);
                NotifyOfPropertyChange(() => IsToday);
            }
        }

        public bool IsToday
        {
            get { return Date.Date == DateTime.Today.Date; }
        }

        private BindableCollection<Appointment> appointments;

        public BindableCollection<Appointment> Appointments
        {
            get { return appointments; }
            set
            {
                if (appointments == value)
                    return;
                appointments = value;
                NotifyOfPropertyChange(() => Appointments);
            }
        }

        public DayTileViewModel()
        {
            Appointments = new BindableCollection<Appointment>();
        }
    }
}
