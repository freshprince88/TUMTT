using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TT.Models;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(Brush))]
    public class MatchPlayerToBrushConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var firstColor = System.Drawing.Color.FromArgb(0xff, 0x4f, 0x81, 0xbd);
            var secondColor = System.Drawing.Color.FromArgb(0xff, 0xc0, 0x50, 0x4d);

            float saturationFactor;
            float brightnessFactor;
            if (parameter == null || (int)parameter == 0)
            {
                saturationFactor = -0.25f;
                brightnessFactor = 0.1f;
            } else if ((int)parameter == 1)
            {
                saturationFactor = -0.4f;
                brightnessFactor = 0.2f;
            } else
            {
                saturationFactor = -0.6f;
                brightnessFactor = 0.3f;
            }

            var brushConverter = new BrushConverter();
            switch ((MatchPlayer) value)
            {
                // original player colors from report:
                // First:   #FF4F81BD"
                // Second:  #FFC0504D"
                // new colors: saturation - 25, brightness + 10
                default:
                case MatchPlayer.First: return new SolidColorBrush(HSL2RGB(firstColor.GetHue(), firstColor.GetSaturation() + saturationFactor, firstColor.GetBrightness() + brightnessFactor)); //(Brush)brushConverter.ConvertFromString("#ff90afd6");
                case MatchPlayer.Second: return new SolidColorBrush(HSL2RGB(secondColor.GetHue(), secondColor.GetSaturation() + saturationFactor, secondColor.GetBrightness() + brightnessFactor));//(Brush)brushConverter.ConvertFromString("#ffd98f8d");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb = new Color();
            rgb.R = System.Convert.ToByte(r * 255.0f);
            rgb.G = System.Convert.ToByte(g * 255.0f);
            rgb.B = System.Convert.ToByte(b * 255.0f);
            return rgb;
        }
    }
}

