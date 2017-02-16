using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TT.Models;
using TT.Models.Statistics;
using TT.Report.ViewModels;
using TT.Report.Views;

namespace TT.Report.Sections
{
    public class LargeTableSection : BaseSection
    {
        protected sealed override string SectionName => "Large Table section";

        public IDictionary<string, BitmapFrame> TableImageBitmapFrames { get; private set; }

        public LargeTableSection(int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            this.TableImageBitmapFrames = new Dictionary<string, BitmapFrame>();

            var player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                player = MatchPlayer.Second;

            var Events = new EventAggregator();
            LargeTableView ltv = new LargeTableView();
            ResultLargeTableViewModel vm = new ResultLargeTableViewModel(Events, match, null);
            ViewModelBinder.Bind(vm, ltv, null);
            ((IActivate)vm).Activate();

            try
            {
                var size = new Size(ltv.TableGrid.Width, ltv.TableGrid.Height);
                var statistics = new MatchStatistics(match);
                var setStrokes = new List<Stroke>();
                var scale = sets.Count > 1 ? 1 : 2.5;

                ltv.Width = size.Width;
                ltv.Height = size.Height;

                foreach (var set in sets.Keys)
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

                    ltv.Strokes = new ObservableCollection<Stroke>(setStrokes);

                    ltv.RenderTransform = new ScaleTransform(scale, scale);
                    ltv.Measure(size);
                    ltv.Arrange(new Rect(size));
                    ltv.UpdateLayout();

                    var bmp = new RenderTargetBitmap((int)(scale * (size.Width * (300 / 96d))), (int)(scale * (size.Height * (300 / 96d))), 300, 300, PixelFormats.Pbgra32);
                    bmp.Render(ltv);

                    var setTitle = GetSetTitleString(set);
                    TableImageBitmapFrames[setTitle] = (BitmapFrame.Create(bmp));

                    setStrokes.Clear();

                    Debug.WriteLine("{2} for stroke {0} of set {1} ready.", GetStrokeNumberString(strokeNumber), set, SectionName);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("LargeTableSection: '{0}' - cannot fill table. ({1})", e.GetType().Name, e.Message);
            }
        }
    }
}