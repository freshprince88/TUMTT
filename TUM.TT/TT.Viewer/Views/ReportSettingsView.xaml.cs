using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
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
using CheckBox = System.Windows.Controls.CheckBox;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ReportSettingsView.xaml
    /// </summary>
    public partial class ReportSettingsView : MetroWindow,
        IHandle<ReportPreviewChangedEvent>,
        IHandle<ReportSettingsChangedEvent>
    {
        private IEventAggregator Events { get; }
        private bool _reflectCheckStateChange = true;
        private string _issuedPreviewCustomizationId;
        private string _currentPreviewCustomizationId;
        private static readonly object DisplayLock = new object();

        public ReportSettingsView()
        {
            InitializeComponent();
            Name = "ReportSettings";

            var blurEffect = new BlurEffect {Radius = 0};
            PreviewOverlayImage.Effect = blurEffect;

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
                // this happens only on first load of view
                foreach (var combi in vm.AvailableCombis)
                    AddCombiView(combi, BinToString(combi));
                foreach (var combi in vm.SelectedCombis)
                    SelectCombiView(combi);

                // generate the report based on the initial settings
                vm.GenerateReport();
            }
        }

        private void SelectCombiView(int combi)
        {
            Debug.WriteLine("Selecting combi view {0}", combi);

            foreach (var i in ReportSettingsGridContentSetsToggleButtons.Children)
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
                foreach (var i in ReportSettingsGridContentSetsToggleButtons.Children)
                {
                    var tb = i as ToggleButton;
                    if (tb != null && tb.Name == combiName)
                        combiPresent = true;
                }
                if (!combiPresent)
                {
                    var textBlock = new TextBlock {FontSize = 12};
                    textBlock.Inlines.Add(new Run(combiSetsString));

                    var combi = new ToggleButton
                    {
                        Style = FindResource("MetroAccentRoundedRectToggleButtonStyle") as Style,
                        Name = combiName,
                        Height = 30,
                        Padding = new Thickness(12, 0, 12, 0),
                        Tag = combiSets,
                        Content = textBlock
                    };

                    combi.Checked += Combi_Checked;
                    combi.Unchecked += Combi_Checked;
                    combi.IsChecked = true;

                    var setCount = ReportSettingsGridContentSetsToggleButtons.Children.Count;
                    ReportSettingsGridContentSetsToggleButtons.Children.Insert(setCount - 1, combi);
                }
            }
        }

        private void ReportSettingsView_Closed(object sender, EventArgs e)
        {
            DataContextChanged -= ReportSettingsView_DataContextChanged;
            ReportPreviewControl.LoadCompleted -= ReportPreviewControl_LoadCompleted;
            Closed -= ReportSettingsView_Closed;
            Events.Unsubscribe(this);

            ReportPreviewControl.Navigate("http://localhost/"); // "about:blank" seems to lock the webbrowser when a pdf was loaded
            ReportPreviewBrowserContainer.Children.Remove(ReportPreviewControl);
        }
        
        private void ReportPreviewControl_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri == null || string.IsNullOrEmpty(e.Uri.ToString()))
                return;
            Debug.WriteLine($"Load complete [uri={e.Uri}]");
            new Thread(DisplayWebBrowserDelayed).Start();
        }

        public void Handle(ReportSettingsChangedEvent message)
        {
            lock (DisplayLock)
            {
                _issuedPreviewCustomizationId = message.CustomziationId;
                if (ReportPreviewGroupBox.IsEnabled && PresentationSource.FromVisual(ReportPreviewControl) != null)
                {

                    var topLeftCorner = ReportPreviewControl.PointToScreen(new System.Windows.Point(0, 0));
                    var topLeftGdiPoint = new System.Drawing.Point((int) topLeftCorner.X, (int) topLeftCorner.Y);
                    var size = new System.Drawing.Size((int) ReportPreviewControl.ActualWidth,
                        (int) ReportPreviewControl.ActualHeight);

                    var screenShot = new Bitmap((int) ReportPreviewControl.ActualWidth,
                        (int) ReportPreviewControl.ActualHeight);

                    using (var graphics = Graphics.FromImage(screenShot))
                    {
                        graphics.CopyFromScreen(topLeftGdiPoint, new System.Drawing.Point(), size,
                            CopyPixelOperation.SourceCopy);
                    }

                    PreviewOverlayImage.Source = Imaging.CreateBitmapSourceFromHBitmap(screenShot.GetHbitmap(),
                        IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    ReportPreviewControl.Visibility = Visibility.Hidden;
                    ((BlurEffect) PreviewOverlayImage.Effect).Radius = 20;

                    ReportPreviewGroupBox.IsEnabled = false;
                }
            }
        }

        public void Handle(ReportPreviewChangedEvent message)
        {
            _currentPreviewCustomizationId = message.CustomizationId;
            var pfc = new PreviewFileChange
            {
                NewPath = message.ReportPreviewPath,
                View = this
            };
            new Thread(pfc.ChangePreviewFile).Start();
        }

        private class PreviewFileChange
        {
            internal string NewPath;
            internal ReportSettingsView View;

            internal void ChangePreviewFile()
            {
                View.Dispatcher.Invoke(() =>
                {
                    View.ReportPreviewControl.Navigate(NewPath + "#toolbar=0&navpanes=0&messages=0&statusbar=0");                    
                });
            }
        }        

        private void DisplayWebBrowserDelayed()
        {
            Thread.Sleep(500);
            Dispatcher.Invoke(() =>
            {
                lock (DisplayLock)
                {
                    if (_currentPreviewCustomizationId == _issuedPreviewCustomizationId)
                    {
                        ReportPreviewControl.Visibility = Visibility.Visible;
                        ((BlurEffect)PreviewOverlayImage.Effect).Radius = 0;
                        ReportPreviewGroupBox.IsEnabled = true;
                    }
                }
            });
        }

        private void PlusCombi_Click(object sender, RoutedEventArgs e)
        {            
            PlusCombiPopup.IsOpen = true;
        }

        private void PlusCombiPopup_Closed(object sender, EventArgs e)
        {
            foreach (var tb in PlusCombiPopupToggleButtons.Children)
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
            foreach (var i in PlusCombiPopupToggleButtons.Children)
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
                vm?.AddCombi(combiSets);
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
            for (var i = 0; i <= Math.Ceiling(Math.Log(combi, 2)); i++)
            {
                var mask = 1 << i;
                if ((mask & combi) == mask)
                    combiString += i + "+";
            }

            return combiString.Substring(0, combiString.Length - 1);
        }

        private void SCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(SAllCheckBox, ReportSettingsGridContentStrokesSContainer);
        }

        private void RCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(RAllCheckBox, ReportSettingsGridContentStrokesRContainer);
        }

        private void ThirdCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(ThirdAllCheckBox, ReportSettingsGridContentStrokesThirdContainer);
        }

        private void FourthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(FourthAllCheckBox, ReportSettingsGridContentStrokesFourthContainer);
        }

        private void LCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(LAllCheckBox, ReportSettingsGridContentStrokesLContainer);
        }

        private void ACheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ReflectChbxState(AAllCheckBox, ReportSettingsGridContentStrokesAContainer);
        }

        private void ReflectChbxState(CheckBox allChbx, Grid otherChbxs)
        {
            if (_reflectCheckStateChange)
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
                else
                    allChbx.IsChecked = null;
            }
        }

        private void SAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGridContentStrokesSContainer, (CheckBox)sender);
        }

        private void RAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGridContentStrokesRContainer, (CheckBox)sender);
        }

        private void ThirdAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGridContentStrokesThirdContainer, (CheckBox)sender);
        }

        private void FourthAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGridContentStrokesFourthContainer, (CheckBox)sender);
        }

        private void LAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGridContentStrokesLContainer, (CheckBox)sender);
        }

        private void AAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckChildChbxs(ReportSettingsGridContentStrokesAContainer, (CheckBox)sender);
        }

        private void CheckChildChbxs(Grid parent, CheckBox allChbx)
        {
            _reflectCheckStateChange = false;

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

            _reflectCheckStateChange = true;
        }
    }
}
