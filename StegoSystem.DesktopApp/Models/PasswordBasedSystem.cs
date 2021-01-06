using StegoSystem.Models;
using StegoSystem.Sudoku.Keys;

namespace StegoSystem.DesktopApp.Models
{
    public abstract class PasswordBasedSystem
    {
        public IKey<string> CreatePassword(string password)
        {
            return new PasswordKey(password);
        }
    }
}
