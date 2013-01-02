using System;
using Caliburn.Micro;

namespace octocal.UI.Calendar.ViewModels
{
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