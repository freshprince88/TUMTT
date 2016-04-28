//-----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Util
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Provides extensions for <c>enum</c> types.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description of an <c>enum</c> field.
        /// </summary>
        /// <typeparam name="T">The <c>enum</c> type.</typeparam>
        /// <param name="value">The <c>enum</c> value.</param>
        /// <returns>The description of the value.</returns>
        public static string GetDescription<T>(this T value)
            where T : struct
        {
            var type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("value must be if Enum type.");
            }

            var member = type.GetMember(value.ToString()).FirstOrDefault();
            if (member != null)
            {
                var attribute = member
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .Cast<DescriptionAttribute>()
                    .FirstOrDefault();
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            // Fall back to the standard string conversion
            return value.ToString();
        }
    }
}
