namespace SudkuStegoSystem.DesktopApp.Services
{
    public interface IFileDialogService
    {
        string GetImagesFilter();
        string GetDefaultFilter();

        string OpenFileDialog(string filter = null);
    }
}