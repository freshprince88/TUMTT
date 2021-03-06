﻿using Caliburn.Micro;
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
using TT.Lib.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für BasicFilterStatisticsView.xaml
    /// </summary>
    public partial class BasicFilterStatisticsView : UserControl, 
        IHandle<StatisticDetailChangedEvent>
    {
        public IEventAggregator Events { get; set; }

        public BasicFilterStatisticsView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public void Handle(StatisticDetailChangedEvent message)
        {
            bool isChecked = message.DetailChecked;
            bool percent = message.Percent;

            if (isChecked == true)
            {
                radio_count.IsEnabled = true;
                radio_percent.IsEnabled = true;
            }
            if (isChecked == false)
            {
                radio_count.IsEnabled = false;
                radio_percent.IsEnabled = false;
            }

        }
    }
}
