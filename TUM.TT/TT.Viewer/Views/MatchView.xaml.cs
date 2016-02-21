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
using TT.Lib.Models;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MatchView.xaml
    /// </summary>
    public partial class MatchView : UserControl,
        IHandle<FullscreenEvent>
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
                    Column1.Width = new GridLength(0);
                    Column2.Width = new GridLength(50);
                    Row2.Height = new GridLength(0);
                    Row3.Height = new GridLength(0);
                    break;
                case false:
                    Column1.Width = new GridLength(1, GridUnitType.Star);
                    Column2.Width = new GridLength(1, GridUnitType.Star);
                    Row2.Height = new GridLength(1, GridUnitType.Star);
                    Row3.Height = new GridLength(1, GridUnitType.Star);




                    break;
                default:
                    break;
            }
        }
    }
}
