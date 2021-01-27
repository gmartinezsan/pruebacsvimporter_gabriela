using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ImporterConsoleApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImporterConsoleApp
{
    public class ImporterService 
    {

        private readonly IConfigurationRoot _config;
        private readonly ILogger<ImporterService> _logger;
        private readonly DbConnectionFactory _connectionFactory;

        private string _localFileName;

        public ImporterService(IConfigurationRoot config, ILoggerFactory loggerFactory, DbConnectionFactory connectionFactory)
        {
                _logger = loggerFactory.CreateLogger<ImporterService>();
                _config = config;
                _connectionFactory = connectionFactory;
                _localFileName = string.Empty;
        }

         /// <summary>
        /// A method that creates a SAS token
        /// A sas token can be shared with a customer or another service instead of using
        /// the connection string which can be more risky.
        // The token has an expiration time which has even more control to share the access.
        /// </summary>
        private Uri GetUriSasToken()
        {
            string storedPolicyName = null;
            string connectionString = _config.GetConnectionString("BlobConnection");
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, "stock");

            // Check whether this BlobContainerClient object has been authorized with Shared Key.
            if (containerClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerClient.Name,
                    Resource = "c"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read | BlobContainerSasPermissions.List);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = containerClient.GenerateSasUri(sasBuilder);
                return sasUri;
            }
            else
            {
                _logger.LogError(@"BlobContainerClient must be authorized with Shared Key credentials to create a service SAS.");
                return null;
            }
                     
        }

        public async Task Run()
        {
            await Download();    

            var ingestion = new IngestionData(_connectionFactory.CreateSqlConnection(_config), _logger);
            await ingestion.ExecuteIngestionAsync(_localFileName, false, _config.GetValue<string>("TableName"));

        }


        /// <summary>
        /// Gets a sas token to get the blob from the Azure Storage
        /// and then downloads the content to a local file.
        /// </summary>
        public async Task Download()
        {        
            
            BlobServiceClient blobServiceClient = new BlobServiceClient(GetUriSasToken(), null);
            var blobName = _config.GetValue<string>("BlobName");
            var containerName = _config.GetValue<string>("ContainerName");            
            var fileName = _config.GetValue<string>("FileName");            
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            try
            {               
                BlobClient blob = containerClient.GetBlobClient(blobName); 
                
                // Download the blob's contents and save it to a file locally
                BlobDownloadInfo download = await blob.DownloadAsync();
                
                var dateTimeString = DateTime.Now.ToString("ddMMyyyy");
                _localFileName = $"{fileName}-{dateTimeString}.csv";
                
                using (FileStream file = File.OpenWrite($"./{_localFileName}"))
                {
                    download.Content.CopyTo(file);
                }                
            }
            catch(RequestFailedException ex)
            {
                _logger.LogError($"Cannot access blob in storage. Exception Message {ex.Message}");
            }
        }

    }

}
