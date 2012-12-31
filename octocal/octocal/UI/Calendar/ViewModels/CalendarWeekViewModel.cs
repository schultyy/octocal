using System;
using System.Windows;
using Caliburn.Micro;
using Castle.Windsor;
using octocal.Domain;
using octocal.UI.Services;
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

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        private IWindsorContainer container;

        private IAppointmentService appointmentService;

        public CalendarWeekViewModel(IWindsorContainer container, IAppointmentService service)
        {
            this.container = container;
            this.appointmentService = service;
            Days = new BindableCollection<DayViewModel>();
            BuildUp();
        }

        private void BuildUp()
        {
            IsBusy = true;
            Days.Clear();
            DateTime startOfWeek = DateTime.Today;
            int delta = DayOfWeek.Monday - startOfWeek.DayOfWeek;
            startOfWeek = startOfWeek.AddDays(delta);

            DateTime endOfWeek = startOfWeek.AddDays(7);

            DateTime currentDay = startOfWeek;

            while (currentDay < endOfWeek)
            {
                Days.Add(new DayViewModel { Date = currentDay, Appointments = new BindableCollection<Appointment>(appointmentService.GetAllByStartDate(currentDay)) });
                currentDay = currentDay.AddDays(1);
            }
            IsBusy = false;
        }

        public void Reload()
        {
            IsBusy = true;

            appointmentService.Reload();

            BuildUp();

            IsBusy = false;
        }

        public void AddEvent()
        {
            container.Resolve<IWindowManager>().ShowModal<EventEditorViewModel>();
            BuildUp();
        }

        public void Delete(Appointment appointment)
        {
            if (appointment == null)
                return;
            if (container.Resolve<IMessageBoxService>().ShowYesNo(string.Format("Do you really want to delete {0}?",
                                                                            appointment.Title)) == MessageBoxResult.Yes)
            {
                appointmentService.DeleteAppointment(appointment);
                BuildUp();
            }
        }

        public void Edit(Appointment currentAppointment)
        {
            var eventEditor = container.Resolve<EventEditorViewModel>();
            eventEditor.Edit(currentAppointment);
            container.Resolve<IWindowManager>().ShowModal(eventEditor);
        }
    }
}
