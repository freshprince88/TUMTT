using Microsoft.Win32;

namespace TT.Lib.Results
{
    /// <summary>
    /// A result to show a dialog to save a file.
    /// </summary>
    public class ExportPlaylistDialogResult : FileDialogResult
    {
        /// <summary>
        /// Creates the dialog.
        /// </summary>
        /// <returns>The dialog</returns>
        protected override FileDialog CreateDialog()
        {
            return new SaveFileDialog()
            {
                OverwritePrompt = true,
                AddExtension = true,
            };
        }
    }
}
