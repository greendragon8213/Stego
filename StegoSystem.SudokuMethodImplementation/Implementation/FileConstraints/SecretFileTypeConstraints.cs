using StegoSystem.GeneralLogic.Abstract;
using StegoSystem.GeneralLogic.Common;

namespace StegoSystem.SudokuMethodImplementation.FileConstraints
{
    public class SecretFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.AnyFiles;
        public override string[] AllowedExtensions => new string[0];
    }
}
