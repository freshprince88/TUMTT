using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using TT.Lib.Events;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ReportSettingsView.xaml
    /// </summary>
    public partial class ReportSettingsView : MetroWindow,
        IHandle<ReportPreviewChangedEvent>
    {
        public IEventAggregator Events { get; private set; }
        private bool reflectCheckStateChange = true;

        public ReportSettingsView()
        {
            InitializeComponent();
            Name = "ReportSettings";

            var blurEffect = new BlurEffect();
            blurEffect.Radius = 0;
            PreviewOverlay.Effect = blurEffect;

            DataContextChanged += ReportSettingsView_DataContextChanged;
            ReportPreviewControl.LoadCompleted += ReportPreviewControl_LoadCompleted;
            Closed += ReportSettingsView_Closed;

            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);            
        }

        private void ReportSettingsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as ReportSettingsViewModel;
            if (vm != null)
            {
                foreach (var combi in vm.AvailableCombis)
                    AddCombiView(combi, BinToString(combi));
                foreach (var combi in vm.SelectedCombis)
                    SelectCombiView(combi);
            }
        }

        private void SelectCombiView(int combi)
        {
            Debug.WriteLine("Selecting combi view {0}", combi);

            foreach (var i in ReportSettingsGrid_Content_Sets_ToggleButtons.Children)
            {
                var tb = i as ToggleButton;
                if (tb != null && tb.Tag is int && (int)tb.Tag == combi && tb.IsChecked.Value)
                    tb.IsChecked = true;
            }
        }

        private void AddCombiView(int combiSets, string combiSetsString)
        {
            Debug.WriteLine("Adding combi view {0}", combiSets);

            if (!string.IsNullOrEmpty(combiSetsString))
            {
                var combiName = "Combi" + combiSetsString.Replace("+", "");

                var combiPresent = false;
                foreach (var i in ReportSettingsGrid_Content_Sets_ToggleButtons.Children)
                {
                    var tb = i as ToggleButton;
                    if (tb != null && tb.Name == combiName)
                        combiPresent = true;
                }
                if (!combiPresent)
                {
                    var combi = new ToggleButton();
                    combi.Style = FindResource("MetroAccentCircleToggleButtonStyle") as Style;
                    combi.Name = combiName;
                    combi.Width = 55;
                    combi.Height = 30;
                    combi.Tag = combiSets;

                    var textBlock = new TextBlock();
                    textBlock.FontSize = 12;
                    textBlock.Inlines.Add(new Run(combiSetsString));
                    combi.Content = textBlock;

                    var setCount = ReportSettingsGrid_Content_Sets_ToggleButtons.Children.Count;
                    ReportSettingsGrid_Content_Sets_ToggleButtons.Children.Insert(setCount - 1, combi);

                    combi.Checked += Combi_Checked;
                    combi.Unchecked += Combi_Checked;
                    combi.IsChecked = true;
                }
            }
        }

        private void ReportSettingsView_Closed(object sender, EventArgs e)
        {
            // navigate away and remove the webbrowser control for it to properly release its resources
            // (so that we can delete the temporary pdf files on exit)
            ReportPreviewControl.Navigate("about:blank");
            ReportPreviewBrowserContainer.Children.Remove(ReportPreviewControl);

            DataContextChanged -= ReportSettingsView_DataContextChanged;
            ReportPreviewControl.LoadCompleted -= ReportPreviewControl_LoadCompleted;
            Closed -= ReportSettingsView_Closed;
            Events.Unsubscribe(this);
        }

        private void ReportPreviewControl_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("Load complete [uri=" + e.Uri.ToString() + "]");
            new Thread(new ThreadStart(DisplayWebBrowserDelayed)).Start();
        }

        public void Handle(ReportPreviewChangedEvent message)
        {
            var topLeftCorner = ReportPreviewControl.PointToScreen(new System.Windows.Point(0, 0));
            var topLeftGdiPoint = new System.Drawing.Point((int)topLeftCorner.X, (int)topLeftCorner.Y);
            var size = new System.Drawing.Size((int)ReportPreviewControl.ActualWidth, (int)ReportPreviewControl.ActualHeight);

            var screenShot = new Bitmap((int)ReportPreviewControl.ActualWidth, (int)ReportPreviewControl.ActualHeight);

            using (var graphics = Graphics.FromImage(screenShot))
            {
                graphics.CopyFromScreen(topLeftGdiPoint, new System.Drawing.Point(), size, CopyPixelOperation.SourceCopy);
            }

            PreviewOverlay.Source = Imaging.CreateBitmapSourceFromHBitmap(screenShot.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ReportPreviewControl.Visibility = Visibility.Hidden;
            ((BlurEffect)PreviewOverlay.Effect).Radius = 20;

            var pfc = new PreviewFileChange();
            pfc.newPath = message.ReportPreviewPath;
            pfc.view = this;
            new Thread(new ThreadStart(pfc.ChangePreviewFile)).Start();
        }

        private class PreviewFileChange
        {
            internal string newPath;
            internal ReportSettingsView view;

            internal void ChangePreviewFile()
            {
                Thread.Sleep(500);
                view.Dispatcher.Invoke(() =>
                {
                    view.ReportPreviewGroupBox.IsEnabled = false;
                    view.ReportPreviewControl.Navigate(newPath + "#toolbar=0&navpanes=0&messages=0&statusbar=0");                    
                });
            }
        }        

        private void DisplayWebBrowserDelayed()
        {
            Thread.Sleep(500);
            Dispatcher.Invoke(() =>
            {
                ReportPreviewControl.Visibility = Visibility.Visible;
                ((BlurEffect)PreviewOverlay.Effect).Radius = 0;
                ReportPreviewGroupBox.IsEnabled = true;
            });
        }

        private void PlusCombi_Click(object sender, RoutedEventArgs e)
        {            
            PlusCombiPopup.IsOpen = true;
        }

        private void PlusCombiPopup_Closed(object sender, EventArgs e)
        {
            foreach (var tb in PlusCombiPopup_ToggleButtons.Children)
                if (tb is ToggleButton && ((ToggleButton)tb).IsChecked.Value)
                    ((ToggleButton)tb).IsChecked = false;
            if (PlusCombi.IsChecked.Value)
                PlusCombi.IsChecked = false;
        }

        private void ReportSettings_Title_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.WriteLine("popup height={0} width={1}", PlusCombiPopup.ActualHeight, PlusCombiPopup.ActualWidth);
        }

        private void OkCombi_Click(object sender, RoutedEventArgs e)
        {
            var combiSets = 0;
            var combiSetsString = "";
            foreach (var i in PlusCombiPopup_ToggleButtons.Children)
            {
                var tb = i as ToggleButton;
                if (tb != null && tb.IsChecked.Value)
                {
                    combiSets += 1 << int.Parse(tb.Tag as string);
                    combiSetsString += tb.Tag + "+";
                }
            }

            var combiSetsName = combiSetsString.Substring(0, combiSetsString.Length - 1);
            if (combiSetsName.Contains("+"))    // only add actual combis, not just one set
            {
                AddCombiView(combiSets, combiSetsName);

                var vm = DataContext as ReportSettingsViewModel;
                if (vm != null)
                    vm.AddCombi(combiSets);
            }

            PlusCombiPopup.IsOpen = false;
        }

        private void Combi_Checked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ReportSettingsViewModel;
            if (vm != null)
            {
                var tb = (ToggleButton)sender;
                vm.SelectCombi((int)tb.Tag, tb.IsChecked.Value);
            }
        }
        
        private string BinToString(int combi)
        {
            if (combi < 1)
                return "";

            var combiString = "";
            for (var i = 0; i < Math.Ceiling(Math.Log(combi, 2)); i++)
            {
                var mask = 1 << i;
                if ((mask & combi) == mask)
                    combiString += i + "+";
            }

            return combiString.Substring(0, combiString.Length - 1);
        }

        private void SCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(SAllCheckBox, ReportSettingsGrid_Content_Strokes_SContainer);
        }

        private void RCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(RAllCheckBox, ReportSettingsGrid_Content_Strokes_RContainer);
        }

        private void ThirdCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(ThirdAllCheckBox, ReportSettingsGrid_Content_Strokes_ThirdContainer);
        }

        private void FourthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(FourthAllCheckBox, ReportSettingsGrid_Content_Strokes_FourthContainer);
        }

        private void LCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(LAllCheckBox, ReportSettingsGrid_Content_Strokes_LContainer);
        }

        private void ACheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(AAllCheckBox, ReportSettingsGrid_Content_Strokes_AContainer);
        }

        private void ReflectChbxState(CheckBox allChbx, Grid otherChbxs)
        {
            if (reflectCheckStateChange)
            {
                bool allChecked = true;
                bool allUnchecked = true;

                foreach (var c in otherChbxs.Children)
                {
                    var chbx = c as CheckBox;
                    if (chbx != null)
                    {
                        allChecked &= chbx.IsChecked.Value;
                        allUnchecked &= !chbx.IsChecked.Value;
                    }
                }

                if (allChecked)
                { 
                    if (allChbx.IsChecked == null || !allChbx.IsChecked.Value)
                        allChbx.IsChecked = true;
                }
                else if (allUnchecked)
                {
                    if (allChbx.IsChecked == null || allChbx.IsChecked.Value)
                        allChbx.IsChecked = false;
                }
                else if (!allChecked && !allUnchecked)
                    allChbx.IsChecked = null;
            }
        }

        private void SAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGrid_Content_Strokes_SContainer, (CheckBox)sender);
        }

        private void RAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGrid_Content_Strokes_RContainer, (CheckBox)sender);
        }

        private void ThirdAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGrid_Content_Strokes_ThirdContainer, (CheckBox)sender);
        }

        private void FourthAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGrid_Content_Strokes_FourthContainer, (CheckBox)sender);
        }

        private void LAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGrid_Content_Strokes_LContainer, (CheckBox)sender);
        }

        private void AAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGrid_Content_Strokes_AContainer, (CheckBox)sender);
        }

        private void CheckChildChbxs(Grid parent, CheckBox allChbx)
        {
            reflectCheckStateChange = false;

            if (allChbx.IsChecked == null)
                allChbx.IsChecked = false;

            foreach (var c in parent.Children)
            {
                var strokeAttributeChbx = c as CheckBox;
                if (strokeAttributeChbx != null)
                {
                    if (strokeAttributeChbx.IsChecked.Value != allChbx.IsChecked.Value)
                        strokeAttributeChbx.IsChecked = allChbx.IsChecked;
                }
            }

            reflectCheckStateChange = true;
        }
    }
}
