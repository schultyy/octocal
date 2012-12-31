using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using octocal.UI.Calendar.ViewModels;

namespace octocal.UI.Shell.ViewModels
{
    public class ShellViewModel : Conductor<ShellContentBase>.Collection.OneActive, IShell
    {
        public ShellViewModel(CalendarWeekViewModel child)
        {
            this.ActivateItem(child);
            DisplayName = "octocal - strange name, great software";
        }

        public override void ActivateItem(ShellContentBase item)
        {
            base.ActivateItem(item);

            if (item == null)
                return;

            item.Shell = this;
        }

        public override void TryClose(bool? dialogResult)
        {
            base.TryClose(dialogResult);  
            this.CloseStrategy = new DefaultCloseStrategy<ShellContentBase>(true);
        }
    }

    public class ShellContentBase : Screen
    {
        public IShell Shell { get; set; }
    }

    public interface IShell
    {
    }
}
