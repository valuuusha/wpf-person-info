using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace WpfPersonInfo.Model
{
    [Serializable]
    public class Person : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate;
        private bool _isAdult;
        private string _westernSign;
        private string _chineseSign;
        private bool _isBirthday;

        public Person() { }

        [JsonConstructor]
        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            Validate();

            CalculateProperties();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.Today)
        {
        }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate)
        {
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged();
                CalculateProperties();
            }
        }

        public bool IsAdult
        {
            get => _isAdult;
            private set
            {
                _isAdult = value;
                OnPropertyChanged();
            }
        }

        public string WesternSign
        {
            get => _westernSign;
            private set
            {
                _westernSign = value;
                OnPropertyChanged();
            }
        }

        public string ChineseSign
        {
            get => _chineseSign;
            private set
            {
                _chineseSign = value;
                OnPropertyChanged();
            }
        }

        public bool IsBirthday
        {
            get => _isBirthday;
            private set
            {
                _isBirthday = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        private void CalculateProperties()
        {
            IsAdult = CalculateIsAdult();
            WesternSign = CalculateWesternSign();
            ChineseSign = CalculateChineseSign();
            IsBirthday = CalculateIsBirthday();
        }

        private bool CalculateIsAdult()
        {
            return Age >= 18;
        }

        private bool CalculateIsBirthday()
        {
            return BirthDate.Month == DateTime.Today.Month && BirthDate.Day == DateTime.Today.Day;
        }
        public void Validate()
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

        [JsonIgnore]
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Email);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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