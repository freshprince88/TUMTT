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
            var player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                player = MatchPlayer.Second;

            foreach (var r in rallies)
            {
                foreach (var stroke in r.Strokes)
                {
                    if (CountStroke(stroke, player, strokeNr))
                    {            
                        this.Forehand += stroke.EnumSide == Util.Enums.Stroke.Hand.Forehand ? 1 : 0;
                        this.Backhand += stroke.EnumSide == Util.Enums.Stroke.Hand.Backhand ? 1 : 0;
                        this.NotAnalysed += stroke.EnumSide == Util.Enums.Stroke.Hand.None ? 1 : 0;
                    }
                }
            }
        }

        public int Backhand { get; private set; }
        public int Forehand { get; private set; }
        public int NotAnalysed { get; private set; }

    }
}
