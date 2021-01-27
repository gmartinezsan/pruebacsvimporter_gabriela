# pruebacsvimporter_gabriela

Code exercise for data ingestion using .Net Core Framework.


## Requirements
* Download the data from Azure Storage to a local file.
* Ingest the data from the local file into a SQL Server local.
* Add Unit Testing.
* Automated tests is optional.

## Implementation
* Created a Console Application using Dependency Injection with a service container.
* Used SAS tokens for authentication to the Azure Storage Service.
* Used SqlBulkCopy to ingest the data to the local Sql Server.
* Serilog library for logging.
* CsvHelper library to read the csv file.
* xUnit for unit testing.