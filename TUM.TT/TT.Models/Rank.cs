//-----------------------------------------------------------------------
// <copyright file="Rank.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// The rank of a <see cref="Player"/>.
    /// </summary>
    public class Rank : IXmlSerializable, IComparable<Rank>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rank"/> class.
        /// </summary>
        /// <param name="position">The position in the ranking.</param>
        /// <param name="date">The date of ranking.</param>
        public Rank(int position, DateTime date)
        {
            this.Position = position;
            this.Date = date;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Rank"/> class from being created.
        /// </summary>
        private Rank()
        {
            // Support XML deserialization
        }

        /// <summary>
        /// Gets the position in the ranking.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets the date of the ranking.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Determines whether this rank is equal to an object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Whether this rank is equal to the object or not.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Rank;
            return other != null
                && this.Position == other.Position
                && this.Date == other.Date;
        }

        /// <summary>
        /// Gets the hash code of this rank.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return this.Position.GetHashCode() ^ this.Date.GetHashCode();
        }

        /// <summary>
        /// Gets a string representation of this rank.
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return string.Format("#{0} in {1:MM-yyyy}", this.Position, this.Date);
        }

        /// <summary>
        /// Gets the schema of this object.
        /// </summary>
        /// <returns><c>null</c></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Fills this object from XML.
        /// </summary>
        /// <param name="reader">The reader to read from/</param>
        public void ReadXml(XmlReader reader)
        {
            this.Position = int.Parse(reader["Position"], CultureInfo.InvariantCulture);
            this.Date = DateTime.ParseExact(reader["Date"], "o", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Writes this object to XML.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Position", this.Position.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Date", this.Date.ToString("o", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Compares this object to another.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>Whether this object is smaller than, equal to or greater than <paramref name="other"/>.</returns>
        public int CompareTo(Rank other)
        {
            if (other == null)
            {
                return 1;
            }
            else if (this.Date != other.Date)
            {
                throw new ArgumentException("Rankings are of different date");
            }
            else
            {
                return this.Position.CompareTo(other.Position);
            }
        }
    }
}
