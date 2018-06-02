using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    //Should be like monostate, but some issues with binding to static
    public class StatusBarUCVM : ViewModelBase
    {
        private static StatusBarUCVM _instance;
        private static object _locker = new object();

        private StatusBarUCVM()
        {
            ShowLocalFileInExplorerCommand = new RelayCommand(ShowLocalFileInExplorer);
        }

        public static StatusBarUCVM GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new StatusBarUCVM();
                    }
                }
            }

            return _instance;
        }
        
        public string Text { get; private set; }
        public string LocalFilePath { get; private set; }
        public bool IsErrorStatus {get; private set;}

        public static void UpdateStatus(string text, string localFilePath = "", bool isErrorStatus = false)
        {
            lock (_locker)
            {
                _instance.Text = text;
                _instance.LocalFilePath = localFilePath;
                _instance.IsErrorStatus = isErrorStatus;

                _instance.RaisePropertyChanged(nameof(Text));
                _instance.RaisePropertyChanged(nameof(LocalFilePath));
                _instance.RaisePropertyChanged(nameof(IsErrorStatus));
            }
        }

        public RelayCommand ShowLocalFileInExplorerCommand {get; set;}

        #region Private methods

        private void ShowLocalFileInExplorer()
        {
            if (!File.Exists(LocalFilePath))
            {
                //ToDo message or smth
                return;
            }
            
            string argument = "/select,\"" + LocalFilePath + "\"";

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        #endregion
    }
}
