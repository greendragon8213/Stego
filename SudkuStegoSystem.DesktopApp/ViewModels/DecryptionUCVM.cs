﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SudkuStegoSystem.Logic.Abstract;

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
            //ToDo validation
            _stegoSystem.Decrypt(DropStegoContainerFileVM.FilePath, PasswordVM.Password, 
                OutputPathVM.Path);            
        }
    }
}
