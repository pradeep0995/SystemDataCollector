using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemErrorsDataCollector.Models
{
    public class SystemLog
    {
        public string  Serial_Number { get; set; }
        public string Machine_Name { get; set; }
        public string  Last_Updated { get; set; }

        public List<SystemEventRecord> Events = new List<SystemEventRecord>();
    }
}
