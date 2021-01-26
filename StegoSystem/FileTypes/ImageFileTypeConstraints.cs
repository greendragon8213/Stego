namespace StegoSystem
{
    public abstract class ImageFileTypeConstraints : FileTypeConstraints
    {
        public abstract int[] AllowedDepth { get; }
        public override FileTypes FileType => FileTypes.Images;
    }
}
