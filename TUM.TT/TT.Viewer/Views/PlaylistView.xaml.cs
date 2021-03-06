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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaction logic for PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : UserControl,IHandle<PlaylistDeletedEvent>
    {

        public IEventAggregator Events { get; private set; }
        public PlaylistView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public void Handle(PlaylistDeletedEvent message)
        {
            Items.SelectedIndex = 0;
        }

       
    }
}
