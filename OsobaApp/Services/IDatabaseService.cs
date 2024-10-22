using OsobaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OsobaApp.Services
{
    public interface IDatabaseService
    {
        Task AddPersonAsync(Person person);
        Task<List<Person>> GetPersonsAsync();
    }
}
