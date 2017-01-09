using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TT.Report.Views
{
    /// <summary>
    /// Interaction logic for TechniqueGridView.xaml
    /// </summary>
    public partial class TechniqueStatisticsGridView : UserControl
    {
        public TechniqueStatisticsGridView()
        {
            try
            {
                InitializeComponent();
            } catch (Exception e) when (e is InvalidOperationException)
            {
                Debug.WriteLine("TechniqueStatisticsGridView: InitializeComponent() failed ({0} - {1})", e.GetType().Name, e.Message);
            }
        }
    }
}
