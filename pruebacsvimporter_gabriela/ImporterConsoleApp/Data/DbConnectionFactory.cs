using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace ImporterConsoleApp.Data
{
    public class DbConnectionFactory
    {

        // <summary>        
        // Creates the connection to the Sql Client.
        /// </summary>
        public DbConnection CreateSqlConnection(IConfigurationRoot config)
        {
            var sqlClientFactory = SqlClientFactory.Instance;
            var connection = sqlClientFactory.CreateConnection();
            connection.ConnectionString = config.GetConnectionString("AcmeDbConnection");
            return connection;
        }
    }
}