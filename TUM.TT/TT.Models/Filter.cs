using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Models
{
    class Filter
    {
        public int StrokeNumber;

        public HashSet<Positions.Length> StrokeLengths;
        public HashSet<Positions.Table> TablePositions;
        public HashSet<Models.Util.Enums.Stroke.Aggressiveness> Aggressiveness;
        public HashSet<Models.Util.Enums.Stroke.Specials> Specials;
        public Models.Util.Enums.Stroke.StepAround StepAround;
        public HashSet<Models.Util.Enums.Stroke.Technique> StrokeTec;
        public Models.Util.Enums.Stroke.Hand Hand;

        public Filter()
        {
            StrokeLengths = new HashSet<Positions.Length>();
            TablePositions = new HashSet<Positions.Table>();
            Aggressiveness = new HashSet<Util.Enums.Stroke.Aggressiveness>();
            Specials = new HashSet<Util.Enums.Stroke.Specials>();
            StepAround = Util.Enums.Stroke.StepAround.Not;
            StrokeTec = new HashSet<Util.Enums.Stroke.Technique>();
            Hand = Util.Enums.Stroke.Hand.None;
        }

        public Filter(int strokeNumber) : this()
        {
            this.StrokeNumber = strokeNumber;
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            List<Rally> returnRallies = new List<Models.Rally>(inputRallies);
            foreach(Rally r in inputRallies)
            {
                // get Stroke on which filter is applied
                Stroke stroke = r.Strokes[StrokeNumber];

                // if Stroke does not Contain one of the Attributes in the HashSets Remove it from the return Array
                if (!stroke.HasStrokeLength(StrokeLengths)) returnRallies.Remove(r);
                if (!stroke.HasTablePosition(TablePositions)) returnRallies.Remove(r);
                if (!stroke.HasAggressiveness(Aggressiveness)) returnRallies.Remove(r);
                if (!stroke.HasSpecials(Specials)) returnRallies.Remove(r);
                if (!stroke.HasStrokeTec(StrokeTec)) returnRallies.Remove(r);
                if (!stroke.HasStepAround(StepAround)) returnRallies.Remove(r);
                if (!stroke.HasHand(Hand)) returnRallies.Remove(r);
            }
            return returnRallies.ToArray();
        }
    }
}
