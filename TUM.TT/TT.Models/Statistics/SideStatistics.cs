using System.Collections.Generic;

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
                    bool stepAround = stroke.StepAround;
                    if (CountStroke(stroke, player, strokeNr))
                    {
                        if (stroke.EnumSide == Util.Enums.Stroke.Hand.Forehand)
                        {
                            Forehand++;
                            ForehandStepAround += stepAround ? 1 : 0;
                            if (stroke.Rally.Winner == player)
                            {
                                ForehandWon++;
                                ForehandStepAroundWon += stepAround ? 1 : 0;
                            }
                        }
                        if (stroke.EnumSide == Util.Enums.Stroke.Hand.Backhand)
                        {
                            Backhand++;
                            BackhandStepAround += stepAround ? 1 : 0;
                            if (stroke.Rally.Winner == player)
                            {
                                BackhandWon++;
                                BackhandStepAroundWon += stepAround ? 1 : 0;
                            }
                        }
                        if (stroke.EnumSide == Util.Enums.Stroke.Hand.None)
                        {
                            NotAnalysed++;
                            NotAnalysedWon += stroke.Rally.Winner == player ? 1 : 0;
                        }
                    }
                }
            }
        }

        public int Backhand { get; private set; }
        public int BackhandWon { get; private set; }
        public int BackhandStepAround { get; private set; }
        public int BackhandStepAroundWon { get; private set; }
        public int Forehand { get; private set; }
        public int ForehandWon { get; private set; }
        public int ForehandStepAround { get; private set; }
        public int ForehandStepAroundWon { get; private set; }
        public int NotAnalysed { get; private set; }
        public int NotAnalysedWon { get; private set; }

    }
}
