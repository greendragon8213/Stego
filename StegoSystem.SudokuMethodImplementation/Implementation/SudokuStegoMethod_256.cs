using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using StegoSystem.GeneralLogic.Models;
using StegoSystem.GeneralLogic.Abstract;
using StegoSystem.GeneralLogic.Common;
using StegoSystem.SudokuMethodImplementation.Abstract;
using StegoSystem.SudokuMethodImplementation.FileConstraints;
using StegoSystem.SudokuMethodImplementation.Matrix;

namespace StegoSystem.SudokuMethodImplementation
{
    /// <summary>
    /// Just encrypts and decrypts data
    /// </summary>
    public class SudokuStegoMethod_256 : ISudokuStegoMethod
    {
        public FileTypeConstraints ContainerFileConstraints => new ContainerFileTypeConstraints();
        public FileTypeConstraints StegoContainerFileConstraints => new StegoContainerFileTypeConstraints();
        public FileTypeConstraints SecretFileConstraints => new SecretFileTypeConstraints();

        public int GetExpectedSudokuSize() => 256;

        public Bitmap Encrypt(Bitmap container, SecretFile secretFile, SudokuMatrix sudokuKey)
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
                throw new InvalidOperationException("Cannot encrypt secret data because container image is too small.");
            }

            EmbedSecretBytesToContainer(coverBytes, secretData, sudokuKey);
            
            container.UpdateBitmapPayloadBytes(coverBytes, coverBitmap);

            return container;
        }

        public SecretFile Decrypt(Bitmap stegocontainer, SudokuMatrix sudokuKey)
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

        private void ValidateSudoku(SudokuMatrix sudokuKey)
        {
            if (sudokuKey.SudokuSize != GetExpectedSudokuSize())
            {
                throw new ArgumentException($"This steganography method works only with matrix {GetExpectedSudokuSize()}x{GetExpectedSudokuSize()}.");
            }

            //ToDo mb, other validation
        }

        /// <summary>
        /// Gets bytes to encode by file. FL = 4byte, FNL = 1byte, FN = computed, Payload = computed 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private byte[] GetSecretBytesToEncode(SecretFile file)
        {            
            byte[] fileLength = BitConverter.GetBytes(file.Payload.Length);//4 bytes

            byte[] fileNameLength = new byte[1] { BitConverter.GetBytes(file.FileName.Length).First() };//1 byte should be enought
            byte[] fileName = Encoding.ASCII.GetBytes(file.FileName);

            byte[] resultBytes = new byte[4 + 1 + fileName.Length + file.Payload.Length];
            Buffer.BlockCopy(fileLength, 0, resultBytes, 0, fileLength.Length);
            Buffer.BlockCopy(fileNameLength, 0, resultBytes, fileLength.Length, fileNameLength.Length);
            Buffer.BlockCopy(fileName, 0, resultBytes, fileLength.Length + fileNameLength.Length, fileName.Length);
            Buffer.BlockCopy(file.Payload, 0, resultBytes, fileLength.Length + fileNameLength.Length + fileName.Length, file.Payload.Length);
            
            return resultBytes;
        }

        private void EmbedSecretBytesToContainer(byte[] containerBytes, byte[] secretBytes, SudokuMatrix sudokuKey)
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

        private Tuple<string, byte[]> ExtractSecretData(byte[] stegoBytes, SudokuMatrix sudokuKey)
        {
            //decode file length
            var fileLengthValueInByteArray = new byte[4];
            int stegoIterator = 0;
            for (int i = 0; i < 4; stegoIterator += 2, i++)
            {
                fileLengthValueInByteArray[i] = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];
            }

            int secretFilePayloadLength = BitConverter.ToInt32(fileLengthValueInByteArray, 0);

            // decode secret file name length (stored in a 1 byte)
            int secretFileNameLength = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];

            //decode file name
            byte[] fileNameBytes = new byte[secretFileNameLength];

            stegoIterator += 2;
            for (int i = 0; i < secretFileNameLength; stegoIterator += 2, i++)
            {
                fileNameBytes[i] = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];
            }

            string secretFileName = Encoding.ASCII.GetString(fileNameBytes, 0, fileNameBytes.Length);

            //decode secret file payload
            byte[] secretFilePayloadBytes = new byte[secretFilePayloadLength];

            for (int i = 0; i < secretFilePayloadLength; stegoIterator += 2, i++)
            {
                secretFilePayloadBytes[i] = sudokuKey[stegoBytes[stegoIterator], stegoBytes[stegoIterator + 1]];
            }
            
            return new Tuple<string, byte[]>(secretFileName, secretFilePayloadBytes);
        }

        #endregion
    }
}