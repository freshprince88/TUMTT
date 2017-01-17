//-----------------------------------------------------------------------
// <copyright file="IReportRenderer.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

using TT.Models.Util;

namespace TT.Report.Renderers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// A report renderer.
    /// </summary>
    public interface IReportRenderer : IReportVisitor
    {
        /// <summary>
        /// Saves the rendered report to the given stream.
        /// </summary>
        /// <param name="sink">The stream to write to.</param>
        void Save(Stream sink);

        /// <summary>
        /// Initializes the renderer.
        /// </summary>
        /// <remarks>
        /// Called before rendering the first section.
        /// </remarks>
        void BeforeRendering();

        /// <summary>
        /// Finalizes the renderer.
        /// </summary>
        /// <remarks>
        /// Called after rendering the last section.
        /// </remarks>
        void AfterRendering();

        /// <summary>
        /// A dictionary of temp file schemes created during report rendering.
        /// </summary>
        IDictionary<TempFileType, TempFileScheme> TempFileSchemes { get; }
    }
}
