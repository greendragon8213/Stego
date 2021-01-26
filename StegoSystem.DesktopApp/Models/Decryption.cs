using StegoSystem.Models;
using System.Threading.Tasks;
using constraints = StegoSystem.Constraints.StegoConstraints
    <StegoSystem.FileTypeConstraints, StegoSystem.FileTypeConstraints, StegoSystem.FileTypeConstraints>;

namespace StegoSystem.DesktopApp.Models
{
    public class Decryption : PasswordBasedSystem
    {
        private readonly IStegoSystem<string, constraints> _stegoSystem;

        public Decryption(IStegoSystem<string, constraints> stegoSystem)
        {
            _stegoSystem = stegoSystem;
        }

        public bool IsStegocontainerExtensionAllowed(string path)
        {
            return _stegoSystem.StegoConstraints.StegoContainerFileConstraints.IsFileExtensionAllowedByPath(path);
        }

        public async Task<string> Decrypt(string stegocontainerFilePath, IKey<string> password, string restoredSecretFilePath)
        {
            return await Task.Run(() => _stegoSystem.Decrypt(stegocontainerFilePath, password,
                    restoredSecretFilePath));
        }
    }
}
