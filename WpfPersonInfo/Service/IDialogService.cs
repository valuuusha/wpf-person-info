using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPersonInfo.ViewModel;

namespace WpfPersonInfo.Service
{
    public interface IDialogService
    {
        bool? ShowEditDialog(UserEditViewModel viewModel);
        bool ShowConfirmation(string message, string title);
        void ShowError(string message);
    }

}

