using System.Drawing;
using StegoSystem.GeneralLogic.Abstract;
using StegoSystem.GeneralLogic.Models;
using StegoSystem.SudokuMethodImplementation.Matrix;

namespace StegoSystem.SudokuMethodImplementation.Abstract
{
    public interface ISudokuStegoMethod
    {
        FileTypeConstraints ContainerFileConstraints { get; }
        FileTypeConstraints StegoContainerFileConstraints { get; }
        FileTypeConstraints SecretFileConstraints { get; }

        int GetExpectedSudokuSize();

        Bitmap Encrypt(Bitmap container, SecretFile secretFile, SudokuMatrix sudokuKey);
        SecretFile Decrypt(Bitmap stegocontainer, SudokuMatrix sudokuKey);
    }
}