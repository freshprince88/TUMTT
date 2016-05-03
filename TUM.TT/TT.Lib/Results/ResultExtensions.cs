//-----------------------------------------------------------------------
// <copyright file="ResultExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Results
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using TT.Lib.Results.Helper;

    /// <summary>
    /// Extensions to <see cref="IResult"/>
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Transforms a single action into a coroutine.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A coroutine executing the action.</returns>
        public static IEnumerable<IResult> AsCoroutine(this IResult action)
        {
            yield return action;
        }

        /// <summary>
        /// Handles a specific error in an action.
        /// </summary>
        /// <typeparam name="TException">The type of error to handle.</typeparam>
        /// <param name="action">The action whose errors to handle.</param>
        /// <returns>The exception handler.</returns>
        public static RescueSyntax<TException> Rescue<TException>(this IResult action)
            where TException : Exception
        {
            return new RescueSyntax<TException>(action);
        }

        /// <summary>
        /// Handles all errors in an action.
        /// </summary>
        /// <param name="action">The action whose errors to handle.</param>
        /// <returns>The exception handler.</returns>
        public static RescueSyntax<Exception> Rescue(this IResult action)
        {
            return action.Rescue<Exception>();
        }

        /// <summary>
        /// Marks the application as busy while executing the given action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="message">A message informing the user about the action.</param>
        /// <returns>The wrapped action.</returns>
        public static BusyIndicatorResult IsBusy(this IResult action, object message)
        {
            return new BusyIndicatorResult(action)
            {
                Message = message
            };
        }
    }
}
