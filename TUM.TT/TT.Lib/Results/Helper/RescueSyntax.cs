//-----------------------------------------------------------------------
// <copyright file="RescueSyntax.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Results.Helper
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;

    /// <summary>
    /// Syntax helper class for rescue results.
    /// </summary>
    /// <typeparam name="TException">The type of the exception to be caught</typeparam>
    public class RescueSyntax<TException>
        where TException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RescueSyntax{TException}"/> class.
        /// </summary>
        /// <param name="inner">The inner result</param>
        public RescueSyntax(IResult inner)
        {
            this.Inner = inner;
        }

        /// <summary>
        /// Gets the inner result.
        /// </summary>
        public IResult Inner { get; private set; }

        /// <summary>
        /// Rescues with a coroutine.
        /// </summary>
        /// <param name="handler">The exception handler.</param>
        /// <returns>The rescue action.</returns>
        public RescueResult<TException> With(Func<TException, IEnumerable<IResult>> handler)
        {
            return new RescueResult<TException>(this.Inner, handler);
        }

        /// <summary>
        /// Rescues with an error message.
        /// </summary>
        /// <param name="title">The title of the message.</param>
        /// <param name="message">The error message.</param>
        /// <returns>The rescue action.</returns>
        public RescueResult<TException> WithMessage(string title, string message)
        {
            return new RescueResult<TException>(
                this.Inner,
                error => new ErrorMessageResult()
                {
                    Title = title,
                    Message = string.Format("{0}\n\n{1}", message, error.Message),
                }.AsCoroutine());
        }
    }
}
