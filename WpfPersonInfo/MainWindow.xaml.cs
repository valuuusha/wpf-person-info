using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfPersonInfo.Service;
using WpfPersonInfo.ViewModel;

namespace WpfPersonInfo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var userService = new UserService();
            var dialogService = new DialogService();
            DataContext = new UserViewModel(userService, dialogService);
        }
    }

}