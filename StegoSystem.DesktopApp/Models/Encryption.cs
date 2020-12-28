namespace StegoSystem.DesktopApp.Models
{
    public class Encryption
    {
        private readonly IStegoSystem _stegoSystem;

        public Encryption(IStegoSystem stegoSystem)
        {
            _stegoSystem = stegoSystem;
        }

        public string Encrypt(string containerFilePath, string secretDataFilePath, string password, string stegocontainerFilePath)
        {
            return _stegoSystem.Encrypt(containerFilePath, secretDataFilePath, password,
                    stegocontainerFilePath);
        }
    }
}
