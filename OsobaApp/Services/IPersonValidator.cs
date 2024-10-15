using OsobaApp.Models;
namespace OsobaApp.Services
{
    public interface IPersonValidator
    {
        ValidationResult Validate(Person person);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
