//-----------------------------------------------------------------------
// <copyright file="IMatchSerializer.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Serialization
{
    using System.IO;

    /// <summary>
    /// Specifies serialization for matches.
    /// </summary>
    public interface IMatchSerializer
    {
        /// <summary>
        /// Serializes a match to a stream.
        /// </summary>
        /// <param name="stream">The stream to serialize to.</param>
        /// <param name="match">The match to serialize.</param>
        void Serialize(Stream stream, Match match);

        /// <summary>
        /// Deserializes a match from a stream.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <returns>The match.</returns>
        Match Deserialize(Stream stream);
    }
}
