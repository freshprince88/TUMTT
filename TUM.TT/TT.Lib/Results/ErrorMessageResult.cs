//-----------------------------------------------------------------------
// <copyright file="ErrorMessageResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Results
{
    using System;
    using System.Linq;
    using System.Windows;
    using Caliburn.Micro;
    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// Shows an error message.
    /// </summary>
    public class ErrorMessageResult : IResult
    {
        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public IDialogCoordinator Dialogs { get; set; }

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
        public async void Execute(CoroutineExecutionContext context)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                AnimateShow = true,
                AnimateHide = false
            };
            var shell = (IoC.Get<IShell>() as Screen); //context.Target

            var curWindow=Application.Current;
            var result = await Dialogs.ShowMessageAsync(context.Target ?? shell, this.Title,
                this.Message,
                MessageDialogStyle.Affirmative, mySettings);

            var args = new ResultCompletionEventArgs();
            this.Completed(this, args);
        }
    }
}
