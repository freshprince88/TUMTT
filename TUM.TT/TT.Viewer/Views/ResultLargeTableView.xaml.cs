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

        private void button_Click(object sender, RoutedEventArgs e)
        {            
            Window legendWindow = new Window();
            legendWindow.Content = new TableLegendView();
            legendWindow.Show();
        }
    }
}
