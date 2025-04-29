using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfPersonInfo.Command;
using WpfPersonInfo.Model;

namespace WpfPersonInfo.ViewModel
{
    public class UserEditViewModel : INotifyPropertyChanged
    {
        private Person _person;
        private string _windowTitle;
        private bool _isNewUser;
        private bool _isSaving;

        public event EventHandler<bool?> RequestClose;
        public Person ResultPerson => _person;

        public string FirstName
        {
            get => _person.FirstName;
            set
            {
                _person.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _person.LastName;
            set
            {
                _person.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _person.Email;
            set
            {
                _person.Email = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get => _person.BirthDate;
            set
            {
                _person.BirthDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Age));
                OnPropertyChanged(nameof(IsAdult));
                OnPropertyChanged(nameof(WesternSign));
                OnPropertyChanged(nameof(ChineseSign));
                OnPropertyChanged(nameof(IsBirthday));
            }
        }

        public int Age => _person.Age;
        public bool IsAdult => _person.IsAdult;
        public string WesternSign => _person.WesternSign;
        public string ChineseSign => _person.ChineseSign;
        public bool IsBirthday => _person.IsBirthday;

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public UserEditViewModel()
        {
            _person = new Person();
            _isNewUser = true;
            WindowTitle = "Add New User";

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public UserEditViewModel(Person person, bool createCopy = true)
        {
            if (createCopy)
            {
                _person = new Person(
                    person.FirstName,
                    person.LastName,
                    person.Email,
                    person.BirthDate);
            }
            else
            {
                _person = person;
            }

            _isNewUser = false;
            WindowTitle = "Edit User";

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave()
        {
            return !IsSaving &&
                   !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email);
        }   

        private async Task SaveAsync()
        {
            IsSaving = true;

            try
            {
                await Task.Run(() => _person.Validate());

                RequestClose?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
            }
        }

        private void Cancel()
        {
            RequestClose?.Invoke(this, false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(FirstName) ||
                propertyName == nameof(LastName) ||
                propertyName == nameof(Email))
            {
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
