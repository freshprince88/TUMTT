//-----------------------------------------------------------------------
// <copyright file="SerializeMatchResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Results
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TT.Models;
    using TT.Models.Serialization;

    /// <summary>
    /// Serializes a match.
    /// </summary>
    public class SerializeMatchResult : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeMatchResult"/> class.
        /// </summary>
        /// <param name="match">The match to serialize.</param>
        /// <param name="fileName">The file name to serialize to.</param>
        /// <param name="serializer">The serializer to use.</param>
        public SerializeMatchResult(Match match, string fileName, IMatchSerializer serializer)
        {
            this.Match = match;
            this.Serializer = serializer;
            this.FileName = fileName;
        }

        /// <summary>
        /// Notifies about the completion of the action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        /// <summary>
        /// Gets the match to serialize.
        /// </summary>
        public Match Match { get; private set; }

        /// <summary>
        /// Gets the serializer to serialize the match.
        /// </summary>
        public IMatchSerializer Serializer { get; private set; }

        /// <summary>
        /// Gets the file name to serialize to.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context</param>
        public void Execute(CoroutineExecutionContext context)
        {
            Task.Run(() => this.SerializeMatch());
        }

        /// <summary>
        /// Serializes the match.
        /// </summary>
        private void SerializeMatch()
        {
            try
            {
                using (var sink = File.Create(this.FileName))
                {
                    this.Serializer.Serialize(sink, this.Match);
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
