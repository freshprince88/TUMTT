//-----------------------------------------------------------------------
// <copyright file="SelectAllTextOnFocusBehavior.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Interactivity
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    /// Selects all text in a text box if it gains focus.
    /// </summary>
    public class SelectAllTextOnFocusBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// Attaches to the associated object.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.GotKeyboardFocus += this.OnGotKeyboardFocus;
            this.AssociatedObject.GotMouseCapture += this.OnGotMouseCapture;
            this.AssociatedObject.GotTouchCapture += this.OnGotTouchCapture;
        }

        /// <summary>
        /// Detaches from the associated object.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.GotKeyboardFocus -= this.OnGotKeyboardFocus;
            this.AssociatedObject.GotMouseCapture -= this.OnGotMouseCapture;
            this.AssociatedObject.GotTouchCapture -= this.OnGotTouchCapture;
        }

        /// <summary>
        /// Handles keyboard focus retrieval in the associated object.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args)
        {
            this.AssociatedObject.SelectAll();
        }

        /// <summary>
        /// Handles mouse capture in the associated object.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnGotMouseCapture(object sender, MouseEventArgs args)
        {
            this.AssociatedObject.SelectAll();
        }

        /// <summary>
        /// Handles touch capture in the associated object.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnGotTouchCapture(object sender, TouchEventArgs args)
        {
            this.AssociatedObject.SelectAll();
        }
    }
}
