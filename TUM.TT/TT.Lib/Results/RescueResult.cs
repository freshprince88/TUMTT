//-----------------------------------------------------------------------
// <copyright file="RescueResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Caliburn.Micro;

    /// <summary>
    /// Catches errors in other actions.
    /// </summary>
    /// <typeparam name="TException">The type of exception to be caught</typeparam>
    public class RescueResult<TException> : IResult
        where TException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RescueResult{TException}"/> class.
        /// </summary>
        /// <param name="inner">The inner result</param>
        /// <param name="handler">The rescue coroutine.</param>
        public RescueResult(IResult inner, Func<TException, IEnumerable<IResult>> handler)
        {
            this.InnerResult = inner;
            this.Handler = handler;
        }

        /// <summary>
        /// Notifies about the completion of this result.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        /// <summary>
        /// Gets or sets the inner result.
        /// </summary>
        public IResult InnerResult { get; set; }

        /// <summary>
        /// Gets the exception handling coroutine.
        /// </summary>
        public Func<TException, IEnumerable<IResult>> Handler { get; private set; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context</param>
        public void Execute(CoroutineExecutionContext context)
        {
            Coroutine.BeginExecute(
                this.InnerResult.AsCoroutine().GetEnumerator(),
                context,
                (innerSender, innerArgs) =>
                {
                    var error = innerArgs.Error as TException;
                    if (error != null)
                    {
                        Coroutine.BeginExecute(
                            this.Handler(error).GetEnumerator(),
                            context,
                            (rescueSender, rescueArgs) =>
                            {
                                this.Completed(this, rescueArgs);
                            });
                    }
                    else
                    {
                        this.Completed(this, innerArgs);
                    }
                });
        }

        /// <summary>
        /// Propagate the error again after handling it.
        /// </summary>
        /// <returns>A new action to propagate the error after handling it.</returns>
        public RescueResult<TException> Propagate()
        {
            return new RescueResult<TException>(
                this.InnerResult,
                exc => this.Handler(exc).Concat(new ErrorResult(exc).AsCoroutine()));
        }
    }
}
