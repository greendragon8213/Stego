﻿using StegoSystem.Common.Extensions;
using StegoSystem.Constraints;
using StegoSystem.Models;
using StegoSystem.Sudoku.Matrix;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace StegoSystem.Sudoku
{
    /// <summary>
    /// Represents adaptation of SudokuStegoMethod to general stegosystems interface
    /// </summary>
    public class SudokuImageStegoSystem<T, TKey> : IStegoSystem<TKey, ImageStegoConstraints>
    {
        private readonly ISudokuStegoMethod<T> _sudokuStegoMethod;
        private readonly ISudokuMatrixFactory<T, TKey> _sudokuMatrixFactory;

        public ImageStegoConstraints StegoConstraints { get; }

        public SudokuImageStegoSystem(
            ISudokuStegoMethod<T> sudokuStegoMethod,
            ISudokuMatrixFactory<T, TKey> sudokuMatrixFactory,
            ImageStegoConstraints stegoConstraints)
        {
            _sudokuStegoMethod = sudokuStegoMethod;
            _sudokuMatrixFactory = sudokuMatrixFactory;
            StegoConstraints = stegoConstraints;
        }

        public string Encrypt(string containerFilePath, byte[] secret, IKey<TKey> key, string stegocontainerFilePath)
        {
            CheckEncryptionArguments(containerFilePath, secret, key, stegocontainerFilePath);

            var containerBitmap = OpenBitmap(containerFilePath, "container");

            var secretData = GetSecretBytesToEmbed(secret);

            EmbedSecret(containerBitmap, secretData, key);

            return SaveStegocontainerFile(containerFilePath, stegocontainerFilePath, containerBitmap);
        }

        public string Encrypt(string containerFilePath, string secretDataFilePath, IKey<TKey> key, string stegocontainerFilePath)
        {
            CheckEncryptionArguments(containerFilePath, secretDataFilePath, key, stegocontainerFilePath);

            var containerBitmap = OpenBitmap(containerFilePath, "container");

            byte[] secretData;
            try
            {
                secretData = GetSecretBytesToEmbed(new SecretFile(secretDataFilePath));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot open secret file and get secret data", e);
            }

            EmbedSecret(containerBitmap, secretData, key);

            return SaveStegocontainerFile(containerFilePath, stegocontainerFilePath, containerBitmap);
        }

        public byte[] Decrypt(string stegocontainerFilePath, IKey<TKey> key)
        {
            CheckDecryptionArguments(stegocontainerFilePath, key);

            var stegocontainerBitmap = OpenBitmap(stegocontainerFilePath, "stegocontainer");
            var stegoBitmapParts = GetBitmapParts(stegocontainerBitmap, ImageLockMode.ReadOnly, StegoConstraints.StegoContainerFileConstraints, "stegocontainer");

            var sudokuKey = _sudokuMatrixFactory.Create(_sudokuStegoMethod.GetExpectedSudokuSize(), key);

            var secret = ExtractSecret(
                () => ExtractSecretBytes(stegoBitmapParts.PayloadBytes, sudokuKey),
                stegocontainerBitmap,
                stegoBitmapParts.Bitmap,
                key.GetKeyName);

            return secret;
        }

        public string Decrypt(string stegocontainerFilePath, IKey<TKey> key, string restoreFilePath)
        {
            CheckDecryptionArguments(stegocontainerFilePath, key, restoreFilePath);

            var stegocontainerBitmap = OpenBitmap(stegocontainerFilePath, "stegocontainer");
            var stegoBitmapParts = GetBitmapParts(stegocontainerBitmap, ImageLockMode.ReadOnly, StegoConstraints.StegoContainerFileConstraints, "stegocontainer");

            var sudokuKey = _sudokuMatrixFactory.Create(_sudokuStegoMethod.GetExpectedSudokuSize(), key);

            var secret = ExtractSecret(
                () => ExtractSecretFile(stegoBitmapParts.PayloadBytes, sudokuKey),
                stegocontainerBitmap,
                stegoBitmapParts.Bitmap,
                key.GetKeyName);

            try
            {
                return secret.Save(restoreFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot save decrypted file. Try again", e);
            }
        }

        #region Private methods

        private void ValidateKey(IKey<TKey> key)
        {
            if (!key.IsValid())
            {
                throw new ArgumentException($"Wrong {key.GetKeyName.ToLower()} format. {key.ValidationDescription}");
            }
        }

        private Bitmap OpenBitmap(string containerFilePath, string bitmapName)
        {
            try
            {
                return new Bitmap(containerFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Cannot open {bitmapName} file", e);
            }
        }

        private (byte[] PayloadBytes, BitmapData Bitmap, int Depth) GetBitmapParts(
            Bitmap bitmap,
            ImageLockMode lockMode,
            ImageFileTypeConstraints constraints,
            string bitmapName)
        {
            (byte[] PayloadBytes, BitmapData Bitmap, int Depth) bitmapParts;
            try
            {
                bitmapParts = bitmap.GetBitmapParts(lockMode);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again", e);
            }

            if (!constraints.IsDepthAllowed(bitmapParts.Depth))
            {
                throw new ArgumentException($"{bitmapParts.Depth} bpp image is not allowed to use as {bitmapName}");
            }

            return bitmapParts;
        }

        #region Encryption

        private void CheckEncryptionArguments(
            string containerFilePath,
            byte[] secret,
            IKey<TKey> key,
            string pathToStegocontainer)
        {
            ValidateContainer(containerFilePath);

            if (secret == null || secret.Length == 0)
            {
                throw new ArgumentException("No secret data provided");
            }

            ValidateStegocontainerPath(pathToStegocontainer);

            ValidateKey(key);
        }

        private void CheckEncryptionArguments(string containerFilePath, string secretDataFilePath, IKey<TKey> key, string pathToStegocontainer)
        {
            ValidateContainer(containerFilePath);

            if (!System.IO.File.Exists(secretDataFilePath))
            {
                throw new ArgumentException("Secret data file does not exist");
            }

            if (!StegoConstraints.SecretFileConstraints.IsFileExtensionAllowedByPath(secretDataFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as secret file with extension \"{Path.GetExtension(secretDataFilePath)}\"");
            }

            ValidateStegocontainerPath(pathToStegocontainer);

            ValidateKey(key);
        }

        private void ValidateContainer(string containerFilePath)
        {
            if (!System.IO.File.Exists(containerFilePath))
            {
                throw new ArgumentException("Container file does not exist");
            }

            if (!StegoConstraints.ContainerFileConstraints.IsFileExtensionAllowedByPath(containerFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as container file with extension \"{Path.GetExtension(containerFilePath)}\"");
            }
        }

        private static void ValidateStegocontainerPath(string pathToStegocontainer)
        {
            if (!Directory.Exists(pathToStegocontainer))
            {
                throw new ArgumentException("Stegocontainer (output) directory does not exist");
            }
        }
        /// <summary>
        /// Gets bytes to encode by file. FL = 4 bytes, FNL = 4 bytes, FN = computed, Payload = computed 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private byte[] GetSecretBytesToEmbed(SecretFile file)
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

        private byte[] GetSecretBytesToEmbed(byte[] secretBytes)
        {
            byte[] secretBytesLength = BitConverter.GetBytes(secretBytes.Length);//4 bytes

            byte[] resultBytes = new byte[secretBytesLength.Length + secretBytes.Length];

            Buffer.BlockCopy(secretBytesLength, 0, resultBytes, 0, secretBytesLength.Length);
            Buffer.BlockCopy(secretBytes, 0, resultBytes, secretBytesLength.Length, secretBytes.Length);

            return resultBytes;
        }

        private void EmbedSecret(Bitmap containerBitmap, byte[] secretData, IKey<TKey> key)
        {
            var containerBitmapParts = GetBitmapParts(containerBitmap, ImageLockMode.ReadWrite, StegoConstraints.ContainerFileConstraints, "container");

            SudokuMatrix<T> sudokuKey = _sudokuMatrixFactory.Create(_sudokuStegoMethod.GetExpectedSudokuSize(), key);

            try
            {
                _sudokuStegoMethod.EmbedSecretData(containerBitmapParts.PayloadBytes, secretData, sudokuKey);

                containerBitmap.UpdateBitmapPayloadBytes(containerBitmapParts.PayloadBytes, containerBitmapParts.Bitmap);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again", e);
            }
        }

        private string SaveStegocontainerFile(string containerFilePath, string pathToStegocontainer, Bitmap containerBitmap)
        {
            try
            {
                return containerBitmap.Save(pathToStegocontainer, containerFilePath, ImageFormat.Bmp);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot save encrypted file. Try again", e);
            }
        }

        #endregion

        #region Decryption

        private void CheckDecryptionArguments(string stegocontainerFilePath, IKey<TKey> key)
        {
            ValidateStegocontainer(stegocontainerFilePath);

            ValidateKey(key);
        }

        private void CheckDecryptionArguments(string stegocontainerFilePath, IKey<TKey> key, string restoreFilePath)
        {
            CheckDecryptionArguments(stegocontainerFilePath, key);

            if (!Directory.Exists(restoreFilePath))
            {
                throw new ArgumentException("Output directory (to restore secret file) does not exist.");
            }
        }

        private void ValidateStegocontainer(string stegocontainerFilePath)
        {
            if (!System.IO.File.Exists(stegocontainerFilePath))
            {
                throw new ArgumentException("Container file does not exist.");
            }

            if (!StegoConstraints.StegoContainerFileConstraints.IsFileExtensionAllowedByPath(stegocontainerFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as stegocontainer file with extension \"{Path.GetExtension(stegocontainerFilePath)}\".");
            }
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

        private TSecretResult ExtractSecret<TSecretResult>(Func<TSecretResult> extractionDelegate,
            Bitmap stegoBitmap, BitmapData stegoBitmapData, string keyName)
        {
            try
            {
                var secret = extractionDelegate();

                stegoBitmap.UnlockBits(stegoBitmapData);

                return secret;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"{e.Message}. Either the {keyName.ToLower()} is wrong or there is no secret data at all", e);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again", e);
            }
        }

        #endregion

        #endregion
    }
}
