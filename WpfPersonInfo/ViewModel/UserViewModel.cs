using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfPersonInfo.Command;
using WpfPersonInfo.Model;
using WpfPersonInfo.Service;
using WpfPersonInfo.View;

namespace WpfPersonInfo.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService;
        private readonly IDialogService _dialogService;
        private ObservableCollection<Person> _persons;
        private Person _selectedPerson;
        private string _filterText;
        private string _selectedFilterProperty;
        private ICollectionView _personsView;

        public ObservableCollection<Person> Persons
        {
            get => _persons;
            set
            {
                _persons = value;
                OnPropertyChanged();
                SetupCollectionView();
            }
        }

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
                ((RelayCommand)EditUserCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteUserCommand).RaiseCanExecuteChanged();
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                _personsView?.Refresh();
            }
        }

        public string[] FilterProperties { get; } = {
            "FirstName", "LastName", "Email", "BirthDate", "Age", "IsAdult", "WesternSign", "ChineseSign", "IsBirthday"
        };

        public string SelectedFilterProperty
        {
            get => _selectedFilterProperty;
            set
            {
                _selectedFilterProperty = value;
                OnPropertyChanged();
                _personsView?.Refresh();
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public UserViewModel(UserService userService,IDialogService dialogService)
        {
            _userService = userService;
            _dialogService = dialogService;
            _selectedFilterProperty = FilterProperties[0];

            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser, () => SelectedPerson != null);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => SelectedPerson != null);
            ClearFilterCommand = new RelayCommand(ClearFilter);

            LoadData();
        }


        private async void LoadData()
        {
            try
            {
                Persons = await _userService.LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SetupCollectionView()
        {
            _personsView = CollectionViewSource.GetDefaultView(Persons);
            _personsView.Filter = PersonFilter;
        }

        private bool PersonFilter(object item)
        {
            if (string.IsNullOrEmpty(FilterText))
                return true;

            var person = item as Person;
            if (person == null)
                return false;

            switch (SelectedFilterProperty)
            {
                case "FirstName":
                    return person.FirstName.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "LastName":
                    return person.LastName.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "Email":
                    return person.Email.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "BirthDate":
                    return person.BirthDate.ToString("dd.MM.yyyy").Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "Age":
                    return person.Age.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "IsAdult":
                    return person.IsAdult.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "WesternSign":
                    return person.WesternSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "ChineseSign":
                    return person.ChineseSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                case "IsBirthday":
                    return person.IsBirthday.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                default:
                    return true;
            }
        }

        private async void AddUser()
        {
            var viewModel = new UserEditViewModel();
            bool? result = _dialogService.ShowEditDialog(viewModel);

            if (result == true)
            {
                Persons.Add(viewModel.ResultPerson);
                await _userService.SaveUsersAsync(Persons);
            }
        }

        private async void EditUser()
        {
            if (SelectedPerson == null)
                return;

            var viewModel = new UserEditViewModel(SelectedPerson, true);
            bool? result = _dialogService.ShowEditDialog(viewModel);

            if (result == true)
            {
                int index = Persons.IndexOf(SelectedPerson);
                if (index >= 0)
                {
                    Persons[index] = viewModel.ResultPerson;
                    await _userService.SaveUsersAsync(Persons);
                    _personsView.Refresh();
                }
            }
        }



        private async void DeleteUser()
        {
            if (SelectedPerson == null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedPerson.FirstName} {SelectedPerson.LastName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Persons.Remove(SelectedPerson);
                await _userService.SaveUsersAsync(Persons);
            }
        }

        private void ClearFilter()
        {
            FilterText = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}