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

        private async void Encrypt()
        {
            #region Validation

            string containerFilePath = DropContainerFileVM.FilePath;
            if (!File.Exists(containerFilePath))
            {
                DropContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Container file does not exist", state: AppState.Error);
                return;
            }

            if (!_encryption.IsContainerExtensionAllowed(containerFilePath))
            {
                DropContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Wrong container file extension", state: AppState.Error);
                return;
            }
            
            string secretFilePath = DropSecretFileVM.FilePath;
            if (!File.Exists(secretFilePath))
            {
                DropSecretFileVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Secret file does not exist", state: AppState.Error);
                return;
            }

            if (!_encryption.IsSecretExtensionAllowed(secretFilePath))
            {
                DropSecretFileVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Wrong secret file extension", state: AppState.Error);
                return;
            }

            string outputPath = OutputPathVM.Path;
            if (!Directory.Exists(outputPath))
            {
                OutputPathVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Output directory does not exist", state: AppState.Error);
                return;
            }

            var password = _encryption.CreatePassword(PasswordVM.Password);
            if (!password.IsValid())
            {
                PasswordVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: password.ValidationDescription, state: AppState.Error);
                return;
            }

            #endregion

            StatusBarUCVM.UpdateState(state: AppState.Working);

            try
            {
                string filePath = await _encryption.Encrypt(containerFilePath, secretFilePath, password, outputPath);

                StatusBarUCVM.UpdateState(text: "Encryption has been successfully done. Stegocontainer is located: ",
                        localFilePath: filePath, state: AppState.Neutral);

                DropContainerFileVM.IsValid = true;
                DropSecretFileVM.IsValid = true;
                PasswordVM.IsValid = true;
                OutputPathVM.IsValid = true;
            }
            catch (Exception e) when (e is InvalidOperationException || e is ArgumentException)
            {
                StatusBarUCVM.UpdateState(text: e.Message, state: AppState.Error);
            }
            catch (Exception e)
            {
                StatusBarUCVM.UpdateState(text: "Something went wrong. Try again", state: AppState.Error);
            }
        }
    }
}
