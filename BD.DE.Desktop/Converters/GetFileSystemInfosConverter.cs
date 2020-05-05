using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using BD.DE.Desktop.Models;

namespace BD.DE.Desktop.Converters
{
    public class GetFileSystemInfosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            try
            {
                if (value is ResponsiveDirectoryInfo)
                {
                    ((ResponsiveDirectoryInfo) value).AnalyseChildren();
                    return ((ResponsiveDirectoryInfo)value).GetFiles();
                }
            }
            catch { }
            return null;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
