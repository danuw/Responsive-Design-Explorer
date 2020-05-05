using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using BD.DE.Desktop.Models;

namespace BD.DE.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for DeviceDesignView.xaml
    /// </summary>
    public partial class DeviceDesignView : System.Windows.Controls.UserControl
    {
        public DeviceDesignView()
        {
            InitializeComponent();
        }

        private void OnOpenLocation(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is FileSystemInfo)
            {
                var filelocation = ((FileSystemInfo)this.DataContext).FullName;
                Process.Start("explorer.exe", @"/select, " + filelocation);
            }

        }
    }
}
