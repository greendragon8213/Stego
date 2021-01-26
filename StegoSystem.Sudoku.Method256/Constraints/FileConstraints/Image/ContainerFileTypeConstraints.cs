namespace StegoSystem.Sudoku.Method256.FileConstraints.Image
{
    public class ContainerFileTypeConstraints : ImageFileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.Images;
        public override string[] AllowedExtensions => new string[] { "bmp", "jpeg", "jpg", "png"};
        public override int[] AllowedDepth => new int[] { 24, 32 };
    }
}