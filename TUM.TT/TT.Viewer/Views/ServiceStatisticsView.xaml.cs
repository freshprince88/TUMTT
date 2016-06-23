using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ServiceStatisticsView.xaml
    /// </summary>
    public partial class ServiceStatisticsView : UserControl,
        IHandle<StatisticDetailChangedEvent>,
        IHandle<BasicFilterSelectionChangedEvent>
    {
        #region Properties
        public IEventAggregator Events { get; set; }
        public IMatchManager Manager { get; set; }
        public List<Rally> SelectedRallies { get; private set; }

        #endregion

        public ServiceStatisticsView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
        }

        public void Handle(StatisticDetailChangedEvent message)
        {
            bool isChecked = message.DetailChecked;
            bool percent = message.Percent;

            if (isChecked == true)
            {
                foreach (ToggleButton btn in this.StatisticsContainer.FindChildren<ToggleButton>())
                {
                    if (btn.Name.Contains("PointsWon") || btn.Name.Contains("DirectPointsWon") || btn.Name.Contains("PointsLost"))
                    {
                        btn.Visibility = Visibility.Visible;
                    }

                }
            }
            if (isChecked == false)
            {
                foreach (ToggleButton btn in this.StatisticsContainer.FindChildren<ToggleButton>())
                {
                    if (btn.Name.Contains("PointsWon") || btn.Name.Contains("DirectPointsWon") || btn.Name.Contains("PointsLost"))
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public void Handle(BasicFilterSelectionChangedEvent message)
        {
            this.SelectedRallies = message.Rallies.ToList();
        }

        public double AufschlagPosition(Rally r)
        {
            Stroke service = r.Strokes.Where(s => s.Number == 1).FirstOrDefault();
            double aufschlagPosition;
            double seite = service.Placement.WY == double.NaN ? 999 : Convert.ToDouble(service.Placement.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (service.Playerposition == double.NaN ? 999 : Convert.ToDouble(service.Playerposition));
            }
            else
            {
                aufschlagPosition = service.Playerposition == double.NaN ? 999 : Convert.ToDouble(service.Playerposition);
            }

            return aufschlagPosition;

        }
    }
}
