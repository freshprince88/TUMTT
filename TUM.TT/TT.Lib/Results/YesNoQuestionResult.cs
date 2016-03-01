namespace TT.Lib.Results
{
    using System;
    using System.Windows;
    using Caliburn.Micro;
    using MahApps.Metro.Controls.Dialogs;
    /// <summary>
    /// Asks a Yes/No question.
    /// </summary>
    public class YesNoQuestionResult : IResult<bool>
    {
        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        /// <summary>
        /// Gets or sets the title of the question.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the question to ask.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to cancel the action.
        /// </summary>
        public bool AllowCancel { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user answered Yes.
        /// </summary>
        public bool Result { get; private set; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context</param>
        public async void Execute(CoroutineExecutionContext context)
        {
            //var result = MessageBox.Show(
            //    this.Question,
            //    this.Title,
            //    this.AllowCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
            //    MessageBoxImage.Question,
            //    MessageBoxResult.Yes);

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Save and Quit",
                NegativeButtonText = "Cancel",
                FirstAuxiliaryButtonText = "Quit Without Saving",
                AnimateShow = true,
                AnimateHide = false
            };
            IDialogCoordinator coordinator = IoC.Get<IDialogCoordinator>();
            var result = await coordinator.ShowMessageAsync(context.Target, this.Title,
                this.Question,
                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

            this.Result = result == MessageDialogResult.Affirmative;
            var args = new ResultCompletionEventArgs()
            {
                WasCancelled = result == MessageDialogResult.Negative,
            };
            this.Completed(this, args);
        }
    }
}
