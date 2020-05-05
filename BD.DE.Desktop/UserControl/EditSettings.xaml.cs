using System.IO;
using BD.DE.Desktop.Models;
using Microsoft.Win32;

namespace BD.DE.Desktop.UserControl
{
    /// <summary>
    /// Interaction logic for EditSettings.xaml
    /// </summary>
    public partial class EditSettings : System.Windows.Controls.UserControl
    {
        public EditSettingViewModel ViewModel { get; set; }
        public EditSettings()
        {
            InitializeComponent();
            this.ViewModel = new EditSettingViewModel();
            this.DataContext = this.ViewModel;
        }
    }

    public class EditSettingViewModel : BaseViewModel
    {
        private string _rootPath;
        private string _desktopFolderName;
        private string _mobileFolderName;
        private string _tabletFolderName;

        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                _rootPath = value;
                OnPropertyChanged("RootPath");
            }
        }

        public string DesktopFolderName
        {
            get { return _desktopFolderName; }
            set
            {
                _desktopFolderName = value;
                OnPropertyChanged("DesktopFolderName");
            }
        }

        public string MobileFolderName
        {
            get { return _mobileFolderName; }
            set
            {
                _mobileFolderName = value;
                OnPropertyChanged("MobileFolderName");
            }
        }

        public string TabletFolderName
        {
            get { return _tabletFolderName; }
            set
            {
                _tabletFolderName = value;
                OnPropertyChanged("TabletFolderName");
            }
        }

        public EditSettingViewModel()
        {
            this.Reset();
            SaveCommand = new RelayCommand(OnSave, () => true);
            BrowseCommand = new RelayCommand(OnBrowse, () => true);
        }

        private void OnBrowse()
        {
            var openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = this._rootPath;
            openFileDialog.FileName = "AnyFile";
            openFileDialog.Filter = string.Empty;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = false;

            if (openFileDialog.ShowDialog() == true)
            {
                this.RootPath = Path.GetDirectoryName(this._rootPath = openFileDialog.FileName);
            }

        }

        private void OnSave()
        {
            Properties.Settings.Default.RootFolder = this.RootPath;
            Properties.Settings.Default.DesktopFolderName = this.DesktopFolderName;
            Properties.Settings.Default.TabletFolderName = this.TabletFolderName;
            Properties.Settings.Default.MobileFolderName = this.MobileFolderName;
        }

        public void Reset()
        {
            this.RootPath = Properties.Settings.Default.RootFolder;
            this.DesktopFolderName = Properties.Settings.Default.DesktopFolderName;
            this.TabletFolderName = Properties.Settings.Default.TabletFolderName;
            this.MobileFolderName = Properties.Settings.Default.MobileFolderName;
        }

        public RelayCommand SaveCommand
        {
            get;
            private set;
        }

        public RelayCommand BrowseCommand
        {
            get;
            private set;
        }

    }
}
