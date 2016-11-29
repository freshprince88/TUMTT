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
            float saturationFactor;
            float brightnessFactor;
            if (parameter == null || (int)parameter == 0)
            {
                saturationFactor = -0.15f;
                brightnessFactor = 0.10f;
            } else if ((int)parameter == 1)
            {
                saturationFactor = -0.25f;
                brightnessFactor = 0.35f;
            } else
            {
                saturationFactor = -0.38f;
                brightnessFactor = 0.42f;
            }

            var brushConverter = new BrushConverter();
            switch ((MatchPlayer) value)
            {
                default:
                case MatchPlayer.First:
                    var firstColor = System.Drawing.Color.FromArgb(0xff, 0x4f, 0x81, 0xbd);
                    return new SolidColorBrush(FromAhsb(firstColor.A, firstColor.GetHue(), firstColor.GetSaturation() + saturationFactor, firstColor.GetBrightness() + brightnessFactor));
                case MatchPlayer.Second:
                    var secondColor = System.Drawing.Color.FromArgb(0xff, 0xc0, 0x50, 0x4d);
                    return new SolidColorBrush(FromAhsb(secondColor.A, secondColor.GetHue(), secondColor.GetSaturation() + saturationFactor, secondColor.GetBrightness() + brightnessFactor));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a Color from alpha, hue, saturation and brightness.
        /// </summary>
        /// <param name="alpha">The alpha channel value.</param>
        /// <param name="hue">The hue value.</param>
        /// <param name="saturation">The saturation value.</param>
        /// <param name="brightness">The brightness value.</param>
        /// <returns>A Color with the given values.</returns>
        public static Color FromAhsb(byte alpha, float hue, float saturation, float brightness)
        {
            alpha = Math.Min(Math.Max(alpha, (byte)0), (byte)255);
            hue = Math.Min(Math.Max(hue, 0), 360);
            saturation = Math.Min(Math.Max(saturation, 0), 1);
            brightness = Math.Min(Math.Max(brightness, 0), 1);

            if (0 == saturation)
            {
                return Color.FromArgb(
                                    alpha,
                                    System.Convert.ToByte(brightness * 255),
                                    System.Convert.ToByte(brightness * 255),
                                    System.Convert.ToByte(brightness * 255));
            }

            float fMax, fMid, fMin;
            int iSextant;
            byte iMax, iMid, iMin;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = System.Convert.ToByte(fMax * 255);
            iMid = System.Convert.ToByte(fMid * 255);
            iMin = System.Convert.ToByte(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(alpha, iMax, iMid, iMin);
            }
        }        
    }
}

