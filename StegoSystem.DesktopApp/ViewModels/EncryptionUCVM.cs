﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StegoSystem;
using System;

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
            try
            {
                string filePath = _stegoSystem.Encrypt(DropContainerFileVM.FilePath, DropSecretFileVM.FilePath, PasswordVM.Password,
                    OutputPathVM.Path);

                StatusBarUCVM.UpdateStatus(text: "Encryption has been successfully done. Stegocontainer is located: ",
                        localFilePath: filePath, isErrorStatus: false);

                DropContainerFileVM.IsValid = true;
                DropSecretFileVM.IsValid = true;
                PasswordVM.IsValid = true;
                OutputPathVM.IsValid = true;
            }
            catch (Exception e)//ToDo KeyException
            {
                DropContainerFileVM.IsValid = false;
                DropSecretFileVM.IsValid = false;
                PasswordVM.IsValid = false;
                OutputPathVM.IsValid = false;
                StatusBarUCVM.UpdateStatus(text: e.Message, isErrorStatus: true);
            }
            //catch (Exception e)
            //{
            //    StatusBarUCVM.UpdateStatus(text: e.Message, isErrorStatus: true);
            //}
        }
    }
}
