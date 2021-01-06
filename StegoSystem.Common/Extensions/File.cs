using System.IO;

namespace StegoSystem.Common.Extensions
{
    public static class File
    {
        public static string SaveAllBytesToFile(string destinationPath, string fileName, byte[] bytes)
        {
            string fileExtension = Path.GetExtension(fileName);
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);

            int i = 0;
            string path = Path.Combine(destinationPath, fileName);

            while (System.IO.File.Exists(path))
            {
                i++;
                string newFileName = $"{baseFileName}_{i}.{fileExtension}";
                path = Path.Combine(destinationPath, newFileName);
            }

            System.IO.File.WriteAllBytes(path, bytes);

            return path;
        }
    }
}
