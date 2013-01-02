using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace octocal.Domain
{
    public class AppointmentService : IAppointmentService
    {
        private readonly string storageDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "octocal");

        private readonly string filename = "allTimeList.xml";

        private List<Appointment> allTimeList;

        public AppointmentService()
        {
            allTimeList = new List<Appointment>();

            if (!Directory.Exists(storageDirectory))
                Directory.CreateDirectory(storageDirectory);

            if (!File.Exists(Path.Combine(storageDirectory, filename)))
                Serialize();

            Deserialize();
        }

        public void SaveAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException("appointment");
            if (appointment.TechnicalId == Guid.Empty)
            {
                //Insert
                appointment.TechnicalId = Guid.NewGuid();
                allTimeList.Add(appointment);
            }
            else
            {
                //Update
                var storedAppointment = allTimeList.SingleOrDefault(c => c.TechnicalId == appointment.TechnicalId);
                var appointmentIndex = allTimeList.IndexOf(storedAppointment);
                allTimeList[appointmentIndex] = appointment;
            }

            Task.Factory.StartNew(Serialize);
        }

        private void Deserialize()
        {
            using (var stream = new StreamReader(Path.Combine(storageDirectory, filename)))
            {
                var serializer = new XmlSerializer(typeof(List<Appointment>));
                allTimeList = serializer.Deserialize(stream) as List<Appointment>;
            }
        }

        private void Serialize()
        {
            using (var streamWriter = new StreamWriter(Path.Combine(storageDirectory, filename)))
            {
                var serializer = new XmlSerializer(typeof(List<Appointment>));
                serializer.Serialize(streamWriter, allTimeList);
            }
        }

        public Appointment[] GetRange(DateTime start, DateTime end)
        {
            return allTimeList.Where(c => c.StartDate >= start && c.StartDate <= end)
                .OrderBy(c => c.StartDate)
                .ThenBy(c => c.EndDate)
                .ToArray();
        }

        public Appointment GetByStartDate(DateTime date)
        {
            return allTimeList.SingleOrDefault(c => c.StartDate.Date == date.Date);
        }

        public Appointment[] GetAllByStartDate(DateTime date)
        {
            return allTimeList.Where(c => c.StartDate.Date == date.Date)
                .OrderBy(c => c.StartDate)
                .ThenBy(c => c.EndDate)
                .ToArray();
        }

        public void DeleteAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException("appointment");

            allTimeList.Remove(appointment);
            Task.Factory.StartNew(Serialize);
        }

        public void DeleteAppointment(Guid technicalId)
        {
            if (technicalId == Guid.Empty)
                throw new ArgumentException("technicalId can not be Guid.empty");

            allTimeList.Remove(allTimeList.Single(c => c.TechnicalId == technicalId));
            Task.Factory.StartNew(Serialize);
        }

        public void Reload()
        {
            Deserialize();
        }
    }
}
