/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SudkuStegoSystem.DesktopApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using SudkuStegoSystem.DesktopApp.Services;
using System;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private readonly string _defaultPath = Environment.SpecialFolder.MyDocuments.ToString();
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
                        
            SimpleIoc.Default.Register<IFileDialogService, FileDialogService>();
            
            SimpleIoc.Default.Unregister<OutputPathUCVM>();
            SimpleIoc.Default.Register(() => new OutputPathUCVM(_defaultPath, SimpleIoc.Default.GetInstance<IFileDialogService>()));

            SimpleIoc.Default.Register<PasswordUCVM>();
            SimpleIoc.Default.Register<MainVM>();
        }

        public PasswordUCVM PasswordUCVM => ServiceLocator.Current.GetInstance<PasswordUCVM>();

        public OutputPathUCVM OutputPathUCVM => ServiceLocator.Current.GetInstance<OutputPathUCVM>();

        public MainVM MainVM => ServiceLocator.Current.GetInstance<MainVM>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}