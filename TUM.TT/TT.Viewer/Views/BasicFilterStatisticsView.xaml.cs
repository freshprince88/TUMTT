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
using TT.Viewer.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für BasicFilterStatisticsView.xaml
    /// </summary>
    public partial class BasicFilterStatisticsView : UserControl, IHandle<MatchInformationEvent>
    {
        public IEventAggregator Events { get; set; }

        public BasicFilterStatisticsView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public void Handle(MatchInformationEvent message)
        {
            if (message.Match != null)
            {
                FilterPlayer1Button.Content = message.Match.FirstPlayer.Name.Split(' ')[0];
                FilterPlayer2Button.Content = message.Match.SecondPlayer.Name.Split(' ')[0];
            }
        }
    }
}