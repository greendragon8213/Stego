using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GongSolutions.Wpf.DragDrop;
using StegoSystem;
using StegoSystem.DesktopApp.ViewModels;
using SudkuStegoSystem.DesktopApp.Services;
using System;
using System.Linq;
using System.Windows;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class DropFileUCVM : ViewModelBase, IDropTarget, IValidatable
    {
        private readonly IFileDialogService _fileDialogService;
        private string _filePath;
        private bool? _isValid = null;

        private FileTypes _fileType;
        private readonly FileTypeConstraints _fileTypeConstraints;
        
        public DropFileUCVM(IFileDialogService fileDialogService, FileTypeConstraints fileTypeConstraints, 
            FileTypes fileType = FileTypes.File)
        {
            _fileTypeConstraints = fileTypeConstraints;
            _fileDialogService = fileDialogService;
            _fileType = fileType;
            OpenFileCommand = new RelayCommand(OpenFile);
        }

        public FileTypes FileType => _fileType;

        public bool IsFilePathProvided => !string.IsNullOrEmpty(FilePath);

        public string FilePath
        {
            get => _filePath;

            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (_fileTypeConstraints.IsFileExtensionAllowedByPath(value))
                {
                    _filePath = value;
                    RaisePropertyChanged(nameof(IsFilePathProvided));
                    IsValid = null;
                    return;
                }

                throw new InvalidOperationException("wrong file type!");
            }
        }

        public bool? IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            var filePath = dragFileList.FirstOrDefault();

            if (_fileTypeConstraints.IsFileExtensionAllowedByPath(filePath))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
            else
            {
                dropInfo.Effects = DragDropEffects.None;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            FilePath = dragFileList.FirstOrDefault();
        }

        public RelayCommand OpenFileCommand { get; set; }

        #region Private methods

        private void OpenFile()
        {
            FilePath = _fileDialogService.OpenFileDialog(_fileTypeConstraints.GetFilter());
        }
        
        #endregion
    }
}
