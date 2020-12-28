using GalaSoft.MvvmLight;

namespace SudkuStegoSystem.DesktopApp.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainVM : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainVM(EncryptionUCVM encryptionUCVM, DecryptionUCVM decryptionUCVM)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            EncryptionUCVM = encryptionUCVM;
            DecryptionUCVM = decryptionUCVM;
        }
        
        public EncryptionUCVM EncryptionUCVM { get; }
        public DecryptionUCVM DecryptionUCVM { get; }

        public string Title => "SudoStego";
    }
}