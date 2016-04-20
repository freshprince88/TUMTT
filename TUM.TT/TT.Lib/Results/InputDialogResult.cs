using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Results
{
    public class InputDialogResult : IResult<string>
    {
        /// <summary>
        /// Gets a value indicating whether the user answered Yes.
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// Gets or sets the title of the question.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the question to ask.
        /// </summary>
        public string Question { get; set; }

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
            var result = await coordinator.ShowInputAsync(context.Target, this.Title, this.Question, mySettings);

            this.Result = result;
            var args = new ResultCompletionEventArgs()
            {
                WasCancelled = result == string.Empty || result == null
            };
            this.Completed(this, args);
        }
    }
}
