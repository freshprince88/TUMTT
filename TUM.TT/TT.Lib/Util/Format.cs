//-----------------------------------------------------------------------
// <copyright file="Format.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Util
{
    using TT.Models.Serialization;

    /// <summary>
    /// A format to read or write matches.
    /// </summary>
    public class Format
    {
        /// <summary>
        /// Initializes static members of the <see cref="Format"/> class.
        /// </summary>
        static Format()
        {
            XML = new Format(new XmlMatchSerializer(), "Table Tennis Observation", ".tto");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        /// <param name="serializer">The serializer for this format.</param>
        /// <param name="description">A description of the format.</param>
        /// <param name="extension">The file extension of the format.</param>
        public Format(IMatchSerializer serializer, string description, string extension)
        {
            this.Serializer = serializer;
            this.Description = description;
            this.Extension = extension;
        }

        /// <summary>
        /// Gets the standard XML serialization format.
        /// </summary>
        public static Format XML
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the serializer for the match.
        /// </summary>
        public IMatchSerializer Serializer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a description of this format.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the file extension of this format
        /// </summary>
        public string Extension
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a string suitable as dialog filter for this format.
        /// </summary>
        public string DialogFilter
        {
            get
            {
                return string.Format("{0}|*{1}", this.Description, this.Extension);
            }
        }
    }
}
