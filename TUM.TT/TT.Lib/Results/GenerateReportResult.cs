//-----------------------------------------------------------------------
// <copyright file="GenerateReportResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Results
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TT.Models;
    using TT.Report.Generators;
    using TT.Report.Renderers;

    /// <summary>
    /// Generates a PDF report.
    /// </summary>
    public class GenerateReportResult : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateReportResult"/> class.
        /// </summary>
        /// <param name="match">The match to generate a report for.</param>
        /// <param name="fileName">The file to save the report to.</param>
        public GenerateReportResult(Match match, string fileName)
        {
            this.Match = match;
            this.FileName = fileName;
        }

        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        /// <summary>
        /// Gets the match to generate a report for.
        /// </summary>
        public Match Match { get; private set; }

        /// <summary>
        /// Gets the file to save the report to.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Executes this action
        /// </summary>
        /// <param name="context">The execution context.</param>
        public void Execute(CoroutineExecutionContext context)
        {
            Task.Run(() => this.GenerateReport());
        }

        /// <summary>
        /// Generates the report.
        /// </summary>
        private void GenerateReport()
        {
            // Get hands on the report generator            
            try
            {
                var generator = IoC.Get<IReportGenerator>();
                var report = generator.GenerateReport(this.Match);

                var renderer = IoC.Get<IReportRenderer>("PDF");

                using (var sink = File.Create(this.FileName))
                {
                    report.RenderToStream(renderer, sink);
                }

                this.Completed(this, new ResultCompletionEventArgs());
                Process.Start(this.FileName);
            }
            catch (Exception exc)
            {
                var args = new ResultCompletionEventArgs()
                {
                    Error = exc
                };
                this.Completed(this, args);
            }
        }
    }
}
