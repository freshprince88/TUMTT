//-----------------------------------------------------------------------
// <copyright file="XmlMatchSerializer.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Serialization
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Serializes matches to XML>
    /// </summary>
    public class XmlMatchSerializer : IMatchSerializer
    {
        /// <summary>
        /// The serializer to use.
        /// </summary>
        private XmlSerializer serializer = new XmlSerializer(typeof(Match));

        /// <summary>
        /// Serializes a match.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="match">The match to serialize</param>
        public void Serialize(Stream stream, Match match)
        {
            this.serializer.Serialize(stream, match);
        }

        /// <summary>
        /// Deserializes a match.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The deserialized match.</returns>
        public Match Deserialize(Stream stream)
        {
            return (Match)this.serializer.Deserialize(stream);
        }
    }
}
