using GalaSoft.MvvmLight;
using MyToolkit.Command;
using SudkuStegoSystem.DesktopApp.Services;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class OutputPathUCVM : ViewModelBase
    {
        private readonly IFileDialogService _fileDialogService = new FileDialogService();
        private string _path;
        
        public OutputPathUCVM(string defaultPath, IFileDialogService fileDialogService)
        {
            Path = defaultPath;
            _fileDialogService = fileDialogService;
            OpenCommand = new RelayCommand(OpenFile);
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
        
        public RelayCommand OpenCommand
        {
            get; set;
        }

        #region Private methods

        private void OpenFile()
        {
            //ToDo customization
            Path = _fileDialogService.OpenFileDialog(_fileDialogService.GetDefaultFilter());            
        }

        #endregion
    }
}
