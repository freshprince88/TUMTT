//-----------------------------------------------------------------------
// <copyright file="Report.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013  Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TTA.Models.Report
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// A report of a match.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        public Report()
        {
        }

        /// <summary>
        /// Gets or sets the list of <see cref="BasicInformations"/>.
        /// </summary>
        [XmlElement]
        public List<BasicInformation> BasicInformations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rally length statistics
        /// </summary>
        [XmlElement]
        public RallyLengthStatistics RallyLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the transition statistics for each player;
        /// </summary>
        [XmlElement]
        public List<AbsoluteNumberOfTransition> Transitions
        {
            get;
            set;
        }
    }
}
