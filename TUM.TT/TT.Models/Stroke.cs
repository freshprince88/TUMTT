using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TT.Models.Util.Enums;

namespace TT.Models
{
    public class Stroke : PropertyChangedBase
    {
        /// <summary>
        /// Backs the <see cref="Rally"/> property.
        /// </summary>
        private Rally rally;

        private Stroketechnique strokeTechniqueField;

        private Spin spinField;

        private Placement placementField;

        private int number;

        private MatchPlayer player;

        private string side;

        private string serviceTechnique;

        private string pointOfContact;

        private string quality;

        private double playerposition;

        private string specials;

        private string course;

        private bool stepAround;

        private string aggressiveness;

        private bool openingShot;

        public delegate void StrokePlacementChangedEventHandler(object source, EventArgs args);

        public event StrokePlacementChangedEventHandler StrokePlacementChanged;

        public Stroke()
        {
            playerposition = -1;
        }


        public Stroketechnique Stroketechnique
        {
            get
            {
                return strokeTechniqueField;
            }
            set
            {
                RaiseAndSetIfChanged(ref strokeTechniqueField, value);
            }
        }

        public Spin Spin
        {
            get
            {
                return spinField;
            }
            set
            {
                RaiseAndSetIfChanged(ref spinField, value);

            }
        }

        public Placement Placement
        {
            get
            {
                return placementField;
            }
            set
            {
                Placement p = new Placement();
                // round Placement to 3 digits so numbers in save-file are not to long
                if (value != null)
                {
                    p.WX = Math.Round(value.WX, 3);
                    p.WY = Math.Round(value.WY, 3);
                }
                else
                {
                    p = value;
                }
                // placementField = p;
                // Notify about changed Placement for Save Method
                RaiseAndSetIfChanged(ref placementField, p);               
                OnPlacementChanged();
            }
        }

