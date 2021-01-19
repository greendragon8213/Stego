using StegoSystem.Sudoku.Matrix;

namespace StegoSystem.Sudoku
{
    public interface ISudokuStegoMethod<T>
    {
        int GetExpectedSudokuSize();

        void EmbedSecretData(byte[] containerBytes, byte[] secretBytes, SudokuMatrix<T> sudokuKey);
        byte[] ExtractSecretData(int bytesAmountToExtract, ref int stegocontainerOffset, byte[] stegocontainerBytes, SudokuMatrix<T> sudokuKey);
    }
}