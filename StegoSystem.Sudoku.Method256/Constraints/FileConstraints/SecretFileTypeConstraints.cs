namespace StegoSystem.Sudoku.Method256.FileConstraints
{
    public class SecretFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.AnyFiles;
        public override string[] AllowedExtensions => new string[0];
    }
}
