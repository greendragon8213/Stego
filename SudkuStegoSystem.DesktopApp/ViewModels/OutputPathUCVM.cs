using MyToolkit.Command;
using SudkuStegoSystem.DesktopApp.Services;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class OutputPathUCVM : BaseVM
    {
        private readonly IFileDialogService _fileDialogService = new FileDialogService();
        private string _path;

        //ToDo remove this!
        public OutputPathUCVM()
        {
            //ToDo map default path
            Path = null;
            OpenCommand = new RelayCommand(OpenFile);
        }

        //ToDo remove this!
        public OutputPathUCVM(string defaultPath = null)
        {
            //ToDo map default path
            Path = defaultPath;
            OpenCommand = new RelayCommand(OpenFile);
        }

        //ToDo DI!
        public OutputPathUCVM(IFileDialogService fileDialogService) : this()
        {
            _fileDialogService = fileDialogService;
        }

        public string Path
        {            
            get { return _path; }
            set
            {
                //ToDo path validation
                _path = value;
                OnPropertyChanged(nameof(Path));
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
