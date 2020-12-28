using GalaSoft.MvvmLight;
using StegoSystem.DesktopApp.ViewModels;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class PasswordUCVM : ViewModelBase, IValidatableUCVM
    {
        private string _password;
        private bool _isErrorStatus;

        public string Password
        {
            get => _password;
            set
            {
                //ToDo validation
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public bool IsErrorStatus
        {
            get => _isErrorStatus;
            set
            {
                _isErrorStatus = value;
                RaisePropertyChanged(nameof(IsErrorStatus));
            }
        }
    }
}
