using System;
using System.Text.RegularExpressions;
using OsobaApp.Models;

namespace OsobaApp.Services
{
    public class PersonValidator : IPersonValidator
    {
        public ValidationResult Validate(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "Jméno a příjmení jsou povinná." };
            }

            if (person.BirthDate > DateTime.Now)
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "Datum narození musí být v minulosti." };
            }

            if (!IsValidIdentificationNumber(person.IdentificationNumber, person.BirthDate))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "Neplatné rodné číslo." };
            }

            return new ValidationResult { IsValid = true };
        }

        private bool IsValidIdentificationNumber(string idNumber, DateTime birthDate)
        {
            if (birthDate < new DateTime(1954, 1, 1))
            {
                // Pattern for cccccc/ccc
                return Regex.IsMatch(idNumber, @"^\d{6}/\d{3}$");
            }
            else
            {
                // Pattern for cccccc/cccc
                return Regex.IsMatch(idNumber, @"^\d{6}/\d{4}$");
            }
        }
    }
}
