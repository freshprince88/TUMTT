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

        private string name1 = "report_tmp";
        private string name2 = "report_tmp - Copy";
        private bool isName1 = true;

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

            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        private void ReportPreviewControl_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("Load complete [uri={0}]", e.Uri.ToString());
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

            new Thread(new ThreadStart(ClearWebBrowserSource)).Start();            
        }

        private void ClearWebBrowserSource()
        {
            Thread.Sleep(500);
            Dispatcher.Invoke(() =>
            {
                ReportPreviewGroupBox.IsEnabled = false;

                Debug.WriteLine("isName1 ? {0}", isName1);
                ReportPreviewControl.Navigate("pack://siteoforigin:,,,/Resources/" + (isName1 ? name1 : name2) + ".pdf#toolbar=0&navpanes=0&messages=0&statusbar=0");
                isName1 = !isName1;
            });            
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
