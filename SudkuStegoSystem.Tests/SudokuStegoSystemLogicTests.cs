using NUnit.Framework;
using SudkuStegoSystem.Logic;
using SudkuStegoSystem.Logic.Abstract;
using SudkuStegoSystem.Logic.SudokuMethod.SudokuMatrix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudkuStegoSystem.Tests
{
    [TestFixture]
    public class SudokuStegoSystemLogicTests
    {
        private string _tempDirectory;

        [OneTimeSetUp]
        public void CreateTempDirectory()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "StegoSystemTesting");
            Directory.CreateDirectory(_tempDirectory);
        }

        [Test]
        public void Lebowski_gecko_jpg()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, nameof(Lebowski_gecko_jpg));
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "Jeff-Bridges-the-Dude-carpet-Big-Lebowski.jpg");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecko_72.jpg");

            string key = "123456";

            IStegoSystem stegoSystem = new SudokuStegoSystem(new SudokuStegoMethod_256(), new SudokuMatrixFactory());

            //Act 
            string stegocontainerPath  = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        [Test]
        public void Lebowski_gecko_bmp()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, nameof(Lebowski_gecko_bmp));
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "Jeff-Bridges-the-Dude-carpet-Big-Lebowski.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecko_72.jpg");

            string key = "123456";

            IStegoSystem stegoSystem = new SudokuStegoSystem(new SudokuStegoMethod_256(), new SudokuMatrixFactory());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        [Test]
        public void Birdth_Gecko()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, nameof(Birdth_Gecko));
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "birdth.jpg");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecko_72.jpg");

            string key = "123456";

            IStegoSystem stegoSystem = new SudokuStegoSystem(new SudokuStegoMethod_256(), new SudokuMatrixFactory());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        [OneTimeTearDown]
        public void CleanTempData()
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }
}
