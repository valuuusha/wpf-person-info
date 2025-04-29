using System.Windows;
using WpfPersonInfo.View;
using WpfPersonInfo.ViewModel;
using WpfPersonInfo.Service;

public class DialogService : IDialogService
{
    public bool? ShowEditDialog(UserEditViewModel viewModel)
    {
        var dialog = new UserEditWindow(viewModel)
        {
            Owner = Application.Current.MainWindow
        };
        return dialog.ShowDialog();
    }

    public bool ShowConfirmation(string message, string title)
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }

    public void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}



