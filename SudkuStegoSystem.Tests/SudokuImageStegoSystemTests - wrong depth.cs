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
        #region bmp

        #region 8bpp

        [Test]
        public void Given_Container8bppBmp_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

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

        #endregion

        #endregion

        #region png

        [Test]
        public void Given_Container1bppPng_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = _tempDirectory;

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
            string outputDirPath = _tempDirectory;

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
    }
}
