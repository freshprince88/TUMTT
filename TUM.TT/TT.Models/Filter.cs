using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Models
{
    public class Filter : IFilter
    {
        public const string FILTER_PATH = "Filters";

        private Guid id;
        public Guid ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public DateTime CreationDate;

        public string Name;

        public int StrokeNumber;

        public bool Enabled;

        /* FOR SERVICE */
        public HashSet<Models.Util.Enums.Stroke.Spin> Spins;
        public HashSet<Models.Util.Enums.Stroke.Services> Services;
        public HashSet<Positions.Server> ServerPositions;

        public HashSet<Positions.Length> StrokeLengths;
        public HashSet<Positions.Table> TablePositions;
        public HashSet<Models.Util.Enums.Stroke.Aggressiveness> Aggressiveness;
        public HashSet<Models.Util.Enums.Stroke.Specials> Specials;
        public Models.Util.Enums.Stroke.StepAround StepAround;
        public HashSet<Models.Util.Enums.Stroke.Technique> StrokeTec;
        public Models.Util.Enums.Stroke.Hand Hand;
        public Models.Util.Enums.Stroke.Quality Quality;

        public Models.Util.Enums.Stroke.Player Player;

        public Filter()
        {
            this.id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;

            Spins = new HashSet<Util.Enums.Stroke.Spin>();
            Services = new HashSet<Util.Enums.Stroke.Services>();
            ServerPositions = new HashSet<Util.Enums.Positions.Server>();


            StrokeLengths = new HashSet<Positions.Length>();
            TablePositions = new HashSet<Positions.Table>();
            Aggressiveness = new HashSet<Util.Enums.Stroke.Aggressiveness>();
            Specials = new HashSet<Util.Enums.Stroke.Specials>();
            StepAround = Util.Enums.Stroke.StepAround.Not;
            StrokeTec = new HashSet<Util.Enums.Stroke.Technique>();
            Hand = Util.Enums.Stroke.Hand.None;
            Quality = Util.Enums.Stroke.Quality.None;
            Player = Models.Util.Enums.Stroke.Player.None;

            Enabled = true;
        }

        public Filter(int strokeNumber) : this()
        {
            this.StrokeNumber = strokeNumber;
        }

        public Filter(int strokeNumber, string name) : this(strokeNumber)
        {
            this.Name = name;
        }

        public Filter(Filter f)
        {
            this.id = f.id;

            this.Name = f.Name;

            Spins = f.Spins;
            Services = f.Services;
            ServerPositions = f.ServerPositions;

            StrokeLengths = f.StrokeLengths;
            TablePositions = f.TablePositions;
            Aggressiveness = f.Aggressiveness;
            Specials = f.Specials;
            StepAround = f.StepAround;
            StrokeTec = f.StrokeTec;
            Hand = f.Hand;
            Quality = f.Quality;
            Player = f.Player;

            Enabled = f.Enabled;
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            List<Rally> returnRallies = new List<Models.Rally>(inputRallies);
            foreach(Rally r in inputRallies)
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
            if (rally.Strokes.Count > (StrokeNumber))
            {
                // get Stroke on which filter is applied
                Stroke stroke = rally.Strokes[StrokeNumber];

                // if Stroke does not Contain one of the Attributes in the HashSets Remove it from the return Array
                if (!stroke.HasServices(Services)) return false;
                if (!stroke.HasServerPosition(ServerPositions)) return false;
                if (!stroke.HasSpins(Spins)) return false;

                if (!stroke.HasStrokeLength(StrokeLengths)) return false;
                if (!stroke.HasTablePosition(TablePositions)) return false;
                if (!stroke.HasAggressiveness(Aggressiveness)) return false;
                if (!stroke.HasSpecials(Specials)) return false;
                if (!stroke.HasStrokeTec(StrokeTec)) return false;
                if (!stroke.HasStepAround(StepAround)) return false;
                if (!stroke.HasHand(Hand)) return false;
                if (!stroke.HasQuality(Quality)) return false;
                if (!stroke.HasPlayer(Player)) return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
