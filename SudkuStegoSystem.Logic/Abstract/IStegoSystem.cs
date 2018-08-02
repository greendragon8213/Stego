namespace StegoSystem.GeneralLogic.Abstract
{
    /// <summary>
    /// Represents stego system functionality in general
    /// </summary>
    public interface IStegoSystem
    {
        FileTypeConstraints ContainerFileConstraints { get; }
        FileTypeConstraints StegoContainerFileConstraints { get; }
        FileTypeConstraints SecretFileConstraints { get; }

        /// <summary>
        /// Embeds secret data (from secretDataFilePath file) into container with key
        /// </summary>
        /// <param name="containerFilePath"></param>
        /// <param name="secretDataFilePath"></param>
        /// <param name="key"></param>
        /// <returns>Path to stogocontainer</returns>
        string Encrypt(string containerFilePath, string secretDataFilePath, string key, string stegocontainerFilePath = null);

        /// <summary>
        /// Restores secret data from stegocontainer with key
        /// </summary>
        /// <param name="stegocontainer"></param>
        /// <param name="key"></param>
        /// <returns>Path to restored data</returns>
        string Decrypt(string stegocontainerFilePath, string key, string restoredSecretFilePath = null);
    }
}
