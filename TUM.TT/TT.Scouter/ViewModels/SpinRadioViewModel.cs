using Caliburn.Micro;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TT.Models.Util.Enums;

namespace TT.Scouter.ViewModels
{
    public class SpinRadioViewModel : Screen
    {
        public TT.Models.Schlag Stroke { get; set; }

        public SpinRadioViewModel(TT.Models.Schlag s)
        {
            Stroke = s;
        }

        public void Checked(RoutedEventArgs e)
        {
            if(e.Source.GetType().Equals(typeof(RadioButton)))
            {
                RadioButton rb = (RadioButton)e.Source;
                TT.Models.Spin newSpin = new TT.Models.Spin();

                if (rb.Name.Contains("ÜS"))
                    newSpin.ÜS = "1";
                else
                    newSpin.ÜS = "0";

                if (rb.Name.Contains("US"))
                    newSpin.US = "1";
                else
                    newSpin.US = "0";

                if (rb.Name.Contains("SL"))
                    newSpin.SL = "1";
                else
                    newSpin.SL = "0";

                if (rb.Name.Contains("SR"))
                    newSpin.SR = "1";
                else
                    newSpin.SR = "0";

                if (rb.Name.Contains("No"))
                    newSpin.No = "1";
                else
                    newSpin.No = "0";

                Stroke.Spin = newSpin;
            }
        }
    }
}
