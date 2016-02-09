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
    public partial class Schlag
    {

        private Schlagtechnik schlagtechnikField;

        private Spin spinField;

        private Platzierung platzierungField;

        private string nummerField;

        private string spielerField;

        private string schlägerseiteField;

        private string aufschlagartField;

        private string balltreffpunktField;

        private string qualitätField;

        private string spielerpositionField;

        private string besonderesField;

        private string verlaufField;

        private string umlaufenField;

        private string aggressivitätField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Schlagtechnik Schlagtechnik
        {
            get
            {
                return this.schlagtechnikField;
            }
            set
            {
                this.schlagtechnikField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Spin Spin
        {
            get
            {
                return this.spinField;
            }
            set
            {
                this.spinField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Platzierung Platzierung
        {
            get
            {
                return this.platzierungField;
            }
            set
            {
                this.platzierungField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Nummer
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
        public string Spieler
        {
            get
            {
                return this.spielerField;
            }
            set
            {
                this.spielerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Schlägerseite
        {
            get
            {
                return this.schlägerseiteField;
            }
            set
            {
                this.schlägerseiteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Aufschlagart
        {
            get
            {
                return this.aufschlagartField;
            }
            set
            {
                this.aufschlagartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Balltreffpunkt
        {
            get
            {
                return this.balltreffpunktField;
            }
            set
            {
                this.balltreffpunktField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Qualität
        {
            get
            {
                return this.qualitätField;
            }
            set
            {
                this.qualitätField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Spielerposition
        {
            get
            {
                return this.spielerpositionField;
            }
            set
            {
                this.spielerpositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Besonderes
        {
            get
            {
                return this.besonderesField;
            }
            set
            {
                this.besonderesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Verlauf
        {
            get
            {
                return this.verlaufField;
            }
            set
            {
                this.verlaufField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Umlaufen
        {
            get
            {
                return this.umlaufenField;
            }
            set
            {
                this.umlaufenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Aggressivität
        {
            get
            {
                return this.aggressivitätField;
            }
            set
            {
                this.aggressivitätField = value;
            }
        }

        public Boolean IsTopLeft()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X >= 0 && X < 50.5 && Y >= 0 && Y <= 46) || (X <= 152.5 && X > 102 && Y <= 274 && Y >= 228);
        }

        public Boolean IsTopMid()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X >= 50.5 && X <= 102 && Y >= 0 && Y <= 46) || (X >= 50.5 && X <= 102 && Y >= 228 && Y <= 274);
        }

        public Boolean IsTopRight()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X <= 152.5 && X > 102 && Y >= 0 && Y <= 46) || (X >= 0 && X < 50.5 && Y >= 228 && Y <= 274);
        }

        public Boolean IsMidLeft()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X >= 0 && X < 50.5 && Y <= 92 && Y > 46) || (X <= 152.5 && X > 102 && Y < 228 && Y >= 182);
        }
        public Boolean IsMidMid()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X >= 50.5 && X <= 102 && Y <= 92 && Y > 46) || (X >= 50.5 && X <= 102 && Y < 228 && Y >= 182);
        }
        public Boolean IsMidRight()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X <= 152.5 && X > 102 && Y <= 92 && Y > 46) || (X >= 0 && X < 50.5 && Y < 228 && Y >= 182);
        }

        public Boolean IsBotLeft()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X >= 0 && X < 50.5 && Y < 137 && Y > 92) || (X <= 152.5 && X > 102 && Y >= 137 && Y < 182);
        }
        public Boolean IsBotMid()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X >= 50.5 && X <= 102 && Y < 137 && Y > 92) || (X >= 50.5 && X <= 102 && Y >= 137 && Y < 182);
        }
        public Boolean IsBotRight()
        {
            double X = this.Platzierung.WX == "" ? -1 : Convert.ToDouble(this.Platzierung.WX);
            double Y = this.Platzierung.WY == "" ? -1 : Convert.ToDouble(this.Platzierung.WY);

            return (X <= 152.5 && X > 102 && Y < 137 && Y > 92) || (X >= 0 && X < 50.5 && Y >= 137 && Y < 182);
        }

        public Boolean IsShort()
        {
            return this.Balltreffpunkt == "" ? false : this.Balltreffpunkt.ToLower() == "über";
        }

        public Boolean IsHalf()
        {
            return this.Balltreffpunkt == "" ? false : this.Balltreffpunkt.ToLower() == "hinter";
        }

        public Boolean IsLong()
        {
            return this.Balltreffpunkt == "" ? false : this.Balltreffpunkt.ToLower() == "halbdistanz";
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Schlagtechnik
    {

        private string artField;

        private string optionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Art
        {
            get
            {
                return this.artField;
            }
            set
            {
                this.artField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Option
        {
            get
            {
                return this.optionField;
            }
            set
            {
                this.optionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Spin
    {

        private string usField;

        private string üsField;

        private string slField;

        private string srField;

        private string noField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string US
        {
            get
            {
                return this.usField;
            }
            set
            {
                this.usField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ÜS
        {
            get
            {
                return this.üsField;
            }
            set
            {
                this.üsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SL
        {
            get
            {
                return this.slField;
            }
            set
            {
                this.slField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SR
        {
            get
            {
                return this.srField;
            }
            set
            {
                this.srField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string No
        {
            get
            {
                return this.noField;
            }
            set
            {
                this.noField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Platzierung
    {

        private string wxField;

        private string wyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string WX
        {
            get
            {
                return this.wxField;
            }
            set
            {
                this.wxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string WY
        {
            get
            {
                return this.wyField;
            }
            set
            {
                this.wyField = value;
            }
        }
    }
}
