using System;
using System.Data.SqlTypes;
using Caliburn.Micro;
using octocal.Domain;

namespace octocal.UI.Calendar.ViewModels
{
    public class ScheduleViewModel : PropertyChangedBase
    {
        private string hour;

        public string Hour
        {
            get { return hour; }
            set
            {
                if (hour == value)
                    return;
                hour = value;
                NotifyOfPropertyChange(() => Hour);
            }
        }

        private Appointment appointment;

        public Appointment Appointment
        {
            get { return appointment; }
            set
            {
                if (appointment == value)
                    return;
                appointment = value;
                NotifyOfPropertyChange(() => Appointment);
            }
        }
    }
}