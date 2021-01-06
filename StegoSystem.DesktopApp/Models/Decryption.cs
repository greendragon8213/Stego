using StegoSystem.Models;
using StegoSystem.Sudoku.Keys;
using System.Threading.Tasks;

namespace StegoSystem.DesktopApp.Models
{
    public class Decryption : PasswordBasedSystem
    {
        private readonly IStegoSystem<string> _stegoSystem;

        public Decryption(IStegoSystem<string> stegoSystem)
        {
            _stegoSystem = stegoSystem;
        }

        public bool IsStegocontainerExtensionAllowed(string path)
        {
            return _stegoSystem.StegoContainerFileConstraints.IsFileExtensionAllowedByPath(path);
        }

        public async Task<string> Decrypt(string stegocontainerFilePath, IKey<string> password, string restoredSecretFilePath)
        {
            return await Task.Run(() => _stegoSystem.Decrypt(stegocontainerFilePath, password,
                    restoredSecretFilePath));
        }
    }
}
