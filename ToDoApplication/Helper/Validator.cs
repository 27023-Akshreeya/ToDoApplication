using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToDoApplication.Helper
{
    public delegate bool InputValidator(string input);
    public static class Validator
    {
        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.All(char.IsWhiteSpace) || password.Length < 8 || password.Length > 12)
            {
                return false;
            }

            string passwordPattern = @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).+$";
            return Regex.IsMatch(password, passwordPattern);
        }

        public static bool IsDateValid(string date)
        {
            return DateOnly.TryParse(date, out var validDate) && validDate > DateOnly.FromDateTime(DateTime.Now);
        }

        internal static bool IslogoutValid(string choice)
        {
            if (string.IsNullOrWhiteSpace(choice) || choice.Length != 1)
            {
                return false;
            }

            return choice.Equals("n", StringComparison.OrdinalIgnoreCase) ||
                     choice.Equals("y", StringComparison.OrdinalIgnoreCase);
        }
    }
}
