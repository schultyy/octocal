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

        private List<Appointment> allTimeList;

        private Dictionary<Guid, KeyValuePair<DateTime, DateTime>> index;

        public AppointmentService()
        {
            allTimeList = new List<Appointment>();
            index = new Dictionary<Guid, KeyValuePair<DateTime, DateTime>>();

            if (!Directory.Exists(storageDirectory))
                Directory.CreateDirectory(storageDirectory);

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
                index.Add(appointment.TechnicalId, new KeyValuePair<DateTime, DateTime>(appointment.StartDate, appointment.EndDate));
            }
            else
            {
                //Update
                var storedAppointment = allTimeList.SingleOrDefault(c => c.TechnicalId == appointment.TechnicalId);
                var appointmentIndex = allTimeList.IndexOf(storedAppointment);
                allTimeList[appointmentIndex] = appointment;
                index[appointment.TechnicalId] = new KeyValuePair<DateTime, DateTime>(appointment.StartDate, appointment.EndDate);
            }

            Task.Factory.StartNew(Serialize);
        }

        private void Deserialize()
        {
            using (var stream = new StreamReader(Path.Combine(storageDirectory, "allTimeList.xml")))
            {
                var serializer = new XmlSerializer(typeof(List<Appointment>));
                allTimeList = serializer.Deserialize(stream) as List<Appointment>;
            }
        }

        private void Serialize()
        {
            using (var streamWriter = new StreamWriter(Path.Combine(storageDirectory, "allTimeList.xml")))
            {
                var serializer = new XmlSerializer(typeof(List<Appointment>));
                serializer.Serialize(streamWriter, allTimeList);
            }
        }

        public Appointment[] GetRange(DateTime start, DateTime end)
        {

            return null;
        }
    }
}
