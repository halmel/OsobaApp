using System.Threading.Tasks;
using SQLite;
using OsobaApp.Models;

namespace OsobaApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private static DatabaseService _instance;
        private readonly SQLiteAsyncConnection _database;

        private DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Person>().Wait();
        }

        public static DatabaseService GetInstance(string dbPath)
        {
            if (_instance == null)
            {
                _instance = new DatabaseService(dbPath);
            }
            return _instance;
        }

        public Task AddPersonAsync(Person person)
        {
            return _database.InsertAsync(person);
        }
    }
}
