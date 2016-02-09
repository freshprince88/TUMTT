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
    public partial class Player
    {

        private Rank rankField;

        private string nameField;

        private string nationalityField;

        private string spielsystemField;

        private string händigkeitField;

        private string griffhaltungField;

        private string materialField;

        /// <remarks/>
        public Rank Rank
        {
            get
            {
                return this.rankField;
            }
            set
            {
                this.rankField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Nationality
        {
            get
            {
                return this.nationalityField;
            }
            set
            {
                this.nationalityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Spielsystem
        {
            get
            {
                return this.spielsystemField;
            }
            set
            {
                this.spielsystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Händigkeit
        {
            get
            {
                return this.händigkeitField;
            }
            set
            {
                this.händigkeitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Griffhaltung
        {
            get
            {
                return this.griffhaltungField;
            }
            set
            {
                this.griffhaltungField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Material
        {
            get
            {
                return this.materialField;
            }
            set
            {
                this.materialField = value;
            }
        }
    }
}
