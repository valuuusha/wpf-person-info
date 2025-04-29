using System.Windows;
using WpfPersonInfo.View;
using WpfPersonInfo.ViewModel;
using WpfPersonInfo.Service;

public class DialogService : IDialogService
{
    public bool? ShowEditDialog(UserEditViewModel viewModel)
    {
        var window = new UserEditWindow();
        window.DataContext = viewModel;
        window.Owner = Application.Current.MainWindow;

        viewModel.RequestClose += (sender, result) =>
        {
            window.DialogResult = result;
            window.Close();
        };

        return window.ShowDialog();
    }

}
