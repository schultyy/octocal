using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using octocal.Domain;
using octocal.UI.Services;
using octocal.UI.Shell.ViewModels;

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

        private Guid technicalId;

        private IMessageBoxService messageBox;

        private IAppointmentService appointmentService;

        public EventEditorViewModel(IMessageBoxService service, IAppointmentService appointmentService)
        {
            messageBox = service;
            this.appointmentService = appointmentService;
            this.StartTime = DateTime.Today.AddHours(DateTime.Now.Hour);
            this.EndTime = StartTime.AddHours(1);
            this.technicalId = Guid.Empty;
        }

        public void Edit(Appointment appointment)
        {
            this.Description = appointment.Description;
            this.EndTime = appointment.EndDate;
            this.StartTime = appointment.StartDate;
            this.Title = appointment.Title;
            this.technicalId = appointment.TechnicalId;
            this.Location = appointment.Location;
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
}
