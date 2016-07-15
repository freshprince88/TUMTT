using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class TableLegendViewModel : Screen
    {

        public ObservableCollection<Stroke> Strokes { get; set; }

        public TableLegendViewModel()
        {
            Strokes = new ObservableCollection<Stroke>();
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();

            Rally rally;
            Stroke stroke;
            Stroketechnique stroketechnique;

            rally = new Rally();
            rally.Number = 1;

            // first stroke (dummy)
            stroke = new Stroke();
            stroke.Number = 1;
            stroke.Placement = new Placement();
            stroke.Placement.WX = 100;
            stroke.Placement.WY = 5;
            stroke.PointOfContact = Models.Util.Enums.Stroke.PointOfContact.Over.ToString();
            stroke.Rally = rally;

            strokesTmp.Add(stroke);

            // second stroke (for preceding stroke
            stroke = new Stroke();
            stroke.Number = 2;
            stroke.Placement = new Placement();
            stroke.Placement.WX = 0;
            stroke.Placement.WY = 5;
            stroke.PointOfContact = Models.Util.Enums.Stroke.PointOfContact.Over.ToString();
            stroke.Rally = rally;
            
            strokesTmp.Add(stroke);

            // push stroke
            stroke = new Stroke();
            stroke.Number = 3;
            stroke.Placement = new Placement();
            stroke.Placement.WX = 100;
            stroke.Placement.WY = 5;
            stroke.PointOfContact = Models.Util.Enums.Stroke.PointOfContact.Over.ToString();
            stroketechnique = new Stroketechnique();
            stroketechnique.Type = Models.Util.Enums.Stroke.Technique.Push.ToString();
            stroke.Stroketechnique = stroketechnique;
            stroke.Rally = rally;

            strokesTmp.Add(stroke);
            Strokes.Add(stroke);

            // service without spin
            stroke = new Stroke();
            stroke.Number = 4;
            stroke.PointOfContact = Models.Util.Enums.Stroke.PointOfContact.Behind.ToString();
            stroke.Rally = rally;

            strokesTmp.Add(stroke);

            rally.Strokes = strokesTmp;
        }

        public void MouseLeft()
        {
            TryClose();
        }
    }
}
