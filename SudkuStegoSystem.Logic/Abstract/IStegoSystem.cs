using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace SudkuStegoSystem.Logic.Abstract
{
    public interface IStegoSystem
    {
        //ToDo Image here??? 
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
