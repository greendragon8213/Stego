using StegoSystem.GeneralLogic.Common;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace StegoSystem.GeneralLogic.Abstract
{
    public abstract class FileTypeConstraints
    {
        private string _filter = string.Empty;
        
        public abstract FileTypes FileType { get; }
        public abstract string[] AllowedExtensions { get; }

        public bool IsFileExtensionAllowedByPath(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            return IsFileExtensionAllowed(extension);
        }

        public bool IsFileExtensionAllowed(string extension)
        {            
            if (!string.IsNullOrEmpty(extension) && extension[0] == '.')
            {
                extension = extension.Remove(0, 1);
            }

            return (AllowedExtensions.Count() == 0
                || AllowedExtensions.Any(ae => extension.Equals(ae, StringComparison.InvariantCultureIgnoreCase)));
        }

        public string GetFilter()
        {
            if (!string.IsNullOrEmpty(_filter))
            {
                return _filter;
            }

            if(FileType == FileTypes.AnyFiles || AllowedExtensions.Count() == 0)
            {
                _filter = "All files |*.*";
                return _filter;
            }

            StringBuilder filter = new StringBuilder();

            foreach(var ae in AllowedExtensions)
            {
                filter.Append($"{ae.ToUpper()} Files (*.{ae.ToLower()})|*.{ae.ToLower()}|");
            }

            filter.Remove(filter.Length-1, 1);

            _filter = filter.ToString();
            return _filter;
        }
    }
}
