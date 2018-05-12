using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SudkuStegoSystem.Logic.Abstract;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class EncryptionUCVM : ViewModelBase
    {
        private readonly IStegoSystem _stegoSystem;
                
        public EncryptionUCVM(IStegoSystem stegoSystem,
            DropFileUCVM dropSecretFileVM, DropFileUCVM dropContainerFileVM, 
            OutputPathUCVM outputPathVM, PasswordUCVM passwordVM)
        {
            _stegoSystem = stegoSystem;
            DropSecretFileVM = dropSecretFileVM;
            DropContainerFileVM = dropContainerFileVM;
            OutputPathVM = outputPathVM;
            PasswordVM = passwordVM;

            EncryptCommand = new RelayCommand(Encrypt);
        }

        public DropFileUCVM DropSecretFileVM { get; }
        public DropFileUCVM DropContainerFileVM { get; }
        public OutputPathUCVM OutputPathVM { get; }
        public PasswordUCVM PasswordVM { get; }

        public RelayCommand EncryptCommand { get; set;}

        private void Encrypt()
        {
            //ToDo validation
            _stegoSystem.Encrypt(DropContainerFileVM.FilePath, DropSecretFileVM.FilePath, PasswordVM.Password, 
                OutputPathVM.Path);            
        }
    }
}
