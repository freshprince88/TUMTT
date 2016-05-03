//-----------------------------------------------------------------------
// <copyright file="PDFGenerator.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013  Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TTA.Models.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>
    /// A generator for a <see cref="Report"/>
    /// </summary>
    public class PDFGenerator
    {
        /// <summary>
        /// The <see cref="Report"/> for which the PDF is generated.
        /// </summary>
        private Report report;

        /// <summary>
        /// The generated document
        /// </summary>
        private Document document = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PDFGenerator"/> class.
        /// </summary>
        /// <param name="report">The <see cref="Report"/> for which the PDF should be generated.</param>
        public PDFGenerator(Report report)
        {
            this.report = report;
        }

        /// <summary>
        /// Accesses the generated Document.
        /// </summary>
        /// <returns>The generated Document.</returns>
        public Document GetPDFDocument()
        {
            if (this.document == null)
            {
                this.GenerateDocument();
            }

            return this.document;
        }

        /// <summary>
        /// Generates the document.
        /// </summary>
        public void GenerateDocument()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Test.pdf";

            float margin = Utilities.MillimetersToPoints(Convert.ToSingle(10));
            Document doc = new Document(iTextSharp.text.PageSize.A4, margin, margin, margin, margin);
            doc.SetMargins(20, 20, 100, 0);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            writer.SetFullCompression();
            writer.CloseStream = true;

            doc.Open();
            doc.NewPage();

            Chapter chapter1 = new Chapter(new Paragraph(string.Empty), 0);
            chapter1.NumberDepth = 0;
            Section section1 = chapter1.AddSection(0f, "Basic Informations", 1);
            PdfPTable table1 = this.CreateBasicInfoTable();
            table1.SpacingAfter = 20.0f;
            table1.SpacingBefore = 15.0f;
            section1.Add(table1);

            Section section2 = chapter1.AddSection(0f, "Rally Length", 1);
            PdfPTable table2 = this.CreateRallyLengthTable();
            table2.SpacingAfter = 20.0f;
            table2.SpacingBefore = 15.0f;
            section2.Add(table2);

            Chapter chapter2 = new Chapter(new Paragraph(string.Empty), 0);
            chapter2.NumberDepth = 0;
            Section section3 = chapter2.AddSection(0f, "Transition Matrix", 1);
            List<PdfPTable> tables = this.CreateTransitionMatrixTables();
            foreach (PdfPTable table in tables)
            {
                table.SpacingAfter = 20.0f;
                table.SpacingBefore = 15.0f;
                section3.Add(table);
            }

            doc.Add(chapter1);
            doc.Add(chapter2);
            if (doc != null)
            {
                doc.Close();
            }

            doc = null;
            Process.Start(path);
        }

        /// <summary>
        /// Creates the table with the basic information.
        /// </summary>
        /// <returns>The created table.</returns>
        public PdfPTable CreateBasicInfoTable()
        {
            PdfPTable table = new PdfPTable(12);
            float[] widths = new float[] { 2.3f, 2.0f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 1.3f, 2.2f, 2.0f, 2.0f, 2.0f };
            Rectangle rect = new Rectangle(17, 1);
            table.SetWidthPercentage(widths, rect);

            BasicInformation playerAInfo = this.report.BasicInformations.First();
            BasicInformation playerBInfo = this.report.BasicInformations.Last();

            //// Hdeaer row.
            PdfPCell cell1 = GetCell("Player", 1, 1, 1);
            cell1.MinimumHeight = 30.0f;
            table.AddCell(cell1);
            table.AddCell(GetCell("Ranking", 1, 1, 2));
            table.AddCell(GetCell("Result\n(" + Convert.ToString(playerAInfo.WinningSets) + ":" + Convert.ToString(playerBInfo.WinningSets) + ")", 5, 1, 2));
            table.AddCell(GetCell("Total\nPoints", 1, 1, 2));
            table.AddCell(GetCell("Comp.\nPerf.", 1, 1, 2));
            table.AddCell(GetCell("Win.\nProb.", 1, 1, 2));
            table.AddCell(GetCell("Service\nFrequency", 1, 1, 2));
            table.AddCell(GetCell("Service\nWin.Prob.", 1, 1, 3));

            List<int> resultsA = playerAInfo.Results;
            //// Inner middle row.
            PdfPCell cell2 = GetCell(playerAInfo.Name, 4);
            cell2.MinimumHeight = 20.0f;
            table.AddCell(cell2);
            table.AddCell(GetCell("Nan", 5));

            //// results cells
            foreach (int res in resultsA)
            {
                table.AddCell(GetCell(Convert.ToString(res), 5));
            }

            for (int i = resultsA.Count; i < 5; i++)
            {
                table.AddCell(GetCell(string.Empty, 5));
            }

            table.AddCell(GetCell(Convert.ToString(playerAInfo.TotalPoints), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(playerAInfo.CompetitionPerformance, 1)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(playerAInfo.WinningProbability * 100, 1)), 5));
            table.AddCell(GetCell(Convert.ToString(playerAInfo.ServingFrequency), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(playerAInfo.ServingWinningProbability * 100, 1)), 6));

            List<int> resultsB = playerBInfo.Results;
            //// Inner middle row.
            PdfPCell cell3 = GetCell(playerBInfo.Name, 7);
            cell3.MinimumHeight = 20.0f;
            table.AddCell(cell3);
            table.AddCell(GetCell("Nan", 8));

            //// results cells
            foreach (int res in resultsB)
            {
                table.AddCell(GetCell(Convert.ToString(res), 8));
            }

            for (int i = resultsB.Count; i < 5; i++)
            {
                table.AddCell(GetCell(string.Empty, 8));
            }

            table.AddCell(GetCell(Convert.ToString(playerBInfo.TotalPoints), 8));
            table.AddCell(GetCell(Convert.ToString(Math.Round(playerBInfo.CompetitionPerformance, 1)), 8));
            table.AddCell(GetCell(Convert.ToString(Math.Round(playerBInfo.WinningProbability * 100, 1)), 8));
            table.AddCell(GetCell(Convert.ToString(playerAInfo.ServingFrequency), 8));
            table.AddCell(GetCell(Convert.ToString(Math.Round(playerBInfo.ServingWinningProbability * 100, 1)), 9));

            return table;
        }

        /// <summary>
        /// Creates the table with the basic information.
        /// </summary>
        /// <returns>The created table.</returns>
        public PdfPTable CreateRallyLengthTable()
        {
            PdfPTable table = new PdfPTable(10);

            float[] widths = new float[] { 1.8f, 2.3f, 1.1f, 1.1f, 1.1f, 1.1f, 2.1f, 2.1f, 2.1f, 2.1f };
            Rectangle rect = new Rectangle(17, 1);
            table.SetWidthPercentage(widths, rect);

            PdfPCell cell1 = GetCell(string.Empty, 1, 1, 1);
            cell1.MinimumHeight = 30.0f;
            table.AddCell(cell1);
            table.AddCell(GetCell("Total Rally Length", 1, 1, 2));
            table.AddCell(GetCell("Serv A", 1, 1, 2));
            table.AddCell(GetCell("Serv B", 1, 1, 2));
            table.AddCell(GetCell("Win A", 1, 1, 2));
            table.AddCell(GetCell("Win B", 1, 1, 2));
            table.AddCell(GetCell("Service A\nWinner A", 1, 1, 2));
            table.AddCell(GetCell("Service A\nWinner B", 1, 1, 2));
            table.AddCell(GetCell("Service B\nWinner A", 1, 1, 2));
            table.AddCell(GetCell("Service B\nWinner B", 1, 1, 3));

            RallyLengthStatistics stats = this.report.RallyLength;

            PdfPCell cell2 = GetCell("Mean", 4);
            cell2.MinimumHeight = 20.0f;
            table.AddCell(cell2);
            table.AddCell(GetCell(Convert.ToString(0), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.ServiceA.Mean, 2)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.ServiceB.Mean, 2)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.WinnerA.Mean, 2)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.WinnerB.Mean, 2)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.WinnerAWithServiceA.Mean, 2)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.WinnerBWithServiceA.Mean, 2)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.WinnerAWithServiceB.Mean, 2)), 5));
            table.AddCell(GetCell(Convert.ToString(Math.Round(stats.WinnerBWithServiceB.Mean, 2)), 5));

            PdfPCell cell3 = GetCell("Median", 4);
            cell3.MinimumHeight = 20.0f;
            table.AddCell(cell3);
            table.AddCell(GetCell(Convert.ToString(0), 5));
            table.AddCell(GetCell(Convert.ToString(stats.ServiceA.Median), 5));
            table.AddCell(GetCell(Convert.ToString(stats.ServiceB.Median), 5));
            table.AddCell(GetCell(Convert.ToString(stats.WinnerA.Median), 5));
            table.AddCell(GetCell(Convert.ToString(stats.WinnerB.Median), 5));
            table.AddCell(GetCell(Convert.ToString(stats.WinnerAWithServiceA.Median), 5));
            table.AddCell(GetCell(Convert.ToString(stats.WinnerBWithServiceA.Median), 5));
            table.AddCell(GetCell(Convert.ToString(stats.WinnerAWithServiceB.Median), 5));
            table.AddCell(GetCell(Convert.ToString(stats.WinnerBWithServiceB.Median), 5));

            return table;
        }

        /// <summary>
        /// Creates the tables with the transition matrixes.
        /// </summary>
        /// <returns>The created tables.</returns>
        public List<PdfPTable> CreateTransitionMatrixTables()
        {
            var list = new List<PdfPTable>();

            var stringOne = "A";
            var stringTwo = "B";

            foreach (var transition in this.report.Transitions)
            {
                PdfPTable table = new PdfPTable(9);

                float[] widths = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
                Rectangle rect = new Rectangle(17, 1);
                table.SetWidthPercentage(widths, rect);

                PdfPCell cell1 = GetCell(stringOne + "->" + stringTwo, 1, 1, 1);
                cell1.MinimumHeight = 20.0f;
                table.AddCell(cell1);
                table.AddCell(GetCell(stringTwo + "2", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "3", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "4", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "5", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "6", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + ">6", 1, 1, 2));
                table.AddCell(GetCell("P" + stringOne, 1, 1, 2));
                table.AddCell(GetCell("P" + stringTwo, 1, 1, 3));

                for (int i = 0; i < transition.Transitions.Count() - 1; i++)
                {
                    PdfPCell cell = GetCell(stringOne + Convert.ToString(i + 1), 1, 1, 4);
                    cell1.MinimumHeight = 20.0f;
                    table.AddCell(cell);
                    for (int j = 0; j < i; j++)
                    {
                        table.AddCell(GetCell(string.Empty, 1, 1, 5));
                    }

                    table.AddCell(GetCell(Convert.ToString(transition.Transitions.ElementAt(i)), 1, 1, 5));
                    for (int j = i + 1; j < 6; j++)
                    {
                        table.AddCell(GetCell(string.Empty, 1, 1, 5));
                    }

                    table.AddCell(GetCell(Convert.ToString(transition.PointsTransitionServer.ElementAt(i)), 1, 1, 5));
                    table.AddCell(GetCell(Convert.ToString(transition.PointsTransitionReceiver.ElementAt(i)), 1, 1, 6));
                }

                PdfPCell cell2 = GetCell(stringOne + ">6", 1, 1, 7);
                cell1.MinimumHeight = 20.0f;
                table.AddCell(cell2);
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(Convert.ToString(transition.Transitions.ElementAt(transition.Transitions.Count() - 1)), 1, 1, 8));
                table.AddCell(GetCell(Convert.ToString(transition.PointsTransitionServer.ElementAt(transition.Transitions.Count() - 1)), 1, 1, 5));
                table.AddCell(GetCell(Convert.ToString(transition.PointsTransitionReceiver.ElementAt(transition.Transitions.Count() - 1)), 1, 1, 6));
                list.Add(table);

                var stringTemp = stringOne;
                stringOne = stringTwo;
                stringTwo = stringTemp;
            }

            foreach (var transition in this.report.Transitions)
            {
                PdfPTable table = new PdfPTable(9);

                float[] widths = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
                Rectangle rect = new Rectangle(17, 1);
                table.SetWidthPercentage(widths, rect);

                PdfPCell cell1 = GetCell(stringOne + "->" + stringTwo, 1, 1, 1);
                cell1.MinimumHeight = 20.0f;
                table.AddCell(cell1);
                table.AddCell(GetCell(stringTwo + "2", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "3", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "4", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "5", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + "6", 1, 1, 2));
                table.AddCell(GetCell(stringTwo + ">6", 1, 1, 2));
                table.AddCell(GetCell("P" + stringOne, 1, 1, 2));
                table.AddCell(GetCell("P" + stringTwo, 1, 1, 3));

                for (int i = 0; i < transition.Transitions.Count() - 1; i++)
                {
                    PdfPCell cell = GetCell(stringOne + Convert.ToString(i + 1), 1, 1, 4);
                    cell1.MinimumHeight = 20.0f;
                    table.AddCell(cell);
                    for (int j = 0; j < i; j++)
                    {
                        table.AddCell(GetCell(string.Empty, 1, 1, 5));
                    }

                    table.AddCell(GetCell(Convert.ToString(Math.Round(transition.TransitionProbabilities.ElementAt(i) * 100, 1)), 1, 1, 5));
                    for (int j = i + 1; j < 6; j++)
                    {
                        table.AddCell(GetCell(string.Empty, 1, 1, 5));
                    }

                    table.AddCell(GetCell(Convert.ToString(Math.Round(transition.PointsPropabilitiesServer.ElementAt(i) * 100, 1)), 1, 1, 5));
                    table.AddCell(GetCell(Convert.ToString(Math.Round(transition.PointsPropabilitiesReceiver.ElementAt(i) * 100, 1)), 1, 1, 6));
                }

                PdfPCell cell2 = GetCell(stringOne + ">6", 1, 1, 7);
                cell1.MinimumHeight = 20.0f;
                table.AddCell(cell2);
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(string.Empty, 1, 1, 8));
                table.AddCell(GetCell(Convert.ToString(Math.Round(transition.TransitionProbabilities.ElementAt(transition.Transitions.Count() - 1) * 100, 1)), 1, 1, 8));
                table.AddCell(GetCell(Convert.ToString(Math.Round(transition.PointsPropabilitiesServer.ElementAt(transition.Transitions.Count() - 1) * 100, 1)), 1, 1, 5));
                table.AddCell(GetCell(Convert.ToString(Math.Round(transition.PointsPropabilitiesReceiver.ElementAt(transition.Transitions.Count() - 1) * 100, 1)), 1, 1, 6));
                list.Add(table);

                var stringTemp = stringOne;
                stringOne = stringTwo;
                stringTwo = stringTemp;
            }

            return list;
        }

        /// <summary>
        /// Creates a Cell
        /// </summary>
        /// <param name="text">The text content.</param>
        /// <param name="pos">The position.</param>
        /// <returns>The created Cell.</returns>
        private static PdfPCell GetCell(string text, int pos)
        {
            return GetCell(text, 1, 1, pos);
        }

        /// <summary>
        /// Creates a cell with detailed information.
        /// </summary>
        /// <param name="text">The text content.</param>
        /// <param name="colSpan">Indicates the col span.</param>
        /// <param name="rowSpan">Indicates the row span.</param>
        /// <param name="pos">The position.</param>
        /// <returns>The generated Cell</returns>
        private static PdfPCell GetCell(string text, int colSpan, int rowSpan, int pos)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));
            cell.HorizontalAlignment = 1;
            cell.Rowspan = rowSpan;
            cell.Colspan = colSpan;
            cell.BorderColor = Color.BLUE;

            float bigBorder = 1;
            float smalBorder = 0.5f;
            float noBorder = 0;

            switch (pos)
            {
                case 1:
                    cell.BorderWidthLeft = bigBorder;
                    cell.BorderWidthTop = bigBorder;
                    cell.BorderWidthRight = smalBorder;
                    cell.BorderWidthBottom = smalBorder;
                    break;
                case 2:
                    cell.BorderWidthLeft = noBorder;
                    cell.BorderWidthTop = bigBorder;
                    cell.BorderWidthRight = smalBorder;
                    cell.BorderWidthBottom = smalBorder;
                    break;
                case 3:
                    cell.BorderWidthLeft = noBorder;
                    cell.BorderWidthTop = bigBorder;
                    cell.BorderWidthRight = bigBorder;
                    cell.BorderWidthBottom = smalBorder;
                    break;
                case 4:
                    cell.BorderWidthLeft = bigBorder;
                    cell.BorderWidthTop = noBorder;
                    cell.BorderWidthRight = smalBorder;
                    cell.BorderWidthBottom = smalBorder;
                    break;
                case 5:
                    cell.BorderWidthLeft = noBorder;
                    cell.BorderWidthTop = noBorder;
                    cell.BorderWidthRight = smalBorder;
                    cell.BorderWidthBottom = smalBorder;
                    break;
                case 6:
                    cell.BorderWidthLeft = noBorder;
                    cell.BorderWidthTop = noBorder;
                    cell.BorderWidthRight = bigBorder;
                    cell.BorderWidthBottom = smalBorder;
                    break;
                case 7:
                    cell.BorderWidthLeft = bigBorder;
                    cell.BorderWidthTop = noBorder;
                    cell.BorderWidthRight = smalBorder;
                    cell.BorderWidthBottom = bigBorder;
                    break;
                case 8:
                    cell.BorderWidthLeft = noBorder;
                    cell.BorderWidthTop = noBorder;
                    cell.BorderWidthRight = smalBorder;
                    cell.BorderWidthBottom = bigBorder;
                    break;
                case 9:
                    cell.BorderWidthLeft = noBorder;
                    cell.BorderWidthTop = noBorder;
                    cell.BorderWidthRight = bigBorder;
                    cell.BorderWidthBottom = bigBorder;
                    break;
                default:
                    cell.BorderWidth = smalBorder;
                    break;
            }

            return cell;
        }
    }
}
