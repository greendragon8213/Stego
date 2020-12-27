namespace StegoSystem.Sudoku.FileConstraints
{
    public class StegoContainerFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.Images;
        public override string[] AllowedExtensions => new string[] { "bmp" };
    }
}
