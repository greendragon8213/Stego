using NUnit.Framework;
using StegoSystem.Constraints;
using StegoSystem.Sudoku.Keys;
using StegoSystem.Sudoku.Matrix;
using StegoSystem.Sudoku.Method256;
using StegoSystem.Sudoku.Method256.Constraints;
using System;
using System.IO;

namespace StegoSystem.Sudoku.Tests.SudokuImageStegoSystem
{
    [TestFixture]
    internal class EncryptTests
    {
        #region Incorrect password format

        [Test]
        public void GivenContainer24bppBmp_PasswordIsTooSmall_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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
        public void GivenContainer24bppBmp_PasswordIsEmpty_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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
        public void GivenContainer24bppBmp_PasswordIsTooLong_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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

        #region Wrong depth

        [Test]
        public void Given_Container8bppBmp_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "320x235_8.bmp");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            var key = new PasswordKey("123456");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                 .And.Message.EqualTo("8 bpp image is not allowed to use as container"),
                 () => stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath));
        }

        [Test]
        public void Given_Container1bppPng_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "300x255_1.png");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            var key = new PasswordKey("123456");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                 .And.Message.EqualTo("1 bpp image is not allowed to use as container"),
                 () => stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath));
        }

        [Test]
        public void Given_Container8bppPng_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

            string containerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Containers",
                "300x255_8_4.png");

            string secretPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Secrets",
                "gecon.jpg");

            var key = new PasswordKey("123456");

            IStegoSystem<string, ImageStegoConstraints> stegoSystem =
                new SudokuImageStegoSystem<byte, string>(new SudokuStegoMethod256(), new SudokuByPasswordMatrixFactory<byte>(),
                new Method256ImageStegoConstraints());

            //Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                 .And.Message.EqualTo("8 bpp image is not allowed to use as container"),
                 () => stegoSystem.Encrypt(containerPath, secretPath, key, outputDirPath));
        }

        #endregion

        [Test]
        public void Given_Container16bppPng_CapacityIsNotEnough_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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

    }
}
