using NUnit.Framework;
using StegoSystem;
using StegoSystem.Constraints;
using StegoSystem.Sudoku;
using StegoSystem.Sudoku.Keys;
using StegoSystem.Sudoku.Matrix;
using StegoSystem.Sudoku.Method256;
using StegoSystem.Sudoku.Method256.Constraints;
using System;
using System.IO;

namespace SudkuStegoSystem.Tests
{
    [TestFixture]
    public partial class SudokuImageStegoSystemTests
    {
        [Test]
        public void Given_Container16bppPng_CapacityIsNotEnough_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "254x256_16.png");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "fox.jpg");

            var key = new PasswordKey("123456");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                 .And.Message.EqualTo("Cannot encrypt secret data because container image is too small"),
                 () => stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath));
        }

        #region Empty StegoContainer (stegocontainer does not contain secret)

        [Test]
        public void Decrypt_GivenEmptyStegoContainer24bppBmp_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string stegocontainerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            var key = new PasswordKey("123456");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                 .And.Message.EqualTo("Unable to extract secret data. Either the password is wrong or there is no secret data at all"),
                 () => stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath));
        }

        [Test]
        public void Decrypt_GivenEmptyStegoContainer32bppBmp_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string stegocontainerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "427x440_32.bmp");

            var key = new PasswordKey("0s8P56");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                 .And.Message.EqualTo("Unable to extract secret data. Either the password is wrong or there is no secret data at all"),
                 () => stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath));
        }

        [Test]
        public void Decrypt_GivenEmptyStegoContainer32bppBmp_2_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string stegocontainerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "490x364_32.bmp");

            var key = new PasswordKey("utGpgfC");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                 .And.Message.EqualTo("Unable to extract secret data. Either the password is wrong or there is no secret data at all"),
                 () => stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath));
        }

        #endregion

        #region Wrong password

        [Test]
        public void GivenContainer24bppBmp_WrongDecryptionKey_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "lemur.jpg");

            var encryptionKey = new PasswordKey("stO73b");
            var decryptionKey = new PasswordKey("stO73c");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, encryptionKey, outputDirPath);

            //Act & Assert
            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                 .And.Message.EqualTo("Unable to extract secret data. Either the password is wrong or there is no secret data at all"),
                 () => stegoSystem.Decrypt(stegocontainerPath, decryptionKey, outputDirPath));
        }

        [Test]
        public void GivenContainer24bppPng_WrongDecryptionKey_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "300x255_24.png");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            var encryptionKey = new PasswordKey("123Pfgh");
            var decryptionKey = new PasswordKey("123Pfg");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, encryptionKey, outputDirPath);

            //Act & Assert
            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                 .And.Message.EqualTo("Unable to extract secret data. Either the password is wrong or there is no secret data at all"),
                 () => stegoSystem.Decrypt(stegocontainerPath, decryptionKey, outputDirPath));
        }

        [Test]
        public void GivenContainer16bppPng_WrongDecryptionKey_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = Path.Combine(_tempDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Directory.CreateDirectory(outputDirPath);

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "254x256_16.png");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            var encryptionKey = new PasswordKey("9hwT5nnp");
            var decryptionKey = new PasswordKey("GhEP7vgDS");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            string stegocontainerPath = stegoSystem.Encrypt(containerPath, secretPath, encryptionKey, outputDirPath);

            //Act & Assert
            Assert.Throws(Is.TypeOf<InvalidOperationException>()
                 .And.Message.EqualTo("Unable to extract secret data. Either the password is wrong or there is no secret data at all"),
                 () => stegoSystem.Decrypt(stegocontainerPath, decryptionKey, outputDirPath));
        }

        #endregion

        #region Incorrect password format

        [Test]
        public void Encrypt_GivenContainer24bppBmp_PasswordIsTooSmall_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "lemur.jpg");

            var key = new PasswordKey("s8");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                 .And.Message.Contains("Wrong password format"),
                 () => stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath));
        }

        [Test]
        public void Encrypt_GivenContainer24bppBmp_PasswordIsEmpty_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "lemur.jpg");

            var key = new PasswordKey("");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                 .And.Message.Contains("Wrong password format"),
                 () => stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath));
        }

        [Test]
        public void Decrypt_GivenContainer24bppBmp_PasswordIsNull_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string stegocontainerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            var key = new PasswordKey(null);

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                 .And.Message.Contains("Wrong password format"),
                 () => stegoSystem.Decrypt(stegocontainerPath, key, outputDirPath));
        }

        [Test]
        public void Encrypt_GivenContainer24bppBmp_PasswordIsTooLong_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "158x200_24.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "lemur.jpg");

            var key = new PasswordKey("s8uyrf7erFOefueRhiye6gUn9851tyhgyv0utMh");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                 .And.Message.Contains("Wrong password format"),
                 () => stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath));
        }

        #endregion
    }
}