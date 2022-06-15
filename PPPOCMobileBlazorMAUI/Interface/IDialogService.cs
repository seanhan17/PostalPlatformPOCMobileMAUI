using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPPOCMobileBlazorMAUI.Interface
{
    internal interface IDialogService
    {
        Task<bool> DisplayConfirm(string title, string message, string accept, string cancel);
        Task DisplayAlert(string title, string message, string accept);
    }
}
