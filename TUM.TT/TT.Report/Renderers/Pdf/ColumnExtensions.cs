//-----------------------------------------------------------------------
// <copyright file="ColumnExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Renderers.Pdf
{
    using MigraDoc.DocumentObjectModel.Tables;

    /// <summary>
    /// Extensions for <see cref="Table"/>.
    /// </summary>
    internal static class ColumnExtensions
    {
        /// <summary>
        /// Set the contents of a column in a table.
        /// </summary>
        /// <param name="column">The column whose contents to set.</param>
        /// <param name="cells">The cells to add.</param>
        public static void SetCells(this Column column, params string[] cells)
        {
            for (int i = 0; i < cells.Length; ++i)
            {
                column.Table.Rows[i].Cells[column.Index].AddParagraph(cells[i]);
            }
        }
    }
}
