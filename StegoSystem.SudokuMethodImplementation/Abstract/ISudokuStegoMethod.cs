using System.Drawing;
using StegoSystem.GeneralLogic.Abstract;
using StegoSystem.GeneralLogic.Models;
using StegoSystem.SudokuMethodImplementation.Matrix;

namespace StegoSystem.SudokuMethodImplementation.Abstract
{
    public interface ISudokuStegoMethod<T>
    {
        FileTypeConstraints ContainerFileConstraints { get; }
        FileTypeConstraints StegoContainerFileConstraints { get; }
        FileTypeConstraints SecretFileConstraints { get; }

        int GetExpectedSudokuSize();

        Bitmap Encrypt(Bitmap container, SecretFile secretFile, SudokuMatrix<T> sudokuKey);
        SecretFile Decrypt(Bitmap stegocontainer, SudokuMatrix<T> sudokuKey);
    }
}