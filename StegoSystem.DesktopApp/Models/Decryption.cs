using StegoSystem.Models;
using StegoSystem.Sudoku.Keys;

namespace StegoSystem.DesktopApp.Models
{
    public class Decryption
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

        public IKey<string> CreatePassword(string password)
        {
            return new PasswordKey(password);
        }

        public string Decrypt(string stegocontainerFilePath, IKey<string> password, string restoredSecretFilePath)
        {
            return _stegoSystem.Decrypt(stegocontainerFilePath, password,
                    restoredSecretFilePath);
        }
    }
}
