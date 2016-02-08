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
    public partial class Score
    {

        private string firstField;

        private string secondField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string First
        {
            get
            {
                return this.firstField;
            }
            set
            {
                this.firstField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Second
        {
            get
            {
                return this.secondField;
            }
            set
            {
                this.secondField = value;
            }
        }
    }
}
