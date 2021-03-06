﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace octocal.Domain
{
    public interface IAppointmentService
    {
        void SaveAppointment(Appointment appointment);
        Appointment[] GetRange(DateTime start, DateTime end);
        Appointment GetByStartDate(DateTime date);
        Appointment[] GetAllByStartDate(DateTime date);
        void DeleteAppointment(Appointment appointment);
        void Reload();
        void DeleteAppointment(Guid technicalId);
    }
}
