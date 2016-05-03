//-----------------------------------------------------------------------
// <copyright file="ErrorMessageResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Results
{
    using System;
    using System.Windows;
    using Caliburn.Micro;

    /// <summary>
    /// Shows an error message.
    /// </summary>
    public class ErrorMessageResult : IResult
    {
        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        /// <summary>
        /// Gets or sets the title of the error message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context.</param>
        public void Execute(CoroutineExecutionContext context)
        {
            Caliburn.Micro.Execute.BeginOnUIThread(() =>
            {
                MessageBox.Show(
                    Window.GetWindow(context.View as FrameworkElement),
                    this.Message,
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                this.Completed(this, new ResultCompletionEventArgs());
            });
        }
    }
}
