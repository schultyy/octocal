using Caliburn.Micro;
using octocal.Domain;

namespace octocal.UI.Calendar.ViewModels
{
    public class HourPartViewModel : PropertyChangedBase
    {
        private int hour;

        public int Hour
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

        public HourPartViewModel()
        {
            Appointments = new BindableCollection<Appointment>();
        }
    }
}