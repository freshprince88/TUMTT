using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TT.Converters
{
    public class CamelCaseConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string enumString = value.ToString();
            string camelCaseString = Regex.Replace(enumString, "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]))", "$1 ").ToLower();
            return char.ToUpper(camelCaseString[0]) + camelCaseString.Substring(1);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}