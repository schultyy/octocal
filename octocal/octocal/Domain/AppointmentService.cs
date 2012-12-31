using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace octocal.Domain
{
    public class AppointmentService : IAppointmentService
    {
        public void SaveAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException("appointment");
            if (appointment.TechnicalId == Guid.Empty)
            {
                //Insert
            }
            else
            {
                //Update
            }
        }

        public Appointment[] GetRange(DateTime start, DateTime end)
        {

            return null;
        }
    }
}
