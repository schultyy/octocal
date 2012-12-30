using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace octocal.UI.Calendar.ViewModels
{
    public class DayViewModel : Screen
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date == value)
                    return;
                date = value;
                NotifyOfPropertyChange(() => Date);
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
    }
}
