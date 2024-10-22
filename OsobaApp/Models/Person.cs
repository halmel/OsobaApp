namespace OsobaApp.Models
{
    public class Person
    {
        public int Id { get; set; } // Přidání Id
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdentificationNumber { get; set; }
    }
}
