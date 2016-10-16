using System;
using System.Windows;
using System.Windows.Controls;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ResultLargeTableView.xaml
    /// </summary>
    public partial class ResultLargeTableView : UserControl
    {
        public ResultLargeTableView()
        {
            InitializeComponent();
        }

        private void TopStatusGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckBoxesWrapPanel.Width = Math.Max(0, TopStatusGrid.ActualWidth - (SelectedStrokeInfoGrid.Visibility == Visibility.Collapsed ? 0 : SelectedStrokeInfoGrid.ActualWidth));
        }
    }
}
