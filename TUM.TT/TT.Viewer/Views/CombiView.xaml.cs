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
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für CombiView.xaml
    /// </summary>
    public partial class CombiView : UserControl
    {
        public IEventAggregator Events { get; set; }
        public CombiView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }
    }
}
