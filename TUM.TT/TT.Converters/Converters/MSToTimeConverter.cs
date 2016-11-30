using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{
    public class MSToTimeConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            TimeSpan ts = TimeSpan.FromMilliseconds(val);
            return String.Format("{0:D0}:{1:D2}:{2:D2},{3:D2}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { string test = (String)value;
            try {
                 
            TimeSpan span = TimeSpan.Parse((string)value);
                //double min = span.TotalMinutes;
                //double milli = span.TotalMilliseconds;
                //return TimeSpan.FromMilliseconds(milli);
                //return min;
                return span.TotalMilliseconds;
            }
            catch (OverflowException)
            {
                return "Fehler";
            }
            catch (FormatException)
            {
                return "Fehler";
            }
        }
    }
}
