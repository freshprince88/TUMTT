using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models.Statistics
{
    public class SideStatistics : MatchStatistics
    {
        public SideStatistics(Match match, object p, int strokeNr, List<Rally> rallies) : base(match)
        {
            this.StrokeNumber = strokeNr;

            this.Player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                this.Player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                this.Player = MatchPlayer.Second;

            foreach (var r in rallies)
            {
                foreach (var s in r.Strokes)
                {
                    if (CountStroke(s))
                    {            
                        this.Forehand += s.EnumSide == Util.Enums.Stroke.Hand.Forehand ? 1 : 0;
                        this.Backhand += s.EnumSide == Util.Enums.Stroke.Hand.Backhand ? 1 : 0;
                        this.NotAnalysed += s.EnumSide == Util.Enums.Stroke.Hand.None ? 1 : 0;
                    }
                }
            }
        }

        public int Backhand { get; private set; }
        public int Forehand { get; private set; }
        public int NotAnalysed { get; private set; }
        public MatchPlayer Player { get; private set; }
        public int StrokeNumber { get; private set; }

        private bool CountStroke(Stroke stroke)
        {
            if (stroke.Player == Player)
            {
                switch (StrokeNumber)
                {
                    case int.MaxValue:
                    {
                        var lastWinnerStroke = stroke.Rally.LastWinnerStroke();
                        return lastWinnerStroke != null && stroke.Number == lastWinnerStroke.Number;
                    }
                    case -1: return true;
                    default: return stroke.Number == StrokeNumber;
                }
            }
            return false;
        }
    }
}
