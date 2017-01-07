using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models.Statistics
{
    public class TechniqueStatistics : MatchStatistics
    {
        public TechniqueStatistics(Match match, object p, List<Rally> rallies, int strokeNumber) : base(match)
        {
            this.Player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                this.Player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                this.Player = MatchPlayer.Second;

            var topspinConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Topspin };
            var pushConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Push };
            var flipConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Flip };
            var smashConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Smash };
            var hiddenSpinConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Miscellaneous };

            foreach (var r in rallies)
            {
                foreach (var s in r.Strokes)
                {
                    if (CountStroke(s, Player, strokeNumber))
                    {
                        if (s.HasStrokeTec(topspinConsts))
                        {
                            this.Topspin++;
                            TopspinWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                        else if (s.HasStrokeTec(pushConsts))
                        {
                            this.Push++;
                            PushWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                        else if (s.HasStrokeTec(flipConsts))
                        {
                            this.Flip++;
                            FlipWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                        else if (s.HasStrokeTec(smashConsts))
                        {
                            this.Smash++;
                            SmashWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                        else if (s.HasStrokeTec(hiddenSpinConsts))
                        {
                            this.NotAnalysed++;
                            NotAnalysedWon += s.Rally.Winner == Player ? 1 : 0;
                        }
                    }
                }
            }
        }

        public int Topspin { get; private set; }
        public int TopspinWon { get; private set; }
        public int Push { get; private set; }
        public int PushWon { get; private set; }
        public int Flip { get; private set; }
        public int FlipWon { get; private set; }
        public int Smash { get; private set; }
        public int SmashWon { get; private set; }
        public int NotAnalysed { get; private set; }
        public int NotAnalysedWon { get; private set; }
        public MatchPlayer Player { get; private set; }

        public override bool CountStroke(Stroke stroke, MatchPlayer player, int strokeNumber = -1)
        {
            return strokeNumber != 1 && base.CountStroke(stroke, player, strokeNumber);
        }
    }
}
