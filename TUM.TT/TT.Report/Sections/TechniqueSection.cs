using System.Collections.Generic;
using Caliburn.Micro;
using TT.Models;
using TT.Report.ViewModels;
using TT.Report.Views;
using TT.Models.Statistics;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;

namespace TT.Report.Sections
{
    public class TechniqueSection : IReportSection
    {
        public IDictionary<string, BitmapFrame> TableImageBitmapFrames { get; private set; }
        private static int i = 0;

        public TechniqueSection(int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            this.TableImageBitmapFrames = new Dictionary<string, BitmapFrame>();

            var player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                player = MatchPlayer.Second;

            var statistics = new MatchStatistics(match);
            var setStrokes = new List<Stroke>();
            var scale = sets.Count > 1 ? 1 : 2.5;
            scale = 1;
            foreach (var set in sets.Keys)
            {
                if (strokeNumber == 1)
                {
                    if (sets[set].Count == 0)
                    {
                        continue;
                    }

                    foreach (var rally in sets[set])
                    {
                        rally.Strokes.Apply(stroke =>
                        {
                            if (statistics.CountStroke(stroke, player, strokeNumber))
                                setStrokes.Add(stroke);
                        });
                    }

                    var Events = new EventAggregator();
                    ServiceStatisticsView ssv = new ServiceStatisticsView();
                    ServiceStatisticsViewModel vm = new ServiceStatisticsViewModel(Events, sets[set]);
                    ViewModelBinder.Bind(vm, ssv, null);
                    ((IActivate)vm).Activate();

                    var size = new Size(ssv.MainGrid.Width, ssv.MainGrid.Height);
                    ssv.Width = size.Width;
                    ssv.Height = size.Height;

                    ssv.RenderTransform = new ScaleTransform(scale, scale);
                    ssv.Measure(size);
                    ssv.Arrange(new Rect(size));
                    ssv.UpdateLayout();

                    size.Width = ssv.MainGrid.ActualWidth;
                    size.Height = ssv.MainGrid.ActualHeight;
                    ssv.Width = size.Width;
                    ssv.Height = size.Height;
                    
                    var bmp = new RenderTargetBitmap((int)(scale * (size.Width * (300 / 96d))), (int)(scale * (size.Height * (300 / 96d))), 300, 300, PixelFormats.Pbgra32);
                    bmp.Render(ssv);

                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));

                    var tempFile = @"E:\Users\Tom\test" + i++ + ".png";
                    using (Stream stm = File.Create(tempFile))
                        encoder.Save(stm);
                    //TableImageBitmapFrames[set] = (BitmapFrame.Create(bmp));
                }
            }
        }
    }
}