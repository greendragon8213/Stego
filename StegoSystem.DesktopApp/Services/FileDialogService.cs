namespace SudkuStegoSystem.DesktopApp.Services
{
    public class FileDialogService : IFileDialogService
    {       
        public string OpenFileDialog(string filter, string defaultPath = "")
        {
            var dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = filter,
                InitialDirectory = defaultPath           
            };

            var result = dialog.ShowDialog();
            
            if (result == false)
                return string.Empty;

            return dialog.FileName;
        }
    }
}
