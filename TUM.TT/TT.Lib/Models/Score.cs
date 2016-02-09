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
        public Score()
        {
            First = 0;
            Second = 0;
        }

        private int firstField;

        private int secondField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int First
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
        public int Second
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

        public override string ToString()
        {
            return string.Format("{0} : {1}", First, Second);
        }

        public Score AddFirst(int add = 1)
        {
            return new Score()
            {
                First = this.First + add,
                Second = this.Second
            };
        }

        public Score AddSecond(int add = 1)
        {
            return new Score()
            {
                First = this.First,
                Second = this.Second + add
            };
        }
    }
}
