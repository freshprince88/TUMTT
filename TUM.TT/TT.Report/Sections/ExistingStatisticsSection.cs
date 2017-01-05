using Caliburn.Micro;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TT.Models;
using TT.Models.Statistics;
using TT.Report.ViewModels;
using TT.Report.Views;

namespace TT.Report.Sections
{
    public abstract class ExistingStatisticsSection
    {
        public IDictionary<string, BitmapFrame> ExistingStatisticsImageBitmapFrames { get; private set; }

        protected void GetImageBitmapFrames(int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p, UserControl view)
        {
            this.ExistingStatisticsImageBitmapFrames = new Dictionary<string, BitmapFrame>();

            var player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                player = MatchPlayer.Second;

            var statistics = new MatchStatistics(match);
            var mainGrid = (Grid)view.Content;

            foreach (var set in sets.Keys)
            {
                if (sets[set].Count == 0)
                {
                    continue;
                }
                if (strokeNumber == 1)
                {
                    // we have to create & re-populate new views because just switching SelectedRallies for some reason doesn't update the view
                    ServiceStatisticsGridViewModel vm = new ServiceStatisticsGridViewModel()
                    {
                        Player = player,
                        SelectedRallies = new System.Collections.ObjectModel.ObservableCollection<Rally>(sets[set])
                    };
                    ViewModelBinder.Bind(vm, view, null);
                    ((IActivate)vm).Activate();

                    var size = new Size(mainGrid.Width, mainGrid.Height);
                    var scale = sets.Count > 1 ? 1 : 2.5;

                    view.Width = size.Width;
                    view.Height = size.Height;

                    view.RenderTransform = new ScaleTransform(scale, scale);
                    view.Measure(size);
                    view.Arrange(new Rect(size));
                    view.UpdateLayout();

                    size.Width = mainGrid.ActualWidth;
                    size.Height = mainGrid.ActualHeight;

                    var bmp = new RenderTargetBitmap((int)(scale * (size.Width * (300 / 96d))), (int)(scale * (size.Height * (300 / 96d))), 300, 300, PixelFormats.Pbgra32);
                    bmp.Render(view);

                    ExistingStatisticsImageBitmapFrames[set] = (BitmapFrame.Create(bmp));
                }
            }
        }
    }
}
