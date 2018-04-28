using SudkuStegoSystem.Logic.Abstract;
using SudkuStegoSystem.Logic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SudkuStegoSystem.Logic
{
    //like adapter
    public class StegoSystem : IStegoSystem
    {
        private const string KeyRegex = "[a-zA-Z0-9]{6,18}";
        private readonly SudokuStegoSystem sudokuStegoSystem = new SudokuStegoSystem();

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
            byte[,] sudokuKey = sudokuStegoSystem.GetSudokuMatrixByKey(key);

            Image stegocontainer = sudokuStegoSystem.Encrypt(containerImage, secretFileToEncode, sudokuKey);

            stegocontainer.Save(pathToStegocontainer);
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
            byte[,] sudokuKey = sudokuStegoSystem.GetSudokuMatrixByKey(key);

            SecretFile secretFile = sudokuStegoSystem.Decrypt(stegocontainerImage, sudokuKey);

            secretFile.Save(pathToRestoreFile);
        }            
    }
}
