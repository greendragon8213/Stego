using StegoSystem.Models;
using StegoSystem.Sudoku.Matrix;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using StegoSystem.Common.Extensions;
using StegoSystem.Sudoku.FileConstraints;
using System.Text;

namespace StegoSystem.Sudoku
{
    /// <summary>
    /// Represents adaptation of SudokuStegoMethod to general stegosystems interface
    /// </summary>
    public class SudokuStegoSystem<T, TKey> : IStegoSystem<TKey>
    {
        private readonly ISudokuStegoMethod<T> _sudokuStegoMethod;
        private readonly ISudokuMatrixFactory<T, TKey> _sudokuMatrixFactory;

        public FileTypeConstraints ContainerFileConstraints => new ContainerFileTypeConstraints();
        public FileTypeConstraints StegoContainerFileConstraints => new StegoContainerFileTypeConstraints();
        public FileTypeConstraints SecretFileConstraints => new SecretFileTypeConstraints();

        public SudokuStegoSystem(ISudokuStegoMethod<T> sudokuStegoMethod, ISudokuMatrixFactory<T, TKey> sudokuMatrixFactory)
        {
            _sudokuStegoMethod = sudokuStegoMethod;
            _sudokuMatrixFactory = sudokuMatrixFactory;
        }

        public string Encrypt(string containerFilePath, byte[] secret, IKey<TKey> key, string pathToStegocontainer = null)
        {
            #region checking arguments

            if (!System.IO.File.Exists(containerFilePath))
            {
                throw new ArgumentException("Container file does not exist");
            }

            if (!ContainerFileConstraints.IsFileExtensionAllowedByPath(containerFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as container file with extension \"{Path.GetExtension(containerFilePath)}\"");
            }

            if (secret == null || secret.Length == 0)
            {
                throw new ArgumentException("No secret data");
            }

            if (!string.IsNullOrEmpty(pathToStegocontainer) && !Directory.Exists(pathToStegocontainer))
            {
                throw new ArgumentException("Stegocontainer (output) directory does not exist");
            }

            ValidateKey(key);

            #endregion

            Bitmap containerBitmap;
            try
            {
                containerBitmap = new Bitmap(containerFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot open container file", e);
            }

            SudokuMatrix<T> sudokuKey = _sudokuMatrixFactory.Create(_sudokuStegoMethod.GetExpectedSudokuSize(), key);

            try
            {
                var cover = containerBitmap.GetByteArrayByImageFile(ImageLockMode.ReadWrite);
                byte[] secretBytes = GetSecretBytesToEncode(secret);

                _sudokuStegoMethod.EmbedSecretData(cover.PayloadBytes, secretBytes, sudokuKey);

                containerBitmap.UpdateBitmapPayloadBytes(cover.PayloadBytes, cover.Bitmap);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again", e);
            }

            try
            {
                return containerBitmap.Save(pathToStegocontainer, containerFilePath, ImageFormat.Bmp);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot save encrypted file. Try again", e);
            }
        }

        public string Encrypt(string containerFilePath, string secretDataFilePath, IKey<TKey> key, string pathToStegocontainer = null)
        {
            #region checking arguments

            if (!System.IO.File.Exists(containerFilePath))
            {
                throw new ArgumentException("Container file does not exist");
            }

            if (!ContainerFileConstraints.IsFileExtensionAllowedByPath(containerFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as container file with extension \"{Path.GetExtension(containerFilePath)}\"");
            }

            if (!System.IO.File.Exists(secretDataFilePath))
            {
                throw new ArgumentException("Secret data file does not exist");
            }

            if (!SecretFileConstraints.IsFileExtensionAllowedByPath(secretDataFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as secret file with extension \"{Path.GetExtension(secretDataFilePath)}\"");
            }

            if (!string.IsNullOrEmpty(pathToStegocontainer) && !Directory.Exists(pathToStegocontainer))
            {
                throw new ArgumentException("Stegocontainer (output) directory does not exist");
            }

            ValidateKey(key);

            #endregion

            Bitmap containerBitmap;
            try
            {
                containerBitmap = new Bitmap(containerFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot open container file", e);
            }

            SecretFile secretFileToEncode;
            try
            {
                secretFileToEncode = new SecretFile(secretDataFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot open secret file", e);
            }

            SudokuMatrix<T> sudokuKey = _sudokuMatrixFactory.Create(_sudokuStegoMethod.GetExpectedSudokuSize(), key);

            try
            {
                var cover = containerBitmap.GetByteArrayByImageFile(ImageLockMode.ReadWrite);
                byte[] secretData = GetSecretBytesToEncode(secretFileToEncode);

                _sudokuStegoMethod.EmbedSecretData(cover.PayloadBytes, secretData, sudokuKey);

                containerBitmap.UpdateBitmapPayloadBytes(cover.PayloadBytes, cover.Bitmap);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again", e);
            }

            try
            {
                return containerBitmap.Save(pathToStegocontainer, containerFilePath, ImageFormat.Bmp);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot save encrypted file. Try again", e);
            }
        }

        public string Decrypt(string stegocontainerFilePath, IKey<TKey> key, string pathToRestoreFile = null)
        {
            #region checking arguments

            if (!System.IO.File.Exists(stegocontainerFilePath))
            {
                throw new ArgumentException("Container file does not exist.");
            }

            if (!StegoContainerFileConstraints.IsFileExtensionAllowedByPath(stegocontainerFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as stegocontainer file with extension \"{Path.GetExtension(stegocontainerFilePath)}\".");
            }

            if (!string.IsNullOrEmpty(pathToRestoreFile) && !Directory.Exists(pathToRestoreFile))
            {
                throw new ArgumentException("Output directory (to restore secret file) does not exist.");
            }

            ValidateKey(key);

            #endregion

            Bitmap stegocontainerBitmap;
            try
            {
                stegocontainerBitmap = new Bitmap(stegocontainerFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot open stegocontainer file", e);
            }

            SudokuMatrix<T> sudokuKey = _sudokuMatrixFactory.Create(_sudokuStegoMethod.GetExpectedSudokuSize(), key);

            SecretFile secretFile;
            try
            {
                var stego = stegocontainerBitmap.GetByteArrayByImageFile(ImageLockMode.ReadOnly);

                secretFile = ExtractSecretFile(stego.PayloadBytes, sudokuKey);

                stegocontainerBitmap.UnlockBits(stego.Bitmap);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"{e.Message}. Either the {key.GetKeyName.ToLower()} is wrong or there is no secret data at all", e);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again", e);
            }

            try
            {
                return secretFile.Save(pathToRestoreFile);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot save decrypted file. Try again", e);
            }
        }

        public byte[] Decrypt(string stegocontainerFilePath, IKey<TKey> key)
        {
            #region checking arguments

            if (!System.IO.File.Exists(stegocontainerFilePath))
            {
                throw new ArgumentException("Container file does not exist.");
            }

            if (!StegoContainerFileConstraints.IsFileExtensionAllowedByPath(stegocontainerFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as stegocontainer file with extension \"{Path.GetExtension(stegocontainerFilePath)}\".");
            }

            ValidateKey(key);

            #endregion

            Bitmap stegocontainerBitmap;
            try
            {
                stegocontainerBitmap = new Bitmap(stegocontainerFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot open stegocontainer file", e);
            }

            SudokuMatrix<T> sudokuKey = _sudokuMatrixFactory.Create(_sudokuStegoMethod.GetExpectedSudokuSize(), key);

            byte[] secret;
            try
            {
                var stego = stegocontainerBitmap.GetByteArrayByImageFile(ImageLockMode.ReadOnly);

                secret = ExtractSecretBytes(stego.PayloadBytes, sudokuKey);

                stegocontainerBitmap.UnlockBits(stego.Bitmap);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"{e.Message}. Either the {key.GetKeyName.ToLower()} is wrong or there is no secret data at all", e);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again", e);
            }

            return secret;
        }

        #region Private methods

        private void ValidateKey(IKey<TKey> key)
        {
            if (!key.IsValid())
            {
                throw new ArgumentException($"Wrong {key.GetKeyName.ToLower()} format. {key.ValidationDescription}");
            }
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
        
        private byte[] GetSecretBytesToEncode(byte[] secretBytes)
        {
            byte[] secretBytesLength = BitConverter.GetBytes(secretBytes.Length);//4 bytes

            byte[] resultBytes = new byte[secretBytesLength.Length + secretBytes.Length];

            Buffer.BlockCopy(secretBytesLength, 0, resultBytes, 0, secretBytesLength.Length);
            Buffer.BlockCopy(secretBytes, 0, resultBytes, secretBytesLength.Length, secretBytes.Length);
            
            return resultBytes;
        }

        private SecretFile ExtractSecretFile(byte[] stegoBytes, SudokuMatrix<T> sudokuKey)
        {
            int offset = 0;

            //extract secret file length (stored in 4 bytes)
            var secretFilePayloadLengthInByteArray = _sudokuStegoMethod.ExtractSecretData(4, ref offset, stegoBytes, sudokuKey);
            int secretFilePayloadLength = BitConverter.ToInt32(secretFilePayloadLengthInByteArray, 0);

            if (secretFilePayloadLength <= 0 || secretFilePayloadLength >= stegoBytes.Length)
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }

            //extract secret file name (in bytes) length (number is stored in a 4 bytes)
            var secretFileNameLengthInByteArray = _sudokuStegoMethod.ExtractSecretData(4, ref offset, stegoBytes, sudokuKey);
            int secretFileNameLength = BitConverter.ToInt32(secretFileNameLengthInByteArray, 0);

            if (secretFileNameLength <= 0 || secretFileNameLength >= stegoBytes.Length)
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }

            try
            {
                //extract secret file name
                var secretFileNameInByteArray = _sudokuStegoMethod.ExtractSecretData(secretFileNameLength, ref offset, stegoBytes, sudokuKey);
                string secretFileName = Encoding.UTF8.GetString(secretFileNameInByteArray, 0, secretFileNameInByteArray.Length);

                //extract secret file payload
                var secretFilePayloadBytes = _sudokuStegoMethod.ExtractSecretData(secretFilePayloadLength, ref offset, stegoBytes, sudokuKey);

                return new SecretFile(secretFileName, secretFilePayloadBytes);
            }
            catch
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }
        }
        
        private byte[] ExtractSecretBytes(byte[] stegoBytes, SudokuMatrix<T> sudokuKey)
        {
            int offset = 0;

            //extract secret bytes length (stored in 4 bytes)
            var secretBytesLengthInByteArray = _sudokuStegoMethod.ExtractSecretData(4, ref offset, stegoBytes, sudokuKey);
            int secretBytesLength = BitConverter.ToInt32(secretBytesLengthInByteArray, 0);

            if (secretBytesLength <= 0 || secretBytesLength >= stegoBytes.Length)
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }

            try
            {
                //extract secret bytes (payload)
                return _sudokuStegoMethod.ExtractSecretData(secretBytesLength, ref offset, stegoBytes, sudokuKey);
            }
            catch
            {
                throw new InvalidOperationException("Unable to extract secret data");
            }
        }

        #endregion
    }
}
