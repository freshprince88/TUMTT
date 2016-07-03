using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using TT.Lib.Events;
using System;
using TT.Models;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ResultLargeTableView.xaml
    /// </summary>
    public partial class ResultLargeTableView : UserControl,
        IHandle<StrokesPaintEvent>
    {
        #region Constants

        private const double STROKE_THICKNESS = 1.75;
        private const double STROKE_THICKNESS_HOVER = 2.5;
        private const double SPIN_ARROW_STROKE_THICKNESS = 1;
        private const double SPIN_ARROW_STROKE_THICKNESS_HOVER = 2;
        private const double STROKE_THICKNESS_SMASH = 3.5;
        private const double STROKE_THICKNESS_SMASH_HOVER = 5;

        private const string TAG_SPIN_ARROW = "spinarrow";
        private const string TAG_ARROW_TIP = "arrowtip";
        private const string TAG_DIRECTION = "direction";

        private const string STROKE_ATTR_SIDE_FOREHAND = "Forehand";
        private const string STROKE_ATTR_SIDE_BACKHAND = "Backhand";

        private const string STROKE_ATTR_HAS_SPIN = "has_spin";

        private const string STROKE_ATTR_TECHNIQUE_PUSH = "Push";
        private const string STROKE_ATTR_TECHNIQUE_FLIP = "Flip";
        private const string STROKE_ATTR_TECHNIQUE_OPTION_BANANA = "Banana";
        private const string STROKE_ATTR_TECHNIQUE_TOPSPIN = "Topspin";
        private const string STROKE_ATTR_TECHNIQUE_BLOCK = "Block";
        private const string STROKE_ATTR_TECHNIQUE_SMASH = "Smash";
        private const string STROKE_ATTR_TECHNIQUE_COUNTER = "Counter";
        private const string STROKE_ATTR_TECHNIQUE_CHOP = "Chop";
        private const string STROKE_ATTR_TECHNIQUE_LOB = "Lob";
        private const string STROKE_ATTR_TECHNIQUE_MISCELLANEOUS = "Miscellaneous";

        private const string STROKE_ATTR_POC_OVER = "over";
        private const string STROKE_ATTR_POC_HALFDISTANCE = "half-distance";
        private const string STROKE_ATTR_POC_BEHIND = "behind";

        #endregion

        public ICollection<Stroke> Strokes
        {
            get { return (ICollection<Stroke>)GetValue(StrokesProperty); }
            set { SetValue(StrokesProperty, value); }
        }

        public static DependencyProperty StrokesProperty = DependencyProperty.Register(
        "Strokes", typeof(ICollection<Stroke>), typeof(ResultLargeTableView), new FrameworkPropertyMetadata(default(ICollection<Stroke>), new PropertyChangedCallback(OnStrokesPropertyChanged)));

        private Dictionary<Stroke, List<Shape>> strokeShapes;
        private int strokeNumber;

        public IEventAggregator Events { get; private set; }

        public ResultLargeTableView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);

            strokeShapes = new Dictionary<Stroke, List<Shape>>();
        }

        #region Event handlers

        private void CheckSpin_Click(object sender, RoutedEventArgs e)
        {
            //if ((sender as CheckBox).IsChecked.Value)
            //    AddServiceStrokesSpinArrows(new List<Stroke>(strokeShapes.Keys));
            //else
            //    RemoveShapesByTag(TAG_SPIN_ARROW);
        }

        private void CheckDirection_Click(object sender, RoutedEventArgs e)
        {
            //if ((sender as CheckBox).IsChecked.Value)
            //    AddStrokesDirectionLines(new List<Stroke>(strokeShapes.Keys), strokeNumber == 1);
            //else
            //    RemoveShapesByTag(TAG_DIRECTION);
        }



        public void Handle(StrokesPaintEvent message)
        {
        //    foreach (UIElement p in TableGrid.Children)
        //    {
        //        if (p is Grid)
        //            (p as Grid).Children.Clear();
        //    }
        //    strokeShapes.Clear();

        //    if (message.Strokes == null)
        //        return;

        //    strokeNumber = message.StrokeNumber;
        //    message.Strokes.ForEach(s => { strokeShapes[s] = new List<Shape>(); });

        //    switch (strokeNumber)
        //    {
        //        case 1:
        //            AddStrokesArrowtips(message.Strokes, true);
        //            if (CheckDirection.IsChecked.Value)
        //                AddStrokesDirectionLines(message.Strokes, true);
        //            if (CheckSpin.IsChecked.Value)
        //                AddServiceStrokesSpinArrows(message.Strokes);
        //            break;
        //        default:
        //            if (CheckDirection.IsChecked.Value)
        //                AddStrokesDirectionLines(message.Strokes, false);
        //            AddStrokesArrowtips(message.Strokes, false);
        //            break;
        //    }
        }

        private static void OnStrokesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("OnStrokesPropertyChanged sender={0}, e.ov={1}, e.nv={2}", sender, e.OldValue, e.NewValue);
        }

        #endregion

        #region Shape addition & removal



        private void RemoveShapesByTag(string tag)
        {
            foreach (var s in strokeShapes.Values)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    if ((string)s[i].Tag == tag)
                    {
                        (s[i].Parent as Grid).Children.Remove(s[i]);
                        s.RemoveAt(i);
                        --i;
                    }
                }
            }
        }

        #endregion

        #region Shape creation



        #endregion

        #region Style



        #endregion

        #region Helper Methods




        private bool PlacementValuesValid(Placement placement)
        {
            return placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }

        #endregion

    }
}
