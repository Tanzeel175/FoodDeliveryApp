using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FoodDeliveryApp.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFallback = parameter != null && parameter.ToString() == "fallback";

            if (value is string path && !string.IsNullOrEmpty(path))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                if (File.Exists(fullPath))
                {
                    if (isFallback)
                        return Visibility.Collapsed;

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    return bitmap;
                }
            }

            if (isFallback)
                return Visibility.Visible;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
