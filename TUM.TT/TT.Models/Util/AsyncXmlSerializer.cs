//-----------------------------------------------------------------------
// <copyright file="AsyncXmlSerializer.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Util
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// Asynchronously serialize XML data.
    /// </summary>
    /// <typeparam name="T">The type to serialize</typeparam>
    public class AsyncXmlSerializer<T> : XmlSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncXmlSerializer{T}"/> class.
        /// </summary>
        public AsyncXmlSerializer()
            : base(typeof(T))
        {
        }

        /// <summary>
        /// Asynchronously serializes an object.
        /// </summary>
        /// <param name="stream">The stream to serialize to.</param>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The task to serialize the object.</returns>
        public async Task SerializeAsync(Stream stream, T obj)
        {
            using (var sink = new MemoryStream())
            {
                this.Serialize(sink, obj);
                sink.Seek(0, SeekOrigin.Begin);
                await sink.CopyToAsync(stream);
            }
        }

        /// <summary>
        /// Asynchronously deserializes an object from a stream.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <returns>A task to deserialize the object.</returns>
        public async Task<T> DeserializeAsync(Stream stream)
        {
            using (var sink = new MemoryStream())
            {
                await stream.CopyToAsync(sink);
                sink.Seek(0, SeekOrigin.Begin);
                return (T)this.Deserialize(sink);
            }
        }
    }
}
