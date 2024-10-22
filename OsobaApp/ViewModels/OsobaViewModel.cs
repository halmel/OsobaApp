using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using OsobaApp.Models;
using OsobaApp.Services;

namespace OsobaApp.ViewModels
{
    public class OsobaViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;
        private string _identificationNumber;
        private readonly IPersonValidator _validator;
        private readonly IDatabaseService _databaseService;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Person> Persons { get; } = new ObservableCollection<Person>();

        public OsobaViewModel(IPersonValidator validator, IDatabaseService databaseService)
        {
            _validator = validator;
            _databaseService = databaseService;
            BirthDate = DateTime.Now; // Nastaví výchozí datum

            LoadPersons();
        }
        private async void LoadPersons()
        {
            var persons = await _databaseService.GetPersonsAsync();
            Persons.Clear(); // Vyprázdní kolekci před načtením nových dat
            foreach (var person in persons)
            {
                Persons.Add(person); // Přidá každou osobu do ObservableCollection
            }
        }
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(); }
        }

        public string IdentificationNumber
        {
            get => _identificationNumber;
            set { _identificationNumber = value; OnPropertyChanged(); }
        }

        public ICommand SavePersonCommand => new RelayCommand(SavePerson);

        private async void SavePerson()
        {
            var person = new Person
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                BirthDate = this.BirthDate,
                IdentificationNumber = this.IdentificationNumber
            };

            var validationResult = _validator.Validate(person);
            if (!validationResult.IsValid)
            {
                // Zde by bylo třeba zobrazit chybu uživateli
                return;
            }

            await _databaseService.AddPersonAsync(person);
            // Zde můžeš zobrazit, že osoba byla úspěšně přidána
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
