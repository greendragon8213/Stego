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

        private void Decrypt()
        {
            #region Validation

            string stegoContainerFilePath = DropStegoContainerFileVM.FilePath;
            if (!File.Exists(stegoContainerFilePath))
            {
                DropStegoContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Stegocontainer file does not exist", isErrorStatus: true);
                return;
            }

            if (!_decryption.IsStegocontainerExtensionAllowed(stegoContainerFilePath))
            {
                DropStegoContainerFileVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Wrong stegocontainer file extension", isErrorStatus: true);
                return;
            }

            string outputPath = OutputPathVM.Path;
            if (!Directory.Exists(outputPath))
            {
                OutputPathVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: "Output directory does not exist", isErrorStatus: true);
                return;
            }

            var password = _decryption.CreatePassword(PasswordVM.Password);
            if (!password.IsValid())
            {
                PasswordVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: password.ValidationDescription, isErrorStatus: true);
                return;
            }

            #endregion

            try
            {
                string filePath = _decryption.Decrypt(stegoContainerFilePath, password, outputPath);

                StatusBarUCVM.UpdateStatus(text: "Decryption has been successfully done. Secret file is located: ",
                    localFilePath: filePath, isErrorStatus: false);

                DropStegoContainerFileVM.IsValid = true;
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
