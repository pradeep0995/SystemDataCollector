using SystemErrorsDataCollector.Models;
using SystemErrorsDataCollector.Helper;

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

