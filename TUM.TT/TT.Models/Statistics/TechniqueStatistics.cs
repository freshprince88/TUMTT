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
            this.NumberToTechniqueCountDict = new SortedDictionary<int, IDictionary<string, int>>();            

            this.Player = MatchPlayer.None;
            if (match.FirstPlayer.Equals(p))
                this.Player = MatchPlayer.First;
            else if (match.SecondPlayer.Equals(p))
                this.Player = MatchPlayer.Second;

            var pushConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Push , Util.Enums.Stroke.Technique.PushAggressive};
            var flipConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Flip , Util.Enums.Stroke.Technique.Banana};
            var topspinConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Topspin, Util.Enums.Stroke.Technique.TopspinSpin, Util.Enums.Stroke.Technique.TopspinTempo };
            var blockConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Block, Util.Enums.Stroke.Technique.BlockTempo, Util.Enums.Stroke.Technique.BlockChop };
            var counterConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Counter };
            var smashConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Smash };
            var lobConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Lob };
            var chopConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Chop };
            var specialConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Special };
            var miscellaneousConsts = new List<Util.Enums.Stroke.Technique>(1) { Util.Enums.Stroke.Technique.Miscellaneous };

            foreach (var r in rallies)
            {
                foreach (var s in r.Strokes)
                {
                    if (CountStroke(s, Player, strokeNumber, stat: "Technique"))
                    {
                        IDictionary<string, int> techniqueToCountDict;
                        if (!NumberToTechniqueCountDict.TryGetValue(s.Number, out techniqueToCountDict))
                        {
                            techniqueToCountDict = new Dictionary<string, int>();
                            foreach (var technique in techniques)
                                techniqueToCountDict[technique] = 0;
                            NumberToTechniqueCountDict[s.Number] = techniqueToCountDict;
                        }

                        if (s.HasStrokeTec(pushConsts))
                        {
                            this.Push++;
                            PushWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Push"]++;
                        }
                        else if (s.HasStrokeTec(flipConsts))
                        {
                            this.Flip++;
                            FlipWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Flip"]++;
                        }
                        else if (s.HasStrokeTec(topspinConsts))
                        {
                            this.Topspin++;
                            TopspinWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Topspin"]++;
                        }
                        else if (s.HasStrokeTec(blockConsts))
                        {
                            this.Block++;
                            BlockWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Block"]++;
                        }
                        else if (s.HasStrokeTec(counterConsts))
                        {
                            this.Counter++;
                            CounterWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Counter"]++;
                        }
                        else if (s.HasStrokeTec(smashConsts))
                        {
                            this.Smash++;
                            SmashWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Smash"]++;
                        }
                        else if (s.HasStrokeTec(lobConsts))
                        {
                            this.Lob++;
                            LobWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Lob"]++;
                        }
                        else if (s.HasStrokeTec(chopConsts))
                        {
                            this.Chop++;
                            ChopWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Chop"]++;
                        }
                        else if (s.HasStrokeTec(specialConsts))
                        {
                            this.Special++;
                            SpecialWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Special"]++;
                        }
                        else if (s.HasStrokeTec(miscellaneousConsts))
                        {
                            this.Miscellaneous++;
                            MiscellaneousWon += s.Rally.Winner == Player ? 1 : 0;
                            NumberToTechniqueCountDict[s.Number]["Miscellaneous"]++;
                        }
                        else
                        {
                            int n;
                            if (!NumberToTechniqueCountDict[s.Number].TryGetValue("N/A", out n))
                                NumberToTechniqueCountDict[s.Number]["N/A"] = 0;
                            NumberToTechniqueCountDict[s.Number]["N/A"]++;
                        }
                    }
                }
            }
        }

        private static readonly IEnumerable<string> techniques = new List<string>() {
            "Push", "Flip", "Topspin", "Block", "Counter", "Smash", "Lob", "Chop", "Special", "Miscellaneous"
        };

        public SortedDictionary<int, IDictionary<string, int>> NumberToTechniqueCountDict { get; set; }
        public int Push { get; private set; }
        public int PushWon { get; private set; }
        public int Flip { get; private set; }
        public int FlipWon { get; private set; }
        public int Topspin { get; private set; }
        public int TopspinWon { get; private set; }
        public int Block { get; private set; }
        public int BlockWon { get; private set; }
        public int Counter { get; private set; }
        public int CounterWon { get; private set; }
        public int Smash { get; private set; }
        public int SmashWon { get; private set; }
        public int Lob { get; private set; }
        public int LobWon { get; private set; }
        public int Chop { get; private set; }
        public int ChopWon { get; private set; }
        public int Special { get; private set; }
        public int SpecialWon { get; private set; }
        public int Miscellaneous { get; private set; }
        public int MiscellaneousWon { get; private set; }
        public MatchPlayer Player { get; private set; }

        public override bool CountStroke(Stroke stroke, MatchPlayer player, int strokeNumber = -1, string stat = null)
        {
            return stroke.Number != 1 && base.CountStroke(stroke, player, strokeNumber, stat);
        }
    }
}
