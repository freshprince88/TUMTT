using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(string))]
    public class MatchPlayerToStringConverter : DependencyObject, IValueConverter
    {
        public Player Player1 {
            get { return (Player)this.GetValue(Player1Property); }
            set { this.SetValue(Player1Property, value); }
        }
        public Player Player2
        {
            get { return (Player)GetValue(Player2Property); }
            set { this.SetValue(Player2Property, value); }
        }
        public string Car
        {
            get { return (string)GetValue(CarProperty); }
            set { this.SetValue(CarProperty, value); }
        }

        public static readonly DependencyProperty Player1Property = DependencyProperty.Register("Player1", typeof(Player), typeof(MatchPlayerToStringConverter), new PropertyMetadata(null));
        public static readonly DependencyProperty Player2Property = DependencyProperty.Register("Player2", typeof(Player), typeof(MatchPlayerToStringConverter), new PropertyMetadata(null));
        public static readonly DependencyProperty CarProperty = DependencyProperty.Register("Car", typeof(string), typeof(MatchPlayerToStringConverter), new PropertyMetadata(null));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.Out.WriteLine("car={0}", Car);
            if (Player1 == null || Player2 == null)
                throw new NullReferenceException("Player 1 or Player 2 properties wasn't set.");

            MatchPlayer player = (MatchPlayer)value;

            switch (player)
            {
                case MatchPlayer.First:
                    return Player1.Name;
                case MatchPlayer.Second:
                    return Player2.Name;
                default:
                    return "None";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Player1 == null || Player2 == null)
                throw new NullReferenceException("Player 1 or Player 2 properties wasn't set.");

            string name = (string)value;

            if (value.Equals(Player1.Name))
                return MatchPlayer.First;
            else if (value.Equals(Player2.Name))
                return MatchPlayer.Second;
            else
                return MatchPlayer.None;
        }
    }
}
