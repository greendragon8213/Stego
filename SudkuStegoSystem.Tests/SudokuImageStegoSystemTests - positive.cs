using NUnit.Framework;
using StegoSystem;
using StegoSystem.Constraints;
using StegoSystem.Sudoku;
using StegoSystem.Sudoku.Keys;
using StegoSystem.Sudoku.Matrix;
using StegoSystem.Sudoku.Method256;
using StegoSystem.Sudoku.Method256.Constraints;
using System.IO;

namespace SudkuStegoSystem.Tests
{
    [TestFixture]
    public partial class SudokuImageStegoSystemTests
    {
        #region bmp

        #region 24bpp

        [Test]
        public void Given_ContainerWithPaddingBytesAnd24bppBmp_UsedCapacityIsHigh_ExpectedDecryptedEqualsInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "lemur.jpg");

            var key = new PasswordKey("123456");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        [Test]
        public void Given_ContainerNoPaddingBytesAnd24bppBmp_UsedCapacityIsHigh_ExpectedDecryptedEqualsInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "160x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            var key = new PasswordKey("f5Wi5xOss");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

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
        public void Given_ContainerNoPaddingBytesAnd32bppBmp_UsedCapacityIsLow_ExpectedDecryptedEqualsInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "427x440_32.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "fox.jpg");

            var key = new PasswordKey("gNoerXq");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

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

        #region 24bpp

        [Test]
        public void Given_ContainerLandscape24bppJpg_UsedCapacityIsLow_ExpectedDecryptedEqualsInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "650x1050.jpg");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "fox.jpg");

            var key = new PasswordKey("lMf83wP");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        [Test]
        public void Given_ContainerPortrait24bppJpg_UsedCapacityIsLow_ExpectedDecryptedEqualsInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "814x556.jpg");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "fox.jpg");

            var key = new PasswordKey("950042");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        #endregion

        #endregion

        #region png

        #region 16bpp

        [Test]
        public void Given_Container16bppPng_UsedCapacityIsLow_ExpectedDecryptedEqualsInitialSecret()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "254x256_16.png");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            var key = new PasswordKey("aaaaaa");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act 
            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath);
            string restoredSecretPath = stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath);

            //Assert
            //restored and initial secrets are equal
            FileAssert.AreEqual(secretPath, restoredSecretPath);
        }

        #endregion

        #endregion
    }
}
