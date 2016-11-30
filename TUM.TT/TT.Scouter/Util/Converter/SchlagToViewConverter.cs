using Caliburn.Micro;
using System;
using System.Globalization;
using System.Windows.Data;
using TT.Converters;
using TT.Lib.Managers;
using TT.Models;
using TT.Scouter.ViewModels;

namespace TT.Scouter.Util.Converter
{
    [ValueConversion(typeof(Stroke), typeof(IScreen))]
    public class SchlagToViewConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke stroke = (Stroke)values[0];
            IMatchManager Manager = (IMatchManager)values[1];
            Rally currentRally = (Rally)values[2];
            if (stroke.Number > 1)
                return new ServiceDetailViewModel(stroke, Manager, currentRally);
            else
                return new StrokeDetailViewModel(stroke, Manager, currentRally);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}
