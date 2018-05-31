using System;

namespace SudkuStegoSystem.DesktopApp.Services
{
    public class FolderDialogService : IFolderDialogService
    {
        public string OpenFolderDialog()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog()
            {
                ShowNewFolderButton = true
            };

            var result = dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK && result != System.Windows.Forms.DialogResult.Yes)
                return string.Empty;

            return dialog.SelectedPath;
        }
    }
}