        protected virtual void OnPlacementChanged()
        {
            if (StrokePlacementChanged != null)
            {
                StrokePlacementChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets the match this rally.
        /// </summary>
        [XmlIgnore]
        public Rally Rally
        {
            get { return this.rally; }
            set { this.RaiseAndSetIfChanged(ref this.rally, value); }
        }

        /// <remarks/>
        [XmlAttribute]
        public bool OpeningShot
        {
            get
            {
                return openingShot;
            }
            set
            {
                RaiseAndSetIfChanged(ref openingShot, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                RaiseAndSetIfChanged(ref number, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public MatchPlayer Player
        {
            get
            {
                return player;
            }
            set
            {
                RaiseAndSetIfChanged(ref player, value);
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string PlayerString
        {
            get
            {
                return Player.ToString();
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Side
        {
            get
            {
                return side;
            }
            set
            {
                RaiseAndSetIfChanged(ref side, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Servicetechnique
        {
            get
            {
                return serviceTechnique;
            }
            set
            {
                RaiseAndSetIfChanged(ref serviceTechnique, value);
                this.NotifyPropertyChanged("Servicetechnique");
                this.NotifyPropertyChanged();
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string PointOfContact
        {
            get
            {
                return pointOfContact;
            }
            set
            {
                RaiseAndSetIfChanged(ref pointOfContact, value);
            }
        }
        
        /// <remarks/>
        [XmlAttribute]
        public string Quality
        {
            get
            {
                return quality;
            }
            set
            {
                RaiseAndSetIfChanged(ref quality, value);
            }
        }

        [XmlAttribute("Playerposition")]
        public string PlayerpositionString
        {
            get { return this.Playerposition.ToString(); }
            set { this.Playerposition = value == string.Empty || value == null ? double.NaN : Convert.ToDouble(value); }
        }

        /// <remarks/>
        [XmlIgnore]
        public double Playerposition
        {
            get
            {
                return playerposition;
            }
            set
            {
                RaiseAndSetIfChanged(ref playerposition, Math.Round(value, 3));
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Specials
        {
            get
            {
                return specials;
            }
            set
            {
                RaiseAndSetIfChanged(ref specials, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Course
        {
            get
            {
                return course;
            }
            set
            { if (course != value) { 
                RaiseAndSetIfChanged(ref course, value);
                    NotifyPropertyChanged("Course");
                    NotifyPropertyChanged();
                }
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public bool StepAround
        {
            get
            {
                return stepAround;
            }
            set
            {
                RaiseAndSetIfChanged(ref stepAround, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Aggressiveness
        {
            get
            {
                return aggressiveness;
            }
            set
            {
                RaiseAndSetIfChanged(ref aggressiveness, value);
            }
        }

        public void Update()
        {
            if (this.Rally == null)
            {
                throw new InvalidOperationException("Stroke not part of a Rally");
            }
            else
            {
                this.UpdateNummer();
                this.UpdatePlayer();
            }
        }

        /// <summary>
        /// Updates the nummer of this rally.
        /// </summary>
        private void UpdateNummer()
        {
            Stroke previousStroke = this.Rally.FindPreviousStroke(this);

            // We don't need to update the server if there is no previous rally
            if (previousStroke != null)
            {
                Number = previousStroke.Number + 1;
            }
            else
            {
                Number = 1;
            }
        }

        /// <summary>
        /// Updates the nummer of this rally.
        /// </summary>
        private void UpdatePlayer()
        {
            Stroke previousStroke = this.Rally.FindPreviousStroke(this);

            // We don't need to update the server if there is no previous rally
            if (previousStroke != null)
            {
                this.Player = previousStroke.Player.Other();
            }
            else
            {
                this.Player = Rally.Server;
            }
        }

        #region 9 Fields

        public bool IsTopLeft()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }


            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X >= 0 && X < 50.5 && Y >= 0 && Y <= 46) || (X <= 152.5 && X > 102 && Y <= 274 && Y >= 228);
        }

        public bool IsTopMid()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X >= 50.5 && X <= 102 && Y >= 0 && Y <= 46) || (X >= 50.5 && X <= 102 && Y >= 228 && Y <= 274);
        }

        public bool IsTopRight()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X <= 152.5 && X > 102 && Y >= 0 && Y <= 46) || (X >= 0 && X < 50.5 && Y >= 228 && Y <= 274);
        }

        public bool IsMidLeft()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X >= 0 && X < 50.5 && Y <= 92 && Y > 46) || (X <= 152.5 && X > 102 && Y < 228 && Y >= 182);
        }

        public bool IsMidMid()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X >= 50.5 && X <= 102 && Y <= 92 && Y > 46) || (X >= 50.5 && X <= 102 && Y < 228 && Y >= 182);
        }

        public bool IsMidRight()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X <= 152.5 && X > 102 && Y <= 92 && Y > 46) || (X >= 0 && X < 50.5 && Y < 228 && Y >= 182);
        }

        public bool IsBotLeft()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X >= 0 && X < 50.5 && Y < 137 && Y > 92) || (X <= 152.5 && X > 102 && Y >= 137 && Y < 182);
        }

        public bool IsBotMid()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X >= 50.5 && X <= 102 && Y < 137 && Y > 92) || (X >= 50.5 && X <= 102 && Y >= 137 && Y < 182);
        }

        public bool IsBotRight()
        {
            if (Placement == null)
            {
                Placement = new Placement();
                Placement.WX = -1;
                Placement.WY = -1;

            }

            double X = Placement.WX == double.NaN ? -1 : Placement.WX;
            double Y = Placement.WY == double.NaN ? -1 : Placement.WY;

            return (X <= 152.5 && X > 102 && Y < 137 && Y > 92) || (X >= 0 && X < 50.5 && Y >= 137 && Y < 182);
        }
        #endregion

        #region Short , HalfLong, Long

        public bool IsShort()
        {
            return this.IsBotLeft() || this.IsBotMid() || this.IsBotRight();
        }

        public bool IsHalfLong()
        {
            return this.IsMidLeft() || this.IsMidMid() || this.IsMidRight();
        }

        public bool IsLong()
        {
            return this.IsTopLeft() || this.IsTopMid() || this.IsTopRight();
        }
        #endregion

        #region Forehand Side, Middle, Backhand Side

        public bool IsForehandSide()
        {
            return this.IsBotLeft() || this.IsMidLeft() || this.IsTopLeft();
        }

        public bool IsBackhandSide()
        {
            return this.IsBotRight() || this.IsMidRight() || this.IsTopRight();
        }

        public bool IsMiddle()
        {
            return this.IsBotMid() || this.IsMidMid() || this.IsTopMid();
        }

        #endregion


        public bool IsOverTheTable()
        {
            return string.IsNullOrWhiteSpace(PointOfContact) ? false : this.PointOfContact.ToLower() == "über";
        }

        public bool IsAtTheTable()
        {
            return string.IsNullOrWhiteSpace(PointOfContact) ? false : PointOfContact.ToLower() == "hinter";
        }

        public bool IsHalfDistance()
        {
            return string.IsNullOrWhiteSpace(PointOfContact) ? false : PointOfContact.ToLower() == "halbdistanz";
        }


        #region Filter Methods

        public bool HasHand(Util.Enums.Stroke.Hand h)
        {
            switch (h)
            {
                case Util.Enums.Stroke.Hand.Fore:
                    return Side == "Forehand";
                case Util.Enums.Stroke.Hand.Back:
                    return Side == "Backhand";
                case Util.Enums.Stroke.Hand.None:
                    return true;
                case Util.Enums.Stroke.Hand.Both:
                    return true;
                default:
                    return false;
            }
        }

        public bool HasStepAround(Util.Enums.Stroke.StepAround s)
        {
            switch (s)
            {
                case Util.Enums.Stroke.StepAround.StepAround:
                    return StepAround;
                case Util.Enums.Stroke.StepAround.Not:
                    return true;
                default:
                    return false;
            }
        }

        public bool HasWinner(Util.Enums.Stroke.WinnerOrNetOut w)
        {
            switch (w)
            {
                case Util.Enums.Stroke.WinnerOrNetOut.Winner:
                    return Course == "Winner";
                case Util.Enums.Stroke.WinnerOrNetOut.NetOut:
                    return Course == "Net/Out";
                case Util.Enums.Stroke.WinnerOrNetOut.None:
                    return true;
                case Util.Enums.Stroke.WinnerOrNetOut.Both:
                    return true;
                default:
                    return false;
            }
        }

        public bool HasStrokeTec(IEnumerable<Util.Enums.Stroke.Technique> tecs)
        {
            if (Stroketechnique == null)
            {
                Stroketechnique = new Stroketechnique();
                Stroketechnique.Type = "";
                Stroketechnique.Option = "";
            }

            List<bool> ORresults = new List<bool>();
            foreach (var stroketec in tecs)
            {
                switch (stroketec)
                {
                    case Util.Enums.Stroke.Technique.Push:
                        ORresults.Add(Stroketechnique.Type == "Push");
                        break;
                    case Util.Enums.Stroke.Technique.PushAggressive:
                        ORresults.Add(Stroketechnique.Type == "Push" && Stroketechnique.Option == "aggressive");
                        break;
                    case Util.Enums.Stroke.Technique.Flip:
                        ORresults.Add(Stroketechnique.Type == "Flip");
                        break;
                    case Util.Enums.Stroke.Technique.Banana:
                        ORresults.Add(Stroketechnique.Type == "Flip" && Stroketechnique.Option == "Banana");
                        break;
                    case Util.Enums.Stroke.Technique.Topspin:
                        ORresults.Add(Stroketechnique.Type == "Topspin");
                        break;
                    case Util.Enums.Stroke.Technique.TopspinSpin:
                        ORresults.Add(Stroketechnique.Type == "Topspin" && Stroketechnique.Option == "Spin");
                        break;
                    case Util.Enums.Stroke.Technique.TopspinTempo:
                        ORresults.Add(Stroketechnique.Type == "Topspin" && Stroketechnique.Option == "Tempo");
                        break;
                    case Util.Enums.Stroke.Technique.Block:
                        ORresults.Add(Stroketechnique.Type == "Block");
                        break;
                    case Util.Enums.Stroke.Technique.BlockTempo:
                        ORresults.Add(Stroketechnique.Type == "Block" && Stroketechnique.Option == "Tempo");
                        break;
                    case Util.Enums.Stroke.Technique.BlockChop:
                        ORresults.Add(Stroketechnique.Type == "Block" && Stroketechnique.Option == "Chop");
                        break;
                    case Util.Enums.Stroke.Technique.Counter:
                        ORresults.Add(Stroketechnique.Type == "Counter");
                        break;
                    case Util.Enums.Stroke.Technique.Smash:
                        ORresults.Add(Stroketechnique.Type == "Smash");
                        break;
                    case Util.Enums.Stroke.Technique.Lob:
                        ORresults.Add(Stroketechnique.Type == "Lob");
                        break;
                    case Util.Enums.Stroke.Technique.Chop:
                        ORresults.Add(Stroketechnique.Type == "Chop");
                        break;
                    case Util.Enums.Stroke.Technique.Special:
                        ORresults.Add(Stroketechnique.Type == "Special");
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasAggressiveness(IEnumerable<Util.Enums.Stroke.Aggressiveness> aggressions)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var agg in aggressions)
            {
                switch (agg)
                {
                    case Util.Enums.Stroke.Aggressiveness.Aggressive:
                        ORresults.Add(Aggressiveness == "aggressive");
                        break;
                    case Util.Enums.Stroke.Aggressiveness.Passive:
                        ORresults.Add(Aggressiveness == "passive");
                        break;
                    case Util.Enums.Stroke.Aggressiveness.Control:
                        ORresults.Add(Aggressiveness == "Control");
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasQuality(Util.Enums.Stroke.Quality q)
        {
            switch (q)
            {
                case Util.Enums.Stroke.Quality.Good:
                    return Quality == "good";
                case Util.Enums.Stroke.Quality.Bad:
                    return Quality == "bad";
                case Util.Enums.Stroke.Quality.None:
                    return true;
                case Util.Enums.Stroke.Quality.Both:
                    return Quality == "good" || Quality == "bad";
                default:
                    return false;
            }
        }

        public bool HasTablePosition(IEnumerable<Positions.Table> pos)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var sel in pos)
            {
                switch (sel)
                {
                    case Positions.Table.TopLeft:
                        ORresults.Add(IsTopLeft());
                        break;
                    case Positions.Table.TopMid:
                        ORresults.Add(IsTopMid());
                        break;
                    case Positions.Table.TopRight:
                        ORresults.Add(IsTopRight());
                        break;
                    case Positions.Table.MidLeft:
                        ORresults.Add(IsMidLeft());
                        break;
                    case Positions.Table.MidMid:
                        ORresults.Add(IsMidMid());
                        break;
                    case Positions.Table.MidRight:
                        ORresults.Add(IsMidRight());
                        break;
                    case Positions.Table.BotLeft:
                        ORresults.Add(IsBotLeft());
                        break;
                    case Positions.Table.BotMid:
                        ORresults.Add(IsBotMid());
                        break;
                    case Positions.Table.BotRight:
                        ORresults.Add(IsBotRight());
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasStrokeLength(IEnumerable<Positions.Length> l)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var sel in l)
            {
                switch (sel)
                {
                    case Positions.Length.OverTheTable:
                        ORresults.Add(IsOverTheTable());
                        break;
                    case Positions.Length.AtTheTable:
                        ORresults.Add(IsAtTheTable());
                        break;
                    case Positions.Length.HalfDistance:
                        ORresults.Add(IsHalfDistance());
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasSpins(IEnumerable<Util.Enums.Stroke.Spin> spins)
        {
            if (Spin == null)
                return true;

            List<bool> ORresults = new List<bool>();
            foreach (var spin in spins)
            {
                switch (spin)
                {
                    case Util.Enums.Stroke.Spin.Hidden:
                        ORresults.Add(Spin.TS == "" || Spin.SL == "" || Spin.SR == "" || Spin.US == "" || Spin.No == "");
                        break;
                    case Util.Enums.Stroke.Spin.TS:
                        ORresults.Add(Spin.TS == "1" && Spin.SL == "0" && Spin.SR == "0");
                        break;
                    case Util.Enums.Stroke.Spin.SR:
                        ORresults.Add(Spin.SR == "1" && Spin.TS == "0" && Spin.US == "0");
                        break;
                    case Util.Enums.Stroke.Spin.No:
                        ORresults.Add(Spin.No == "1" && Spin.SL == "0" && Spin.SR == "0" && Spin.TS == "0" && Spin.US == "0");
                        break;
                    case Util.Enums.Stroke.Spin.SL:
                        ORresults.Add(Spin.SL == "1" && Spin.TS == "0" && Spin.US == "0");
                        break;
                    case Util.Enums.Stroke.Spin.US:
                        ORresults.Add(Spin.US == "1" && Spin.SL == "0" && Spin.SR == "0");
                        break;
                    case Util.Enums.Stroke.Spin.USSL:
                        ORresults.Add(Spin.US == "1" && Spin.SL == "1");
                        break;
                    case Util.Enums.Stroke.Spin.USSR:
                        ORresults.Add(Spin.US == "1" && Spin.SR == "1");
                        break;
                    case Util.Enums.Stroke.Spin.TSSL:
                        ORresults.Add(Spin.TS == "1" && Spin.SL == "1");
                        break;
                    case Util.Enums.Stroke.Spin.TSSR:
                        ORresults.Add(Spin.TS == "1" && Spin.SR == "1");
                        break;
                    default:
                        break;

                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasServices(IEnumerable<Util.Enums.Stroke.Services> services)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var service in services)
            {
                switch (service)
                {
                    case Util.Enums.Stroke.Services.Pendulum:
                        ORresults.Add(Servicetechnique == "Pendulum");
                        break;
                    case Util.Enums.Stroke.Services.Reverse:
                        ORresults.Add(Servicetechnique == "Reverse");
                        break;
                    case Util.Enums.Stroke.Services.Tomahawk:
                        ORresults.Add(Servicetechnique == "Tomahawk");
                        break;
                    case Util.Enums.Stroke.Services.Special:
                        ORresults.Add(Servicetechnique == "Special");
                        break;
                    default:
                        break;

                }

            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasServiceWinners(IEnumerable<Util.Enums.Stroke.ServiceWinner> serviceWinner)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var service in serviceWinner)
            {
                switch (service)
                {
                    case Util.Enums.Stroke.ServiceWinner.All:
                        ORresults.Add((Servicetechnique == "Pendulum" || Servicetechnique == "Reverse" || Servicetechnique == "Tomahawk" || Servicetechnique == "Special"));
                        break;
                    case Util.Enums.Stroke.ServiceWinner.Short:
                        ORresults.Add((Servicetechnique == "Pendulum" || Servicetechnique == "Reverse" || Servicetechnique == "Tomahawk" || Servicetechnique == "Special") && this.IsShort());
                        break;
                    case Util.Enums.Stroke.ServiceWinner.Long:
                        ORresults.Add((Servicetechnique == "Pendulum" || Servicetechnique == "Reverse" || Servicetechnique == "Tomahawk" || Servicetechnique == "Special") && this.IsLong());
                        break;
                    default:
                        break;

                }

            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasSpecials(Util.Enums.Stroke.Specials specials)
        {
            switch (specials)
            {
                case Util.Enums.Stroke.Specials.EdgeTable:
                    return Specials == "EdgeTable";
                case Util.Enums.Stroke.Specials.EdgeRacket:
                    return Specials == "EdgeRacket";
                case Util.Enums.Stroke.Specials.EdgeNet:
                    return Specials == "EdgeNet";
                case Util.Enums.Stroke.Specials.None:
                    return true;
                case Util.Enums.Stroke.Specials.Both:
                    return Specials == "EdgeTable" || Specials == "EdgeNet";
                default:
                    return false;
            }
        }

        public bool HasServerPosition(IEnumerable<Positions.Server> server)
        {
            if (Placement == null)
                return true;

            List<bool> ORresults = new List<bool>();
            double X;
            double Seite = Placement.WY == double.NaN ? 999 : Placement.WY;
            if (Seite >= 137)
                X = 152.5 - (Playerposition == double.NaN ? 999 : Playerposition);
            else
                X = Playerposition == double.NaN ? 999 : Playerposition;

            foreach (var sel in server)
            {
                switch (sel)
                {
                    case Positions.Server.Left:
                        ORresults.Add(0 <= X && X <= 30.5);
                        break;
                    case Positions.Server.HalfLeft:
                        ORresults.Add(30.5 < X && X <= 61);
                        break;
                    case Positions.Server.Mid:
                        ORresults.Add(61 < X && X <= 91.5);
                        break;
                    case Positions.Server.HalfRight:
                        ORresults.Add(91.5 < X && X <= 122);
                        break;
                    case Positions.Server.Right:
                        ORresults.Add(122 < X && X <= 152.5);
                        break;
                    default:
                        break;
                }
            }

            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool IsLeftServicePosition()
        {
            if (Placement == null)
                return true;

            double aufschlagPosition;
            double seite = this.Placement.WY == double.NaN ? 999 : Convert.ToDouble(this.Placement.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (this.Playerposition == double.NaN ? 999 : Convert.ToDouble(this.Playerposition));
            }
            else
            {
                aufschlagPosition = this.Playerposition == double.NaN ? 999 : Convert.ToDouble(this.Playerposition);
            }

            return (0 <= aufschlagPosition && aufschlagPosition < 50.5);
        }

        public bool IsMiddleServicePosition()
        {
            if (Placement == null)
                return true;

            double aufschlagPosition;
            double seite = this.Placement.WY == double.NaN ? 999 : Convert.ToDouble(this.Placement.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (this.Playerposition == double.NaN ? 999 : Convert.ToDouble(this.Playerposition));
            }
            else
            {
                aufschlagPosition = this.Playerposition == double.NaN ? 999 : Convert.ToDouble(this.Playerposition);
            }

            return (50.5 <= aufschlagPosition && aufschlagPosition <= 102);
        }

        public bool IsRightServicePosition()
        {
            if (Placement == null)
                return true;

            double aufschlagPosition;
            double seite = this.Placement.WY == double.NaN ? 999 : Convert.ToDouble(this.Placement.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (this.Playerposition == double.NaN ? 999 : Convert.ToDouble(this.Playerposition));
            }
            else
            {
                aufschlagPosition = this.Playerposition == double.NaN ? 999 : Convert.ToDouble(this.Playerposition);
            }

            return (102 < aufschlagPosition && aufschlagPosition <= 152.5);
        }

        #endregion

        #region Statistic Methods



        #endregion

    }



    public class Stroketechnique : PropertyChangedBase
    {
        private string type;

        private string option;

        /// <remarks/>
        [XmlAttribute]
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                RaiseAndSetIfChanged(ref type, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Option
        {
            get
            {
                return option;
            }
            set
            {
                option = value;
            }
        }
    }

    public class Spin : PropertyChangedBase
    {

        private string us;

        private string ts;

        private string sl;

        private string sr;

        private string no;

        /// <remarks/>
        [XmlAttribute]
        public string US
        {
            get
            {
                return us;
            }
            set
            {
                RaiseAndSetIfChanged(ref us, value);
                NotifyPropertyChanged();
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string TS
        {
            get
            {
                return ts;
            }
            set
            {
                RaiseAndSetIfChanged(ref ts, value);
                NotifyPropertyChanged();

            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string SL
        {
            get
            {
                return sl;
            }
            set
            {
                RaiseAndSetIfChanged(ref sl, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string SR
        {
            get
            {
                return sr;
            }
            set
            {
                RaiseAndSetIfChanged(ref sr, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string No
        {
            get
            {
                return no;
            }
            set
            {
                RaiseAndSetIfChanged(ref no, value);
            }
        }
    }

    public class Placement : PropertyChangedBase
    {

        private double wx;

        private double wy;

        /// <remarks/>
        [XmlIgnore]
        public double WX
        {
            get
            {
                return wx;
            }
            set
            {
                RaiseAndSetIfChanged(ref wx, value);
            }
        }

        [XmlAttribute("WX")]
        public string WXString
        {
            get { return this.WX.ToString(); }
            set { this.WX = value == string.Empty || value == null ? double.NaN : Convert.ToDouble(value); }
        }

        /// <remarks/>
        [XmlIgnore]
        public double WY
        {
            get
            {
                return wy;
            }
            set
            {
                RaiseAndSetIfChanged(ref wy, value);
            }
        }

        [XmlAttribute("WY")]
        public string WYString
        {
            get { return this.WY.ToString(); }
            set { this.WY = value == string.Empty || value == null ? double.NaN : Convert.ToDouble(value); }
        }
    }
}
