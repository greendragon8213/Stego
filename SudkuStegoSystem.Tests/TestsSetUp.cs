using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace StegoSystem.Sudoku.Tests
{
    [SetUpFixture]
    class TestsSetUp
    {
        internal static string TempDirectory;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            SetUpTempDirectory();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            CleanTempData();
        }

        private void SetUpTempDirectory()
        {
            TempDirectory = Path.Combine(Path.GetTempPath(), "StegoSystemTest");
            Debug.WriteLine(TempDirectory);

            if (Directory.Exists(TempDirectory))
            {
                Directory.Delete(TempDirectory, recursive: true);
            }

            Directory.CreateDirectory(TempDirectory);
        }

        private void CleanTempData()
        {
            Directory.Delete(TempDirectory, recursive: true);
        }
    }
}
