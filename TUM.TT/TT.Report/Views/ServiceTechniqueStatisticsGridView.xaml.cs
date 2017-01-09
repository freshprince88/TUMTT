using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace TT.Report.Views
{
    /// <summary>
    /// Interaction logic for ServiceStatisticsView.xaml
    /// </summary>
    public partial class ServiceTechniqueStatisticsGridView : UserControl
    {
        public ServiceTechniqueStatisticsGridView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e) when (e is InvalidOperationException)
            {
                Debug.WriteLine("ServiceTechniqueStatisticsGridView: InitializeComponent() failed ({0} - {1})", e.GetType().Name, e.Message);
            }
        }
    }
}
