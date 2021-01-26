using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace SudkuStegoSystem.Tests
{
    [TestFixture]
    public partial class SudokuImageStegoSystemTests
    {
        private string _tempDirectory;

        [OneTimeSetUp]
        public void CreateTempDirectory()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "StegoSystemTesting");
            Debug.WriteLine(_tempDirectory);

            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }

            Directory.CreateDirectory(_tempDirectory);
        }

        [OneTimeTearDown]
        public void CleanTempData()
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }
}
