using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    public class StatusBarUCVM : ViewModelBase
    {
        public StatusBarUCVM()
        {
            ShowLocalFileInExplorerCommand = new RelayCommand(ShowLocalFileInExplorer);
        }

        public string Text { get; set; }
        public string LocalFilePath { get; set; }
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
