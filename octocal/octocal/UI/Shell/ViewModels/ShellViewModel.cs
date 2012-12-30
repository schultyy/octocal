using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using octocal.UI.Calendar.ViewModels;

namespace octocal.UI.Shell.ViewModels
{
    public class ShellViewModel : Screen
    {
        private object child;

        public object Child
        {
            get { return child; }
            set
            {
                if (child == value)
                    return;
                child = value;
                NotifyOfPropertyChange(() => Child);
            }
        }

        public ShellViewModel(CalendarWeekViewModel child)
        {
            this.Child = child;
        }
    }
}
