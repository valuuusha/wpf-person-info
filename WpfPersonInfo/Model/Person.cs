using System;

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
    }
}
