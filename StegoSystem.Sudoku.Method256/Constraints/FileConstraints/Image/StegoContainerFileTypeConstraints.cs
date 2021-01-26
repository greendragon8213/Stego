namespace StegoSystem.Sudoku.Method256.FileConstraints.Image
{
    public class StegoContainerFileTypeConstraints : ImageFileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.Images;
        public override string[] AllowedExtensions => new string[] { "bmp" };
        public override int[] AllowedDepth => new int[] { 24, 32 };
    }
}
