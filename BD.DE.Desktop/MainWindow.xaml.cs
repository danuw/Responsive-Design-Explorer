using System.IO;
using System.Windows;
using BD.DE.Desktop.Models;

namespace BD.DE.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ExplorerFilesViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            UpdateContext();
        }

        public void UpdateContext()
        {
            var itsOk = true;
            if (Directory.Exists(Properties.Settings.Default.RootFolder))
            {

                if (!Directory.Exists(ExplorerFilesViewModel.GetMobilePath(Properties.Settings.Default.RootFolder)))
                {
                    itsOk = false;
                }
                if (!Directory.Exists(ExplorerFilesViewModel.GetTabletPath(Properties.Settings.Default.RootFolder)))
                {
                    itsOk = false;
                }
                if (!Directory.Exists(ExplorerFilesViewModel.GetDesktopPath(Properties.Settings.Default.RootFolder)))
                {
                    itsOk = false;
                }
            }
            else
            {
                itsOk = false;
            }
            if (itsOk)
            {
                ViewModel = new ExplorerFilesViewModel(Properties.Settings.Default.RootFolder);

                DataContext = this.ViewModel;
            }
            else
            {
                menuEditSettings.IsChecked = true;
                MessageBox.Show(
                    string.Format("One or more of the indicated folders could not be found. Does {0} exist? Please check your settings are pointing to a valid root folder and its three configured {1}, {2} and {3} children...",
                        Properties.Settings.Default.RootFolder, Properties.Settings.Default.MobileFolderName, Properties.Settings.Default.DesktopFolderName, Properties.Settings.Default.TabletFolderName), 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // TODO: save latest visited designs to a last viewed items list

            //if (menuViewAutoSelect.IsChecked)
            //{
            //    var dataSource = this.ViewModel;
            //    try
            //    {

            //        var source = e.OriginalSource;
            //        var newValue = e.NewValue;

            //        //var desktopSelectedItem = (from i in (IEnumerable<DirectoryInfo>)tvDesktop.Items where i.Name.EndsWith(newValue.ToString().ToLower().Replace("mobile", "")) select i);
            //        var desktopItem = (from s in dataSource.Desktop where s.Name.ToString().ToLower().Replace("desktop", "").Replace(" ", "").Replace("_", "").Replace("-", "").EndsWith(newValue.ToString().ToLower().Replace("mobile", "").Replace(" ", "").Replace("_", "").Replace("-", "")) select s).FirstOrDefault();
            //        if (desktopItem == null)
            //        {
            //            desktopItem = dataSource.Desktop.FirstOrDefault(s => s.Name.EndsWith(".png"));
            //        }
            //    }
            //    catch (Exception)
            //    {

            //        //throw;
            //    }
            //}
        }

        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            UpdateContext();
        }

        private void OnHelp(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please visit https://github.com/danuw/Responsive-Design-Explorer/blob/master/README.md", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}