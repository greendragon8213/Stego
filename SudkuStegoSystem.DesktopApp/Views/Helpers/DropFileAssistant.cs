using System.Windows;

namespace SudkuStegoSystem.DesktopApp
{
    public static class DropFileAssistant
    {
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
