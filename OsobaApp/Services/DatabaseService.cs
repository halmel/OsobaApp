using Microsoft.Data.Sqlite;
using OsobaApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OsobaApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private static DatabaseService _instance;
        private static readonly object _lock = new object();
        private readonly string _databasePath;


        public static DatabaseService GetInstance(string databaseName)
        {
            lock (_lock)
            {
                return _instance ??= new DatabaseService(databaseName);
            }
        }

        private DatabaseService(string databaseName)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(folderPath); // Ensure the directory exists
            _databasePath = Path.Combine(folderPath, databaseName);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={_databasePath}");
                connection.Open();

                var createTableCommand = connection.CreateCommand();
                createTableCommand.CommandText =
                @"
            CREATE TABLE IF NOT EXISTS Persons (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                BirthDate TEXT NOT NULL,
                IdentificationNumber TEXT NOT NULL
            )
        ";
                createTableCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            }
        }


        public async Task AddPersonAsync(Person person)
        {
            using var connection = new SqliteConnection($"Data Source={_databasePath}");
            await connection.OpenAsync();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText =
            @"
                INSERT INTO Persons (FirstName, LastName, BirthDate, IdentificationNumber)
                VALUES ($firstName, $lastName, $birthDate, $identificationNumber)
            ";

            insertCommand.Parameters.AddWithValue("$firstName", person.FirstName);
            insertCommand.Parameters.AddWithValue("$lastName", person.LastName);
            insertCommand.Parameters.AddWithValue("$birthDate", person.BirthDate.ToString("yyyy-MM-dd")); // Formátování na string
            insertCommand.Parameters.AddWithValue("$identificationNumber", person.IdentificationNumber);

            await insertCommand.ExecuteNonQueryAsync();
        }

        public async Task<List<Person>> GetPersonsAsync()
        {
            var persons = new List<Person>();

            using var connection = new SqliteConnection($"Data Source={_databasePath}");
            await connection.OpenAsync();

            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText = "SELECT FirstName, LastName, BirthDate, IdentificationNumber FROM Persons";

            using var reader = await selectCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var person = new Person
                {
                    FirstName = reader.GetString(0),
                    LastName = reader.GetString(1),
                    BirthDate = DateTime.Parse(reader.GetString(2)), // Načtení jako DateTime
                    IdentificationNumber = reader.GetString(3)
                };
                persons.Add(person);
            }

            return persons;
        }
    }
}
