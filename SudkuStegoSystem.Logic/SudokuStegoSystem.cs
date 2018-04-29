using SudkuStegoSystem.Logic.Abstract;
using SudkuStegoSystem.Logic.Models;
using SudkuStegoSystem.Logic.SudokuMethod.SudokuMatrix;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace SudkuStegoSystem.Logic
{
    /// <summary>
    /// Represents adaptation of SudokuStegoMethod to general stegosystems interface
    /// </summary>
    public class SudokuStegoSystem : IStegoSystem
    {
        private const string KeyRegex = "[a-zA-Z0-9]{6,18}";
        private readonly ISudokuStegoMethod _sudokuStegoMethod;
        private readonly SudokuMatrixFactory _sudokuMatrixFactory;

        public SudokuStegoSystem(ISudokuStegoMethod sudokuStegoMethod, SudokuMatrixFactory sudokuMatrixFactory)
        {
            _sudokuStegoMethod = sudokuStegoMethod;
            _sudokuMatrixFactory = sudokuMatrixFactory;
        }
        
        public void Encrypt(string containerFilePath, string secretDataFilePath, string key, string pathToStegocontainer = null)
        {
            #region checking arguments

            if (!File.Exists(containerFilePath))
            {
                throw new ArgumentException("Container file does not exist.");
            }

            if (!File.Exists(secretDataFilePath))
            {
                throw new ArgumentException("Secret data file does not exist.");
            }

            if (!Regex.Match(key, KeyRegex).Success)
            {
                throw new ArgumentException("Wrong key format.");
            }

            #endregion

            //ToDo exceptions 
            Image containerImage = Image.FromFile(containerFilePath);           
            SecretFile secretFileToEncode = new SecretFile(secretDataFilePath);
            SudokuMatrix sudokuKey = GenerateSudokuKey(key);

            Image stegocontainer = _sudokuStegoMethod.Encrypt(containerImage, secretFileToEncode, sudokuKey);

            string containerFileName = new FileInfo(containerFilePath).Name;            
            stegocontainer.Save(Path.Combine(pathToStegocontainer, containerFileName), ImageFormat.Bmp);
        }

        public void Decrypt(string stegocontainerFilePath, string key, string pathToRestoreFile = null)
        {
            #region checking arguments

            if (!File.Exists(stegocontainerFilePath))
            {
                throw new ArgumentException("Container file does not exist.");
            }

            //ToDo validate restoredSecretFilePath 

            if (!Regex.Match(key, KeyRegex).Success)
            {
                throw new ArgumentException("Wrong key format.");
            }

            #endregion

            Image stegocontainerImage = Image.FromFile(stegocontainerFilePath);
            SudokuMatrix sudokuKey = GenerateSudokuKey(key);

            SecretFile secretFile = _sudokuStegoMethod.Decrypt(stegocontainerImage, sudokuKey);

            secretFile.Save(pathToRestoreFile);
        }

        #region Private methods

        private SudokuMatrix GenerateSudokuKey(string password)
        {
            return _sudokuMatrixFactory.GetByPassword(_sudokuStegoMethod.GetExpectedSudokuSize(), password);
        }

        #endregion
    }
}
