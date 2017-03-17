using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TT.Models.Serialization
{
    public class XmlCombinationSerializer : ICombinationSerializer
    {
        /// <summary>
        /// The serializer to use.
        /// </summary>
        private XmlSerializer serializer = new XmlSerializer(typeof(Combination));

        /// <summary>
        /// Serializes a Combination.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="combination">The Combination to serialize</param>
        public void Serialize(Stream stream, Combination combination)
        {
            this.serializer.Serialize(stream, combination);
        }

        /// <summary>
        /// Deserializes a Combination.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The deserialized Combination.</returns>
        public Combination Deserialize(Stream stream)
        {
            return (Combination)this.serializer.Deserialize(stream);
        }
    }
}
