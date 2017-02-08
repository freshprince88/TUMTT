using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TT.Models;
using TT.Report.ViewModels;

namespace TT.Report.Sections
{
    public abstract class ExistingStatisticsSection : BaseSection
    {
        public IDictionary<string, object> ExistingStatisticsImageBitmapFrames { get; private set; }

        protected void GetImageBitmapFrames(int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p, Type v)
        {
            ExistingStatisticsImageBitmapFrames = new Dictionary<string, object>();

            var player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                player = MatchPlayer.Second;

            foreach (var set in sets.Keys)
            {
                if (sets[set].Count == 0)
                {
                    continue;
                }

                // we have to create & re-populate new viewmodels because just switching SelectedRallies for some reason doesn't update the view
                StatisticsGridViewModel vm = new StatisticsGridViewModel()
                {
                    Player = player,
                    StrokeNumber = strokeNumber,
                    SelectedRallies = new System.Collections.ObjectModel.ObservableCollection<Rally>(sets[set])
                };

                try
                {
                    var view = (UserControl)Activator.CreateInstance(v);
                    ViewModelBinder.Bind(vm, view, null);
                    ((IActivate)vm).Activate();

                    var mainGrid = (Grid)view.Content;
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

                    var setTitle = GetSetTitleString(set);
                    ExistingStatisticsImageBitmapFrames[setTitle] = BitmapFrame.Create(bmp);
                }
                catch (Exception e) when (e is NullReferenceException || e is InvalidOperationException)
                {
                    Debug.WriteLine("{1}: '{0}' - cannot get statistics bitmap", e.GetType().Name, SectionName);
                }
                Debug.WriteLine("{2} for stroke {0} of set {1} ready.", GetStrokeNumberString(strokeNumber), set, SectionName);
            }
        }
    }
}
