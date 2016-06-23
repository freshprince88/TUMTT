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

        private Stroketechnique schlagtechnikField;

        private Spin spinField;

        private Placement platzierungField;

        private int nummer;

        private MatchPlayer spieler;

        private string schlägerseite;

        private string aufschlagart;

        private string balltreffpunkt;

        private string qualität;

        private double spielerposition;

        private string besonderes;

        private string verlauf;

        private bool umlaufen;

        private string aggressivität;

        private bool eröffnung;

        public Stroketechnique Schlagtechnik
        {
            get
            {
                return schlagtechnikField;
            }
            set
            {
                schlagtechnikField = value;
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
                spinField = value;
            }
        }

        public Placement Placement
        {
            get
            {
                return platzierungField;
            }
            set
            {
                platzierungField = value;
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
        public bool Eröffnung
        {
            get
            {
                return eröffnung;
            }
            set
            {
                RaiseAndSetIfChanged(ref eröffnung, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public int Number
        {
            get
            {
                return nummer;
            }
            set
            {
                RaiseAndSetIfChanged(ref nummer, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public MatchPlayer Player
        {
            get
            {
                return spieler;
            }
            set
            {
                RaiseAndSetIfChanged(ref spieler, value);
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string SpielerString
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
                return schlägerseite;
            }
            set
            {
                RaiseAndSetIfChanged(ref schlägerseite, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Servicetechnique
        {
            get
            {
                return aufschlagart;
            }
            set
            {
                RaiseAndSetIfChanged(ref aufschlagart, value);
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
                return balltreffpunkt;
            }
            set
            {
                RaiseAndSetIfChanged(ref balltreffpunkt, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Quality
        {
            get
            {
                return qualität;
            }
            set
            {
                RaiseAndSetIfChanged(ref qualität, value);
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
                return spielerposition;
            }
            set
            {
                RaiseAndSetIfChanged(ref spielerposition, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Specials
        {
            get
            {
                return besonderes;
            }
            set
            {
                RaiseAndSetIfChanged(ref besonderes, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Course
        {
            get
            {
                return verlauf;
            }
            set
            {
                RaiseAndSetIfChanged(ref verlauf, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public bool StepAround
        {
            get
            {
                return umlaufen;
            }
            set
            {
                RaiseAndSetIfChanged(ref umlaufen, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Aggressiveness
        {
            get
            {
                return aggressivität;
            }
            set
            {
                RaiseAndSetIfChanged(ref aggressivität, value);
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
                    return Course == "Net" || Course == "Out";
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
            if (Schlagtechnik == null)
            {
                Schlagtechnik = new Stroketechnique();
                Schlagtechnik.Type = "";
                Schlagtechnik.Option = "";
            }

            List<bool> ORresults = new List<bool>();
            foreach (var stroketec in tecs)
            {
                switch (stroketec)
                {
                    case Util.Enums.Stroke.Technique.Push:
                        ORresults.Add(Schlagtechnik.Type == "Push");
                        break;
                    case Util.Enums.Stroke.Technique.PushAggressive:
                        ORresults.Add(Schlagtechnik.Type == "Push" && Schlagtechnik.Option == "aggressive");
                        break;
                    case Util.Enums.Stroke.Technique.Flip:
                        ORresults.Add(Schlagtechnik.Type == "Flip");
                        break;
                    case Util.Enums.Stroke.Technique.Banana:
                        ORresults.Add(Schlagtechnik.Type == "Flip" && Schlagtechnik.Option == "Banana");
                        break;
                    case Util.Enums.Stroke.Technique.Topspin:
                        ORresults.Add(Schlagtechnik.Type == "Topspin");
                        break;
                    case Util.Enums.Stroke.Technique.TopspinSpin:
                        ORresults.Add(Schlagtechnik.Type == "Topspin" && Schlagtechnik.Option == "Spin");
                        break;
                    case Util.Enums.Stroke.Technique.TopspinTempo:
                        ORresults.Add(Schlagtechnik.Type == "Topspin" && Schlagtechnik.Option == "Tempo");
                        break;
                    case Util.Enums.Stroke.Technique.Block:
                        ORresults.Add(Schlagtechnik.Type == "Block");
                        break;
                    case Util.Enums.Stroke.Technique.BlockTempo:
                        ORresults.Add(Schlagtechnik.Type == "Block" && Schlagtechnik.Option == "Tempo");
                        break;
                    case Util.Enums.Stroke.Technique.BlockChop:
                        ORresults.Add(Schlagtechnik.Type == "Block" && Schlagtechnik.Option == "Chop");
                        break;
                    case Util.Enums.Stroke.Technique.Counter:
                        ORresults.Add(Schlagtechnik.Type == "Counter");
                        break;
                    case Util.Enums.Stroke.Technique.Smash:
                        ORresults.Add(Schlagtechnik.Type == "Smash");
                        break;
                    case Util.Enums.Stroke.Technique.Lob:
                        ORresults.Add(Schlagtechnik.Type == "Lob");
                        break;
                    case Util.Enums.Stroke.Technique.Chop:
                        ORresults.Add(Schlagtechnik.Type == "Chop");
                        break;
                    case Util.Enums.Stroke.Technique.Special:
                        ORresults.Add(Schlagtechnik.Type == "Special");
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasAggression(IEnumerable<Util.Enums.Stroke.Aggression> aggressions)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var agg in aggressions)
            {
                switch (agg)
                {
                    case Util.Enums.Stroke.Aggression.Aggressive:
                        ORresults.Add(Aggressiveness == "aggressive");
                        break;
                    case Util.Enums.Stroke.Aggression.Passive:
                        ORresults.Add(Aggressiveness == "passive");
                        break;
                    case Util.Enums.Stroke.Aggression.Control:
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
                    case Util.Enums.Stroke.Spin.ÜS:
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
                    case Util.Enums.Stroke.Spin.ÜSSL:
                        ORresults.Add(Spin.TS == "1" && Spin.SL == "1");
                        break;
                    case Util.Enums.Stroke.Spin.ÜSSR:
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
        private string art;

        private string option;

        /// <remarks/>
        [XmlAttribute]
        public string Type
        {
            get
            {
                return art;
            }
            set
            {
                RaiseAndSetIfChanged(ref art, value);
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

        private string üs;

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
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string TS
        {
            get
            {
                return üs;
            }
            set
            {
                RaiseAndSetIfChanged(ref üs, value);
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
