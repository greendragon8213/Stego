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
    internal class DecryptTests
    {
        #region Empty StegoContainer (stegocontainer does not contain secret)

        [Test]
        public void GivenEmptyStegoContainer24bppBmp_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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
        public void GivenEmptyStegoContainer32bppBmp_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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
        public void GivenEmptyStegoContainer32bppBmp_2_ExpectedInvalidOperationExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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

        #region Incorrect password format

        [Test]
        public void GivenContainer24bppBmp_PasswordIsNull_ExpectedArgumentExceptionThrown()
        {
            //Arrange
            string outputDirPath = TestsSetUp.TempDirectory;

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

        #endregion
    }
}