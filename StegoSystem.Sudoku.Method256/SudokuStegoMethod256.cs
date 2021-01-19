using StegoSystem.Sudoku.Matrix;
using System;

namespace StegoSystem.Sudoku.Method256
{
    /// <summary>
    /// Embeds and extracts data based on steganography method with 256x256 matrix
    /// </summary>
    public class SudokuStegoMethod256 : ISudokuStegoMethod<byte>
    {
        public int GetExpectedSudokuSize() => 256;

        public void EmbedSecretData(byte[] containerBytes, byte[] secretBytes, SudokuMatrix<byte> sudokuKey)
        {
            if (containerBytes == null || containerBytes.Length == 0 || secretBytes == null || secretBytes.Length == 0)
                throw new ArgumentException();

            if (secretBytes.Length * 2 >= containerBytes.Length)
            {
                throw new InvalidOperationException("Cannot encrypt secret data because container image is too small");
            }

            ValidateSudoku(sudokuKey);

            for (int i = 0, secretDataIterator = 0;
                i + 1 < containerBytes.Length && secretDataIterator < secretBytes.Length;
                i += 2, secretDataIterator++)
            {
                byte currentByte = secretBytes[secretDataIterator];
                SudokuCoordinates initialCoordinates = new SudokuCoordinates(containerBytes[i], containerBytes[i + 1]);
                SudokuCoordinates nearestCoordinates = sudokuKey.FindNearestCoordinates(currentByte, initialCoordinates);

                if (initialCoordinates != nearestCoordinates)
                {
                    containerBytes[i] = nearestCoordinates.X;
                    containerBytes[i + 1] = nearestCoordinates.Y;
                }
            }
        }

        public byte[] ExtractSecretData(int bytesAmountToExtract, ref int stegocontainerOffset, byte[] stegocontainerBytes, SudokuMatrix<byte> sudokuKey)
        {
            if (bytesAmountToExtract <= 0 || stegocontainerOffset < 0 || stegocontainerBytes == null || stegocontainerBytes.Length == 0)
                throw new ArgumentException();

            ValidateSudoku(sudokuKey);

            int stegoIterator = stegocontainerOffset;

            byte[] secretData = new byte[bytesAmountToExtract];

            for (int i = 0; i < bytesAmountToExtract; stegoIterator += 2, i++)
            {
                secretData[i] = sudokuKey[stegocontainerBytes[stegoIterator], stegocontainerBytes[stegoIterator + 1]];
            }

            stegocontainerOffset = stegoIterator;
            return secretData;
        }
        
        #region Private methods

        private void ValidateSudoku(SudokuMatrix<byte> sudokuKey)
        {
            if (sudokuKey == null)
            {
                throw new ArgumentException();
            }

            if (sudokuKey.SudokuSize != GetExpectedSudokuSize())
            {
                throw new ArgumentException($"This steganography method works only with matrix {GetExpectedSudokuSize()}x{GetExpectedSudokuSize()}.");
            }

            //ToDo mb, other validation
        }

        #endregion
    }
}