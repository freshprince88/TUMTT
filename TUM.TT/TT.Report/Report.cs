//-----------------------------------------------------------------------
// <copyright file="Report.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report
{
    using System.Collections.Generic;
    using System.IO;
    using TT.Report.Renderers;
    using TT.Report.Sections;

    /// <summary>
    /// A report for a match.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        public Report()
        {
            this.Sections = new List<IReportSection>();
        }

        /// <summary>
        /// Gets the sections of the report.
        /// </summary>
        public IList<IReportSection> Sections { get; private set; }

        /// <summary>
        /// Visits all sections in this report.
        /// </summary>
        /// <param name="visitor">The report visitor.</param>
        public void Visit(IReportVisitor visitor)
        {
            foreach (var section in this.Sections)
            {
                // Cast to dynamic to force runtime overload selection
                visitor.Visit((dynamic)section);
            }
        }

        /// <summary>
        /// Renders a report to a stream.
        /// </summary>
        /// <param name="renderer">The renderer for the report</param>
        /// <param name="stream">The stream to render to.</param>
        public void RenderToStream(IReportRenderer renderer, Stream stream)
        {
            renderer.BeforeRendering();
            this.Visit(renderer);
            renderer.AfterRendering();
            renderer.Save(stream);
        }
    }
}
