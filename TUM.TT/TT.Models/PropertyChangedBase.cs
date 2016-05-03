//-----------------------------------------------------------------------
// <copyright file="PropertyChangedBase.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Base class for models that notify about changed properties.
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Notifies about a changed property.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets a property field and notifies about a changed property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="backingField">The backing field to set.</param>
        /// <param name="value">The value to assign to <paramref name="backingField"/>.</param>
        /// <param name="property">The name of the property, defaulting to the name of the calling property.</param>
        /// <returns>Whether the property was changed or not.</returns>
        protected bool RaiseAndSetIfChanged<T>(ref T backingField, T value, [CallerMemberName] string property = null)
        {
            if (!object.Equals(backingField, value))
            {
                backingField = value;
                this.NotifyPropertyChanged(property);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Notifies about a changed property.
        /// </summary>
        /// <param name="property">The name of the changed property, defaulting to the name of the calling property.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string property = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
