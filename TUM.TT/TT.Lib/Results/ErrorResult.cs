//-----------------------------------------------------------------------
// <copyright file="ErrorResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Results
{
    using System;
    using Caliburn.Micro;

    /// <summary>
    /// Reports an error from the current coroutine.
    /// </summary>
    public class ErrorResult : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult"/> class.
        /// </summary>
        /// <param name="error">The error to report.</param>
        public ErrorResult(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            this.Error = error;
        }

        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        /// <summary>
        /// Gets the error to report.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context.</param>
        public void Execute(CoroutineExecutionContext context)
        {
            this.Completed(
                this,
                new ResultCompletionEventArgs() { Error = this.Error });
        }
    }
}
