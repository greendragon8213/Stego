using System.IO;

namespace SudkuStegoSystem.Logic.Models
{
    public class SecretFile
    {
        public string FileName { get; set; }
        public byte[] Payload { get; set; }

        public SecretFile(string fileName, byte[] payload)
        {
            FileName = fileName;
            Payload = payload;
        }

        public SecretFile(string filePath)
        {
            FileInfo secretFileInfo = new FileInfo(filePath);
            
            FileName = secretFileInfo.Name;
            Payload = File.ReadAllBytes(filePath);
        }

        public void Save(string destinationPath)
        {
            File.WriteAllBytes(Path.Combine(destinationPath, FileName), Payload);
        }
    }
}
