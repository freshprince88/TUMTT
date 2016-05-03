//-----------------------------------------------------------------------
// <copyright file="MetadataSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Sections
{
    /// <summary>
    /// Report metadata.
    /// </summary>
    public class MetadataSection : IReportSection
    {
        /// <summary>
        /// Gets or sets the document title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the document subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the document author.
        /// </summary>
        public string Author { get; set; }
    }
}
