using System.Windows;
using WpfPersonInfo.Service;
using WpfPersonInfo.ViewModel;

namespace WpfPersonInfo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var userService = new UserService();
            var dialogService = new DialogService();
            var viewModel = new UserViewModel(userService, dialogService);
            DataContext = viewModel;
            await viewModel.InitializeAsync();
        }
    }
}
