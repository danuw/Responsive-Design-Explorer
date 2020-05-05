using System.Collections.ObjectModel;
using System.IO;

namespace BD.DE.Desktop.Models
{
    public class ExplorerFilesViewModel
    {
        public ExplorerFilesViewModel(string rootFolder)
        {

            var test = new ResponsiveDirectoryInfo("Designs", GetMobilePath(rootFolder));
            test.AnalyseChildren();
            this.Test = new ObservableCollection<ResponsiveDirectoryInfo>(test.Items);
        }

        public static string GetTabletPath(string rootFolder)
        {
            return Path.Combine(rootFolder, Properties.Settings.Default.TabletFolderName);
        }

        public static string GetDesktopPath(string rootFolder)
        {
            return Path.Combine(rootFolder, Properties.Settings.Default.DesktopFolderName);
        }

        public static string GetMobilePath(string rootFolder)
        {
            return Path.Combine(rootFolder, Properties.Settings.Default.MobileFolderName);
        }
        public ObservableCollection<ResponsiveDirectoryInfo> Test { get; set; }
    }
}