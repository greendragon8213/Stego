using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StegoSystem;
using System;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class DecryptionUCVM : ViewModelBase
    {
        private readonly IStegoSystem _stegoSystem;
                
        public DecryptionUCVM(IStegoSystem stegoSystem,
            DropFileUCVM dropStegoContainerFileVM, 
            OutputPathUCVM outputPathVM, PasswordUCVM passwordVM)
        {
            _stegoSystem = stegoSystem;
            DropStegoContainerFileVM = dropStegoContainerFileVM;
            OutputPathVM = outputPathVM;
            PasswordVM = passwordVM;

            DecryptCommand = new RelayCommand(Decrypt);
        }
        
        public DropFileUCVM DropStegoContainerFileVM { get; }
        public OutputPathUCVM OutputPathVM { get; }
        public PasswordUCVM PasswordVM { get; }

        public RelayCommand DecryptCommand { get; set;}

        private void Decrypt()
        {
            try
            {
                string filePath = _stegoSystem.Decrypt(DropStegoContainerFileVM.FilePath, PasswordVM.Password,
                    OutputPathVM.Path);

                StatusBarUCVM.UpdateStatus(text: "Decryption has been successfully done. Secret file is located: ", 
                    localFilePath: filePath, isErrorStatus: false);
                
                DropStegoContainerFileVM.IsErrorStatus = false;
                PasswordVM.IsErrorStatus = false;
                OutputPathVM.IsErrorStatus = false;
            }
            catch (Exception e)//ToDo KeyException
            {
                DropStegoContainerFileVM.IsErrorStatus = true;
                PasswordVM.IsErrorStatus = true;
                OutputPathVM.IsErrorStatus = true;
                StatusBarUCVM.UpdateStatus(text: e.Message, isErrorStatus: true);
            }
            //catch (Exception e)
            //{
            //    StatusBarUCVM.UpdateStatus(text: e.Message, isErrorStatus: true);
            //}
        }
    }
}
