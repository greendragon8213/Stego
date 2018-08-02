using StegoSystem.GeneralLogic.Abstract;
using StegoSystem.GeneralLogic.Common;

namespace StegoSystem.SudokuMethodImplementation.FileConstraints
{
    public class StegoContainerFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.Images;
        public override string[] AllowedExtensions => new string[] { "bmp" };
    }
}
