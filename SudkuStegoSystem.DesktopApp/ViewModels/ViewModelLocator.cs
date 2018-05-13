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
        private readonly string _defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            //all this things are singletons.
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IFileDialogService, FileDialogService>();

            SimpleIoc.Default.Register<ISudokuStegoMethod, SudokuStegoMethod_256>();
            SimpleIoc.Default.Register<SudokuMatrixFactory>();
            SimpleIoc.Default.Register<IStegoSystem, SudokuStegoSystem>();

            SimpleIoc.Default.Register<StatusBarUCVM>();
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

                OutputPathUCVM outputPathVM = OutputPathUCVM;//ServiceLocator.Current.GetInstance<OutputPathUCVM>();
                PasswordUCVM passwordVM = PasswordUCVM;//ServiceLocator.Current.GetInstance<PasswordUCVM>();

                return new EncryptionUCVM(stegoSystem, dropSecretFileVM, dropContainerFileVM, outputPathVM, passwordVM);
            }
        }

        public DecryptionUCVM DecryptionUCVM
        {
            get
            {
                IStegoSystem stegoSystem = ServiceLocator.Current.GetInstance<IStegoSystem>();

                DropFileUCVM dropStegoContainerFileVM = new DropFileUCVM(ServiceLocator.Current.GetInstance<IFileDialogService>(),
                    stegoSystem.StegoContainerFileConstraints)
                { FileType = (stegoSystem.StegoContainerFileConstraints.FileType == Logic.FileTypes.Images) ? FileTypes.Image : FileTypes.File };

                OutputPathUCVM outputPathVM = OutputPathUCVM;//ServiceLocator.Current.GetInstance<OutputPathUCVM>();
                PasswordUCVM passwordVM = PasswordUCVM;//ServiceLocator.Current.GetInstance<PasswordUCVM>();

                return new DecryptionUCVM(stegoSystem, dropStegoContainerFileVM, outputPathVM, passwordVM);
            }
        }

        public StatusBarUCVM StatusBarUCVM => ServiceLocator.Current.GetInstance<StatusBarUCVM>();

        public PasswordUCVM PasswordUCVM => new PasswordUCVM();
        public OutputPathUCVM OutputPathUCVM => new OutputPathUCVM(_defaultPath, SimpleIoc.Default.GetInstance<IFileDialogService>());

        public MainVM MainVM => new MainVM(EncryptionUCVM, DecryptionUCVM);

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}