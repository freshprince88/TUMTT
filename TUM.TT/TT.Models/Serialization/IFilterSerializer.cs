using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TT.Models.Serialization
{
    using System.IO;

    /// <summary>
    /// Specifies serialization for Filters.
    /// </summary>
    public interface IFilterSerializer
    {
        /// <summary>
        /// Serializes a Filter to a stream.
        /// </summary>
        /// <param name="stream">The stream to serialize to.</param>
        /// <param name="filter">The filter to serialize.</param>
        void Serialize(Stream stream, Filter filter);

        /// <summary>
        /// Deserializes a Filter from a stream.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <returns>The match.</returns>
        Filter Deserialize(Stream stream);
    }
}