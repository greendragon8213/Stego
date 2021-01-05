using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StegoSystem.DesktopApp.Models;
using System;
using System.IO;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class EncryptionUCVM : ViewModelBase
    {
        private readonly Encryption _encryption;

        public EncryptionUCVM(
            Encryption encryption,
            DropFileUCVM dropSecretFileVM,
            DropFileUCVM dropContainerFileVM,
            OutputPathUCVM outputPathVM,
            PasswordUCVM passwordVM)
        {
            _encryption = encryption;
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

        public RelayCommand EncryptCommand { get; set; }

        private void Encrypt()
        {
            #region Validation

            string containerFilePath = DropContainerFileVM.FilePath;
            if (!File.Exists(containerFilePath))
            {
                DropContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Container file does not exist", isErrorStatus: true);
                return;
            }

            if (!_encryption.IsContainerExtensionAllowed(containerFilePath))
            {
                DropContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Wrong container file extension", isErrorStatus: true);
                return;
            }
            
            string secretFilePath = DropSecretFileVM.FilePath;
            if (!File.Exists(secretFilePath))
            {
                DropSecretFileVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Secret file does not exist", isErrorStatus: true);
                return;
            }

            if (!_encryption.IsSecretExtensionAllowed(secretFilePath))
            {
                DropSecretFileVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Wrong secret file extension", isErrorStatus: true);
                return;
            }

            string outputPath = OutputPathVM.Path;
            if (!Directory.Exists(outputPath))
            {
                OutputPathVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Output directory does not exist", isErrorStatus: true);
                return;
            }

            var password =_encryption.CreatePassword(PasswordVM.Password);
            if (!password.IsValid())
            {
                PasswordVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: password.ValidationDescription, isErrorStatus: true);
                return;
            }

            #endregion

            try
            {
                string filePath = _encryption.Encrypt(containerFilePath, secretFilePath, password, outputPath);

                StatusBarUCVM.UpdateStatus(text: "Encryption has been successfully done. Stegocontainer is located: ",
                        localFilePath: filePath, isErrorStatus: false);

                DropContainerFileVM.IsValid = true;
                DropSecretFileVM.IsValid = true;
                PasswordVM.IsValid = true;
                OutputPathVM.IsValid = true;
            }
            catch (Exception e) when (e is InvalidOperationException || e is ArgumentException)
            {
                StatusBarUCVM.UpdateStatus(text: e.Message, isErrorStatus: true);
            }
            catch (Exception e)
            {
                StatusBarUCVM.UpdateStatus(text: "Something went wrong. Try again", isErrorStatus: true);
            }
        }
    }
}
