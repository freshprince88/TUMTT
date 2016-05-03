using Caliburn.Micro;
using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Converters;
using TT.Models;
using TT.Scouter.ViewModels;

namespace TT.Scouter.Util.Converter
{
    [ValueConversion(typeof(Schlag), typeof(IScreen))]
    public class SchlagToViewConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Schlag stroke = (Schlag)value;
            if (stroke.Nummer > 1)
                return new ServiceDetailViewModel(stroke);
            else
                return new SchlagDetailViewModel(stroke);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
