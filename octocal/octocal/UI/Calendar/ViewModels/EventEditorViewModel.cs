using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Windows;
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

        private IMessageBoxService messageBox;

        private IAppointmentService appointmentService;

        public EventEditorViewModel(IMessageBoxService service, IAppointmentService appointmentService)
        {
            messageBox = service;
            this.appointmentService = appointmentService;
        }

        public void Dismiss()
        {
            if (messageBox.ShowYesNo("Do you really want to close?") == MessageBoxResult.Yes)
                TryClose();
        }

        public void Save()
        {
            appointmentService.SaveAppointment(new Appointment
                                                   {
                                                       Description = Description,
                                                       EndDate = EndTime,
                                                       StartDate = StartTime,
                                                       Title = Title
                                                   });
            TryClose();
        }
    }
}
