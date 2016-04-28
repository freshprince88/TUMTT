//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.ReportGenerator
{
    using System;
    using System.IO;
    using System.Linq;
    using TT.Lib.Models;
    using TT.Lib.Models.Serialization;
    using TT.Report.Generators;
    using TT.Report.Renderers;

    /// <summary>
    /// Application entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The report generator.
        /// </summary>
        private static readonly IReportGenerator Generator = new DefaultReportGenerator();

        /// <summary>
        /// Executes the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            var reports = args
                .AsParallel()
                .Select(fn => Tuple.Create(fn, LoadMatch(fn)))
                .Where(p => p.Item2 != null)
                .Select(p => Tuple.Create(p.Item1, Generator.GenerateReport(p.Item2)));

            foreach (var pair in reports)
            {
                try
                {
                    var renderer = new PdfRenderer();
                    var target = Path.ChangeExtension(pair.Item1, ".pdf");
                    using (var sink = File.Create(target))
                    {
                        pair.Item2.RenderToStream(renderer, sink);
                    }

                    Console.WriteLine("{0} -> {1}", pair.Item1, target);
                }
                catch (Exception e)
                {
                    Console.WriteLine("FAILED {0}: {1}", pair.Item1, e.Message);
                }
            }
        }

        /// <summary>
        /// Loads a single match.
        /// </summary>
        /// <param name="fileName">The file to load from.</param>
        /// <returns>The loaded match.</returns>
        private static Match LoadMatch(string fileName)
        {
            try
            {
                using (var source = File.OpenRead(fileName))
                {
                    return new XmlMatchSerializer().Deserialize(source);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("FAILED {0}: {1}", fileName, e.Message);
                return null;
            }
        }
    }
}
