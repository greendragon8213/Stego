namespace SudkuStegoSystem.Logic.Abstract
{
    /// <summary>
    /// Represents stego system functionality in general
    /// </summary>
    public interface IStegoSystem
    {
        /// <summary>
        /// Embeds secret data (from secretDataFilePath file) into container with key
        /// </summary>
        /// <param name="container"></param>
        /// <param name="secretDataFilePath"></param>
        /// <param name="key"></param>
        /// <returns>Path to stogocontainer</returns>
        void Encrypt(string containerFilePath, string secretDataFilePath, string key, string stegocontainerFilePath = null);

        /// <summary>
        /// Restores secret data from stegocontainer with key
        /// </summary>
        /// <param name="stegocontainer"></param>
        /// <param name="key"></param>
        /// <returns>Path to restored data</returns>
        void Decrypt(string stegocontainerFilePath, string key, string restoredSecretFilePath = null);
    }
}
