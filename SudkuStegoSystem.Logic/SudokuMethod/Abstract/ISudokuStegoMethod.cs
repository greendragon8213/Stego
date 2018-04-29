using System.Drawing;
using SudkuStegoSystem.Logic.Models;

namespace SudkuStegoSystem.Logic
{
    public interface ISudokuStegoMethod
    {
        int GetExpectedSudokuSize();
        Image Encrypt(Image container, SecretFile secretFile, SudokuMatrix sudokuKey);
        SecretFile Decrypt(Image stegocontainer, SudokuMatrix sudokuKey);
    }
}