using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Caliburn.Micro;
using Xceed.Wpf.DataGrid.Converters;
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
                LoadDaySchedule();
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

        public bool CanDelete
        {
            get { return technicalId != Guid.Empty; }
        }

        private Guid technicalId;

        private IMessageBoxService messageBox;

        private IAppointmentService appointmentService;

        public EventEditorViewModel(IMessageBoxService service, IAppointmentService appointmentService)
        {
            messageBox = service;
            this.appointmentService = appointmentService;
            this.StartTime = DateTime.Today.AddHours(DateTime.Now.Hour);
            this.EndTime = StartTime.AddHours(1);
            this.Title = string.Empty;
            this.technicalId = Guid.Empty;
            NotifyOfPropertyChange(() => CanDelete);
        }

        public void Edit(Appointment appointment)
        {
            this.Description = appointment.Description;
            this.EndTime = appointment.EndDate;
            this.StartTime = appointment.StartDate;
            this.Title = appointment.Title;
            this.technicalId = appointment.TechnicalId;
            this.Location = appointment.Location;
            NotifyOfPropertyChange(() => CanDelete);
        }

        public override void NotifyOfPropertyChange(string propertyName)
        {
            base.NotifyOfPropertyChange(propertyName);
            LoadDaySchedule();
        }

        private void LoadDaySchedule()
        {
            IsDayScheduleLoading = false;
            Task.Factory.StartNew(LoadDayScheduleInternal);
        }

        private void LoadDayScheduleInternal()
        {
            var schedule = appointmentService.GetAllByStartDate(this.StartTime).Select(c => new ScheduleViewModel
            {
                StartTime = c.StartDate,
                EndTime = c.EndDate,
                Title = c.Title
            })
            .Concat(new[]{new ScheduleViewModel
                              {
                                  EndTime = EndTime,
                                  Title = this.Title,
                                  IsCurrentAppointment = true,
                                  StartTime = StartTime
                              }})
            .OrderBy(c => c.StartTime)
            .ThenBy(c => c.EndTime);

            DaySchedule = new BindableCollection<ScheduleViewModel>(schedule);
            Dispatcher.CurrentDispatcher.Invoke(new Action(() => IsDayScheduleLoading = false));
        }

        public void Dismiss()
        {
            TryClose();
        }

        public void Save()
        {
            appointmentService.SaveAppointment(new Appointment
                                                   {
                                                       Description = Description,
                                                       EndDate = EndTime,
                                                       StartDate = StartTime,
                                                       Title = Title,
                                                       TechnicalId = technicalId,
                                                       Location = location
                                                   });
            TryClose();
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

    public class ScheduleViewModel : PropertyChangedBase
    {
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

        private bool isCurrentAppointment;

        public bool IsCurrentAppointment
        {
            get { return isCurrentAppointment; }
            set
            {
                if (isCurrentAppointment == value)
                    return;
                isCurrentAppointment = value;
                NotifyOfPropertyChange(() => IsCurrentAppointment);
            }
        }
    }
}
