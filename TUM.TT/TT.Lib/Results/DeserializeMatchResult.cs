//-----------------------------------------------------------------------
// <copyright file="DeserializeMatchResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Results
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TT.Models;
    using TT.Models.Serialization;

    /// <summary>
    /// Deserializes a match.
    /// </summary>
    public class DeserializeMatchResult : IResult<Match>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeserializeMatchResult"/> class.
        /// </summary>
        /// <param name="fileName">The file name to deserialize from.</param>
        /// <param name="serializer">The serializer to use.</param>
        public DeserializeMatchResult(string fileName, IMatchSerializer serializer)
        {
            this.FileName = fileName;
            this.Serializer = serializer;
        }

        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed;

        /// <summary>
        /// Gets the name of the file to deserialize from.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the serialize to use.
        /// </summary>
        public IMatchSerializer Serializer { get; private set; }

        /// <summary>
        /// Gets the deserialized match.
        /// </summary>
        public Match Result { get; private set; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context</param>
        public void Execute(CoroutineExecutionContext context)
        {
            Task.Run(() => this.DeserializeMatch());
        }

        /// <summary>
        /// Deserializes the match.
        /// </summary>
        private void DeserializeMatch()
        {
            try
            {
                using (var source = File.OpenRead(this.FileName))
                {
                    this.Result = this.Serializer.Deserialize(source);
                }

                this.Completed(this, new ResultCompletionEventArgs());
            }
            catch (Exception exc)
            {
                var args = new ResultCompletionEventArgs()
                {
                    Error = exc
                };
                this.Completed(this, args);
            }
        }
    }
}
