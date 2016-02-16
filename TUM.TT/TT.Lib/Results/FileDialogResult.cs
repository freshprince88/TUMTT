//-----------------------------------------------------------------------
// <copyright file="FileDialogResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Results
{
    using System;
    using System.Windows;
    using Caliburn.Micro;
    using Microsoft.Win32;

    /// <summary>
    /// Base class for file dialog results.
    /// </summary>
    public abstract class FileDialogResult : IResult<string>
    {
        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed;

        /// <summary>
        /// Gets or sets the default file name.
        /// </summary>
        public string DefaultFileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the dialog.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file name filter for the dialog.
        /// </summary>
        public string Filter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the dialog was canceled.
        /// </summary>
        public bool Cancelled
        {
            get { return this.Result == null; }
        }

        /// <summary>
        /// Gets the file name reported by the user, or <c>null</c> if the user cancelled the dialog.
        /// </summary>
        public string Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context</param>
        public void Execute(CoroutineExecutionContext context)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dialog = this.CreateDialog();
                dialog.Title = this.Title;
                dialog.Filter = this.Filter;
                dialog.FileName = this.DefaultFileName;
                var test1 = context.View as FrameworkElement;
                var test2 = Window.GetWindow(test1);
                this.Result = dialog.ShowDialog(test2) == true ?
                             dialog.FileName : null;

                var args = new ResultCompletionEventArgs()
                {
                    WasCancelled = this.Cancelled,
                    Error = null,
                };
                this.Completed(this, args);
            });
        }

        /// <summary>
        /// Creates the dialog.
        /// </summary>
        /// <returns>The created dialog</returns>
        protected abstract FileDialog CreateDialog();
    }
}
