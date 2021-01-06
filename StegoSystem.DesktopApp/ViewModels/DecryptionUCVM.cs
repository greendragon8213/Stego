using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StegoSystem.DesktopApp.Models;
using System;
using System.IO;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class DecryptionUCVM : ViewModelBase
    {
        private readonly Decryption _decryption;

        public DecryptionUCVM(
            Decryption decryption,
            DropFileUCVM dropStegoContainerFileVM,
            OutputPathUCVM outputPathVM,
            PasswordUCVM passwordVM)
        {
            _decryption = decryption;
            DropStegoContainerFileVM = dropStegoContainerFileVM;
            OutputPathVM = outputPathVM;
            PasswordVM = passwordVM;

            DecryptCommand = new RelayCommand(Decrypt);
        }

        public DropFileUCVM DropStegoContainerFileVM { get; }
        public OutputPathUCVM OutputPathVM { get; }
        public PasswordUCVM PasswordVM { get; }

        public RelayCommand DecryptCommand { get; set; }

        private async void Decrypt()
        {
            #region Validation

            string stegoContainerFilePath = DropStegoContainerFileVM.FilePath;
            if (!File.Exists(stegoContainerFilePath))
            {
                DropStegoContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Stegocontainer file does not exist", state: AppState.Error);
                return;
            }

            if (!_decryption.IsStegocontainerExtensionAllowed(stegoContainerFilePath))
            {
                DropStegoContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Wrong stegocontainer file extension", state: AppState.Error);
                return;
            }

            string outputPath = OutputPathVM.Path;
            if (!Directory.Exists(outputPath))
            {
                OutputPathVM.IsValid = false;
                StatusBarUCVM.UpdateState(text: "Output directory does not exist", state: AppState.Error);
                return;
            }

            var password = _decryption.CreatePassword(PasswordVM.Password);
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
                string filePath = await _decryption.Decrypt(stegoContainerFilePath, password, outputPath);

                StatusBarUCVM.UpdateState(text: "Decryption has been successfully done. Secret file is located: ",
                    localFilePath: filePath, state: AppState.Neutral);

                DropStegoContainerFileVM.IsValid = true;
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
