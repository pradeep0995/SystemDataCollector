using System;
using System.Diagnostics.Eventing.Reader;
using System.Management;
using SystemErrorsDataCollector.Models;
using System.Configuration;
using SystemErrorsDataCollector.Helper;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace SystemErrorsDataCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            ErrorDataCollector errorDataCollector = new ErrorDataCollector();
            SystemLog systemLog = errorDataCollector.CollectSystemErrors();
            
            DBClient dbClient = new DBClient();
            dbClient.UploadErrorsToBlob(systemLog);
        }
    }
}

