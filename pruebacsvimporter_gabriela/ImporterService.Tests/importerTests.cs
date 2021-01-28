using System;
using Serilog;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ImporterConsoleApp.Data;
using System.IO;
using System.Threading.Tasks;

namespace ImporterService.Tests
{
    public class ImporterTests : IDisposable
    {
            
        private readonly ITestOutputHelper _testOutputHelper;
        private static Serilog.ILogger _log;

        private Mock<ILoggerFactory> _loggerMock;
        private IConfigurationRoot _config;

        private ImporterConsoleApp.ImporterService _serv;

        private Mock<DbConnectionFactory> _dbConnMock;

        public ImporterTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;  
            createLogInstance();
            
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json")                
                .Build();

            _loggerMock = new Mock<ILoggerFactory>();            
            _dbConnMock = new Mock<DbConnectionFactory>();
            _serv = new ImporterConsoleApp.ImporterService(_config, _loggerMock.Object, _dbConnMock.Object);
        }

        private void createLogInstance()
        {
            _log = new LoggerConfiguration()
           .MinimumLevel.Debug()        
           .WriteTo.File("log.txt", LogEventLevel.Information)
           .CreateLogger();
            _log.Information("Log initialization");           
        }


        [Fact]
        public void CanConnectToAzureBlobStorageService()
        {            
            var uriToken = _serv.GetUriSasToken();          
            Assert.True(uriToken.IsWellFormedOriginalString() , "Uri sas token is not valid.");
        }


        [Fact]
        public async Task CanDownloadFileFromStorageService()
        {
            await _serv.Download();
            var dateTimeString = DateTime.Now.ToString("ddMMyyyy");
            var localFileName = $"stock-{dateTimeString}.csv";
            _log.Information("Downloading File from Azure Storage Local Emulator");            
            var fileExists = File.Exists($"{Directory.GetCurrentDirectory()}//{localFileName}");
            Assert.True(fileExists, "Downloading file failed.");
        }


        [Fact]
        public void CanIngestDataToDB()
        {
            //TODO
        }


        [Fact]
        public void CanLogErrorsWhenFailedIngestion()
        {
            //TODO
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}
