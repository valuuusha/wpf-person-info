using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPersonInfo.Service
{
    public interface IDialogService
    {
        bool? ShowEditDialog(ViewModel.UserEditViewModel viewModel);
    }
}

