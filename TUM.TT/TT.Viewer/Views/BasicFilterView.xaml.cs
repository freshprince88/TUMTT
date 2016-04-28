using Caliburn.Micro;
using System;
using System.Collections.Generic;
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
using TT.Models.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für BasicFilterView.xaml
    /// </summary>
    public partial class BasicFilterView : UserControl
    {
        public IEventAggregator Events { get; set; }

        public BasicFilterView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

       
    }
}
