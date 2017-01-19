using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using TT.Report.ViewModels;
using TT.Report.Views;

namespace TT.Report.Sections
{
    public class TableLegendSection : BaseSection
    {
        public BitmapFrame LegendImage { get; }

        public TableLegendSection()
        {
            var vm = new TableLegendViewModel();
            var view = (UserControl)Activator.CreateInstance(typeof(TableLegendView));
            ViewModelBinder.Bind(vm, view, null);
            ((IActivate)vm).Activate();

            var mainGrid = (Grid)view.Content;
            if (mainGrid == null)
            {
                Debug.WriteLine("TableLegendSection: mainGrid is null - system probably shutting down. Returning null.");
                return;
            }

            var size = new Size(mainGrid.Width, mainGrid.Height);
            var scale = 2.5;

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
            
            LegendImage = BitmapFrame.Create(bmp);
        }
    }
}
