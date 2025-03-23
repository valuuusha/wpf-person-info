using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPersonInfo.Model
{
    public class UserModel
    {
        public DateTime BirthDate { get; set; }

        public int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public bool IsValidAge(int age)
        {
            return age >= 0 && age <= 135;
        }

        public bool IsBirthdayToday()
        {
            var today = DateTime.Today;
            return BirthDate.Month == today.Month && BirthDate.Day == today.Day;
        }

        public string GetWesternZodiac()
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

        public string GetChineseZodiac()
        {
            int year = BirthDate.Year;
            string[] zodiacs = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
            return zodiacs[(year - 4) % 12];
        }
    }
}
