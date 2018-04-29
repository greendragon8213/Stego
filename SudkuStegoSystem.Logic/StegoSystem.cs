using SudkuStegoSystem.Logic.Abstract;
using SudkuStegoSystem.Logic.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace SudkuStegoSystem.Logic
{
    //like adapter to general stegosystems
    public class StegoSystem : IStegoSystem
    {
        private const string KeyRegex = "[a-zA-Z0-9]{6,18}";
        private readonly SudokuStegoSystem sudokuStegoSystem = new SudokuStegoSystem();
        //ToDo get from factory
        private INearestCoordinatesFinder _nearestCoordinatesFinder = new NearestCoordinatesFinder();
        private const int MatrixSize = 256;
        
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
            SudokuMatrix sudokuKey = new SudokuMatrix(_nearestCoordinatesFinder, key, MatrixSize);

            Image stegocontainer = sudokuStegoSystem.Encrypt(containerImage, secretFileToEncode, sudokuKey);

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
            SudokuMatrix sudokuKey = new SudokuMatrix(_nearestCoordinatesFinder, key, MatrixSize);

            SecretFile secretFile = sudokuStegoSystem.Decrypt(stegocontainerImage, sudokuKey);

            secretFile.Save(pathToRestoreFile);
        }            
    }
}
