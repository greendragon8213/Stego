using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GongSolutions.Wpf.DragDrop;
using SudkuStegoSystem.DesktopApp.Services;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class DropFileUCVM : ViewModelBase, IDropTarget
    {
        private readonly IFileDialogService _fileDialogService;
        private string _filePath;

        public DropFileUCVM(IFileDialogService fileDialogService)
        {
            AllowedExtensions = new string[0];
            _fileDialogService = fileDialogService;
            OpenFileCommand = new RelayCommand(OpenFile);
        }

        //ToDo improve AllowedExtensions and file dialog filters
        //By default all extensions are allowed
        //public string[] AllowedExtensions => new string[] { ".jpg" };
        public string[] AllowedExtensions { get; set; }

        public bool IsFilePathProvided => !string.IsNullOrEmpty(FilePath);

        public string FilePath
        {
            get => _filePath;

            protected set
            {                
                if(IsFileExtensionAllowed(value))
                {
                    _filePath = value;
                    RaisePropertyChanged(nameof(IsFilePathProvided));
                    return;
                }

                throw new InvalidOperationException("wrong file type!");
                //ToDo show message or smth
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            var filePath = dragFileList.FirstOrDefault();

            if (IsFileExtensionAllowed(filePath))
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
            //ToDo file filter customization
            FilePath = _fileDialogService.OpenFileDialog(_fileDialogService.GetDefaultFilter());
        }

        private bool IsFileExtensionAllowed(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            return (AllowedExtensions.Count() == 0 
                || AllowedExtensions.Any(ae => extension.Equals(ae, StringComparison.InvariantCultureIgnoreCase)));            
        }

        #endregion
    }
}
