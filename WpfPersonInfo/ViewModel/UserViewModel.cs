using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfPersonInfo.Model;

namespace WpfPersonInfo.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private UserModel _userModel;
        private DateTime _birthDate;
        private string _age;
        private string _westernZodiac;
        private string _chineseZodiac;

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate == value) return; 

                _birthDate = value;
                _userModel.BirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                CalculateAge();
                CheckForBirthday();
            }
        }

        public string TbAge
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(TbAge));
            }
        }

        public string TbWesternZodiac
        {
            get => _westernZodiac;
            set
            {
                _westernZodiac = value;
                OnPropertyChanged(nameof(TbWesternZodiac));
            }
        }

        public string TbChineseZodiac
        {
            get => _chineseZodiac;
            set
            {
                _chineseZodiac = value;
                OnPropertyChanged(nameof(TbChineseZodiac));
            }
        }
        public UserViewModel()
        {
            _userModel = new UserModel();
            _age = string.Empty;
            _westernZodiac = string.Empty;
            _chineseZodiac = string.Empty; 
        }

            private void CalculateAge()
            {
                int age = _userModel.CalculateAge();
                Console.WriteLine("CalculateAge called");
                if (_userModel.IsValidAge(age))
                {
                TbAge = age.ToString();
                TbWesternZodiac = _userModel.GetWesternZodiac();
                TbChineseZodiac = _userModel.GetChineseZodiac();
            }
            else
            {
                MessageBox.Show("Invalid age. Please enter a valid birth date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckForBirthday()
        {
            if (_userModel.IsBirthdayToday())
            {
                MessageBox.Show("Happy Birthday!", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
