namespace SudkuStegoSystem.DesktopApp.Services
{
    public interface IFileDialogService
    {
        string OpenFileDialog(string filter, string defaultPath = "");
    }
}