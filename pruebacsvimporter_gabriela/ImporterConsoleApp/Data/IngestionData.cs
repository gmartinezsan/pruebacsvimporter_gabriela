using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ImporterConsoleApp.Data
{
    public class IngestionData
    {

        private readonly SqlConnection _connection;    
        public IngestionData(DbConnection connection)
        {
            _connection = connection as SqlConnection;            
        }

        public async Task ExecuteIngestionAsync(string pathAndFileName, bool cleanTable)
        {

            using (_connection)
            {

                await _connection.OpenAsync();
                if (cleanTable)
                {

                }  
               var table = new DataTable();

                // read the table structure from the database
                using (var adapter = new SqlDataAdapter($"SELECT TOP 0 * FROM stock", _connection))
                {
                    adapter.Fill(table);
                };

                for (var i = 0; i < 100; i++)
                {
                    var row = table.NewRow();
                    // add columns
                    table.Rows.Add(row);
                }

                using (var bulk = new SqlBulkCopy(_connection))
                {
                    bulk.DestinationTableName = "test";
                    bulk.WriteToServer(table);
                }
            }
        }
    }
}