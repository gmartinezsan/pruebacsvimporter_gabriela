using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImporterConsoleApp.Data
{
    public class DbConnectionFactory
    {
        public DbConnection CreateSqlConnection(IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            var sqlClientFactory = SqlClientFactory.Instance;
            var connection = sqlClientFactory.CreateConnection();
            connection.ConnectionString = config.GetConnectionString("AcmeDbConnection");
            return connection;
        }
    }
}