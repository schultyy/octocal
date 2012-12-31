using System;
using Caliburn.Micro;
using Castle.Windsor;
using octocal.Domain;
using octocal.UI.Shell.ViewModels;

namespace octocal.UI.Calendar.ViewModels
{
    public class CalendarWeekViewModel : ShellContentBase
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

        private ShellContentBase detailsViewModel;

        public ShellContentBase DetailsViewModel
        {
            get { return detailsViewModel; }
            set
            {
                if (detailsViewModel == value)
                    return;
                detailsViewModel = value;
                NotifyOfPropertyChange(() => DetailsViewModel);
            }
        }

        private IWindsorContainer container;

        public CalendarWeekViewModel(IWindsorContainer container)
        {
            this.container = container;
            Days = new BindableCollection<DayViewModel>();
            BuildUp();
        }

        private void BuildUp()
        {
            Days.Clear();
            DateTime startOfWeek = DateTime.Today;
            int delta = DayOfWeek.Monday - startOfWeek.DayOfWeek;
            startOfWeek = startOfWeek.AddDays(delta);

            DateTime endOfWeek = startOfWeek.AddDays(7);

            DateTime currentDay = startOfWeek;
            var appointmentService = container.Resolve<IAppointmentService>();
            while (currentDay < endOfWeek)
            {
                Days.Add(new DayViewModel { Date = currentDay, Appointments = new BindableCollection<Appointment>(appointmentService.GetAllByStartDate(currentDay)) });
                currentDay = currentDay.AddDays(1);
            }
        }

        public void AddEvent()
        {
            IoC.Get<IWindowManager>().ShowModal<EventEditorViewModel>();
            BuildUp();
        }
    }
}
