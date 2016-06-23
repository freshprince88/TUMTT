using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using TT.Lib.Events;
using System;
using TT.Models;
using System.Windows.Shapes;
using System.Windows.Media;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ResultLargeTableView.xaml
    /// </summary>
    public partial class ResultLargeTableView : UserControl,
        IHandle<ResultListControlEvent>,
        IHandle<FullscreenEvent>,
        IHandle<StrokesPaintEvent>
    {

        public IEventAggregator Events { get; private set; }

        public ResultLargeTableView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);            
        }


        public void Handle(ResultListControlEvent msg)
        {
            
        }
        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                case false:
                    break;
                default:
                    break;
            }
        }

        private void Items_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void CheckSchlagrichtung_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckSpin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckHand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckPunkt_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Handle(StrokesPaintEvent message)
        {
            InnerFieldGrid.Children.Clear();

            if (message.Strokes == null)
                return;
            
            foreach (var s in message.Strokes)
            {
                if (PlacementValuesValid(s.Placement))
                {
                    Line line = new Line();
                    line.X1 = 0; line.Y1 = InnerFieldGrid.ActualHeight;
                    line.X2 = s.Placement.WX; line.Y2 = InnerFieldGrid.ActualHeight - s.Placement.WY;
                    line.StrokeThickness = 2;
                    line.Stroke = Brushes.Black;
                    line.Fill = Brushes.Black;
                    InnerFieldGrid.Children.Add(line);
                }
                else
                {
                    Console.Out.WriteLine("invalid Placement of stroke {0}: x={1} y={2}", s, s.Placement.WX, s.Placement.WY);
                }
            }
        }

        private bool PlacementValuesValid(Placement placement)
        {
            return placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }
    }
}
