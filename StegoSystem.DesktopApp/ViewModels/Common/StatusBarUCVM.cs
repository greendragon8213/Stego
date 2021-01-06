using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public enum AppState
    {
        Neutral,
        Error,
        Working
    }

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

        public AppState State {get; private set;}
        
        public static void UpdateState(string text = "", string localFilePath = "", AppState state = AppState.Neutral)
        {
            if (_instance == null)
            {
                GetInstance();
            }

            lock (_locker)
            {
                if(state == AppState.Working && string.IsNullOrEmpty(text))
                {
                    text = "Working...";
                }

                _instance.Text = text;
                _instance.LocalFilePath = localFilePath;
                _instance.State = state;

                _instance.RaisePropertyChanged(nameof(Text));
                _instance.RaisePropertyChanged(nameof(LocalFilePath));
                _instance.RaisePropertyChanged(nameof(State));
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
