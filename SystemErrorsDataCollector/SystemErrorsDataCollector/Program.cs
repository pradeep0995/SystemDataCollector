using System;
using System.Diagnostics.Eventing.Reader;
using System.Management;
using SystemErrorsDataCollector.Models;

namespace SystemErrorsDataCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemLog systemlog = new SystemLog();

            ManagementObjectSearcher ComSerial = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            foreach (ManagementObject wmi in ComSerial.Get())
            {
                try
                {
                    systemlog.Serial_Number = wmi.GetPropertyValue("SerialNumber").ToString();
                }
                catch { }
            }
            systemlog.Last_Updated = System.DateTime.Now.ToString();

            string query = "*[System/Level=1 or System/Level=2 ]";
            EventLogQuery eventsQuery = new EventLogQuery("System", PathType.LogName, query);
            try
            {
                EventLogReader logReader = new EventLogReader(eventsQuery);
                systemlog.Machine_Name = logReader.ReadEvent().MachineName;
                for (EventRecord eventdetail = logReader.ReadEvent(); eventdetail != null; eventdetail = logReader.ReadEvent())
                {
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

            // serialize to json
            var jsonsytemlog = Newtonsoft.Json.JsonConvert.SerializeObject(systemlog);

        }
    }
}

