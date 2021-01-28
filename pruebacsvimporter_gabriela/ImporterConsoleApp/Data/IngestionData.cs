using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using ImporterConsoleApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ImporterConsoleApp.Data
{
    public class IngestionData
    {
        private readonly SqlConnection _connection;    

        private readonly ILogger<ImporterService> _logger;
        public IngestionData(DbConnection connection, ILogger<ImporterService> logger)
        {
            _connection = connection as SqlConnection;         
            _logger = logger;   
        }


        // <summary>        
        // Gets a csv file for ingesting the data 
        // In Sql Server using SqlBulkCopy and a csvhelper for mapping the csv
        /// </summary>
        public async Task ExecuteIngestionAsync(string pathAndFileName, bool cleanTable, string tableName)
        {

            using (_connection)
            {
                await _connection.OpenAsync();
                if (cleanTable)
                {
                    var cmd = new SqlCommand("Delete from Stocks");
                    await cmd.ExecuteNonQueryAsync();
                }  
  
                using (var streamReader = new StreamReader(pathAndFileName))
                {
                    var confCsv = new CsvConfiguration(CultureInfo.CurrentCulture, delimiter: ";");                                                     
                    using (var csvReader = new CsvReader(streamReader, confCsv))                            
                    using (var csvDataReader = new CsvDataReader(csvReader))
                    {                                                
                        using (var bulkCopy = new SqlBulkCopy(_connection))
                        {
                            bulkCopy.DestinationTableName = tableName;
                            bulkCopy.BulkCopyTimeout = 0;
                           
                            SqlBulkCopyColumnMapping mapPointOfSale = new SqlBulkCopyColumnMapping("PointOfSale", "PointOfSale");
                            bulkCopy.ColumnMappings.Add(mapPointOfSale);

                            SqlBulkCopyColumnMapping mapProduct = new SqlBulkCopyColumnMapping("Product", "Product");
                            bulkCopy.ColumnMappings.Add(mapProduct);

                            SqlBulkCopyColumnMapping mapDate = new SqlBulkCopyColumnMapping("Date", "Date");
                            bulkCopy.ColumnMappings.Add(mapDate);

                            SqlBulkCopyColumnMapping mapStock = new SqlBulkCopyColumnMapping("Stock", "Stock");
                            bulkCopy.ColumnMappings.Add(mapStock);

                            try
                            {                                
                                await bulkCopy.WriteToServerAsync(csvDataReader);
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, "Failed to ingest data to table {@TableName}", tableName);
                            }
                            finally
                            {
                                csvDataReader.Close();

                                //TODO add number of rows
                                await LogIngestionRow(1, 0, "No errors");
                            }
                        }
                    }
                }                                
            }
        }

        // <summary>
        /// Adds a row to the Ingestion Log in the Sql Server Database                
        /// </summary>
        public async Task LogIngestionRow(int finalState, long rowsIngested, string error)
        {
            var cmd = new SqlCommand("Insert into dbo.LogIngestion (IngestionTimestamp, FinalState, RowsIngested, Error) Values(@IngestionTimestamp, @FinalState, @RowsIngested, @Error)", _connection);
            
            //Parameters
            cmd.Parameters.Add("@IngestionTimestamp", SqlDbType.DateTime);
            cmd.Parameters.Add("@FinalState", SqlDbType.SmallInt);
            cmd.Parameters.Add("@RowsIngested", SqlDbType.BigInt);
            cmd.Parameters.Add("@Error", SqlDbType.VarChar);

            //Values
            cmd.Parameters["@IngestionTimestamp"].Value = DateTime.Now;
            cmd.Parameters["@FinalState"].Value = finalState;
            cmd.Parameters["@RowsIngested"].Value = rowsIngested;
            cmd.Parameters["@Error"].Value = error;


            await cmd.ExecuteNonQueryAsync();
        }  
    }
}