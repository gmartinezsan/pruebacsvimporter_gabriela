using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImporterConsoleApp
{
    // <summary>
    /// This class could be used to implement a service to ingest data     
    // with Entity Framework.
    /// </summary>
    public class SqlIngestionService 
    {

        private readonly IConfigurationRoot _config;
        private readonly ILogger<SqlIngestionService> _logger;

        public SqlIngestionService(IConfigurationRoot config, ILoggerFactory loggerFactory, DbContext AcmeDBContext)
        {
                _logger = loggerFactory.CreateLogger<SqlIngestionService>();
                _config = config;                                
        }

        public void IngestData(string fileName)
        {
          
        }

    }
}