using System.Linq;

namespace StegoSystem
{
    public abstract class ImageFileTypeConstraints : FileTypeConstraints
    {
        public abstract int[] AllowedDepth { get; }
        public override FileTypes FileType => FileTypes.Images;

        public bool IsDepthAllowed(int depth)
        {
            return AllowedDepth.Count() == 0
                || AllowedDepth.Any(d => d == depth);
        }
    }
}
