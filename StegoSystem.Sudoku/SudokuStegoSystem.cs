using StegoSystem.Models;
using StegoSystem.Sudoku.Matrix;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace StegoSystem.Sudoku
{
    /// <summary>
    /// Represents adaptation of SudokuStegoMethod to general stegosystems interface
    /// </summary>
    public class SudokuStegoSystem<T> : IStegoSystem
    {
        private const string KeyRegex = "[a-zA-Z0-9]{6,18}";
        private readonly ISudokuStegoMethod<T> _sudokuStegoMethod;
        private readonly SudokuMatrixFactory<T> _sudokuMatrixFactory;

        public FileTypeConstraints ContainerFileConstraints => _sudokuStegoMethod.ContainerFileConstraints;
        public FileTypeConstraints StegoContainerFileConstraints => _sudokuStegoMethod.StegoContainerFileConstraints;
        public FileTypeConstraints SecretFileConstraints => _sudokuStegoMethod.SecretFileConstraints;

        public SudokuStegoSystem(ISudokuStegoMethod<T> sudokuStegoMethod, SudokuMatrixFactory<T> sudokuMatrixFactory)
        {
            _sudokuStegoMethod = sudokuStegoMethod;
            _sudokuMatrixFactory = sudokuMatrixFactory;
        }

        public string Encrypt(string containerFilePath, string secretDataFilePath, string key, string pathToStegocontainer = null)
        {
            #region checking arguments

            if (!File.Exists(containerFilePath))
            {
                throw new ArgumentException("Container file does not exist.");
            }

            if (!ContainerFileConstraints.IsFileExtensionAllowedByPath(containerFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as container file with extension \"{Path.GetExtension(containerFilePath)}\".");
            }

            if (!File.Exists(secretDataFilePath))
            {
                throw new ArgumentException("Secret data file does not exist.");
            }

            if (!SecretFileConstraints.IsFileExtensionAllowedByPath(secretDataFilePath))
            {
                throw new ArgumentException($"This steganography system does not allow to use as secret file with extension \"{Path.GetExtension(secretDataFilePath)}\".");
            }

            if (!string.IsNullOrEmpty(pathToStegocontainer) && !Directory.Exists(pathToStegocontainer))
            {
                throw new ArgumentException("Stegocontainer directory does not exist.");
            }

            if (string.IsNullOrEmpty(key) || !Regex.Match(key, KeyRegex).Success)
            {
                throw new ArgumentException("Wrong key format.");
            }

            #endregion

            Bitmap containerBitmap;
            try
            {
                containerBitmap = new Bitmap(containerFilePath);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot open container file.", e);
            }

            SecretFile secretFileToEncode;
            try
            {
                secretFileToEncode = new SecretFile(secretDataFilePath);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot open secret file.", e);
            }

            SudokuMatrix<T> sudokuKey;
            sudokuKey = GenerateSudokuKey(key);

            Bitmap stegocontainer;
            try
            {
                stegocontainer = _sudokuStegoMethod.Encrypt(containerBitmap, secretFileToEncode, sudokuKey);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again.", e);
            }

            try
            {
                string stegocontainerFilePath = Path.Combine(pathToStegocontainer, new FileInfo(containerFilePath).Name);
                ImageFormat stegoContainerFormat = ImageFormat.Bmp;

                stegocontainerFilePath = Path.ChangeExtension(stegocontainerFilePath, stegoContainerFormat.ToString().ToLower());
                stegocontainer.Save(stegocontainerFilePath, stegoContainerFormat);

                return stegocontainerFilePath;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot save encrypted file. Try again.", e);
            }
        }

        public string Decrypt(string stegocontainerFilePath, string key, string pathToRestoreFile = null)
        {
            #region checking arguments

            if (!File.Exists(stegocontainerFilePath))
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

            if (!Regex.Match(key, KeyRegex).Success)
            {
                throw new ArgumentException("Wrong key format.");
            }

            #endregion

            Bitmap stegocontainerBitmap;
            try
            {
                stegocontainerBitmap = new Bitmap(stegocontainerFilePath);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot open stegocontainer file.", e);
            }

            SudokuMatrix<T> sudokuKey = GenerateSudokuKey(key);

            SecretFile secretFile;
            try
            {
                secretFile = _sudokuStegoMethod.Decrypt(stegocontainerBitmap, sudokuKey);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong. Try again.", e);
            }

            try
            {
                return secretFile.Save(pathToRestoreFile);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot save decrypted file. Try again.", e);
            }
        }

        #region Private methods

        private SudokuMatrix<T> GenerateSudokuKey(string password)
        {
            return _sudokuMatrixFactory.GetByPassword(_sudokuStegoMethod.GetExpectedSudokuSize(), password);
        }

        #endregion
    }
}
