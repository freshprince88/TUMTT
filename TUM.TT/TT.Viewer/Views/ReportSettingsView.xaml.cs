using MahApps.Metro.Controls;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ReportSettingsView.xaml
    /// </summary>
    public partial class ReportSettingsView : MetroWindow
    {

        public ReportSettingsView()
        {
            InitializeComponent();
            Name = "ReportSettings";

            var blurEffect = new BlurEffect();
            blurEffect.Radius = 0;
            PreviewOverlay.Effect = blurEffect;

            BitmapEffectCheckbox.Checked += BitmapEffectCheckbox_CheckedChanged;
            BitmapEffectCheckbox.Unchecked += BitmapEffectCheckbox_CheckedChanged;
        }

        private void BitmapEffectCheckbox_CheckedChanged(object sender, RoutedEventArgs e)
        {            
            if ((sender as CheckBox).IsChecked == true)
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
                PreviewOverlay.Visibility = Visibility.Visible;
                ReportPreviewControl.Visibility = Visibility.Hidden;
                ((BlurEffect)PreviewOverlay.Effect).Radius = 20;
            }
            else
            {
                PreviewOverlay.Visibility = Visibility.Hidden;
                ReportPreviewControl.Visibility = Visibility.Visible;
                PreviewOverlay.Source = null;
                ((BlurEffect)PreviewOverlay.Effect).Radius = 0;
            }
        }
    }
}
