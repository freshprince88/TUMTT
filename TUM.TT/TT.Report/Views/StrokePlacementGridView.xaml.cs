using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace TT.Report.Views
{
    /// <summary>
    /// Interaction logic for StrokePlacementGridView.xaml
    /// </summary>
    public partial class StrokePlacementGridView : UserControl
    {
        public StrokePlacementGridView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e) when (e is InvalidOperationException)
            {
                Debug.WriteLine("StrokePlacementGridView: InitializeComponent() failed ({0} - {1})", e.GetType().Name, e.Message);
            }
        }
    }
}
