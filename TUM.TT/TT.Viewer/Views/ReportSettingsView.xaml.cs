using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using TT.Lib.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ReportSettingsView.xaml
    /// </summary>
    public partial class ReportSettingsView : MetroWindow,
        IHandle<ReportPreviewChangedEvent>
    {
        public IEventAggregator Events { get; private set; }
        
        public ReportSettingsView()
        {
            InitializeComponent();
            Name = "ReportSettings";

            var blurEffect = new BlurEffect();
            blurEffect.Radius = 0;
            PreviewOverlay.Effect = blurEffect;

            BitmapEffectCheckbox.Checked += BitmapEffectCheckbox_CheckedChanged;
            BitmapEffectCheckbox.Unchecked += BitmapEffectCheckbox_CheckedChanged;
            
            ReportPreviewControl.LoadCompleted += ReportPreviewControl_LoadCompleted;
            Closed += ReportSettingsView_Closed;

            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        private void ReportSettingsView_Closed(object sender, EventArgs e)
        {
            ReportPreviewControl.Navigate("about:blank");
        }

        private void ReportPreviewControl_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("Load complete [uri=" + e.Uri.ToString() + "]");
            new Thread(new ThreadStart(DisplayWebBrowserDelayed)).Start();
        }

        private void BitmapEffectCheckbox_CheckedChanged(object sender, RoutedEventArgs e)
        {            
            
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
    }
}
