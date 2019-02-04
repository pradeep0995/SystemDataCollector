﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemErrorsDataCollector.Models
{
    class SystemLog
    {
        public string  Serial_Number { get; set; }
        public string Machine_Name { get; set; }
        public string  Last_Updated { get; set; }

        List<SystemEventRecord> Events = new List<SystemEventRecord>();
    }
}
