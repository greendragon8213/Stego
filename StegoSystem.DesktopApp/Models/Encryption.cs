using StegoSystem.Models;
using StegoSystem.Sudoku.Keys;

namespace StegoSystem.DesktopApp.Models
{
    public class Encryption
    {
        private readonly IStegoSystem<string> _stegoSystem;

        public Encryption(IStegoSystem<string> stegoSystem)
        {
            _stegoSystem = stegoSystem;
        }

        public bool IsContainerExtensionAllowed(string path)
        {
            return _stegoSystem.ContainerFileConstraints.IsFileExtensionAllowedByPath(path);
        }        
        
        public bool IsSecretExtensionAllowed(string path)
        {
            return _stegoSystem.SecretFileConstraints.IsFileExtensionAllowedByPath(path);
        }

        public IKey<string> CreatePassword(string password)
        {
            return new PasswordKey(password);
        } 

        public string Encrypt(string containerFilePath, string secretDataFilePath, IKey<string> passwordKey, string stegocontainerFilePath)
        {
            return _stegoSystem.Encrypt(containerFilePath, secretDataFilePath, passwordKey,
                    stegocontainerFilePath);
        }
    }
}
