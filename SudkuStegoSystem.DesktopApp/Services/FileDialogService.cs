namespace SudkuStegoSystem.DesktopApp.Services
{
    public class FileDialogService : IFileDialogService
    {       
        public string OpenFileDialog(string filter, string defaultPath = "")
        {
            var ofd = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = filter,
                InitialDirectory = defaultPath           
            };

            var result = ofd.ShowDialog();
            
            if (result == false)
                return string.Empty;

            return ofd.FileName;
        }
    }
}
