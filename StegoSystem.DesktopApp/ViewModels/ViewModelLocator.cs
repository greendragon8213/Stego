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
using StegoSystem;
using StegoSystem.DesktopApp.Models;
using StegoSystem.Sudoku;
using StegoSystem.Sudoku.Matrix;
using StegoSystem.Sudoku.Method256;
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
        private static readonly string _defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private const string DropSecretFileKey = "DropSecretFile";
        private const string DropContainerFileKey = "DropContainerFile";
        private const string DropStegoContainerFileKey = "DropStegoContainerFile";

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterDomainServices();

            RegisterAppServices();
        }

        //for data binding
        public StatusBarUCVM StatusBarUCVM => ServiceLocator.Current.GetInstance<StatusBarUCVM>();
        public MainVM MainVM => ServiceLocator.Current.GetInstance<MainVM>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        #region Private methods

        private static void RegisterDomainServices()
        {
            SimpleIoc.Default.Register<ISudokuStegoMethod<byte>, SudokuStegoMethod256>(true);
            SimpleIoc.Default.Register<ISudokuMatrixFactory<byte, string>, SudokuByPasswordMatrixFactory<byte>>(true);
            SimpleIoc.Default.Register<IStegoSystem<string>, SudokuStegoSystem<byte, string>>(true);
        }

        private static void RegisterAppServices()
        {
            //Models
            SimpleIoc.Default.Register<Encryption>();
            SimpleIoc.Default.Register<Decryption>();

            //UI
            SimpleIoc.Default.Register<IFileDialogService, FileDialogService>(true); //single instance per app is ok
            SimpleIoc.Default.Register<IFolderDialogService, FolderDialogService>(); //single instance per app is ok

            //UI VMs
            var stegoSystem = ServiceLocator.Current.GetInstance<IStegoSystem<string>>();
            SimpleIoc.Default.Register(() => CreateDropFileUCVM(stegoSystem.SecretFileConstraints, FileTypes.SecretFile), DropSecretFileKey);
            SimpleIoc.Default.Register(() => CreateDropFileUCVM(stegoSystem.ContainerFileConstraints), DropContainerFileKey);
            SimpleIoc.Default.Register(() => CreateDropFileUCVM(stegoSystem.StegoContainerFileConstraints), DropStegoContainerFileKey);

            //I need them to be new every time requested, but for test it's ok to use:
            //SimpleIoc.Default.Register(() => new PasswordUCVM());
            //SimpleIoc.Default.Register(() => new OutputPathUCVM(_defaultPath, SimpleIoc.Default.GetInstance<IFolderDialogService>()));

            SimpleIoc.Default.Register(() => CreateEncryptionUCVM);
            SimpleIoc.Default.Register(() => CreateDecryptionUCVM);

            SimpleIoc.Default.Register(() => StatusBarUCVM.GetInstance());
            SimpleIoc.Default.Register<MainVM>();
        }

        private static EncryptionUCVM CreateEncryptionUCVM
        {
            get
            {
                DropFileUCVM dropSecretFileVM = ServiceLocator.Current.GetInstance<DropFileUCVM>(DropSecretFileKey);
                DropFileUCVM dropContainerFileVM = ServiceLocator.Current.GetInstance<DropFileUCVM>(DropContainerFileKey);
                OutputPathUCVM outputPathVM = CreateOutputPathUCVM();
                PasswordUCVM passwordVM = CreateNewPasswordUCVM();
                Encryption enc = ServiceLocator.Current.GetInstance<Encryption>();

                return new EncryptionUCVM(enc, dropSecretFileVM, dropContainerFileVM, outputPathVM, passwordVM);
            }
        }

        private static DecryptionUCVM CreateDecryptionUCVM
        {
            get
            {
                DropFileUCVM dropStegoContainerFileVM = ServiceLocator.Current.GetInstance<DropFileUCVM>(DropStegoContainerFileKey);
                OutputPathUCVM outputPathVM = CreateOutputPathUCVM();
                PasswordUCVM passwordVM = CreateNewPasswordUCVM();
                Decryption decr = ServiceLocator.Current.GetInstance<Decryption>();

                return new DecryptionUCVM(decr, dropStegoContainerFileVM, outputPathVM, passwordVM);
            }
        }

        private static DropFileUCVM CreateDropFileUCVM(FileTypeConstraints fileTypeConstraints, FileTypes? fileType = null)
        {
            FileTypes fileTypeFilled = (fileType != null) ? (FileTypes)fileType :
                (fileTypeConstraints.FileType == StegoSystem.FileTypes.Images) ? FileTypes.Image : FileTypes.File;

            return new DropFileUCVM(ServiceLocator.Current.GetInstance<IFileDialogService>(), fileTypeConstraints, fileTypeFilled);
        }

        private static PasswordUCVM CreateNewPasswordUCVM() => new PasswordUCVM();

        private static OutputPathUCVM CreateOutputPathUCVM() => new OutputPathUCVM(_defaultPath, SimpleIoc.Default.GetInstance<IFolderDialogService>());

        #endregion
    }
}