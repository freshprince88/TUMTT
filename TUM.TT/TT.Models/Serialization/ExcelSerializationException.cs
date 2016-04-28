//-----------------------------------------------------------------------
// <copyright file="ExcelSerializationException.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Serialization
{
    using System;

    /// <summary>
    /// Indicates an error in Excel serialization.
    /// </summary>
    public class ExcelSerializationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelSerializationException"/> class.
        /// </summary>
        /// <param name="message">The exception message</param>
        public ExcelSerializationException(string message)
            : base(message)
        {
        }
    }
}
