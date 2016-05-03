//-----------------------------------------------------------------------
// <copyright file="Score.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
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
    /// A match score.
    /// </summary>
    public class Score : IEquatable<Score>, IXmlSerializable
    {
        /// <summary>
        /// The attribute name to serialize <see cref="First"/>.
        /// </summary>
        private const string FirstAttr = "First";

        /// <summary>
        /// The attribute name to serialize <see cref="Second"/>.
        /// </summary>
        private const string SecondAttr = "Second";

        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class.
        /// </summary>
        /// <param name="first">The score of the first player.</param>
        /// <param name="second">The score of the second player.</param>
        public Score(int first, int second)
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Score" /> class from being created.
        /// </summary>
        private Score()
        {
            // To support XML serialization
        }

        /// <summary>
        /// Gets the score of the first player.
        /// </summary>
        public int First { get; private set; }

        /// <summary>
        /// Gets the score of the second player.
        /// </summary>
        public int Second { get; private set; }

        /// <summary>
        /// Gets the absolute difference in this score.
        /// </summary>
        public int Difference
        {
            get { return Math.Abs(this.First - this.Second); }
        }

        /// <summary>
        /// Gets the highest value in this score.
        /// </summary>
        public int Highest
        {
            get { return Math.Max(this.First, this.Second); }
        }

        /// <summary>
        /// Gets the lowest value in this score.
        /// </summary>
        public int Lowest
        {
            get { return Math.Min(this.First, this.Second); }
        }

        /// <summary>
        /// Gets the total points in this score.
        /// </summary>
        public int Total
        {
            get { return this.First + this.Second; }
        }

        /// <summary>
        /// Gets a value indicating whether this score would win the rally.
        /// </summary>
        public bool WinsRally
        {
            get { return this.Difference >= 2 && this.Highest >= 11; }
        }

        /// <summary>
        /// Gets the score of the given player.
        /// </summary>
        /// <param name="player">The player whose score to get.</param>
        /// <returns>The score of the given player.</returns>
        public int Of(MatchPlayer player)
        {
            switch (player)
            {
                case MatchPlayer.First:
                    return this.First;
                case MatchPlayer.Second:
                    return this.Second;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Calculates an updated score
        /// </summary>
        /// <param name="winner">The winning player.</param>
        /// <returns>The score with the victory counted.</returns>
        public Score WinningScore(MatchPlayer winner)
        {
            return new Score(
                this.First + (winner == MatchPlayer.First ? 1 : 0),
                this.Second + (winner == MatchPlayer.Second ? 1 : 0));
        }

        /// <summary>
        /// Gets the XML schema associated with this object.
        /// </summary>
        /// <returns><c>null</c>.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads this object from XML.
        /// </summary>
        /// <param name="reader">The XML reader.</param>
        public void ReadXml(XmlReader reader)
        {
            this.First = int.Parse(reader[FirstAttr], CultureInfo.InvariantCulture);
            this.Second = int.Parse(reader[SecondAttr], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Writes this object to XML.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(FirstAttr, this.First.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(SecondAttr, this.Second.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Determines whether this object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Whether the objects are equal or not.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Score);
        }

        /// <summary>
        /// Determines whether this object is equal to another object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>Whether the objects are equal or not.</returns>
        public bool Equals(Score other)
        {
            return this.First == other.First && this.Second == other.Second;
        }

        /// <summary>
        /// Gets the hash code of this object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.First.GetHashCode() ^ this.Second.GetHashCode();
        }

        /// <summary>
        /// Converts this object to a string.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", this.First, this.Second);
        }
    }
}
