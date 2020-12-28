namespace StegoSystem.DesktopApp.Models
{
    public class Decryption
    {
        private readonly IStegoSystem _stegoSystem;

        public Decryption(IStegoSystem stegoSystem)
        {
            _stegoSystem = stegoSystem;
        }

        public string Decrypt(string stegocontainerFilePath, string password, string restoredSecretFilePath)
        {
            return _stegoSystem.Decrypt(stegocontainerFilePath, password,
                    restoredSecretFilePath);
        }
    }
}
