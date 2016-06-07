using Caliburn.Micro;
using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Converters;
using TT.Lib.Managers;
using TT.Models;
using TT.Scouter.ViewModels;

namespace TT.Scouter.Util.Converter
{
    [ValueConversion(typeof(Schlag), typeof(IScreen))]
    public class SchlagToViewConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Schlag stroke = (Schlag)values[0];
            IMatchManager Manager = (IMatchManager)values[1];
            if (stroke.Nummer > 1)
                return new ServiceDetailViewModel(stroke, Manager);
            else
                return new SchlagDetailViewModel(stroke,Manager);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}
