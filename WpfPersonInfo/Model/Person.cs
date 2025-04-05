using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using WpfPersonInfo.ViewModel;

namespace WpfPersonInfo.Model
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDate { get; }

        private readonly bool _isAdult;
        private readonly string _westernSign;
        private readonly string _chineseSign;
        private readonly bool _isBirthday;

        public bool IsAdult => _isAdult;
        public string WesternSign => _westernSign;
        public string ChineseSign => _chineseSign;
        public bool IsBirthday => _isBirthday;

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            Validate();

            _isAdult = CalculateIsAdult();
            _westernSign = CalculateWesternSign();
            _chineseSign = CalculateChineseSign();
            _isBirthday = CalculateIsBirthday();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.Today)
        {
        }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate)
        {
        }

        private bool CalculateIsAdult()
        {
            var age = DateTime.Today.Year - BirthDate.Year;
            if (BirthDate > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }

        private bool CalculateIsBirthday()
        {
            return BirthDate.Month == DateTime.Today.Month && BirthDate.Day == DateTime.Today.Day;
        }
        private void Validate()
        {
            if (!IsValidName(FirstName))
                throw new ArgumentException("Invalid first name format (must be 2 to 50 characters).");

            if (!IsValidName(LastName))
                throw new ArgumentException("Invalid last name format(must be 2 to 50 characters).");

            if (!IsValidEmail(Email))
                throw new InvalidEmailException();

            if (BirthDate > DateTime.Today)
                throw new FutureBirthDateException();

            if (BirthDate < DateTime.Today.AddYears(-135))
                throw new TooOldBirthDateException();
        }

        private string CalculateWesternSign()
        {
            int month = BirthDate.Month;
            int day = BirthDate.Day;

            if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
                return "Aries";
            if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
                return "Taurus";
            if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
                return "Gemini";
            if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
                return "Cancer";
            if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
                return "Leo";
            if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
                return "Virgo";
            if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
                return "Libra";
            if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
                return "Scorpio";
            if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
                return "Sagittarius";
            if ((month == 12 && day >= 22) || (month == 1 && day <= 19))
                return "Capricorn";
            if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
                return "Aquarius";
            if ((month == 2 && day >= 19) || (month == 3 && day <= 20))
                return "Pisces";

            return "Unknown";
        }
        private string CalculateChineseSign()
        {
            int year = BirthDate.Year;
            string[] zodiacs = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
            return zodiacs[(year - 4) % 12];
        }
        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (name.Length < 2 || name.Length > 50) return false;

            return Regex.IsMatch(name, @"^[\p{L}\-'’ ]+$");
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        }
    }
    public class FutureBirthDateException : Exception
    {
        public FutureBirthDateException()
            : base("Birth date cannot be in the future.") { }
    }

    public class TooOldBirthDateException : Exception
    {
        public TooOldBirthDateException()
            : base("Birth date is outside valid range for living persons.") { }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
            : base("Invalid email format") { }
    }
}
