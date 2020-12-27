namespace StegoSystem.Sudoku.FileConstraints
{
    public class ContainerFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.Images;
        public override string[] AllowedExtensions => new string[] { "bmp", "jpeg", "jpg", "png"};
    }
}