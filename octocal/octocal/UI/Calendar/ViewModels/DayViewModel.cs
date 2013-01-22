using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Castle.Windsor;
using octocal.Domain;

namespace octocal.UI.Calendar.ViewModels
{
    public class DayViewModel : Screen
    {
        private readonly IAppointmentService service;
        private readonly DateTime currentDayDate;

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

        public DayViewModel(IWindsorContainer container,
            IAppointmentService service,
            DateTime currentDayDate)
        {
            this.service = service;
            this.currentDayDate = currentDayDate;
            DisplayName = "Day Schedule";
        }

        private void LoadDaySchedule()
        {
            Appointments = new BindableCollection<Appointment>
                                    (service.GetAllByStartDate(currentDayDate));

        }
    }
}
