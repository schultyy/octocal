using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace octocal.Domain
{
    public interface IAppointmentService
    {
        void SaveAppointment(Appointment appointment);
        Appointment[] GetRange(DateTime start, DateTime end);
    }
}
