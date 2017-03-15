namespace TT.Models.Serialization
{
    using System.IO;
    using System.Xml.Serialization;
    using System.Linq;

    /// <summary>
    /// Serializes Filters to XML>
    /// </summary>
    public class XmlFilterSerializer : IFilterSerializer
    {
        /// <summary>
        /// The serializer to use.
        /// </summary>
        private XmlSerializer serializer = new XmlSerializer(typeof(Filter));

        /// <summary>
        /// Serializes a Filter.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="filter">The Filter to serialize</param>
        public void Serialize(Stream stream, Filter filter)
        {
            this.serializer.Serialize(stream, filter);
        }

        /// <summary>
        /// Deserializes a Filter.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The deserialized match.</returns>
        public Filter Deserialize(Stream stream)
        {
            return (Filter)this.serializer.Deserialize(stream);
        }
    }
}
