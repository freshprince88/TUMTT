using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models.Statistics
{
    public class SpinStatistics : MatchStatistics
    {
        public SpinStatistics(Match match, object p, List<Rally> rallies) : base(match)
        {
            this.Player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                this.Player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                this.Player = MatchPlayer.Second;

            var spinUpConsts = new List<Util.Enums.Stroke.Spin>(3) { Util.Enums.Stroke.Spin.TS, Util.Enums.Stroke.Spin.TSSL, Util.Enums.Stroke.Spin.TSSR };
            var spinDownConsts = new List<Util.Enums.Stroke.Spin>(3) { Util.Enums.Stroke.Spin.US, Util.Enums.Stroke.Spin.USSL, Util.Enums.Stroke.Spin.USSR };
            var noSpinConsts = new List<Util.Enums.Stroke.Spin>(1) { Util.Enums.Stroke.Spin.No };
            var hiddenSpinConsts = new List<Util.Enums.Stroke.Spin>(1) { Util.Enums.Stroke.Spin.Hidden };

            foreach (var r in rallies)
            {
                foreach (var s in r.Strokes)
                {
                    if (CountStroke(s))
                    {            
                        if (s.HasSpins(spinUpConsts))
                        {
                            this.SpinUp++;
                            SpinUpWon += s.Rally.Winner == Player ? 1 : 0;                            
                        }
                        else if (s.HasSpins(spinDownConsts))
                        {
                            this.SpinDown++;
                            SpinDownWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                        else if (s.HasSpins(noSpinConsts))
                        {
                            this.NoSpin++;
                            NoSpinWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                        else if (s.HasSpins(hiddenSpinConsts))
                        {
                            this.NotAnalysed++;
                            NotAnalysedWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                    }
                }
            }
        }
        
        public int SpinUp { get; private set; }
        public int SpinUpWon { get; private set; }
        public int SpinDown { get; private set; }
        public int SpinDownWon { get; private set; }
        public int NoSpin { get; private set; }
        public int NoSpinWon { get; private set; }
        public int NotAnalysed { get; private set; }
        public int NotAnalysedWon { get; private set; }
        public MatchPlayer Player { get; private set; }

        private bool CountStroke(Stroke stroke)
        {
            return stroke.Player == Player && stroke.Number == 1;
        }
    }
}
