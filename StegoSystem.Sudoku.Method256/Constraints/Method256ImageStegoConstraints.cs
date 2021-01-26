using StegoSystem.Constraints;
using StegoSystem.Sudoku.Method256.FileConstraints;
using StegoSystem.Sudoku.Method256.FileConstraints.Image;

namespace StegoSystem.Sudoku.Method256.Constraints
{
    /// <summary>
    /// Recommended constraints for stegosystem that uses Sudoku Stego Method 256 and images as (stego) container
    /// </summary>
    public class Method256ImageStegoConstraints : ImageStegoConstraints
    {
        public ImageFileTypeConstraints ContainerFileConstraints => new ContainerFileTypeConstraints();

        public ImageFileTypeConstraints StegoContainerFileConstraints => new StegoContainerFileTypeConstraints();

        public FileTypeConstraints SecretFileConstraints => new SecretFileTypeConstraints();
    }
}
