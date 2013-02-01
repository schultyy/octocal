using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Caliburn.Micro;
using octocal.Domain;
using octocal.UI.Services;
using octocal.UI.Shell.ViewModels;
using Action = System.Action;

namespace octocal.UI.Calendar.ViewModels
{
    public class EventEditorViewModel : ShellContentBase
    {
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                if (title == value)
                    return;
                title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                if (description == value)
                    return;
                description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime == value)
                    return;
                startTime = value;
                NotifyOfPropertyChange(() => StartTime);

                CheckTime();
            }
        }

        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime == value)
                    return;
                endTime = value;
                NotifyOfPropertyChange(() => EndTime);
            }
        }

        private string location;

        public string Location
        {
            get { return location; }
            set
            {
                if (location == value)
                    return;
                location = value;
                NotifyOfPropertyChange(() => Location);
            }
        }

        private BindableCollection<ScheduleViewModel> daySchedule;

        public BindableCollection<ScheduleViewModel> DaySchedule
        {
            get { return daySchedule; }
            set
            {
                if (daySchedule == value)
                    return;
                daySchedule = value;
                NotifyOfPropertyChange(() => DaySchedule);
            }
        }

        private bool isDayScheduleLoading;

        public bool IsDayScheduleLoading
        {
            get { return isDayScheduleLoading; }
            set
            {
                if (isDayScheduleLoading == value)
                    return;
                isDayScheduleLoading = value;
                NotifyOfPropertyChange(() => IsDayScheduleLoading);
            }
        }

        private DateTimeViewModel startTimeViewModel;

        public DateTimeViewModel StartTimeViewModel
        {
            get { return startTimeViewModel; }
            set
            {
                if (startTimeViewModel == value)
                    return;
                startTimeViewModel = value;
                NotifyOfPropertyChange(() => StartTimeViewModel);
            }
        }

        private DateTimeViewModel endTimeViewModel;

        public DateTimeViewModel EndTimeViewModel
        {
            get { return endTimeViewModel; }
            set
            {
                if (endTimeViewModel == value)
                    return;
                endTimeViewModel = value;
                NotifyOfPropertyChange(() => EndTimeViewModel);
            }
        }

        public bool CanDelete
        {
            get { return technicalId != Guid.Empty; }
        }

        private Guid technicalId;

        private IMessageBoxService messageBox;

        private IAppointmentService appointmentService;

        public EventEditorViewModel(IMessageBoxService service,
            IAppointmentService appointmentService)
        {
            messageBox = service;
            this.appointmentService = appointmentService;
            this.StartTime = DateTime.Today.AddHours(DateTime.Now.Hour);
            this.EndTime = StartTime.AddHours(1);
            this.Title = string.Empty;
            this.technicalId = Guid.Empty;

            StartTimeViewModel = new DateTimeViewModel();
            EndTimeViewModel = new DateTimeViewModel();

            LoadDaySchedule();
            NotifyOfPropertyChange(() => CanDelete);
        }

        public void Edit(Appointment appointment)
        {
            this.Description = appointment.Description;
            this.EndTime = appointment.EndDate;
            //this.StartTime = appointment.StartDate;
            this.StartTimeViewModel = new DateTimeViewModel(appointment.StartDate);
            this.Title = appointment.Title;
            this.technicalId = appointment.TechnicalId;
            this.Location = appointment.Location;
            LoadDaySchedule();
            NotifyOfPropertyChange(() => CanDelete);
        }

        private void LoadDaySchedule()
        {
            if (IsDayScheduleLoading)
                return;
            IsDayScheduleLoading = true;
            Task.Factory.StartNew(LoadDayScheduleInternal);
        }

        private void LoadDayScheduleInternal()
        {
            DaySchedule = new BindableCollection<ScheduleViewModel>();
            for (var i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    var hourFormat = string.Format("0{0}", i);
                    this.DaySchedule.Add(new ScheduleViewModel { Hour = hourFormat });
                }
                else
                    this.DaySchedule.Add(new ScheduleViewModel { Hour = i.ToString(CultureInfo.InvariantCulture) });
            }

            foreach (var appointment in appointmentService.GetAllByStartDate(this.StartTime))
                DaySchedule.Single(c => Convert.ToInt32(c.Hour) == appointment.StartDate.Hour).Appointment = appointment;
            Dispatcher.CurrentDispatcher.Invoke(new Action(() => IsDayScheduleLoading = false));
        }

        public void Dismiss()
        {
            TryClose();
        }

        public void Save()
        {
            Validate();
            appointmentService.SaveAppointment(new Appointment
                                                   {
                                                       Description = Description,
                                                       EndDate = EndTime,
                                                       StartDate = StartTimeViewModel.DateTime,
                                                       Title = Title,
                                                       TechnicalId = technicalId,
                                                       Location = location
                                                   });
            TryClose();
        }

        private void Validate()
        {
            var results = new List<string>();
            if (EndTime < StartTime)
                results.Add("EndTime can not lay in the past");
            if (string.IsNullOrEmpty(Title))
                results.Add("Title must not be null or empty");

            if (results.Count > 0)
                throw new Exception(String.Join("\n", results));
        }

        private void CheckTime()
        {
            if (StartTime.Day > EndTime.Day)
                EndTime = EndTime.AddDays(StartTime.Day - EndTime.Day);

            if (StartTime.Hour > EndTime.Hour)
                EndTime = EndTime.AddHours(StartTime.Hour - EndTime.Hour + 1);
        }

        public void Delete()
        {
            if (messageBox.ShowYesNo("Are you sure that you want to delete this appointment?") == MessageBoxResult.Yes)
            {
                appointmentService.DeleteAppointment(technicalId);
                TryClose();
            }
        }

        public void OnKeyDown(KeyEventArgs args)
        {
            if (args.Key == Key.Escape)
            {
                Dismiss();
            }
        }
    }
}
