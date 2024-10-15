using System.Threading.Tasks;
using OsobaApp.Models;

namespace OsobaApp.Services
{
    public interface IDatabaseService
    {
        Task AddPersonAsync(Person person);
    }
}
