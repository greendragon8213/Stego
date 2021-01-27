using StegoSystem.Constraints;
using StegoSystem.Models;

namespace StegoSystem
{
    /// <summary>
    /// Represents stego system functionality in general
    /// </summary>
    public interface IStegoSystem<T, out TConstraints> where TConstraints : StegoConstraints<FileTypeConstraints, FileTypeConstraints, FileTypeConstraints>
    {
        TConstraints StegoConstraints { get; }

        string Encrypt(string containerFilePath, byte[] secret, IKey<T> key, string stegocontainerFilePath);

        /// <summary>
        /// Embeds secret data (from secretDataFilePath file) into container with key
        /// </summary>
        /// <param name="containerFilePath"></param>
        /// <param name="secretDataFilePath"></param>
        /// <param name="key"></param>
        /// <returns>Path to stogocontainer</returns>
        string Encrypt(string containerFilePath, string secretDataFilePath, IKey<T> key, string stegocontainerFilePath);

        byte[] Decrypt(string stegocontainerFilePath, IKey<T> key);

        /// <summary>
        /// Restores secret data from stegocontainer with key
        /// </summary>
        /// <param name="stegocontainer"></param>
        /// <param name="key"></param>
        /// <returns>Path to restored data</returns>
        string Decrypt(string stegocontainerFilePath, IKey<T> key, string restoredSecretFilePath);
    }
}
