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

        public string RallyNumLong
        {
            get
            {
                return string.Format("Rally #{0}", Nummer);
            }
        }

        public string RallyNumShort
        {
            get
            {
                return string.Format("#{0}", Nummer);
            }
        }

        public Rally NextRally(int winner)
        {
            //TODO: SetScore und RallyScore verknüpfen, damit Increment Methoden auch Setscore updaten und umgekehrt
            switch (winner)
            {
                case 1:
                    break;
                case 2:
                    break;
                default:
                    return null;
            }

            return null;
        }
    }
}
