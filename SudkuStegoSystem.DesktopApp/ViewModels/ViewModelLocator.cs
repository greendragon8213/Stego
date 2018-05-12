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
using SudkuStegoSystem.Logic;
using SudkuStegoSystem.Logic.Abstract;
using SudkuStegoSystem.Logic.SudokuMethod.SudokuMatrix;
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
            //all this things are singletons. Need to fix that
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IFileDialogService, FileDialogService>();

            SimpleIoc.Default.Register<ISudokuStegoMethod, SudokuStegoMethod_256>();
            SimpleIoc.Default.Register<SudokuMatrixFactory>();
            SimpleIoc.Default.Register<IStegoSystem, SudokuStegoSystem>();

            SimpleIoc.Default.Register<EncryptionUCVM>();

            SimpleIoc.Default.Unregister<OutputPathUCVM>();
            SimpleIoc.Default.Register(() => new OutputPathUCVM(_defaultPath, SimpleIoc.Default.GetInstance<IFileDialogService>()));
            
            SimpleIoc.Default.Register<StatusBarUCVM>();
            SimpleIoc.Default.Register<PasswordUCVM>();
            SimpleIoc.Default.Register<MainVM>();
        }

        public EncryptionUCVM EncryptionUCVM
        {
            get
            {
                IStegoSystem stegoSystem = ServiceLocator.Current.GetInstance<IStegoSystem>();

                DropFileUCVM dropSecretFileVM = new DropFileUCVM(ServiceLocator.Current.GetInstance<IFileDialogService>(),
                    stegoSystem.SecretFileConstraints)
                { FileType = FileTypes.SecretFile };

                DropFileUCVM dropContainerFileVM = new DropFileUCVM(ServiceLocator.Current.GetInstance<IFileDialogService>(),
                    stegoSystem.ContainerFileConstraints)
                { FileType = (stegoSystem.ContainerFileConstraints.FileType == Logic.FileTypes.Images) ? FileTypes.Image : FileTypes.File };

                OutputPathUCVM outputPathVM = ServiceLocator.Current.GetInstance<OutputPathUCVM>();
                PasswordUCVM passwordVM = ServiceLocator.Current.GetInstance<PasswordUCVM>();

                return new EncryptionUCVM(stegoSystem, dropSecretFileVM, dropContainerFileVM, outputPathVM, passwordVM);
            }
        }

        public StatusBarUCVM StatusBarUCVM => ServiceLocator.Current.GetInstance<StatusBarUCVM>();
        public PasswordUCVM PasswordUCVM => ServiceLocator.Current.GetInstance<PasswordUCVM>();
        public OutputPathUCVM OutputPathUCVM => ServiceLocator.Current.GetInstance<OutputPathUCVM>();
        public MainVM MainVM => ServiceLocator.Current.GetInstance<MainVM>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}