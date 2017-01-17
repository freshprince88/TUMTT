//-----------------------------------------------------------------------
// <copyright file="PdfRenderer.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

using MigraDoc.DocumentObjectModel.Shapes;

namespace TT.Report.Renderers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.Statistics;
    using MigraDoc.DocumentObjectModel;
    using MigraDoc.DocumentObjectModel.Fields;
    using MigraDoc.DocumentObjectModel.Tables;
    using MigraDoc.Rendering;
    using OxyPlot.Pdf;
    using TT.Models;
    using TT.Models.Statistics;
    using TT.Report.Renderers.Pdf;
    using TT.Report.Sections;
    using OxyPlot.Series;
    using System.Diagnostics;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using System.Threading;

    /// <summary>
    /// Renders a report to a PDF file.
    /// </summary>
    public class PdfRenderer : IReportRenderer
    {
        private readonly Func<object, int, int, string> bitmapFrameToTempFileFunction;
        private readonly Func<object, int, int, string> oxyPlotToTempFilePathFunction;

        /// <summary>
        /// Format for probability values.
        /// </summary>
        private const string ProbabilityFormat = "P2";

        /// <summary>
        /// Format for performance values.
        /// </summary>
        private const string PerformanceFormat = "F2";

        /// <summary>
        /// Format for average values.
        /// </summary>
        private const string AverageFormat = "F2";

        /// <summary>
        /// The color for borders.
        /// </summary>        
        private static readonly Color BorderColor = Color.Parse("0xFF154171");

        /// <summary>
        /// The color for titles.
        /// </summary>
        private static readonly Color TitleColor = Color.Parse("0xFF154171");

        /// <summary>
        /// A color for plot titles that matches TitleColor.
        /// </summary>
        private static readonly OxyPlot.OxyColor PlotTitleColor = OxyPlot.OxyColor.Parse("#FF154171");

        /// <summary>
        /// The color for the first player.
        /// </summary>
        private static readonly Color FirstPlayerColor = Color.Parse("0xFF4F81BD");

        /// <summary>
        /// The color for the second player.
        /// </summary>
        private static readonly Color SecondPlayerColor = Color.Parse("0xFFC0504D");

        /// <summary>
        /// The table shading color for the first player.
        /// </summary>
        private static readonly Color FirstPlayerShadingColor = Color.Parse("0x444F81BD");

        /// <summary>
        /// The table shading color for the second player.
        /// </summary>
        private static readonly Color SecondPlayerShadingColor = Color.Parse("0x44C0504D");

        /// <summary>
        /// The color for italic warning strings.
        /// </summary>
        private static readonly Color WarningColor = Color.Parse("0xFF9C9C9C");

        /// <summary>
        /// Indent for tables.
        /// </summary>
        private static readonly Unit TableIndent = Unit.FromCentimeter(0.25);

        /// <summary>
        /// Maximum width of a column of a table that fits perfectly on one A4 page (portrait)
        /// </summary>
        private static readonly int MaxTableColWidth = 455;

        /// <summary>
        /// Keeps track of the heading numbers.
        /// </summary>
        private int[] headingCounters;

        /// <summary>
        /// The renderer to render this document.
        /// </summary>
        private PdfDocumentRenderer renderer;

        /// <summary>
        /// The temporary files created for rendering.
        /// </summary>
        private List<string> temporaryFiles = new List<string>();

        /// <summary>
        /// Gets the rendered document.
        /// </summary>
        public Document Document { get; private set; }

        public PdfRenderer()
        {
            bitmapFrameToTempFileFunction = (item, width, height) =>
            {
                if (item == null)
                    return @"pack://application:,,,/TT.Report;component/Resources/image_loading_error.png";

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add((BitmapFrame)(object)item);

                var tempFile = this.GetTempFile();
                using (Stream stm = File.Create(tempFile))
                {
                    encoder.Save(stm);
                }
                return tempFile;
            };
            oxyPlotToTempFilePathFunction = (p, width, height) =>
            {
                if (p == null)
                    return @"pack://application:,,,/TT.Report;component/Resources/image_loading_error.png";

                var plot = (OxyPlot.PlotModel)p;
                FixPlotColor(plot);
                var tempFile = GetTempFile();
                PdfExporter.Export(plot, tempFile, width, height);
                return tempFile;
            };
        }

        /// <summary>
        /// Initializes the renderer.
        /// </summary>
        public void BeforeRendering()
        {
            this.Document = new Document();
            this.Document.AddSection();
            this.headingCounters = new int[] { 0, 0, 0 };
            this.SetupStyles();
        }

        /// <summary>
        /// Finalizes the renderer.
        /// </summary>
        public void AfterRendering()
        {
            try
            {
                // Really render the document, and clean up the temporary files
                this.renderer = new PdfDocumentRenderer(true)
                {
                    Document = this.Document,
                    Language = "en"
                };
                this.renderer.RenderDocument();
            }
            catch (Exception e) when (e is NullReferenceException || e is ArgumentException)
            {
                Debug.WriteLine("PdfRenderer: {2} in 'RenderDocument()' (this.Hash={0} Thread.Name={1})", GetHashCode(), Thread.CurrentThread.Name, e.GetType().Name);
            }
            finally
            {
                foreach (var temp in this.temporaryFiles)
                {
                    try
                    {
                        File.Delete(temp);
                    }
                    catch (IOException)
                    {
                    }
                }

                this.temporaryFiles.Clear();
            }
        }

        /// <summary>
        /// Renders the document metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public void Visit(MetadataSection metadata)
        {
            this.Document.Info.Title = metadata.Title;
            this.Document.Info.Subject = metadata.Subject;
            this.Document.Info.Author = metadata.Author;

            // General page setup
            this.Document.DefaultPageSetup.PageFormat = PageFormat.A4;

            var section = this.Document.LastSection;
            section.PageSetup.DifferentFirstPageHeaderFooter = true;

            var header = section.Headers.Primary.AddParagraph();

            header.AddInfoField(InfoFieldType.Title);
            header.AddTab();
            header.AddInfoField(InfoFieldType.Subject);
            header.Format.AddTabStop(Unit.FromCentimeter(16), TabAlignment.Right);

            section.Footers.Primary.Format.Alignment = ParagraphAlignment.Right;
            section.Footers.FirstPage.Format.Alignment = ParagraphAlignment.Right;

            // Create a paragraph with centered page number. See definition of style "Footer".
            Paragraph paragraph = new Paragraph();
            paragraph.AddPageField();
            paragraph.AddTab();
            paragraph.AddTab();
            paragraph.AddText("© Technische Universität München · 2015");

            // Add paragraph to footer for odd pages.
            section.Footers.Primary.Add(paragraph);
         
            section.Footers.FirstPage.Add(paragraph.Clone());
        }

        /// <summary>
        /// Renders the document header
        /// </summary>
        /// <param name="header">The header</param>
        public void Visit(HeaderSection header)
        {
            var section = this.Document.LastSection;
            section.AddParagraph(header.Headline)
                .Style = OurStyleNames.Title;
            section.AddParagraph(header.Tournament)
                .Style = OurStyleNames.Subtitle;
            section.AddParagraph(header.Round)
                .Style = OurStyleNames.Subtitle;
            var subtitleRound = section.AddParagraph(header.Date.ToShortDateString());
            subtitleRound.Style = OurStyleNames.Subtitle;
            subtitleRound.Format.SpaceAfter = 10;
        }

        /// <summary>
        /// Renders the basic info section.
        /// </summary>
        /// <param name="info">The basic information</param>
        public void Visit(BasicInformationSection info)
        {
            var head = this.AddHeading(2, Properties.Resources.section_basicinfo_title);
            head.Format.SpaceAfter = 10;

            var table = this.Document.LastSection.AddTable();
            table.Format.Alignment = ParagraphAlignment.Center;
            table.BottomPadding = 3;
            table.TopPadding = 3;
            table.Rows.LeftIndent = TableIndent;

            var names = table.AddColumn(Unit.FromCentimeter(2));
            var ranking = table.AddColumn(Unit.FromCentimeter(2));
            var ralliesOffset = ranking.Index + 1;
            foreach (var result in info.FinalRallyScores)
            {
                // One column for each rally result
                table.AddColumn(new Unit(0.6, UnitType.Centimeter));
            }

            var totalPoints = table.AddColumn(Unit.FromCentimeter(1.2));
            var performance = table.AddColumn(Unit.FromCentimeter(2));
            var winningProbability = table.AddColumn(Unit.FromCentimeter(2));
            var serviceFrequency = table.AddColumn(Unit.FromCentimeter(1.2));
            var serviceWinningProbability = table.AddColumn(Unit.FromCentimeter(2));

            var titleRow = table.AddRow();
            var subTitleRow = table.AddRow();
            var firstPlayer = table.AddRow();
            var secondPlayer = table.AddRow();
            foreach (Row row in table.Rows)
            {
                row.VerticalAlignment = VerticalAlignment.Center;
                if (row != titleRow && row != subTitleRow)
                {
                    row[names.Index].Format.Alignment = ParagraphAlignment.Left;
                    row[ranking.Index].Format.Alignment = ParagraphAlignment.Left;
                }
            }

            // Keep table on one page
            titleRow.KeepWith = table.Rows.Count - 1;

            titleRow.HeadingFormat = true;
            subTitleRow.HeadingFormat = true;

            // Merge all title cells into subtitle cells, with the exception of the service cells
            for (int i = 0; i < serviceFrequency.Index; ++i)
            {
                titleRow.Cells[i].MergeDown = 1;
            }

            // Merge the title of the service cells right
            titleRow.Cells[serviceFrequency.Index].MergeRight = 1;

            table.SetEdge(0, 0, table.Columns.Count, 1, Edge.Top, BorderStyle.Single, 1.5, BorderColor);
            table.SetEdge(0, 1, table.Columns.Count, 1, Edge.Bottom, BorderStyle.Single, 1, BorderColor);
            table.SetEdge(0, 3, table.Columns.Count, 1, Edge.Bottom, BorderStyle.Single, 1.5, BorderColor);

            names.SetCells(
                Properties.Resources.section_basicinfo_player,
                string.Empty,
                info.FirstPlayer.Name,
                info.SecondPlayer.Name);
            ranking.SetCells(
                Properties.Resources.section_basicinfo_ranking,
                string.Empty,
                FormatRank(info.FirstPlayer.Rank ?? new Rank(0, DateTime.Now)),
                FormatRank(info.SecondPlayer.Rank ?? new Rank(0, DateTime.Now)));
            totalPoints.SetCells(
                Properties.Resources.section_basicinfo_totalpoints,
                string.Empty,
                info.FirstPlayerStats.TotalPoints.ToString(),
                info.SecondPlayerStats.TotalPoints.ToString());
            performance.SetCells(
                Properties.Resources.section_basicinfo_compref,
                string.Empty,
                info.FirstPlayerStats.CompetitionPerformance.ToString(PerformanceFormat),
                info.SecondPlayerStats.CompetitionPerformance.ToString(PerformanceFormat));
            winningProbability.SetCells(
                Properties.Resources.section_basicinfo_service_winprob,
                string.Empty,
                info.FirstPlayerStats.WinningProbability.ToString(ProbabilityFormat),
                info.SecondPlayerStats.WinningProbability.ToString(ProbabilityFormat));
            serviceFrequency.SetCells(
                Properties.Resources.section_basicinfo_service,
                Properties.Resources.section_basicinfo_service_freq,
                info.FirstPlayerStats.ServiceFrequency.ToString(),
                info.SecondPlayerStats.ServiceFrequency.ToString());
            serviceWinningProbability.SetCells(
                string.Empty,
                Properties.Resources.section_basicinfo_service_winprob,
                info.FirstPlayerStats.ProbabilityOfWinningAfterService.ToString(ProbabilityFormat),
                info.SecondPlayerStats.ProbabilityOfWinningAfterService.ToString(ProbabilityFormat));

            titleRow.Cells[ralliesOffset].AddParagraph(
                string.Format("{0} ({1}:{2})", Properties.Resources.section_basicinfo_result, info.FinalSetScore.First, info.FinalSetScore.Second));
            titleRow.Cells[ralliesOffset].MergeRight = info.FinalRallyScores.Count() - 1;
            var scores = info.FinalRallyScores
                .Select((score, i) => Tuple.Create(ralliesOffset + i, score));
            foreach (var pair in scores)
            {
                firstPlayer.Cells[pair.Item1].AddParagraph(pair.Item2.First.ToString());
                secondPlayer.Cells[pair.Item1].AddParagraph(pair.Item2.Second.ToString());
            }
        }

        /// <summary>
        /// Renders the rally lengths section.
        /// </summary>
        /// <param name="lengths">The lengths</param>
        public void Visit(RallyLengthSection lengths)
        {
            var stats = lengths.Statistics;

            var section = this.Document.LastSection;
            this.AddHeading(2, Properties.Resources.section_rallylength_title);

            var head = this.AddHeading(3, Properties.Resources.section_rallylength_avg_title);
            head.Format.SpaceAfter = 10;

            var table = section.AddTable();
            table.Format.Alignment = ParagraphAlignment.Center;
            table.BottomPadding = 3;
            table.TopPadding = 3;
            table.Rows.LeftIndent = TableIndent;

            var titleColumn = table.AddColumn(Unit.FromCentimeter(1.3));
            var totalLengths = table.AddColumn(Unit.FromCentimeter(2));
            var serviceFirst = table.AddColumn(Unit.FromCentimeter(1.2));
            var serviceSecond = table.AddColumn(Unit.FromCentimeter(1.2));
            var winnerFirst = table.AddColumn(Unit.FromCentimeter(1.2));
            var winnerSecond = table.AddColumn(Unit.FromCentimeter(1.2));
            var serviceFirstWinnerFirst = table.AddColumn(Unit.FromCentimeter(1.8));
            var serviceFirstWinnerSecond = table.AddColumn(Unit.FromCentimeter(1.8));
            var serviceSecondWinnerFirst = table.AddColumn(Unit.FromCentimeter(1.8));
            var serviceSecondWinnerSecond = table.AddColumn(Unit.FromCentimeter(1.8));

            var titleRow = table.AddRow();
            var subTitleRow = table.AddRow();
            var meanRow = table.AddRow();
            var medianRow = table.AddRow();
            foreach (Row row in table.Rows)
            {
                row.VerticalAlignment = VerticalAlignment.Center;
                if (row != titleRow && row != subTitleRow)
                {
                    row.Cells[titleColumn.Index].Format.Alignment = ParagraphAlignment.Left;
                }
            }

            titleRow.Cells[totalLengths.Index].MergeDown = 1;
            titleRow.Cells[serviceFirst.Index].MergeRight = 1;
            titleRow.Cells[winnerFirst.Index].MergeRight = 1;
            titleRow.Cells[serviceFirstWinnerFirst.Index].MergeRight = 1;
            titleRow.Cells[serviceSecondWinnerFirst.Index].MergeRight = 1;

            titleColumn.HeadingFormat = true;
            titleRow.HeadingFormat = true;
            subTitleRow.HeadingFormat = true;

            table.SetEdge(0, 0, table.Columns.Count, 1, Edge.Top, BorderStyle.Single, 1.5, BorderColor);
            table.SetEdge(0, 1, table.Columns.Count, 1, Edge.Bottom, BorderStyle.Single, 1, BorderColor);
            table.SetEdge(0, 3, table.Columns.Count, 1, Edge.Bottom, BorderStyle.Single, 1.5, BorderColor);

            titleColumn.SetCells(
                string.Empty,
                string.Empty,
                Properties.Resources.section_rallylength_avg_mean,
                Properties.Resources.section_rallylength_avg_median);
            totalLengths.SetCells(
                Properties.Resources.section_rallylength_avg_totallengths,
                string.Empty,
                stats.TotalLengths.Select(l => (double)l).Mean().ToString(AverageFormat),
                stats.TotalLengths.CategoricalMedian().ToString(AverageFormat));
            serviceFirst.SetCells(
                Properties.Resources.section_rallylength_avg_service,
                "A",
                stats.ByServer[MatchPlayer.First].Select(l => (double)l).Mean().ToString(AverageFormat),
                stats.ByServer[MatchPlayer.First].CategoricalMedian().ToString(AverageFormat));
            serviceSecond.SetCells(
                string.Empty,
                "B",
                stats.ByServer[MatchPlayer.Second].Select(l => (double)l).Mean().ToString(AverageFormat),
                stats.ByServer[MatchPlayer.Second].CategoricalMedian().ToString(AverageFormat));
            winnerFirst.SetCells(
                Properties.Resources.section_rallylength_avg_winner,
                "A",
                stats.ByWinner[MatchPlayer.First].Select(l => (double)l).Mean().ToString(AverageFormat),
                stats.ByWinner[MatchPlayer.First].CategoricalMedian().ToString(AverageFormat));
            winnerSecond.SetCells(
                string.Empty,
                "B",
                stats.ByWinner[MatchPlayer.Second].Select(l => (double)l).Mean().ToString(AverageFormat),
                stats.ByWinner[MatchPlayer.Second].CategoricalMedian().ToString(AverageFormat));

            var offset = serviceFirstWinnerFirst.Index;
            var players = new MatchPlayer[] { MatchPlayer.First, MatchPlayer.Second };
            foreach (var server in players)
            {
                foreach (var winner in players)
                {
                    table.Columns[offset].SetCells(
                        string.Format("{0} {1}", Properties.Resources.section_rallylength_avg_service, (server == MatchPlayer.First ? "A" : "B")),
                        string.Format("{0} {1}", Properties.Resources.section_rallylength_avg_winner, (winner == MatchPlayer.First ? "A" : "B")),
                        stats.ByServerAndWinner[server][winner].Select(l => (double)l).Mean().ToString(AverageFormat),
                        stats.ByServerAndWinner[server][winner].CategoricalMedian().ToString(AverageFormat));
                    offset++;
                }
            }

            this.AddHeading(3, Properties.Resources.section_rallylength_dist_title);
            this.AddPlot(lengths.Plot);
        }

        /// <summary>
        /// Renders the scoring process section.
        /// </summary>
        /// <param name="process">The section</param>
        public void Visit(ScoringProcessSection process)
        {
            var head = this.AddHeading(2, Properties.Resources.section_scoringprocess_title);
            head.Format.SpaceBefore = 10;
            this.AddPlot(process.Plot, height: 200);
        }

        /// <summary>
        /// Renders the match dynamics section.
        /// </summary>
        /// <param name="section">The section.</param>
        public void Visit(MatchDynamicsSection section)
        {
            this.AddHeading(2, Properties.Resources.section_matchdynamics_title);

            this.AddHeading(3, Properties.Resources.section_matchdynamics_overall);

            this.AddPlot(section.OverallPlot, height: 180);

            this.AddHeading(3, Properties.Resources.section_matchdynamics_byplayer);

            this.AddPlot(section.ByServerPlot, height: 180);
        }

        /// <summary>
        /// Renders the transitions section.
        /// </summary>
        /// <param name="section">The section.</param>
        public void Visit(TransitionsSection section)
        {
            var transitions = section.Transitions;

            this.AddHeading(2, Properties.Resources.section_transitionmatrix_title);

            var head = this.AddHeading(3, Properties.Resources.section_transitionmatrix_abs_title);
            head.Format.SpaceAfter = 10;

            var absolute = transitions.TransitionsByPlayer;
            var firstAbsolute = this.ProjectTransitionMatrix(absolute[MatchPlayer.First]);
            var points = transitions.PointsAtStrokeByPlayer;
            var errors = transitions.ErrorsAtStrokeByPlayer;
            var absoluteTable = this.AddTransitionTable(
                firstAbsolute.RowCount,
                firstAbsolute.ColumnCount);

            this.FillTransitionTable(
                absoluteTable,
                firstAbsolute,
                this.ProjectTransitionVector(points[MatchPlayer.First]),
                this.ProjectTransitionVector(errors[MatchPlayer.First]),
                v => v.ToString("F0"));
            this.FillTransitionTable(
                absoluteTable,
                this.ProjectTransitionMatrix(absolute[MatchPlayer.Second]),
                this.ProjectTransitionVector(points[MatchPlayer.Second]),
                this.ProjectTransitionVector(errors[MatchPlayer.Second]),
                v => v.ToString("F0"),
                firstAbsolute.ColumnCount + 4);

            head = this.AddHeading(3, Properties.Resources.section_transitionmatrix_prob_title);
            head.Format.SpaceAfter = 10;

            var pTransition = transitions.ProbabilitiesByPlayer;
            var pFirst = this.ProjectTransitionMatrix(pTransition[MatchPlayer.First]);
            var pPoint = transitions.PointAtStrokeProbabilityByPlayer;
            var pError = transitions.ErrorAtStrokeProbabilityByPlayer;
            var probabilityTable = this.AddTransitionTable(
               pFirst.RowCount,
               pFirst.ColumnCount);
            this.FillTransitionTable(
                probabilityTable,
                pFirst,
                this.ProjectTransitionVector(pPoint[MatchPlayer.First]),
                this.ProjectTransitionVector(pError[MatchPlayer.First]),
                v => (v * 100).ToString("F1"));
            this.FillTransitionTable(
                probabilityTable,
                this.ProjectTransitionMatrix(pTransition[MatchPlayer.Second]),
                this.ProjectTransitionVector(pPoint[MatchPlayer.Second]),
                this.ProjectTransitionVector(pError[MatchPlayer.Second]),
                v => (v * 100).ToString("F1"),
                pFirst.ColumnCount + 4);
        }

        /// <summary>
        /// Renders the technical efficiency section.
        /// </summary>
        /// <param name="section">The technical efficiency section</param>
        public void Visit(TechnicalEfficiencySection section)
        {
            var head = this.AddHeading(2, Properties.Resources.section_techefficiency_title);
            head.Format.SpaceAfter = 10;

            var te = section.TechnicalEfficiency;

            this.AddTechnicalEfficiencyTable(MatchPlayer.First, te);
            this.Document.LastSection.AddParagraph();
            this.AddTechnicalEfficiencyTable(MatchPlayer.Second, te);
        }

        public void Visit(PartSection section)
        {
            // each new part means new heading counters
            //this.ResetHeadingCounters();
            
            var partName = string.Format(
                section.Player != null ? "{0} - {1}" : "{0}",
                section.PartName,
                section.Player != null ? section.Player.Name : "");
            var paragraph = AddHeading(1, partName);

            if (section.Type != PartSection.PartType.General)
                paragraph.Format.PageBreakBefore = true;
        }

        public void Visit(StrokeStatsHeadingSection section)
        {
            this.AddHeading(2, section.StrokeName);
        }

        public void Visit(SideSection section)
        {
            this.AddHeading(3, section.HasStepAround ? Properties.Resources.section_side_steparound : Properties.Resources.section_side);

            AddItemsToTable(section.SidePlots, null, oxyPlotToTempFilePathFunction, 450, 210, 270, 150);
        }

        public void Visit(SpinSection section)
        {
            AddHeading(3, Properties.Resources.section_spin);

            AddItemsToTable(section.SpinPlots, null, oxyPlotToTempFilePathFunction, 450, 210, 270, 150);
        }

        public void Visit(TechniqueSection section)
        {
            Debug.WriteLine("Visiting Technique section {0}", section);
            AddHeading(3, Properties.Resources.section_technique);

            var itemsList = new List<object>();
            foreach (var set in section.ExistingStatisticsImageBitmapFrames.Keys)
                itemsList.Add(section.ExistingStatisticsImageBitmapFrames[set]);

            if (itemsList.Count > 0 && itemsList.ElementAt(0) is BitmapFrame)
                AddItemsToTable(itemsList, section.ExistingStatisticsImageBitmapFrames.Keys.ToList(), bitmapFrameToTempFileFunction, 300, 200, tableIndentLeft: 2.25);
            else if (itemsList.Count > 0 && itemsList.ElementAt(0) is List<object>)
                AddItemsToTwoColTable(itemsList, section.ExistingStatisticsImageBitmapFrames.Keys.ToList(), oxyPlotToTempFilePathFunction, bitmapFrameToTempFileFunction, 280, 190, keepCol1AspectRation: false);
        }

        public void Visit(PlacementSection section)
        {
            AddHeading(3, Properties.Resources.section_placement);

            var itemsList = new List<object>();
            foreach (var set in section.ExistingStatisticsImageBitmapFrames.Keys)
                itemsList.Add(section.ExistingStatisticsImageBitmapFrames[set]);
            AddItemsToTable(itemsList, section.ExistingStatisticsImageBitmapFrames.Keys.ToList(), bitmapFrameToTempFileFunction, 300, 200, tableIndentLeft: 2.25);
        }

        public void Visit(LargeTableSection section)
        {
            AddHeading(3, Properties.Resources.section_table);

            var itemsList = new List<BitmapFrame>();
            foreach (var set in section.TableImageBitmapFrames.Keys)
                itemsList.Add(section.TableImageBitmapFrames[set]);
            AddItemsToTable(itemsList, section.TableImageBitmapFrames.Keys.ToList(), bitmapFrameToTempFileFunction, 430, 210);
        }

        public void Visit(StrokeNumberSection section)
        {
            AddHeading(3, Properties.Resources.section_strokenumber);

            AddItemsToTable(section.NumberPlots, null, oxyPlotToTempFilePathFunction, 450, 210, 290, 190);
        }

        public void Visit(LastStrokeServiceSection section)
        {
        }

        public void Visit(SectionEmptyWarningSection section)
        {
            Document.LastSection.AddParagraph(string.Format(Properties.Resources.section_empty_warning, args: section.Player.Name), OurStyleNames.Warning);
        }

        public void Visit(TableLegendSection section)
        {
            var sec = Document.LastSection;
            var table = sec.AddTable();
            table.Borders.Visible = false;
            table.AddColumn(MaxTableColWidth);

            var row1 = table.AddRow();
            row1.KeepWith = 1;
            var par = row1.Cells[0].AddParagraph(Properties.Resources.legend_section_header);
            par.Style = OurStyleNames.SetName;

            var row2 = table.AddRow();
            par = row2.Cells[0].AddParagraph();
            par.Format.Alignment = ParagraphAlignment.Center;

            var tmpImageFile = bitmapFrameToTempFileFunction.Invoke(section.LegendImage, 180, 400);
            var img = par.AddImage(tmpImageFile);
            img.LockAspectRatio = true;
            img.Width = 180;
        }

        /// <summary>
        /// Saves the rendered PDF.
        /// </summary>
        /// <param name="sink">The stream to write to.</param>
        public void Save(Stream sink)
        {
            try
            {
                this.renderer.PdfDocument.Save(sink);
            }
            catch (Exception e) when (e is NullReferenceException || e is InvalidOperationException)
            {
                Debug.WriteLine("PdfRenderer: {2} in 'Save(Stream)' (this.Hash={0} Thread.Name={1})", GetHashCode(), Thread.CurrentThread.Name, e.GetType().Name);
            }
        }

        /// <summary>
        /// Formats a ranking.
        /// </summary>
        /// <param name="rank">The ranking.</param>
        /// <returns>The formatted value.</returns>
        private static string FormatRank(Rank rank)
        {
            if (rank.Position <= 0)
            {
                return "-";
            }
            else
            {
                return string.Format("{0} ({1:MMM yyyy})", rank.Position, rank.Date);
            }
        }

        /// <summary>
        /// Sets up the document styles.
        /// </summary>
        private void SetupStyles()
        {
            var normal = this.Document.Styles[StyleNames.Normal];
            normal.Font.Name = "Calibri";
            normal.Font.Size = 10;
            normal.Font.Color = Colors.Black;
            normal.ParagraphFormat.KeepTogether = true;

            var header = this.Document.Styles[StyleNames.Header];
            header.Font.Size = 9;

            var footer = this.Document.Styles[StyleNames.Footer];
            footer.Font.Size = 9;
            footer.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            var title = this.Document.Styles.AddStyle(
                OurStyleNames.Title, StyleNames.Normal);
            title.Font.Name = "Calibri";
            title.Font.Size = 22;
            title.Font.Color = TitleColor;
            title.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            title.ParagraphFormat.Borders.DistanceFromBottom = 2;
            title.ParagraphFormat.Borders.Bottom.Width = 1;
            title.ParagraphFormat.Borders.Bottom.Visible = true;
            title.ParagraphFormat.Borders.Bottom.Color = Color.Parse("0xFF2166B1");
            title.ParagraphFormat.SpaceAfter = 18;

            var subtitle = this.Document.Styles.AddStyle(
                OurStyleNames.Subtitle, OurStyleNames.Title);
            subtitle.Font.Size = 14;
            subtitle.Font.Bold = true;
            subtitle.ParagraphFormat.Borders.Bottom.Visible = false;
            subtitle.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            subtitle.ParagraphFormat.SpaceAfter = 3;
            subtitle.ParagraphFormat.SpaceBefore = 3;
            
            var heading1 = this.Document.Styles[StyleNames.Heading1];
            heading1.Font = title.Font.Clone();
            heading1.Font.Size = 18;
            heading1.Font.Bold = true;
            heading1.Font.Color = TitleColor;
            heading1.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            heading1.ParagraphFormat.SpaceAfter = 3;
            heading1.ParagraphFormat.SpaceBefore = 3;

            var heading2 = this.Document.Styles[StyleNames.Heading2];
            heading2.BaseStyle = StyleNames.Heading1;            
            heading2.Font.Size = 15;
            heading2.Font.Bold = true;
            heading2.ParagraphFormat.PageBreakBefore = false;
            heading2.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            heading2.ParagraphFormat.SpaceAfter = 5;
            heading2.ParagraphFormat.SpaceBefore = 9;
            heading2.ParagraphFormat.KeepWithNext = true;

            var heading3 = this.Document.Styles[StyleNames.Heading3];
            heading3.Font.Size = 12;
            heading3.BaseStyle = StyleNames.Heading1;
            heading3.ParagraphFormat.SpaceBefore = 9;
            heading3.ParagraphFormat.SpaceAfter = 5;
            heading3.ParagraphFormat.PageBreakBefore = false;
            heading3.ParagraphFormat.KeepWithNext = true;

            var setName = this.Document.Styles.AddStyle(
                OurStyleNames.SetName, StyleNames.Normal);
            setName.Font.Size = 15;
            setName.Font.Bold = true;
            setName.Font.Color = TitleColor;
            setName.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            setName.ParagraphFormat.SpaceBefore = 10;
            setName.ParagraphFormat.SpaceAfter = 5;
            setName.ParagraphFormat.Font.Size = 16;
            setName.ParagraphFormat.KeepWithNext = true;

            var warning = this.Document.Styles.AddStyle(
                OurStyleNames.Warning, StyleNames.Normal);
            warning.Font.Size = 14;
            warning.Font.Italic = true;
            warning.Font.Color = WarningColor;
            warning.ParagraphFormat.SpaceBefore = 30;
            warning.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        }

        /// <summary>
        /// Creates a transition table.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns per player.</param>
        /// <returns>The table.</returns>
        private Table AddTransitionTable(int rows, int columns)
        {
            var section = this.Document.LastSection;

            // We create a single big table to show transitions for both players,
            // to have them aligned nicely.
            var table = section.AddTable();
            table.Rows.LeftIndent = TableIndent;
            table.BottomPadding = 2;
            table.TopPadding = 2;
            table.RightPadding = 2;
            table.LeftPadding = 2;
            table.Format.Font.Size = 9.5;
            table.Format.Alignment = ParagraphAlignment.Center;
            table.Borders.Color = Colors.Black;

            var titleWidth = Unit.FromCentimeter(0.8);
            var valueWidth = Unit.FromCentimeter(0.85);

            var labels = new Tuple<string, string>[]
            {
                Tuple.Create("A", "B"),
                Tuple.Create("B", "A"),
            };

            var colors = new Color[] 
            {
                FirstPlayerColor,
                SecondPlayerColor
            };

            var shadingColors = new Color[] 
            {
                FirstPlayerShadingColor,
                SecondPlayerShadingColor
            };

            Column[] titleCols = new Column[labels.Length];
            Column[] pointCols = new Column[labels.Length];
            Column[] errorCols = new Column[labels.Length];

            for (int part = 0; part < labels.Length; ++part)
            {
                titleCols[part] = table.AddColumn(titleWidth);
                titleCols[part].HeadingFormat = true;
                titleCols[part].Format.Alignment = ParagraphAlignment.Center;
                titleCols[part].Format.Font.Bold = true;
                for (int i = 0; i < columns; ++i)
                {
                    table.AddColumn(valueWidth);
                }

                pointCols[part] = table.AddColumn(valueWidth);
                errorCols[part] = table.AddColumn(valueWidth);

                if (part != labels.Length - 1)
                {
                    // Now a separating column.  Calculate its width based on the page 
                    // width and the table width
                    var page = section.Document.DefaultPageSetup;
                    var width = page.PageWidth.Point
                        - page.LeftMargin - page.RightMargin
                        - (labels.Length * TableIndent.Point) // Table indent, twice for centering
                        - (titleCols.Length * titleWidth)
                        - (pointCols.Length * valueWidth.Point)
                        - (errorCols.Length * valueWidth.Point)
                        - (labels.Length * columns * valueWidth.Point); // Width of all transition columns
                    var separator = table.AddColumn(width);
                    separator.Borders.Width = 0;
                }
            }

            // Add the rows
            var titleRow = table.AddRow();
            titleRow.HeadingFormat = true;
            titleRow.Format.Alignment = ParagraphAlignment.Center;
            titleRow.Format.Font.Color = Colors.White;
            titleRow.Format.Font.Bold = true;
            for (int i = 0; i < rows; ++i)
            {
                table.AddRow();
            }

            // Keep the table on one page
            titleRow.KeepWith = table.Rows.Count - 1;

            for (int part = 0; part < labels.Length; ++part)
            {
                var from = labels[part].Item1;
                var to = labels[part].Item2;

                titleRow[titleCols[part].Index].AddParagraph(from + "→" + to);
                table[rows, titleCols[part].Index].AddParagraph(from + ">" + (rows - 1));
                for (int row = 1; row < rows; ++row)
                {
                    table[row, titleCols[part].Index].AddParagraph(from + row);
                }

                var titleCol = titleCols[part].Index;
                titleRow[titleCol + columns].AddParagraph(to + ">" + columns);
                titleRow[pointCols[part].Index].AddParagraph("P" + from);
                titleRow[errorCols[part].Index].AddParagraph("E" + from);
                for (int column = 1; column < columns; ++column)
                {
                    titleRow[titleCol + column].AddParagraph(to + (column + 1));
                }

                // Set the shading of the title row
                var color = colors[part];
                var shadingColor = shadingColors[part];
                for (int j = titleCols[part].Index; j <= errorCols[part].Index; ++j)
                {
                    titleRow[j].Shading.Color = color;

                    // And for all subsequent odd rows
                    for (int i = 2; i < table.Rows.Count; i += 2)
                    {
                        table[i, j].Shading.Color = shadingColor;
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Project a transition matrix into a smaller matrix to remove initial zero rows and columns.
        /// </summary>
        /// <param name="transitions">The transition matrix</param>
        /// <returns>The projected matrix</returns>
        private Matrix<double> ProjectTransitionMatrix(Matrix<double> transitions)
        {
            return SparseMatrix.OfColumns(
               transitions.RowCount - 1,
               transitions.ColumnCount - 2,
               transitions.EnumerateColumnsIndexed()
                   .Skip(2)
                   .Select(c => this.ProjectTransitionVector(c.Item2)));
        }

        /// <summary>
        /// Projects a transition vector to remove initial zero positions.
        /// </summary>
        /// <param name="v">The vector</param>
        /// <returns>The projected vector</returns>
        private Vector<double> ProjectTransitionVector(Vector<double> v)
        {
            return DenseVector.OfEnumerable(v.Skip(1));
        }

        /// <summary>
        /// Fills a transition table.
        /// </summary>
        /// <param name="table">The table to fill</param>
        /// <param name="transitions">The transitions</param>
        /// <param name="points">The points</param>
        /// <param name="errors">The errors</param>
        /// <param name="format">Function to format the value</param>
        /// <param name="offset">The column offset</param>
        private void FillTransitionTable(
            Table table,
            Matrix<double> transitions,
            Vector<double> points,
            Vector<double> errors,
            Func<double, string> format,
            int offset = 0)
        {
            offset += 1; // Take title column into account
            foreach (var triple in transitions.EnumerateIndexed(Zeros.AllowSkip))
            {
                table[triple.Item1 + 1, triple.Item2 + offset]
                    .AddParagraph(format(triple.Item3));
            }

            var pointsCol = transitions.ColumnCount + offset;
            foreach (var pair in points.EnumerateIndexed(Zeros.AllowSkip))
            {
                table[pair.Item1 + 1, pointsCol].AddParagraph(format(pair.Item2));
            }

            var errorsCol = pointsCol + 1;
            foreach (var pair in errors.EnumerateIndexed(Zeros.AllowSkip))
            {
                table[pair.Item1 + 1, errorsCol].AddParagraph(format(pair.Item2));
            }
        }

        /// <summary>
        /// Adds a table for technical efficiency.
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="te">The technical efficiency</param>
        /// <returns>The table</returns>
        private Table AddTechnicalEfficiencyTable(MatchPlayer player, TechnicalEfficiency te)
        {
            var section = this.Document.LastSection;

            var table = section.AddTable();
            table.Rows.LeftIndent = TableIndent;
            table.BottomPadding = 2;
            table.TopPadding = 2;
            table.RightPadding = 2;
            table.LeftPadding = 2;
            table.Format.Font.Size = 9.5;
            table.Format.Alignment = ParagraphAlignment.Center;
            table.Borders.Color = Colors.Black;

            var titleWidth = Unit.FromCentimeter(3.6);
            var valueWidth = Unit.FromCentimeter(1.7);

            var name = te.Match.Players.ElementAt(player == MatchPlayer.First ? 0 : 1).Name;
            var color = player == MatchPlayer.First ? FirstPlayerColor : SecondPlayerColor;
            var shading = player == MatchPlayer.First ?
                FirstPlayerShadingColor : SecondPlayerShadingColor;

            var titleColumn = table.AddColumn(titleWidth);
            for (int i = 0; i < 7; ++i)
            {
                table.AddColumn(valueWidth);
            }

            var titleRow = table.AddRow();
            var scoreRow = table.AddRow();
            var errorRow = table.AddRow();
            var scoringRow = table.AddRow();
            var usageRow = table.AddRow();
            var aggregateScoringRow = table.AddRow();
            var aggregateUsageRow = table.AddRow();
            var originalTERow = table.AddRow();
            var simpleTERow = table.AddRow();

            // Keep the table rows together
            titleRow.KeepWith = table.Rows.Count - 1;

            // The title column and row
            titleRow.HeadingFormat = true;
            titleRow.Shading.Color = color;
            titleRow.Format.Font.Color = Colors.White;
            titleRow.Format.Font.Bold = true;
            titleColumn.HeadingFormat = true;
            titleColumn.Format.Font.Bold = true;
            titleColumn.SetCells(
                name,
                "Score",
                Properties.Resources.section_techefficiency_error,
                "SR",
                "UR",
                "SR Service/Return/Long",
                "UR S/R/L",
                "TE (original) S/R/L",
                "TE (simple) S/R/L");
            titleRow[7].AddParagraph(">6");
            for (int i = 1; i <= 6; ++i)
            {
                titleRow[i].AddParagraph(i.ToString());
            }

            // Shade the rows
            for (int i = 2; i < table.Rows.Count; i += 2)
            {
                table.Rows[i].Shading.Color = shading;
            }

            for (int i = 1; i < table.Columns.Count; ++i)
            {
                scoreRow[i].AddParagraph(te.ScoresAtLength(player, i).ToString());
                errorRow[i].AddParagraph(te.ErrorsAtLength(player, i).ToString());

                var t = te.TechnicalEfficiencyAtLength(player, i);
                scoringRow[i].AddParagraph(t.ScoringRate.ToString(AverageFormat));
                usageRow[i].AddParagraph(t.UsageRate.ToString(AverageFormat));
            }

            var aggregates = new TechnicalEfficiency.TE[] 
            {
                te.ServiceTechnicalEfficiency(player),
                te.ReturnTechnicalEfficiency(player),
                te.LongRallyTechnicalEfficiency(player),
            };
            for (int i = 0; i < aggregates.Length; ++i)
            {
                var t = aggregates[i];
                var column = 1 + (i * 2);
                var mergeFactor = i == 2 ? 2 : 1;
                aggregateUsageRow[column].MergeRight = mergeFactor;
                aggregateUsageRow[column].AddParagraph(t.UsageRate.ToString(AverageFormat));
                aggregateScoringRow[column].MergeRight = mergeFactor;
                aggregateScoringRow[column].AddParagraph(t.ScoringRate.ToString(AverageFormat));
                originalTERow[column].MergeRight = mergeFactor;
                originalTERow[column].AddParagraph(t.Polynomial.ToString(AverageFormat));
                simpleTERow[column].MergeRight = mergeFactor;
                simpleTERow[column].AddParagraph(t.Simple.ToString(AverageFormat));
            }

            return table;
        }

        private void ResetHeadingCounters()
        {
            for (var i = 0; i < this.headingCounters.Length; i++)
            {
                this.headingCounters[i] = 0;
            }
        }

        /// <summary>
        /// Creates a heading.
        /// </summary>
        /// <param name="level">The heading level.</param>
        /// <param name="heading">The heading text.</param>
        /// <returns>The heading</returns>
        private Paragraph AddHeading(int level, string heading)
        {
            // Reset all higher heading levels
            for (int i = level; i < this.headingCounters.Length; ++i)
            {
                this.headingCounters[i] = 0;
            }

            this.headingCounters[level - 1] += 1;

            var counters = Enumerable.Range(1, level)
                .Select(i => this.headingCounters[i - 1]);

            var p = this.Document.LastSection.AddParagraph(
                string.Join(".", counters),
                "Heading" + level);
            p.AddTab();
            p.AddText(heading);
            return p;
        }

        /// <summary>
        /// Adds a plot to the current file.
        /// </summary>
        /// <param name="plot">The plot to add</param>
        /// <returns>The plot paragraph.</returns>
        /// <param name="width">The width of the plot.</param>
        /// <param name="height">The height of the plot.</param>
        private Paragraph AddPlot(OxyPlot.PlotModel plot, double width = 450, double height = 300)
        {
            var paragraph = this.Document.LastSection.AddParagraph();
            if (plot != null)
            {
                FixPlotColor(plot);
                var tempFile = this.GetTempFile();
                PdfExporter.Export(plot, tempFile, width, height);
                var image = paragraph.AddImage(tempFile);
                image.Width = width;
                image.Height = height;
            }
            return paragraph;
        }

        /// <summary>
        /// Fix up the plot text color.  For some insane reason, PDFSharp turns
        /// the plot's black into blue
        /// </summary>
        private void FixPlotColor(OxyPlot.PlotModel plot)
        {            
            if (plot.TextColor.Equals(OxyPlot.OxyColors.Black))
            {
                plot.TitleColor = PlotTitleColor;
                plot.TextColor = OxyPlot.OxyColor.FromArgb(255, 0, 0, 1);
            }
        }

        private Table AddItemsToTable<T>(List<T> items,
            List<string> setTitles,
            Func<T, int, int, string> itemToTempFilePathFunction,
            int normalWidth,
            int smallWidth,
            int normalHeight = 0,
            int smallHeight = 0,
            double tableIndentLeft = 0)
        {
            var tableItemsCount = items.Count;
            bool multipleItems = tableItemsCount > 1;

            Table table = Document.LastSection.AddTable();
            table.Borders.Visible = false;
            table.Rows.LeftIndent = Unit.FromCentimeter(multipleItems ? 0 : tableIndentLeft);

            Column col = table.AddColumn();
            col.Width = (int)(MaxTableColWidth / (multipleItems ?  2d : 1d));

            int sizeW, sizeH;
            if (multipleItems)
            {
                sizeW = smallWidth;
                sizeH = smallHeight;
                col = table.AddColumn();
                col.Width = (int)(MaxTableColWidth / 2d);
            }
            else
            {
                sizeW = normalWidth;
                sizeH = normalHeight;
            }

            int cellAmount;
            if (setTitles != null)
                cellAmount = (tableItemsCount % 2 == 1 ? (tableItemsCount + 1) : tableItemsCount) * 2;
            else
                cellAmount = tableItemsCount + (tableItemsCount % 2 == 1 ? 1 : 0);

            Row row = null;
            bool headerRow = false;
            int i = 0;
            for (var c = 0; c < cellAmount; c++)
            {
                var set = setTitles?.ElementAtOrDefault(i);

                int rowIndex = c % 2;
                if (rowIndex == 0)
                    row = table.AddRow();

                if (setTitles != null)
                {
                    if (c % 4 == 0 || c % 4 == 1)
                    {
                        headerRow = true;
                        row.KeepWith = 1;
                    }
                    else
                        headerRow = false;
                }

                if (headerRow)
                {
                    if (set != null)
                    {
                        var setHeading = row.Cells[rowIndex].AddParagraph(set);
                        setHeading.Style = OurStyleNames.SetName;
                    }
                    if (rowIndex == 1)
                        i -= 2;
                }
                else
                {
                    if (i < items.Count)
                    {
                        if (sizeH == 0)
                            sizeH = sizeW;

                        var tempFile = itemToTempFilePathFunction.Invoke(items.ElementAt(i), sizeW, sizeH);
                        var image = row.Cells[rowIndex].AddParagraph().AddImage(tempFile);
                        row.Cells[rowIndex].Format.Alignment = ParagraphAlignment.Center;

                        if (sizeH == sizeW)
                            image.LockAspectRatio = true;
                        else
                            image.Height = sizeH;

                        image.Width = sizeW;
                    }
                }
                i++;
            }
            if (multipleItems && row.Cells.Count == 1)
            {
                if (setTitles != null)
                    table.Rows[row.Index - 1].Cells[0].MergeRight = 1;
                row.Cells[0].MergeRight = 1;
            }

            return table;
        }

        private Table AddItemsToTwoColTable<T>(List<T> items,
            List<string> setTitles,
            Func<T, int, int, string> firstColItemToTempFilePathFunction,
            Func<T, int, int, string> secondColItemToTempFilePathFunction,
            int firstColWidth,
            int secondColWidth,
            int firstColHeight = 0,
            int secondColHeight = 0,
            bool keepCol1AspectRation = true,
            bool keepCol2AspectRation = true)
        {
            Table table = Document.LastSection.AddTable();
            table.Borders.Visible = false;
            //table.Rows.LeftIndent = Unit.FromCentimeter(-0.85);

            Column col1 = table.AddColumn();
            col1.Width = (int)(MaxTableColWidth / 2d);

            Column col2 = table.AddColumn();
            col2.Width = (int)(MaxTableColWidth / 2d);

            int sizeH = Math.Max(firstColHeight, secondColHeight);
            if (sizeH == 0)
                sizeH = Math.Max(firstColWidth, secondColWidth);

            int rowIndex = 0;
            foreach (var colItems in items)
            {
                var colItemsList = (List<T>)(object)colItems;

                Row row = table.AddRow();
                var heading = row.Cells[0].AddParagraph(setTitles[rowIndex]);
                heading.Style = OurStyleNames.SetName;
                row.Cells[0].MergeRight = 1;
                row.KeepWith = 1;

                row = table.AddRow();
                var tempFile = firstColItemToTempFilePathFunction.Invoke(colItemsList.ElementAt(0), firstColWidth, sizeH);
                var image = row.Cells[0].AddParagraph().AddImage(tempFile);
                if (keepCol1AspectRation)
                    image.LockAspectRatio = true;
                else
                    image.Height = sizeH;
                image.Width = firstColWidth;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                tempFile = secondColItemToTempFilePathFunction.Invoke(colItemsList.ElementAt(1), secondColWidth, sizeH);
                image = row.Cells[1].AddParagraph().AddImage(tempFile);
                if (keepCol2AspectRation)
                    image.LockAspectRatio = true;
                else
                    image.Height = sizeH;
                image.Width = secondColWidth;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

                rowIndex++;
            }

            return table;
        }

        /// <summary>
        /// Gets a temporary file.
        /// </summary>
        /// <returns>The path to the temporary file.</returns>
        private string GetTempFile()
        {
            var tmp = Path.GetTempFileName();
            this.temporaryFiles.Add(tmp);
            return tmp;
        }
    }
}
