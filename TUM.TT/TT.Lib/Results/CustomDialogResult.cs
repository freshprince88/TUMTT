using System;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Util;

namespace TT.Lib.Results
{
    public class CustomDialogResult<T> : IResult<T>
    {
        /// <summary>
        /// Gets a value indicating whether the user answered Yes.
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// Gets or sets the title of the question.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the question to ask.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the content of the dialog.
        /// </summary>
        public CustomClosableComboDialog<T> DialogContent { get; set; }

        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public async void Execute(CoroutineExecutionContext context)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                AnimateShow = true,
                AnimateHide = false                
            };
            IDialogCoordinator coordinator = IoC.Get<IDialogCoordinator>();
            DialogContent.CloseButton.Click += async (s, e) =>
            {
                Result = (T)DialogContent.Combo.SelectedItem;
                await coordinator.HideMetroDialogAsync(context.Target, DialogContent);

                var args = new ResultCompletionEventArgs()
                {
                    WasCancelled = Result == null
                };
                this.Completed(this, args);
            };

            DialogContent.Title = Title;
            
            await coordinator.ShowMetroDialogAsync(context.Target, DialogContent);
        }
    }
}
