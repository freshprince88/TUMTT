﻿using Caliburn.Micro;
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
using TT.Models;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaction logic for TableLegendView.xaml
    /// </summary>
    public partial class TableLegendView : TableView
    {
        public TableLegendView()
        {
            InitializeComponent();
        }        
        
        public override Grid View_InnerFieldBehindGrid
        {
            get
            {
                return InnerFieldBehindGrid;
            }
        }

        public override Grid View_InnerFieldGrid
        {
            get
            {
                return InnerFieldGrid;
            }
        }

        public override Grid View_InnerFieldHalfDistanceGrid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Grid View_InnerFieldSpinGrid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Border View_TableBorder
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Grid View_TableGrid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override void AttachEventHandlerToShape(Shape shape, Stroke stroke)
        {
            // DEBUG
            shape.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            shape.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            shape.DataContext = stroke;
            // ---
        }

        protected override double GetAdjustedX(Stroke stroke, double oldX)
        {
            return oldX;
        }

        protected override double GetAdjustedY(Stroke stroke, double oldY)
        {
            return oldY;
        }

        protected override double GetSecondStrokePrecedingStartY()
        {
            throw new NotImplementedException();
        }

        protected override void ProcessStrokes(List<Stroke> strokes)
        {
            //Debug.WriteLine("TableLegendView: {0} strokes to process", strokes.Count);

            foreach (Stroke s in strokes)
            {
                StrokeShapes[s] = new List<Shape>();
                AddStrokesDirectionShapes(s);
                AddStrokesArrowtips(s);
                AddInterceptArrows(s);
            }
        }

        #region DEBUG

        private void Stroke_MouseEnter(Object sender, MouseEventArgs e)
        {
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            Debug.WriteLine("mouse enter on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);            
        }

        private void Stroke_MouseLeave(Object sender, MouseEventArgs e)
        {
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            Debug.WriteLine("mouse leave on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);            
        }

        #endregion
    }
}
