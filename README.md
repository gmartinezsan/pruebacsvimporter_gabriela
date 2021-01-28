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

## How to Use it
1. Install the Azure Storage Local Emulator
2. Create a "stock" blob container
3. Create a "data" folder inside the "stock" container
4. Upload a csv file to the data folder as the source of the data
5. Setup the configuration values at the local.settings.json file
6. Create the "AcmeDB" database in your local sql server using the DatabaseScript/database.sql file
7. Add the connection string for the database to the local.settings.json file
8. Build the project and run the ImporterConsoleApp.
9. Run the tests using dotnet test.

