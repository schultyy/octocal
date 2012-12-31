using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace octocal.Domain
{
    public class Appointment
    {
        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid TechnicalId { get; set; }
    }
}
