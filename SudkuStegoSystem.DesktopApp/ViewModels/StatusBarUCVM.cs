﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    //Should be like monostate
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

        public static string Text { get; private set; }
        public static string LocalFilePath { get; private set; }
        public static bool IsErrorStatus {get; private set;}

        public static void UpdateStatus(string text, string localFilePath = "", bool isErrorStatus = false)
        {
            lock (_locker)
            {
                Text = text;
                LocalFilePath = localFilePath;
                IsErrorStatus = isErrorStatus;

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
