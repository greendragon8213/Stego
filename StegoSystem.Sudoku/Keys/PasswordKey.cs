using StegoSystem.Models;
using System.Text.RegularExpressions;

namespace StegoSystem.Sudoku.Keys
{
    public class PasswordKey : IKey<string>
    {
        private const string KeyName = "Password";
        private const string KeyRegex = "^[a-zA-Z0-9]{6,30}$";
        private static string RegexDescription = $"{KeyName} can consist of numbers and letters. {KeyName} length must be in range of 6-30";

        public string GetKeyName => KeyName;
        public string Payload { get; private set; }

        public PasswordKey(string password)
        {
            Payload = password;
        }

        public string ValidationDescription => RegexDescription;

        public bool IsValid() => !string.IsNullOrEmpty(Payload) && Regex.Match(Payload, KeyRegex).Success;
    }
}