using SudkuStegoSystem.DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SudkuStegoSystem.DesktopApp
{
    public static class DropFileAssistant
    {
        #region FileType

        public static readonly DependencyProperty FileType =
          DependencyProperty.RegisterAttached("FileType", typeof(FileTypes), typeof(DropFileAssistant), 
              new PropertyMetadata(FileTypes.File));

        public static FileTypes GetFileType(DependencyObject dp)
        {
            return (FileTypes)dp.GetValue(FileType);
        }

        public static void SetFileType(DependencyObject dp, FileTypes value)
        {
            dp.SetValue(FileType, value);
        }

        #endregion

        #region Content
        public static readonly DependencyProperty Content =
          DependencyProperty.RegisterAttached("Content", typeof(string), typeof(DropFileAssistant),
              new PropertyMetadata(string.Empty));

        public static string GetContent(DependencyObject dp)
        {
            return (string)dp.GetValue(Content);
        }

        public static void SetContent(DependencyObject dp, string value)
        {
            dp.SetValue(Content, value);
        }

        #endregion
    }
}
