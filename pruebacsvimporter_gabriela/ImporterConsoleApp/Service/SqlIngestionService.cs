using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImporterConsoleApp
{
    public class SqlIngestionService 
    {

        private readonly IConfigurationRoot _config;
        private readonly ILogger<SqlIngestionService> _logger;

        private readonly IDbConnection _dbConnection;

        public SqlIngestionService(IConfigurationRoot config, ILoggerFactory loggerFactory, IDbConnection dbConnection)
        {
                _logger = loggerFactory.CreateLogger<SqlIngestionService>();
                _config = config;                
                _dbConnection = dbConnection;
        }

        public void IngestData(string fileName, string schemaFile)
        {
            // delete all the data existing in the table to ingest             
            // TODO map the schema with the data in the csv file
            // read the schema file and use it to create a dictionary of columns with types
            // read each row from the file and make the casting to the type in the dictionary
            // insert each row in the sql table            
        }

    }
}