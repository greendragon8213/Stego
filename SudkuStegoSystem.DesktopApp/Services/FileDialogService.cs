using System;

namespace SudkuStegoSystem.DesktopApp.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string GetDefaultFilter()
        {
            return "All files |*.*";
        }

        public string GetImagesFilter()
        {
            return "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
    }

        public string OpenFileDialog(string filter = null)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = string.IsNullOrEmpty(filter) ? GetDefaultFilter() : filter
            };
            var result = ofd.ShowDialog();

            //ToDo default path
            if (result == false) return "";
            return ofd.FileName;
        }
    }
}
