using System;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Management;
using SystemErrorsDataCollector.Models;

namespace SystemErrorsDataCollector.Helper
{
    public class ErrorDataCollector
    {
        public SystemLog CollectSystemErrors()
        {
            SystemLog systemlog = new SystemLog();

            ManagementObjectSearcher ComSerial = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            foreach (ManagementObject wmi in ComSerial.Get())
            {
                try
                {
                    systemlog.Serial_Number = wmi.GetPropertyValue("SerialNumber").ToString();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            systemlog.Last_Updated = System.DateTime.Now.ToString();
            int MonthsToSearch = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MonthsWindow"]);
            var startTime = System.DateTime.Now.AddMonths(MonthsToSearch);
            string query = string.Format(@"*[System/Level=1 or System/Level=2] and *[System[TimeCreated[@SystemTime >= '{0}']]]",
                startTime.ToUniversalTime().ToString("o"));


            MappingSection mappingSection = ConfigurationManager.GetSection("Mappings") as MappingSection;

            EventLogQuery eventsQuery = new EventLogQuery("System", PathType.LogName, query);
            try
            {
                EventLogReader logReader = new EventLogReader(eventsQuery);
                systemlog.Machine_Name = logReader.ReadEvent().MachineName;
                for (EventRecord eventdetail = logReader.ReadEvent(); eventdetail != null; eventdetail = logReader.ReadEvent())
                {
                    var mappings = mappingSection.ExclusionEvents.Cast<MappingElement>().Where(c => c.EventID.Equals(eventdetail.Id.ToString()) && c.Source.Equals(eventdetail.ProviderName));
                    if (mappings.Any()) continue;
                    SystemEventRecord systemeventrecord = new SystemEventRecord();
                    systemeventrecord.Level = eventdetail.LevelDisplayName;
                    systemeventrecord.Source = eventdetail.ProviderName;
                    systemeventrecord.Time_Created = eventdetail.TimeCreated.ToString();
                    systemeventrecord.EventId = eventdetail.Id;
                    systemeventrecord.Message = eventdetail.FormatDescription();
                    systemlog.Events.Add(systemeventrecord);
                }
            }
            catch (EventLogNotFoundException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return systemlog;
        }
    }
}
