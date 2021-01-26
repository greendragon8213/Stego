using StegoSystem.Models;
using System.Threading.Tasks;
using constraints = StegoSystem.Constraints.StegoConstraints
    <StegoSystem.FileTypeConstraints, StegoSystem.FileTypeConstraints, StegoSystem.FileTypeConstraints>;

namespace StegoSystem.DesktopApp.Models
{
    public class Encryption : PasswordBasedSystem
    {
        private readonly IStegoSystem<string, constraints> _stegoSystem;

        public Encryption(IStegoSystem<string, constraints> stegoSystem)
        {
            _stegoSystem = stegoSystem;
        }

        public bool IsContainerExtensionAllowed(string path)
        {
            return _stegoSystem.StegoConstraints.ContainerFileConstraints.IsFileExtensionAllowedByPath(path);
        }        
        
        public bool IsSecretExtensionAllowed(string path)
        {
            return _stegoSystem.StegoConstraints.SecretFileConstraints.IsFileExtensionAllowedByPath(path);
        } 

        public async Task<string> Encrypt(string containerFilePath, string secretDataFilePath, IKey<string> passwordKey, string stegocontainerFilePath)
        {
            return await Task.Run(() => _stegoSystem.Encrypt(containerFilePath, secretDataFilePath, passwordKey,
                    stegocontainerFilePath));
        }
    }
}
