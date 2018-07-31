using System.Drawing;
using SudkuStegoSystem.Logic.Abstract;
using SudkuStegoSystem.Logic.Models;

namespace SudkuStegoSystem.Logic
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