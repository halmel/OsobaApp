using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
using OsobaApp.Services;


namespace OsobaApp.ViewModels
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;
        private string _identificationNumber;
        private readonly IPersonValidator _validator;
        private readonly IDatabaseService _databaseService;

        public event PropertyChangedEventHandler PropertyChanged;

        public PersonViewModel(IPersonValidator validator, IDatabaseService databaseService)
        {
            _validator = validator;
            _databaseService = databaseService;
            BirthDate = DateTime.Now; // Defaultně nastavíme aktuální datum
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
                // Zobrazíme chybu uživateli
                // Zde by bylo třeba volat například dialogové okno, které ukáže chybu.
                return;
            }

            await _databaseService.AddPersonAsync(person);

            // Zde můžeš zobrazit, že operace proběhla úspěšně
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
