using StegoSystem.Models;
using System.Threading.Tasks;

namespace StegoSystem.DesktopApp.Models
{
    public class Encryption : PasswordBasedSystem
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

        public async Task<string> Encrypt(string containerFilePath, string secretDataFilePath, IKey<string> passwordKey, string stegocontainerFilePath)
        {
            return await Task.Run(() => _stegoSystem.Encrypt(containerFilePath, secretDataFilePath, passwordKey,
                    stegocontainerFilePath));
        }
    }
}
