using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
            IAppointmentService service,
            DateTime currentDayDate)
        {
            this.service = service;
            this.currentDayDate = currentDayDate;
            DisplayName = "Day Schedule";

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

    public class HourPart : PropertyChangedBase
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

        public HourPart()
        {
            Appointments = new BindableCollection<Appointment>();
        }
    }
}
