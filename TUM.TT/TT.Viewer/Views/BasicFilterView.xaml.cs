using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TT.Lib.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für BasicFilterView.xaml
    /// </summary>
    public partial class BasicFilterView : UserControl, IHandle<RallyLengthChangedEvent>
    {
        public IEventAggregator Events { get; set; }

        public BasicFilterView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public void Handle(RallyLengthChangedEvent message)
        {

            switch (message)
            {
                case 1:
                    FilterRallyLength1Button.Visibility = Visibility.Visible;
                    FilterRallyLength2Button.Visibility = Visibility.Visible;
                    FilterRallyLength3Button.Visibility = Visibility.Visible;
                    break;
                case 2:
                    FilterRallyLength1Button.Visibility = Visibility.Collapsed;
                    FilterRallyLength2Button.Visibility = Visibility.Visible;
                    FilterRallyLength3Button.Visibility = Visibility.Visible;
                    break;
                case 3:
                    FilterRallyLength1Button.Visibility = Visibility.Collapsed;
                    FilterRallyLength2Button.Visibility = Visibility.Collapsed;
                    FilterRallyLength3Button.Visibility = Visibility.Visible;
                    break;
                case 4:
                    FilterRallyLength1Button.Visibility = Visibility.Collapsed;
                    FilterRallyLength2Button.Visibility = Visibility.Collapsed;
                    FilterRallyLength3Button.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    FilterRallyLength1Button.Visibility = Visibility.Collapsed;
                    FilterRallyLength2Button.Visibility = Visibility.Visible;
                    FilterRallyLength3Button.Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }
        }

        //public void Handle(MatchInformationEvent message)
        //{
        //    if (message.Match != null)
        //    {
        //        FilterPointPlayer1Button.Content = message.Match.FirstPlayer.Name.Split(' ')[0]; 
        //        FilterPointPlayer2Button.Content = message.Match.SecondPlayer.Name.Split(' ')[0]; 
        //        FilterServerPlayer1Button.Content = message.Match.FirstPlayer.Name.Split(' ')[0]; 
        //        FilterServerPlayer2Button.Content = message.Match.SecondPlayer.Name.Split(' ')[0]; 
        //    }
        //}
    }
}
