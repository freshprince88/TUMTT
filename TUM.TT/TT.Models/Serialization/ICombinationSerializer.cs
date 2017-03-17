using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models.Serialization
{
    using System.IO;

    /// <summary>
    /// Specifies serialization for Combinations.
    /// </summary>
    interface ICombinationSerializer
    {
        /// <summary>
        /// Serializes a Filter to a stream.
        /// </summary>
        /// <param name="stream">The stream to serialize to.</param>
        /// <param name="filter">The combination to serialize.</param>
        void Serialize(Stream stream, Combination filter);

        /// <summary>
        /// Deserializes a Combination from a stream.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <returns>The combination.</returns>
        Combination Deserialize(Stream stream);
    }
}
