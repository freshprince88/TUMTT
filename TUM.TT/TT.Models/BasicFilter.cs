using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public class BasicFilter : IFilter
    {

        public Models.Util.Enums.Stroke.Point Point { get; set; }

        public Models.Util.Enums.Stroke.Crunch Crunch { get; set; }

        public HashSet<int> Sets { get; set; }
        public HashSet<int> RallyLengths { get; set; }
        public int MinRallyLength { get; set; }
        public bool LastStroke { get; set; }

        public BasicFilter(int minRallyLength = 0, bool lastStroke = false)
        {
            Point = Models.Util.Enums.Stroke.Point.None;
            Crunch = Models.Util.Enums.Stroke.Crunch.Not;
            Sets = new HashSet<int>();
            RallyLengths = new HashSet<int>();

            MinRallyLength = minRallyLength;
            LastStroke = lastStroke;
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            List<Rally> returnRallies = new List<Models.Rally>(inputRallies);
            foreach (Rally r in inputRallies)
            {
                if (!this.accepts(r))
                {
                    returnRallies.Remove(r);
                }
            }
            return returnRallies.ToArray();
        }

        public bool accepts(Rally rally)
        {
            if (Convert.ToInt32(rally.Length) > MinRallyLength)
            {
                // if Stroke does not Contain one of the Attributes in the HashSets Remove it from the return Array
                if (!rally.HasSet(Sets)) return false;
                if (!rally.HasRallyLength(RallyLengths)) return false;
                if (!rally.HasCrunchTime(Crunch)) return false;
                if (!rally.HasPoint(Point)) return false;
            }
            else
            {
                return false;
            }

            return true;
    }
        
    }
}
