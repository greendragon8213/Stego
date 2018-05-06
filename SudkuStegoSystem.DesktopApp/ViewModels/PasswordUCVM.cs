using GalaSoft.MvvmLight;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class PasswordUCVM : ViewModelBase
    {
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                //ToDo validation
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
    }
}
