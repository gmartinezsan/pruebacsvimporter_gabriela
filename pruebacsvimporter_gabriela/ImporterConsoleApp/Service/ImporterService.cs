using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImporterConsoleApp
{
    public class ImporterService 
    {

        private readonly IConfigurationRoot _config;
        private readonly ILogger<ImporterService> _logger;

        public ImporterService(IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
                _logger = loggerFactory.CreateLogger<ImporterService>();
                _config = config;
        }

        public async Task Run()
        {
            //TODO
        }


        public void Download()
        {            
            // Get a connection string to our Azure Storage account.
            string connectionString = _config.GetConnectionString("BlobConnection");
            string downloadPath = "Some path from the configuration";
            string blobContainerName = "ContainerName";
            string fileName = "File with the data";

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, blobContainerName);
            container.Create();
            try
            {
                // Get a reference to a blob named "sample-file"
                BlobClient blob = container.GetBlobClient(fileName);
 
                // Download the blob's contents and save it to a file locally
                BlobDownloadInfo download = blob.Download();
                using (FileStream file = File.OpenWrite(downloadPath))
                {
                    download.Content.CopyTo(file);
                }

                // Verify the contents
                //Assert.AreEqual(SampleFileContent, File.ReadAllText(downloadPath));
            }
            finally
            {
                //TODO                
                container.Delete();
            }
        }

    }

}
