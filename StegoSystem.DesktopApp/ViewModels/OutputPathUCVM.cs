﻿using GalaSoft.MvvmLight;
using MyToolkit.Command;
using StegoSystem.DesktopApp.ViewModels;
using SudkuStegoSystem.DesktopApp.Services;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class OutputPathUCVM : ViewModelBase, IValidatableUCVM
    {
        private readonly IFolderDialogService _folderDialogService;
        private string _path;
        private bool _isErrorStatus;

        public OutputPathUCVM(string defaultPath, IFolderDialogService folderDialogService)
        {
            Path = defaultPath;
            _folderDialogService = folderDialogService;
            OpenCommand = new RelayCommand(OpenFolder);
        }

        public string Path
        {            
            get { return _path; }
            set
            {
                //ToDo path validation
                _path = value;
                RaisePropertyChanged(nameof(Path));
            }
        }

        public bool IsErrorStatus
        {
            get => _isErrorStatus;
            set
            {
                _isErrorStatus = value;
                RaisePropertyChanged(nameof(IsErrorStatus));
            }
        }

        public RelayCommand OpenCommand { get; set; }

        #region Private methods

        private void OpenFolder()
        {
            Path = _folderDialogService.OpenFolderDialog();            
        }

        #endregion
    }
}
