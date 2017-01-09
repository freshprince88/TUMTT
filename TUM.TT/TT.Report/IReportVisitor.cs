//-----------------------------------------------------------------------
// <copyright file="IReportVisitor.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report
{
    using TT.Report.Sections;

    /// <summary>
    /// A report visitor.
    /// </summary>
    public interface IReportVisitor
    {
        /// <summary>
        /// Visits a metadata section.
        /// </summary>
        /// <param name="section">The section.</param>
        void Visit(MetadataSection section);

        /// <summary>
        /// Visits the header section.
        /// </summary>
        /// <param name="section">The section.</param>
        void Visit(HeaderSection section);

        /// <summary>
        /// Visits the basic info section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(BasicInformationSection section);

        /// <summary>
        /// Visits the rally length section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(RallyLengthSection section);

        /// <summary>
        /// Visits the scoring process section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(ScoringProcessSection section);

        /// <summary>
        /// Visits the match dynamics section.
        /// </summary>
        /// <param name="section">The section.</param>
        void Visit(MatchDynamicsSection section);

        /// <summary>
        /// Visits the transition sections.
        /// </summary>
        /// <param name="section">The section.</param>
        void Visit(TransitionsSection section);

        /// <summary>
        /// Visits the technical efficiency section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(TechnicalEfficiencySection section);        

        /// <summary>
        /// Visits the part section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(PartSection section);

        /// <summary>
        /// Visits the stroke stats heading section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(StrokeStatsHeadingSection section);

        /// <summary>
        /// Visits the side section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(SideSection section);

        /// <summary>
        /// Visits the spin section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(SpinSection section);

        /// <summary>
        /// Visits the technique section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(TechniqueSection section);

        /// <summary>
        /// Visits the placement section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(PlacementSection section);

        /// <summary>
        /// Visits the large table section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(LargeTableSection section);

        /// <summary>
        /// Visits the last stroke number section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(StrokeNumberSection section);

        /// <summary>
        /// Visits the last stroke service section.
        /// </summary>
        /// <param name="section">The section</param>
        void Visit(LastStrokeServiceSection section);
        
    }
}
