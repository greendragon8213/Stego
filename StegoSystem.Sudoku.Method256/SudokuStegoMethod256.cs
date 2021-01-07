using StegoSystem.Common.Extensions;
using StegoSystem.Models;
using StegoSystem.Sudoku.FileConstraints;
using StegoSystem.Sudoku.Matrix;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace StegoSystem.Sudoku.Method256
{
    /// <summary>
    /// Just encrypts and decrypts data
    /// </summary>
    public class SudokuStegoMethod256 : ISudokuStegoMethod<byte>
    {
        public FileTypeConstraints ContainerFileConstraints => new ContainerFileTypeConstraints();
        public FileTypeConstraints StegoContainerFileConstraints => new StegoContainerFileTypeConstraints();
        public FileTypeConstraints SecretFileConstraints => new SecretFileTypeConstraints();

        public int GetExpectedSudokuSize() => 256;

        public Bitmap Encrypt(Bitmap container, SecretFile secretFile, SudokuMatrix<byte> sudokuKey)
        {
            ValidateSudoku(sudokuKey);

            if (!ContainerFileConstraints.IsFileExtensionAllowed(container.GetImageExtension()))
            {
                throw new ArgumentException("Container file extension not allowed", nameof(container));
            }

            if (!SecretFileConstraints.IsFileExtensionAllowedByPath(secretFile.FileName))
            {
                throw new ArgumentException("Secret file extension not allowed", nameof(secretFile));
            }

            Tuple<byte[], BitmapData> cover = container.GetByteArrayByImageFile(ImageLockMode.ReadWrite);
            byte[] coverBytes = cover.Item1;
            BitmapData coverBitmap = cover.Item2;
            byte[] secretData = GetSecretBytesToEncode(secretFile);

            if (secretData.Length * 2 >= coverBytes.Length)
            {
                throw new InvalidOperationException("Cannot encrypt secret data because container image is too small");
            }

            EmbedSecretBytesToContainer(coverBytes, secretData, sudokuKey);

            container.UpdateBitmapPayloadBytes(coverBytes, coverBitmap);

            return container;
        }

        public SecretFile Decrypt(Bitmap stegocontainer, SudokuMatrix<byte> sudokuKey)
        {
            ValidateSudoku(sudokuKey);

            if (!StegoContainerFileConstraints.IsFileExtensionAllowed(stegocontainer.GetImageExtension()))
            {
                throw new ArgumentException("Stegocontainer file extension not allowed", nameof(stegocontainer));
            }

            Tuple<byte[], BitmapData> stego = stegocontainer.GetByteArrayByImageFile(ImageLockMode.ReadOnly);
            byte[] stegoBytes = stego.Item1;
            BitmapData stegoBitmap = stego.Item2;

            var secretData = ExtractSecretData(stegoBytes, sudokuKey);
            string secretFileName = secretData.Item1;
            byte[] secretFilePayloadBytes = secretData.Item2;

            stegocontainer.UnlockBits(stegoBitmap);
            return new SecretFile(secretFileName, secretFilePayloadBytes);
        }

        #region Private methods

        private void ValidateSudoku(SudokuMatrix<byte> sudokuKey)
        {
            if (sudokuKey.SudokuSize != GetExpectedSudokuSize())
            {
                throw new ArgumentException($"This steganography method works only with matrix {GetExpectedSudokuSize()}x{GetExpectedSudokuSize()}.");
            }

            //ToDo mb, other validation
        }

        /// <summary>
        /// Gets bytes to encode by file. FL = 4 bytes, FNL = 4 bytes, FN = computed, Payload = computed 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private byte[] GetSecretBytesToEncode(SecretFile file)
        {
            byte[] fileLength = BitConverter.GetBytes(file.Payload.Length);//4 bytes

            byte[] fileName = Encoding.UTF8.GetBytes(file.FileName);
            byte[] fileNameLength = BitConverter.GetBytes(fileName.Length);//length of file name (in bytes) is stored in 4 byte

            byte[] resultBytes = new byte[4 + 4 + fileName.Length + file.Payload.Length];
            Buffer.BlockCopy(fileLength, 0, resultBytes, 0, fileLength.Length);
            Buffer.BlockCopy(fileNameLength, 0, resultBytes, fileLength.Length, fileNameLength.Length);
            Buffer.BlockCopy(fileName, 0, resultBytes, fileLength.Length + fileNameLength.Length, fileName.Length);
            Buffer.BlockCopy(file.Payload, 0, resultBytes, fileLength.Length + fileNameLength.Length + fileName.Length, file.Payload.Length);

            return resultBytes;
        }

        private void EmbedSecretBytesToContainer(byte[] containerBytes, byte[] secretBytes, SudokuMatrix<byte> sudokuKey)
        {
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

        private Tuple<string, byte[]> ExtractSecretData(byte[] stegoBytes, SudokuMatrix<byte> sudokuKey)
        {
            //decode file length
            var fileLengthValueInByteArray = new byte[4];
            int stegoIterator = 0;
            for (int i = 0; i < 4; stegoIterator += 2, i++)
            {
                fileLengthValueInByteArray[i] = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];
            }

            int secretFilePayloadLength = BitConverter.ToInt32(fileLengthValueInByteArray, 0);

            if(secretFilePayloadLength <= 0 || secretFilePayloadLength >= stegoBytes.Length)
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }

            //decode secret file name (in bytes) length (number is stored in a 4 bytes)
            var fileNameLengthValueInByteArray = new byte[4];            
            for (int i = 0; i < 4; stegoIterator += 2, i++)
            {
                fileNameLengthValueInByteArray[i] = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];
            }

            int secretFileNameLength = BitConverter.ToInt32(fileNameLengthValueInByteArray, 0);

            if (secretFileNameLength <= 0 || secretFileNameLength >= stegoBytes.Length)
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }

            //decode file name
            byte[] fileNameBytes = new byte[secretFileNameLength];

            try
            {
                for (int i = 0; i < secretFileNameLength; stegoIterator += 2, i++)
                {
                    fileNameBytes[i] = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];
                }

                string secretFileName = Encoding.UTF8.GetString(fileNameBytes, 0, fileNameBytes.Length);

                //decode secret file payload
                byte[] secretFilePayloadBytes = new byte[secretFilePayloadLength];

                for (int i = 0; i < secretFilePayloadLength; stegoIterator += 2, i++)
                {
                    secretFilePayloadBytes[i] = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];
                }

                return new Tuple<string, byte[]>(secretFileName, secretFilePayloadBytes);
            }
            catch
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }
        }

        #endregion
    }
}