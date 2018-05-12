namespace SudkuStegoSystem.DesktopApp.Services
{
    public class FileDialogService : IFileDialogService
    {       
        public string OpenFileDialog(string filter)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = filter
            };

            var result = ofd.ShowDialog();

            //ToDo default path
            if (result == false) return "";
            return ofd.FileName;
        }
    }
}
