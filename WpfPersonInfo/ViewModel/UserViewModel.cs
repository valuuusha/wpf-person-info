using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfPersonInfo.Command;
using WpfPersonInfo.Model;

namespace WpfPersonInfo.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate = DateTime.Today;

        private string _age;
        private string _isAdult;
        private string _westernSign;
        private string _chineseSign;
        private string _isBirthday;

        private bool _proceedEnabled = true;
        public bool ProceedEnabled
        {
            get => _proceedEnabled;
            set
            {
                _proceedEnabled = value;
                OnPropertyChanged();
                UpdateCommandState(); 
            }
        }


        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged();
                    UpdateCommandState();
                }
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                    UpdateCommandState();
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                    UpdateCommandState();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                    UpdateCommandState();
                }
            }
        }

        public string Age
        {
            get => _age;
            private set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        public string IsAdult
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

        public string IsBirthday
        {
            get => _isBirthday;
            private set
            {
                _isBirthday = value;
                OnPropertyChanged();
            }
        }

        public ICommand ProceedCommand { get; }

        public UserViewModel()
        {
            ProceedCommand = new RelayCommand(async () => await OnProceedAsync(), CanProceed);
        }

        private bool CanProceed()
        {
            return !string.IsNullOrWhiteSpace(FirstName)
                && !string.IsNullOrWhiteSpace(LastName)
                && !string.IsNullOrWhiteSpace(Email)
                && BirthDate != default;
        }
        private async Task OnProceedAsync()
        {
            ProceedEnabled = false;

            try
            {
                await Task.Delay(1000);
                await Task.Run(() =>
                {
                    try
                    {
                        var person = new Person(FirstName, LastName, Email, BirthDate);

                        Age = CalculateAge(BirthDate).ToString();
                        IsAdult = person.IsAdult ? "Yes" : "No";
                        WesternSign = person.WesternSign;
                        ChineseSign = person.ChineseSign;
                        IsBirthday = person.IsBirthday ? "Yes" : "No";

                        if (person.IsBirthday)
                        {
                            MessageBox.Show("Happy Birthday!", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                });
            }
            finally
            {
                ProceedEnabled = true;
            }
        }


        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        private void UpdateCommandState()
        {
            if (ProceedCommand is RelayCommand relayCommand)
            {
                relayCommand.RaiseCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
