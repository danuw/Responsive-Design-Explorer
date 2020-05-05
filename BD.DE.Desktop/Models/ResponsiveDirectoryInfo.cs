using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BD.DE.Desktop.Models
{
    public class ResponsiveDirectoryInfo : BindableBase
    {
        private string _id;
        private string _name;
        private Dictionary<string, FileSystemInfo> _variants;

        public ResponsiveDirectoryInfo(string name, string rootFolder, FileSystemInfo file)
        {
            // TODO: Complete member initialization
            this._id = name;
            this.Name = file.Name;
            this.Path = rootFolder;
            this.FileSystemInfo = file;
            //this.Variants = new Dictionary<string, string>
            //{
            //    {"Mobile",string.Empty},
            //    {"Tablet",string.Empty},
            //    {"Desktop",string.Empty},
            //};
        }

        public ResponsiveDirectoryInfo(string name, string path)
        {
            this._id = name;
            this.Name = name;
            this.Path = path;
        }

        #region - Props -
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Path { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Fullname { get; set; }
        public bool Exists { get; private set; }
        //public ResponsiveDirectoryInfo Parent { get; set; }
        //public ResponsiveDirectoryInfo Root { get; set; }

        /// <summary>
        /// List of children items (files and folders the current folder
        /// </summary>
        public ObservableCollection<ResponsiveDirectoryInfo> Items { get; set; }
        /// <summary>
        /// Filesystem info of the current item (to ensure relevant full path to a file that existed when we updated this item
        /// </summary>
        public FileSystemInfo FileSystemInfo { get; set; }

        /// <summary>
        /// List of all the variants for that folder accross devices/screen sizes - hopefully allows for a lot more than 3 in the future - work in progress
        /// </summary>
        public Dictionary<string, FileSystemInfo> Variants
        {
            get { return _variants; }
            set { SetProperty(ref _variants, value); }
        }


        public FileSystemInfo Mobile
        {
            get
            {
                if (this.Variants.ContainsKey("Mobile"))
                    return this.Variants["Mobile"];
                return null;
            }
        }

        public FileSystemInfo Desktop
        {
            get
            {
                if (this.Variants.ContainsKey("Desktop"))
                    return this.Variants["Desktop"];
                return null;
            }
        }

        public FileSystemInfo Tablet
        {
            get
            {
                if (this.Variants.ContainsKey("Tablet"))
                    return this.Variants["Tablet"];
                return null;
            }
        }

        public bool HasMobile { get { return Mobile != null; } }
        public bool HasDesktop { get { return Desktop != null; } }
        public bool HasTablet { get { return Tablet != null; } }

        #endregion - Props -

        #region - Public Methods -


        public void AnalyseChildren()
        {
            if (Directory.Exists(this.Path))
            //if (Directory.Exists(System.IO.Path.Combine(this.Path, this.Name)))
            {
                var mobilePath = string.Empty;
                var desktopPath = string.Empty;
                var tabletPath = string.Empty;

                if (this.Path.StartsWith(ExplorerFilesViewModel.GetMobilePath(Properties.Settings.Default.RootFolder)))
                {
                    mobilePath = this.Path;
                    desktopPath = this.Path.Replace(ExplorerFilesViewModel.GetMobilePath(Properties.Settings.Default.RootFolder),
                        ExplorerFilesViewModel.GetDesktopPath(Properties.Settings.Default.RootFolder));
                    tabletPath =
                        this.Path.Replace(ExplorerFilesViewModel.GetMobilePath(Properties.Settings.Default.RootFolder),
                            ExplorerFilesViewModel.GetTabletPath(Properties.Settings.Default.RootFolder));

                }
                else if (this.Path.StartsWith(ExplorerFilesViewModel.GetDesktopPath(Properties.Settings.Default.RootFolder)))
                {
                    mobilePath = this.Path.Replace(ExplorerFilesViewModel.GetDesktopPath(Properties.Settings.Default.RootFolder),
                        ExplorerFilesViewModel.GetMobilePath(Properties.Settings.Default.RootFolder));
                    desktopPath = this.Path;
                    tabletPath = this.Path.Replace(ExplorerFilesViewModel.GetDesktopPath(Properties.Settings.Default.RootFolder),
                            ExplorerFilesViewModel.GetTabletPath(Properties.Settings.Default.RootFolder));
                }
                else if (this.Path.StartsWith(ExplorerFilesViewModel.GetDesktopPath(Properties.Settings.Default.RootFolder)))
                {
                    mobilePath = this.Path.Replace(ExplorerFilesViewModel.GetTabletPath(Properties.Settings.Default.RootFolder),
                        ExplorerFilesViewModel.GetMobilePath(Properties.Settings.Default.RootFolder));
                    desktopPath = this.Path.Replace(ExplorerFilesViewModel.GetTabletPath(Properties.Settings.Default.RootFolder),
                        ExplorerFilesViewModel.GetDesktopPath(Properties.Settings.Default.RootFolder));
                    tabletPath = this.Path;
                }
                else
                {
                    Console.WriteLine("Really??!!");
                }
                // Get Children
                //new DirectoryInfo(name).GetFileSystemInfos().ToList().ForEach(a => );
                UpdateFolderItems(mobilePath, "Mobile");
                UpdateFolderItems(desktopPath, "Desktop");
                UpdateFolderItems(tabletPath, "Tablet");
            }
        }

        private void UpdateFolderItems(string path, string name)
        {
            if (Directory.Exists(path))
            {
                var mobile = new DirectoryInfo(path).GetFileSystemInfos().ToList();
                mobile.ForEach(f => AddOrUpdate(f, System.IO.Path.Combine(path, f.Name), name));
            }
        }

        public ObservableCollection<ResponsiveDirectoryInfo> GetFiles()
        {
            return this.Items;
        }

        #endregion - Public Methods -

        #region - Private methods -

        private void AddOrUpdate(FileSystemInfo file, string path, string device)
        {
            var corename = CleanupFileName(file.Name, device);
            if (this.Items == null)
            {
                this.Items = new ObservableCollection<ResponsiveDirectoryInfo>();
            }
            var item = this.Items.FirstOrDefault(f => f.Id.ToLower().Equals(corename.ToLower()));
            if (item == null)
            {
                this.Items.Add(new ResponsiveDirectoryInfo(corename, path, file) { Variants = new Dictionary<string, FileSystemInfo> { { device, file } } });
            }
            else
            {
                item.Variants.Add(device, file);
            }
        }

        private string CleanupFileName(string name, string device)
        {
            return name.ToLower().Replace(device.ToLower(), "").Replace(" ", "").Replace("_", "").Replace("-", "");

            // todo: convert to a regex replacement
            var pattern = @"";
            var replacement = "";
            var rgx = new Regex(pattern);
            var result = rgx.Replace(name, replacement);

            Console.WriteLine("Original String:    '{0}'", name);
            Console.WriteLine("Replacement String: '{0}'", result);
        }

        #endregion
    }
}
