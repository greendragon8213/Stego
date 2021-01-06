using System.IO;

namespace StegoSystem.Models
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

        public string Save(string destinationPath)
        {
            return Common.Extensions.File.SaveAllBytesToFile(destinationPath, FileName, Payload);
        }
    }
}
