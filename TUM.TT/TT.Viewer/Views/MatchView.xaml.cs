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
using TT.Lib.Managers;
using TT.Models;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MatchView.xaml
    /// </summary>
    public partial class MatchView : UserControl,
        IHandle<FullscreenEvent>, IHandle<FullscreenHideHitlistEvent>
    {
        #region Properties

        public IEventAggregator Events { get; set; }
        private IMatchManager Manager;
        public GridLengthConverter gridLengthConverter;
        #endregion

        public MatchView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Manager = IoC.Get<IMatchManager>();
            gridLengthConverter = new GridLengthConverter();
            Events.Subscribe(this);
        }

        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:

                    ResultListGrid.SetValue(Grid.ColumnProperty, 2);
                    ResultListGrid.Width = 105;
                    ResultListGrid.HorizontalAlignment = HorizontalAlignment.Left;
                    (ResultListGrid as Grid).Opacity = 0.5;
                    Thickness mg = new Thickness();
                    mg.Bottom = 75;
                    ResultListGrid.Margin = mg;
                        Column0.Width = new GridLength(0);
                    Column1.Width = new GridLength(0);
                    MediaRow1.Height = new GridLength(0);
                    MediaRow2.Height = new GridLength(0);



                    break;
                case false:

                    ResultListGrid.SetValue(Grid.ColumnProperty, 1);
                    ResultListGrid.ClearValue(WidthProperty);
                    ResultListGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    (ResultListGrid as Grid).Opacity = 1;
                    ResultListGrid.Margin = new Thickness(0);
                    Column0.Width = new GridLength(1, GridUnitType.Star);
                    Column1.Width = new GridLength(1, GridUnitType.Star);
                    MediaRow1.Height = new GridLength(5, GridUnitType.Star);
                    MediaRow2.Height = new GridLength(3, GridUnitType.Star);

                    break;
                default:
                    break;
            }
        }
        public void Handle(FullscreenHideHitlistEvent message)
        {
            switch (message.Hide)
            {
                case true:
                    ResultListGrid.Width = 0;

                    break;
                case false:
                    ResultListGrid.Width = 105;

                    break;
                default:
                    break;
            }
        }
    }
}
