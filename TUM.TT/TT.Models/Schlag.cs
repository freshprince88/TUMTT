using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TT.Models.Util.Enums;

namespace TT.Models
{
    public class Schlag : PropertyChangedBase
    {
        /// <summary>
        /// Backs the <see cref="Rally"/> property.
        /// </summary>
        private Rally rally;

        private Schlagtechnik schlagtechnikField;

        private Spin spinField;

        private Platzierung platzierungField;

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

        public Schlagtechnik Schlagtechnik
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

        public Platzierung Platzierung
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
        public int Nummer
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
        public MatchPlayer Spieler
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
                return Spieler.ToString();
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Schlägerseite
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
        public string Aufschlagart
        {
            get
            {
                return aufschlagart;
            }
            set
            {
                RaiseAndSetIfChanged(ref aufschlagart, value);
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string Balltreffpunkt
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
        public string Qualität
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

        [XmlAttribute("Spielerposition")]
        public string SpielerpositionString
        {
            get { return this.Spielerposition.ToString(); }
            set { this.Spielerposition = value == string.Empty || value == null ? double.NaN : Convert.ToDouble(value); }
        }

        /// <remarks/>
        [XmlIgnore]
        public double Spielerposition
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
        public string Besonderes
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
        public string Verlauf
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
        public bool Umlaufen
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
        public string Aggressivität
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
            Schlag previousStroke = this.Rally.FindPreviousStroke(this);

            // We don't need to update the server if there is no previous rally
            if (previousStroke != null)
            {
                Nummer = previousStroke.Nummer + 1;
            }
            else
            {
                Nummer = 1;
            }
        }

        /// <summary>
        /// Updates the nummer of this rally.
        /// </summary>
        private void UpdatePlayer()
        {
            Schlag previousStroke = this.Rally.FindPreviousStroke(this);

            // We don't need to update the server if there is no previous rally
            if (previousStroke != null)
            {
                this.Spieler = previousStroke.Spieler.Other();
            }
            else
            {
                this.Spieler = Rally.Server;
            }
        }

        #region 9 Fields

        public bool IsTopLeft()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X >= 0 && X < 50.5 && Y >= 0 && Y <= 46) || (X <= 152.5 && X > 102 && Y <= 274 && Y >= 228);
        }

