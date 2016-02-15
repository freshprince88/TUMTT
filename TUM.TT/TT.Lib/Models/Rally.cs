using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Models
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Rally
    {

        private Score currentRallyScoreField;

        private Score currentSetScoreField;

        private List<Schlag> schlagField;

        private int nummerField;

        private string winnerField;

        private int lengthField;

        private string serverField;

        private double anfangField;

        private double endeField;

        private string kommentarField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Score CurrentRallyScore
        {
            get
            {
                return this.currentRallyScoreField;
            }
            set
            {
                this.currentRallyScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Score CurrentSetScore
        {
            get
            {
                return this.currentSetScoreField;
            }
            set
            {
                this.currentSetScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Schlag", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<Schlag> Schlag
        {
            get
            {
                return this.schlagField;
            }
            set
            {
                this.schlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Nummer
        {
            get
            {
                return this.nummerField;
            }
            set
            {
                this.nummerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Winner
        {
            get
            {
                return this.winnerField;
            }
            set
            {
                this.winnerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Length
        {
            get
            {
                return this.lengthField;
            }
            set
            {
                this.lengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Server
        {
            get
            {
                return this.serverField;
            }
            set
            {
                this.serverField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double Anfang
        {
            get
            {
                return this.anfangField;
            }
            set
            {
                this.anfangField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double Ende
        {
            get
            {
                return this.endeField;
            }
            set
            {
                this.endeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Kommentar
        {
            get
            {
                return this.kommentarField;
            }
            set
            {
                this.kommentarField = value;
            }
        }

        #region Helper Methods
        public Boolean IsDiagonal(int i)
        {
            if (i <= 0)
            {
                return false;
            }
            else
            {
                int now = i;
                int prev = i - 1;

                return Math.Abs(Convert.ToInt32(this.Schlag[prev].Platzierung.WX) - Convert.ToInt32(this.Schlag[now].Platzierung.WX)) > 100;
            }
            
        }

        public Boolean IsMiddle(int i)
        {
            if (i <= 0)
            {
                return false;
            }
            else
            {
                int now = i;
                int prev = i - 1;

                return this.Schlag[now].IsBotMid() || this.Schlag[now].IsMidMid() || this.Schlag[now].IsTopMid();
            }

        }

        public Boolean IsParallel(int i)
        {
            if (i <= 0)
            {
                return false;
            }
            else
            {
                int now = i;
                int prev = i - 1;

                return Math.Abs(Convert.ToInt32(this.Schlag[prev].Platzierung.WX) - Convert.ToInt32(this.Schlag[now].Platzierung.WX)) <= 100;
            }

        }
        #region Helper Methods Statistics

        public Boolean HasBasisInformationStatistics(int minlegth, string name)
        {
            switch (name)
            {
                case "":
                    return true;
                case "TotalReceivesCount":
                    return Convert.ToInt32(this.Length) >= minlegth;
                case "TotalReceivesCountPointPlayer1":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == "First";
                case "TotalReceivesCountPointPlayer2":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == "Second";
                default:
                    return true;

            }
        }

        public Boolean HasTechniqueStatistics(int stroke, string name)
        {
            switch (name)
            {
                case "":
                    return true;

                #region Flip 
                case "ForehandFlipTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip";
                case "ForehandFlipPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler == this.Winner;
                case "ForehandFlipDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandFlipPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler != this.Winner;

                case "BackhandFlipTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip";
                case "BackhandFlipPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler == this.Winner;
                case "BackhandFlipDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandFlipPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler != this.Winner;

                case "AllFlipTotalButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].Schlagtechnik.Art == "Flip";
                case "AllFlipPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler == this.Winner;
                case "AllFlipDirectPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllFlipPointsLostButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].Schlagtechnik.Art == "Flip" && this.Schlag[stroke].Spieler != this.Winner;

                #endregion

                #region Push short

                case "ForehandPushShortTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort();
                case "ForehandPushShortPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Spieler == this.Winner;
                case "ForehandPushShortDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandPushShortPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Spieler != this.Winner;

                case "BackhandPushShortTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort();
                case "BackhandPushShortPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Spieler == this.Winner;
                case "BackhandPushShortDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandPushShortPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Spieler != this.Winner;

                case "AllPushShortTotalButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushShortPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler == this.Winner;
                case "AllPushShortDirectPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllPushShortPointsLostButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsShort() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler != this.Winner;

                #endregion

                #region Push halflong

                case "ForehandPushHalfLongTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong();
                case "ForehandPushHalfLongPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Spieler == this.Winner;
                case "ForehandPushHalfLongDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandPushHalfLongPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Spieler != this.Winner;

                case "BackhandPushHalfLongTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong();
                case "BackhandPushHalfLongPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Spieler == this.Winner;
                case "BackhandPushHalfLongDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandPushHalfLongPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Spieler != this.Winner;

                case "AllPushHalfLongTotalButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushHalfLongPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler == this.Winner;
                case "AllPushHalfLongDirectPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllPushHalfLongPointsLostButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsHalfLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler != this.Winner;


                #endregion

                #region Push long

                case "ForehandPushLongTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong();
                case "ForehandPushLongPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Spieler == this.Winner;
                case "ForehandPushLongDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandPushLongPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Spieler != this.Winner;

                case "BackhandPushLongTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong();
                case "BackhandPushLongPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Spieler == this.Winner;
                case "BackhandPushLongDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandPushLongPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Spieler != this.Winner;

                case "AllPushLongTotalButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushLongPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler == this.Winner;
                case "AllPushLongDirectPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllPushLongPointsLostButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.Schlag[stroke].IsLong() && this.Schlag[stroke].Schlagtechnik.Art == "Schupf" && this.Schlag[stroke].Spieler != this.Winner;


                #endregion

                #region Topspin diagonal

                case "ForehandTopspinDiagonalTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1);
                case "ForehandTopspinDiagonalPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schlag[stroke].Spieler == this.Winner;
                case "ForehandTopspinDiagonalDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandTopspinDiagonalPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schlag[stroke].Spieler != this.Winner;

                case "BackhandTopspinDiagonalTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1);
                case "BackhandTopspinDiagonalPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schlag[stroke].Spieler == this.Winner;
                case "BackhandTopspinDiagonalDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandTopspinDiagonalPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schlag[stroke].Spieler != this.Winner;

                case "AllTopspinDiagonalTotalButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinDiagonalPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler == this.Winner;
                case "AllTopspinDiagonalDirectPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllTopspinDiagonalPointsLostButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler != this.Winner;

                #endregion

                #region Topspin Middle

                case "ForehandTopspinMiddleTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1);
                case "ForehandTopspinMiddlePointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schlag[stroke].Spieler == this.Winner;
                case "ForehandTopspinMiddleDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandTopspinMiddlePointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schlag[stroke].Spieler != this.Winner;

                case "BackhandTopspinMiddleTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1);
                case "BackhandTopspinMiddlePointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schlag[stroke].Spieler == this.Winner;
                case "BackhandTopspinMiddleDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandTopspinMiddlePointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schlag[stroke].Spieler != this.Winner;

                case "AllTopspinMiddleTotalButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinMiddlePointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler == this.Winner;
                case "AllTopspinMiddleDirectPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllTopspinMiddlePointsLostButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler != this.Winner;

                #endregion

                #region Topspin parallel

                case "ForehandTopspinParallelTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1);
                case "ForehandTopspinParallelPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schlag[stroke].Spieler == this.Winner;
                case "ForehandTopspinParallelDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandTopspinParallelPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Vorhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schlag[stroke].Spieler != this.Winner;

                case "BackhandTopspinParallelTotalButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1);
                case "BackhandTopspinParallelPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schlag[stroke].Spieler == this.Winner;
                case "BackhandTopspinParallelDirectPointsWonButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandTopspinParallelPointsLostButton":
                    return this.Schlag[stroke].Schlägerseite == "Rückhand" && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schlag[stroke].Spieler != this.Winner;

                case "AllTopspinParallelTotalButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinParallelPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler == this.Winner;
                case "AllTopspinParallelDirectPointsWonButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllTopspinParallelPointsLostButton":
                    return (this.Schlag[stroke].Schlägerseite == "Vorhand" || this.Schlag[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schlag[stroke].Schlagtechnik.Art == "Topspin" && this.Schlag[stroke].Spieler != this.Winner;

                #endregion





                default:
                    return true;
            }

        }



        #endregion
        #endregion
    }
}
