//-----------------------------------------------------------------------
// <copyright file="OpenFileDialogResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Results
{
    using Microsoft.Win32;

    /// <summary>
    /// A result to show a dialog to open a file.
    /// </summary>
    public class OpenFileDialogResult : FileDialogResult
    {
        /// <summary>
        /// Creates the dialog.
        /// </summary>
        /// <returns>The created dialog</returns>
        protected override FileDialog CreateDialog()
        {
            return new OpenFileDialog()
            {
                CheckFileExists = true,
                Multiselect = false,
            };
        }
    }
}