        public bool IsTopMid()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X >= 50.5 && X <= 102 && Y >= 0 && Y <= 46) || (X >= 50.5 && X <= 102 && Y >= 228 && Y <= 274);
        }

        public bool IsTopRight()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X <= 152.5 && X > 102 && Y >= 0 && Y <= 46) || (X >= 0 && X < 50.5 && Y >= 228 && Y <= 274);
        }

        public bool IsMidLeft()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X >= 0 && X < 50.5 && Y <= 92 && Y > 46) || (X <= 152.5 && X > 102 && Y < 228 && Y >= 182);
        }

        public bool IsMidMid()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X >= 50.5 && X <= 102 && Y <= 92 && Y > 46) || (X >= 50.5 && X <= 102 && Y < 228 && Y >= 182);
        }

        public bool IsMidRight()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X <= 152.5 && X > 102 && Y <= 92 && Y > 46) || (X >= 0 && X < 50.5 && Y < 228 && Y >= 182);
        }

        public bool IsBotLeft()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X >= 0 && X < 50.5 && Y < 137 && Y > 92) || (X <= 152.5 && X > 102 && Y >= 137 && Y < 182);
        }

        public bool IsBotMid()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

            return (X >= 50.5 && X <= 102 && Y < 137 && Y > 92) || (X >= 50.5 && X <= 102 && Y >= 137 && Y < 182);
        }

        public bool IsBotRight()
        {
            if (Platzierung == null)
                return true;

            double X = Platzierung.WX == double.NaN ? -1 : Platzierung.WX;
            double Y = Platzierung.WY == double.NaN ? -1 : Platzierung.WY;

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
            return string.IsNullOrWhiteSpace(Balltreffpunkt) ? false : this.Balltreffpunkt.ToLower() == "über";
        }

        public bool IsAtTheTable()
        {
            return string.IsNullOrWhiteSpace(Balltreffpunkt) ? false : Balltreffpunkt.ToLower() == "hinter";
        }

        public bool IsHalfDistance()
        {
            return string.IsNullOrWhiteSpace(Balltreffpunkt) ? false : Balltreffpunkt.ToLower() == "halbdistanz";
        }


        #region Filter Methods

        public bool HasHand(Stroke.Hand h)
        {
            switch (h)
            {
                case Stroke.Hand.Fore:
                    return Schlägerseite == "Vorhand";
                case Stroke.Hand.Back:
                    return Schlägerseite == "Rückhand";
                case Stroke.Hand.None:
                    return true;
                case Stroke.Hand.Both:
                    return true;
                default:
                    return false;
            }
        }

        public bool HasStepAround(Stroke.StepAround s)
        {
            switch (s)
            {
                case Stroke.StepAround.StepAround:
                    return Umlaufen;
                case Stroke.StepAround.Not:
                    return true;
                default:
                    return false;
            }
        }

        public bool HasWinner(Stroke.WinnerOrNetOut w)
        {
            switch (w)
            {
                case Stroke.WinnerOrNetOut.Winner:
                    return Verlauf == "Winner";
                case Stroke.WinnerOrNetOut.NetOut:
                    return Verlauf == "Netz" || Verlauf == "Aus";
                case Stroke.WinnerOrNetOut.None:
                    return true;
                case Stroke.WinnerOrNetOut.Both:
                    return true;
                default:
                    return false;
            }
        }

        public bool HasStrokeTec(IEnumerable<Stroke.Technique> tecs)
        {
            if (Schlagtechnik == null)
                return false;

            List<bool> ORresults = new List<bool>();
            foreach (var stroketec in tecs)
            {
                switch (stroketec)
                {
                    case Stroke.Technique.Push:
                        ORresults.Add(Schlagtechnik.Art == "Schupf");
                        break;
                    case Stroke.Technique.PushAggressive:
                        ORresults.Add(Schlagtechnik.Art == "Schupf" && Schlagtechnik.Option == "aggressiv");
                        break;
                    case Stroke.Technique.Flip:
                        ORresults.Add(Schlagtechnik.Art == "Flip");
                        break;
                    case Stroke.Technique.Banana:
                        ORresults.Add(Schlagtechnik.Art == "Flip" && Schlagtechnik.Option == "Banane");
                        break;
                    case Stroke.Technique.Topspin:
                        ORresults.Add(Schlagtechnik.Art == "Topspin");
                        break;
                    case Stroke.Technique.TopspinSpin:
                        ORresults.Add(Schlagtechnik.Art == "Topspin" && Schlagtechnik.Option == "Spin");
                        break;
                    case Stroke.Technique.TopspinTempo:
                        ORresults.Add(Schlagtechnik.Art == "Topspin" && Schlagtechnik.Option == "Tempo");
                        break;
                    case Stroke.Technique.Block:
                        ORresults.Add(Schlagtechnik.Art == "Block");
                        break;
                    case Stroke.Technique.BlockTempo:
                        ORresults.Add(Schlagtechnik.Art == "Block" && Schlagtechnik.Option == "Tempo");
                        break;
                    case Stroke.Technique.BlockChop:
                        ORresults.Add(Schlagtechnik.Art == "Block" && Schlagtechnik.Option == "Chop");
                        break;
                    case Stroke.Technique.Counter:
                        ORresults.Add(Schlagtechnik.Art == "Konter");
                        break;
                    case Stroke.Technique.Smash:
                        ORresults.Add(Schlagtechnik.Art == "Schuss");
                        break;
                    case Stroke.Technique.Lob:
                        ORresults.Add(Schlagtechnik.Art == "Ballonabwehr");
                        break;
                    case Stroke.Technique.Chop:
                        ORresults.Add(Schlagtechnik.Art == "Schnittabwehr");
                        break;
                    case Stroke.Technique.Special:
                        ORresults.Add(Schlagtechnik.Art == "Sonstige");
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasAggression(IEnumerable<Stroke.Aggression> aggressions)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var agg in aggressions)
            {
                switch (agg)
                {
                    case Stroke.Aggression.Aggressive:
                        ORresults.Add(Aggressivität == "aggressiv");
                        break;
                    case Stroke.Aggression.Passive:
                        ORresults.Add(Aggressivität == "passiv");
                        break;
                    case Stroke.Aggression.Control:
                        ORresults.Add(Aggressivität == "Kontrolle");
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasQuality(Stroke.Quality q)
        {
            switch (q)
            {
                case Stroke.Quality.Good:
                    return Qualität == "gut";
                case Stroke.Quality.Bad:
                    return Qualität == "schlecht";
                case Stroke.Quality.None:
                    return true;
                case Stroke.Quality.Both:
                    return Qualität == "gut" || Qualität == "schlecht";
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

        public bool HasSpins(IEnumerable<Stroke.Spin> spins)
        {
            if (Spin == null)
                return true;

            List<bool> ORresults = new List<bool>();
            foreach (var spin in spins)
            {
                switch (spin)
                {
                    case Stroke.Spin.Hidden:
                        ORresults.Add(Spin.ÜS == "" || Spin.SL == "" || Spin.SR == "" || Spin.US == "" || Spin.No == "");
                        break;
                    case Stroke.Spin.ÜS:
                        ORresults.Add(Spin.ÜS == "1" && Spin.SL == "0" && Spin.SR == "0");
                        break;
                    case Stroke.Spin.SR:
                        ORresults.Add(Spin.SR == "1" && Spin.ÜS == "0" && Spin.US == "0");
                        break;
                    case Stroke.Spin.No:
                        ORresults.Add(Spin.No == "1" && Spin.SL == "0" && Spin.SR == "0" && Spin.ÜS == "0" && Spin.US == "0");
                        break;
                    case Stroke.Spin.SL:
                        ORresults.Add(Spin.SL == "1" && Spin.ÜS == "0" && Spin.US == "0");
                        break;
                    case Stroke.Spin.US:
                        ORresults.Add(Spin.US == "1" && Spin.SL == "0" && Spin.SR == "0");
                        break;
                    case Stroke.Spin.USSL:
                        ORresults.Add(Spin.US == "1" && Spin.SL == "1");
                        break;
                    case Stroke.Spin.USSR:
                        ORresults.Add(Spin.US == "1" && Spin.SR == "1");
                        break;
                    case Stroke.Spin.ÜSSL:
                        ORresults.Add(Spin.ÜS == "1" && Spin.SL == "1");
                        break;
                    case Stroke.Spin.ÜSSR:
                        ORresults.Add(Spin.ÜS == "1" && Spin.SR == "1");
                        break;
                    default:
                        break;

                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasServices(IEnumerable<Stroke.Services> services)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var service in services)
            {
                switch (service)
                {
                    case Stroke.Services.Pendulum:
                        ORresults.Add(Aufschlagart == "Pendulum");
                        break;
                    case Stroke.Services.Reverse:
                        ORresults.Add(Aufschlagart == "Gegenläufer");
                        break;
                    case Stroke.Services.Tomahawk:
                        ORresults.Add(Aufschlagart == "Tomahawk");
                        break;
                    case Stroke.Services.Special:
                        ORresults.Add(Aufschlagart == "Spezial");
                        break;
                    default:
                        break;

                }

            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasServiceWinners(IEnumerable<Stroke.ServiceWinner> serviceWinner)
        {
            List<bool> ORresults = new List<bool>();
            foreach (var service in serviceWinner)
            {
                switch (service)
                {
                    case Stroke.ServiceWinner.All:
                        ORresults.Add((Aufschlagart == "Pendulum" || Aufschlagart == "Gegenläufer" || Aufschlagart == "Tomahawk" || Aufschlagart == "Spezial"));
                        break;
                    case Stroke.ServiceWinner.Short:
                        ORresults.Add((Aufschlagart == "Pendulum" || Aufschlagart == "Gegenläufer" || Aufschlagart == "Tomahawk" || Aufschlagart == "Spezial") && this.IsShort());
                        break;
                    case Stroke.ServiceWinner.Long:
                        ORresults.Add((Aufschlagart == "Pendulum" || Aufschlagart == "Gegenläufer" || Aufschlagart == "Tomahawk" || Aufschlagart == "Spezial") && this.IsLong());
                        break;
                    default:
                        break;

                }

            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        public bool HasSpecials(Stroke.Specials specials)
        {        
            switch (specials)
            {
                case Stroke.Specials.EdgeTable:
                    return Besonderes == "Tischkante";
                case Stroke.Specials.EdgeRacket:
                    return Besonderes == "Schlägerkante";
                case Stroke.Specials.EdgeNet:
                    return Besonderes == "Netzkante";
                case Stroke.Specials.None:
                    return true;
                case Stroke.Specials.Both:
                    return Besonderes == "Tischkante" || Besonderes == "Netzkante";
                default:
                    return false;
            }
        }

        public bool HasServerPosition(IEnumerable<Positions.Server> server)
        {
            if (Platzierung == null)
                return true;

            List<bool> ORresults = new List<bool>();
            double X;
            double Seite = Platzierung.WY == double.NaN ? 999 : Platzierung.WY;
            if (Seite >= 137)
                X = 152.5 - (Spielerposition == double.NaN ? 999 : Spielerposition);
            else
                X = Spielerposition == double.NaN ? 999 : Spielerposition;

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
            if (Platzierung == null)
                return true;

            double aufschlagPosition;
            double seite = this.Platzierung.WY == double.NaN ? 999 : Convert.ToDouble(this.Platzierung.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (this.Spielerposition == double.NaN ? 999 : Convert.ToDouble(this.Spielerposition));
            }
            else
            {
                aufschlagPosition = this.Spielerposition == double.NaN ? 999 : Convert.ToDouble(this.Spielerposition);
            }

            return (0 <= aufschlagPosition && aufschlagPosition < 50.5);
        }

        public bool IsMiddleServicePosition()
        {
            if (Platzierung == null)
                return true;

            double aufschlagPosition;
            double seite = this.Platzierung.WY == double.NaN ? 999 : Convert.ToDouble(this.Platzierung.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (this.Spielerposition == double.NaN ? 999 : Convert.ToDouble(this.Spielerposition));
            }
            else
            {
                aufschlagPosition = this.Spielerposition == double.NaN ? 999 : Convert.ToDouble(this.Spielerposition);
            }

            return (50.5 <= aufschlagPosition && aufschlagPosition <= 102);
        }

        public bool IsRightServicePosition()
        {
            if (Platzierung == null)
                return true;

            double aufschlagPosition;
            double seite = this.Platzierung.WY == double.NaN ? 999 : Convert.ToDouble(this.Platzierung.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (this.Spielerposition == double.NaN ? 999 : Convert.ToDouble(this.Spielerposition));
            }
            else
            {
                aufschlagPosition = this.Spielerposition == double.NaN ? 999 : Convert.ToDouble(this.Spielerposition);
            }

            return (102 < aufschlagPosition && aufschlagPosition <= 152.5);
        }

        #endregion

        #region Statistic Methods

        

        #endregion

    }



    public class Schlagtechnik : PropertyChangedBase
    {
        private string art;

        private string option;

        /// <remarks/>
        [XmlAttribute]
        public string Art
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

    public class Spin :PropertyChangedBase
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
        public string ÜS
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

    public class Platzierung : PropertyChangedBase
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
