using NUnit.Framework;
using SudkuStegoSystem.Logic;
using SudkuStegoSystem.Logic.Abstract;
using SudkuStegoSystem.Logic.SudokuMethod.SudokuMatrix;
using System.Diagnostics;
using System.IO;

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
            Debug.WriteLine(_tempDirectory);
            Directory.CreateDirectory(_tempDirectory);
        }

        #region bmp

        #region 8bpp

        [Test]
        public void Given_ContainerWithPaddingBytesAnd8bppBmp_SecretIsGeconImg_UsedCapacityIsHigh_ExpectedDecryptedEqualInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "533x235_8.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

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
        public void Given_ContainerNoPaddingBytesAnd8bppBmp_SecretIsLemurImg_UsedCapacityIsHigh_ExpectedDecryptedEqualInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "320x235_8.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "lemur.jpg");

            string key = "123456";

            IStegoSystem stegoSystem = new SudokuStegoSystem(new SudokuStegoMethod_256(), new SudokuMatrixFactory());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        #endregion

        #region 24bpp

        [Test]
        public void Given_ContainerWithPaddingBytesAnd24bppBmp_SecretIsLemurImg_UsedCapacityIsHigh_ExpectedDecryptedEqualInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "lemur.jpg");

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
        public void Given_ContainerNoPaddingBytesAnd24bppBmp_SecretIsGeconImg_UsedCapacityIsHigh_ExpectedDecryptedEqualInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "160x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            string key = "123456";

            IStegoSystem stegoSystem = new SudokuStegoSystem(new SudokuStegoMethod_256(), new SudokuMatrixFactory());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        #endregion
        
        #region 32bpp

        [Test]
        public void Given_ContainerNoPaddingBytesAnd32bppBmp_SecretIsFoxImg_UsedCapacityIsLow_ExpectedDecryptedEqualInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "427x440_32.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "fox.jpg");

            string key = "123456";

            IStegoSystem stegoSystem = new SudokuStegoSystem(new SudokuStegoMethod_256(), new SudokuMatrixFactory());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        #endregion

        #endregion

        #region jpg

        [Test]
        public void Given_ContainerJpg650x1050_SecretIsFoxImg_ExpectedDecryptedEqualInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "650x1050.jpg");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "fox.jpg");

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
        public void Given_ContainerJpg814x556_SecretIsFoxImg_ExpectedDecryptedEqualInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "814x556.jpg");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "fox.jpg");

            string key = "123456";

            IStegoSystem stegoSystem = new SudokuStegoSystem(new SudokuStegoMethod_256(), new SudokuMatrixFactory());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }
        
        #endregion
        
        [OneTimeTearDown]
        public void CleanTempData()
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }
}
