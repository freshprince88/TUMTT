using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace TT.Converters

{
    [ValueConversion(typeof(ICollection<string>), typeof(BitmapImage))]
    public class ImageUrlConcatConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = "";
            foreach (var value in values)
            {
                result += value.ToString();
            }

            BitmapImage image = null;
            try
            {
                image = new BitmapImage(new Uri(result, UriKind.Absolute));
            }
            catch { }
            return image;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

