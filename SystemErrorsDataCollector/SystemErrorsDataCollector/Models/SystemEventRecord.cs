using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemErrorsDataCollector.Models
{
    public class SystemEventRecord
    {
        public string  Level{ get; set; }
        public string  Source { get; set; }
        public string Time_Created { get; set; }
        public int EventId { get; set; }
        public string Message { get; set; }
    }
}
