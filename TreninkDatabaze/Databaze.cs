using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace TreninkDatabaze
{
    public class DatabaseHandler
    {
        private string _connectionString;
        private SqliteConnection _connection;

        public DatabaseHandler()
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var folderPath = Path.Combine(Environment.GetFolderPath(folder), "TreninkDatabaze");
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Join(folderPath, "database.db");

            var csb = new SqliteConnectionStringBuilder
            {
                DataSource = filePath
            };

            _connectionString = csb.ConnectionString;
        }

        public async Task CreateTablePlanAsync()
        {
            
            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();

                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    CREATE TABLE IF NOT EXISTS Trenink_plan 
                    (
                        Trenink_planId INTEGER PRIMARY KEY,
                        Jmeno TEXT,
                        Typ TEXT,
                        Partie TEXT
                    );
                     ";

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task CreateTableCvikAsync()
        {

            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();

                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    CREATE TABLE IF NOT EXISTS Cvik
                    (
                        CvikId INTEGER PRIMARY KEY,
                        Jmeno TEXT,
                        1x_PR INTEGER,
                        5x_PR INTEGER,
                        10x_PR INTEGER,
                        PlanId INTEGER,
                        FOREIGN KEY (PlanId) REFERENCES Trenink_plan(Trenink_planId)
                    );
                    ";

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task CreateTableTreninkAsync()
        {

            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();

                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    CREATE TABLE IF NOT EXISTS Trenink 
                    (
                        TreninkId INTEGER PRIMARY KEY,
                        Datum TEXT,
                        FOREIGN KEY (CvikId) REFERENCES Cvik(CvikId),
                        FOREIGN KEY (PlanId) REFERENCES Trenink_plan(Trenink_planId)
                    );
                    ";

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task AddTreninkPlanAsync(string typ, string partie)
        {
            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();

                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    INSERT INTO Trenink_plan (Jmeno, Typ, Partie)
                    VALUES (@jmeno, @typ, @partie);
                    SELECT last_insert_rowid();
                     ";
                    command.Parameters.Add("@jmeno", SqliteType.Text).Value = typ;
                    command.Parameters.Add("@typ", SqliteType.Text).Value = typ;
                    command.Parameters.Add("@partie", SqliteType.Text).Value = partie;

                    object? result = await command.ExecuteScalarAsync();
                    if (result is long treninkId)
                    {
                        Console.WriteLine($"Inserted Trenink with ID: {treninkId}");
                    }
                }
            }      
        }
        public async Task AddCvikAsync(string jmeno, int pr1, int pr5, int pr10, int planId)
        {
            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();
                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    INSERT INTO Cvik (Jmeno, 1x_PR, 5x_PR, 10x_PR, PlanId)
                    VALUES (@jmeno, @pr1, @pr5, @pr10, @planId);
                    SELECT last_insert_rowid();
                     ";
                    command.Parameters.Add("@jmeno", SqliteType.Text).Value = jmeno;
                    command.Parameters.Add("@pr1", SqliteType.Integer).Value = pr1;
                    command.Parameters.Add("@pr5", SqliteType.Integer).Value = pr5;
                    command.Parameters.Add("@pr10", SqliteType.Integer).Value = pr10;
                    command.Parameters.Add("@planId", SqliteType.Integer).Value = planId;
                    object? result = await command.ExecuteScalarAsync();
                    if (result is long cvikId)
                    {
                        Console.WriteLine($"Inserted Cvik with ID: {cvikId}");
                    }
                }
            }
        }
        public async Task AddTreninkAsync(string datum, int cvikId, int planId)
        {
            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();
                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    INSERT INTO Trenink (Datum, CvikId, PlanId)
                    VALUES (@datum, @cvikId, @planId);
                    SELECT last_insert_rowid();
                     ";
                    command.Parameters.Add("@datum", SqliteType.Text).Value = datum;
                    command.Parameters.Add("@cvikId", SqliteType.Integer).Value = cvikId;
                    command.Parameters.Add("@planId", SqliteType.Integer).Value = planId;
                    object? result = await command.ExecuteScalarAsync();
                    if (result is long treninkId)
                    {
                        Console.WriteLine($"Inserted Trenink with ID: {treninkId}");
                    }
                }
            }

        }
        public async Task ReadDataPlan()
        {
            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();
                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    SELECT * FROM Trenink_plan;
                    ";
                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var treninkId = reader.GetInt32(0);
                            var jmeno = reader.GetString(1);
                            var typ = reader.GetString(2);
                            var partie = reader.GetString(3);
                            Console.WriteLine($"TreninkId: {treninkId}, Jmeno: {jmeno}, Typ: {typ}, Partie: {partie}");
                        }
                    }
                }
            }
        }
        public async Task ReadDataCvik()
        {
            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();
                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    SELECT * FROM Cvik;
                    ";
                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var cvikId = reader.GetInt32(0);
                            var jmeno = reader.GetString(1);
                            var pr1 = reader.GetInt32(2);
                            var pr5 = reader.GetInt32(3);
                            var pr10 = reader.GetInt32(4);
                            var planId = reader.GetInt32(5);
                            Console.WriteLine($"CvikId: {cvikId}, Jmeno: {jmeno}, 1x_PR: {pr1}, 5x_PR: {pr5}, 10x_PR: {pr10}, PlanId: {planId}");
                        }
                    }
                }
            }
        }
        public async Task ReadDataTrenink()
        {
            await using (_connection = new SqliteConnection(_connectionString))
            {
                await _connection.OpenAsync();
                await using (var command = _connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    SELECT * FROM Trenink;
                    ";
                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var treninkId = reader.GetInt32(0);
                            var datum = reader.GetString(1);
                            var cvikId = reader.GetInt32(2);
                            var planId = reader.GetInt32(3);
                            Console.WriteLine($"TreninkId: {treninkId}, Datum: {datum}, CvikId: {cvikId}, PlanId: {planId}");
                        }
                    }
                }
            }
        }

    }

}
