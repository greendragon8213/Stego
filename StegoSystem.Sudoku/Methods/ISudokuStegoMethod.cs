using StegoSystem.Models;
using StegoSystem.Sudoku.Matrix;
using System.Drawing;

namespace StegoSystem.Sudoku
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