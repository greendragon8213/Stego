using GalaSoft.MvvmLight;
using StegoSystem.DesktopApp.ViewModels;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class PasswordUCVM : ViewModelBase, IValidatable
    {
        private string _password;
        private bool? _isValid = null;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
                IsValid = null;
            }
        }

        public bool? IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                RaisePropertyChanged(nameof(IsValid));
            }
        }
    }
}
