using ConvertersInUWP.ViewModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ConvertersInUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel _mainVm;
        public MainPage()
        {
            this.InitializeComponent();
            _mainVm = App.Current.MainVM;
        }
    }
}
