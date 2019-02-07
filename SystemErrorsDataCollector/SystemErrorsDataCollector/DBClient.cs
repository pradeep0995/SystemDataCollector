using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using SystemErrorsDataCollector.Models;

namespace SystemErrorsDataCollector
{
    public class DBClient
    {
        public static string connectionString;

        public static string containerName;

        public CloudStorageAccount storageAccount;

        public CloudBlobClient blobClient;
        
        public DBClient()
        {
            connectionString = string.Format(System.Configuration.ConfigurationManager.AppSettings["BlobConnectionString"]);
            containerName = "systemerrorscontainer";
            storageAccount = CloudStorageAccount.Parse(connectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public void UploadErrorsToBlob(SystemLog systemLog)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(systemLog);
                string dateString = DateTime.Now.ToString("MMddyyyyhhmm");

                CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName);

                if (!cloudBlobContainer.Exists())
                    cloudBlobContainer.Create();

                CloudBlobDirectory directory = cloudBlobContainer.GetDirectoryReference(systemLog.Serial_Number);
                CloudBlockBlob cloudBlockBlob = directory.GetBlockBlobReference(dateString + ".json");
                cloudBlockBlob.UploadText(jsonString);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
