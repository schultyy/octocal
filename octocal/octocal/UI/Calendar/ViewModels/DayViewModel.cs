using System;
using System.Linq;
using Caliburn.Micro;
using Castle.Windsor;
using octocal.Domain;
using octocal.UI.Shell.ViewModels;

namespace octocal.UI.Calendar.ViewModels
{
    public class DayViewModel : ShellContentBase
    {
        private readonly IAppointmentService service;
        private readonly DateTime currentDayDate;

        private BindableCollection<HourPart> timeLine;

        public BindableCollection<HourPart> TimeLine
        {
            get { return timeLine; }
            set
            {
                if (timeLine == value)
                    return;
                timeLine = value;
                NotifyOfPropertyChange(() => TimeLine);
            }
        }

        public DayViewModel(IWindsorContainer container,
            IAppointmentService service)
        {
            this.service = service;
            this.currentDayDate = DateTime.Today;
            DisplayName = "Day Schedule";
            TimeLine = new BindableCollection<HourPart>();

            BuildupTimeLine();
            LoadDaySchedule();
        }

        private void BuildupTimeLine()
        {
            for (var i = 1; i < 25; i++)
                TimeLine.Add(new HourPart { Hour = i });
        }

        private void LoadDaySchedule()
        {
            var appointments = new BindableCollection<Appointment>
                                    (service.GetAllByStartDate(currentDayDate));

            foreach (var appointment in appointments)
            {
                var hourPart = TimeLine.Single(c => c.Hour == appointment.StartDate.Hour);

                hourPart.Appointments.Add(appointment);
            }
        }
    }
}
