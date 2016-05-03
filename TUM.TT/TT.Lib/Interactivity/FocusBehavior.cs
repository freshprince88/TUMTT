//-----------------------------------------------------------------------
// <copyright file="FocusBehavior.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Interactivity
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// Control the focus of a view element.
    /// </summary>
    public class FocusBehavior : Behavior<Control>
    {
        /// <summary>
        /// Identifies the <see cref="HasInitialFocus"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasInitialFocusProperty = DependencyProperty.Register(
            "HasInitialFocus",
            typeof(bool),
            typeof(FocusBehavior),
            new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the attached object has the initial focus.
        /// </summary>
        public bool HasInitialFocus
        {
            get { return (bool)this.GetValue(HasInitialFocusProperty); }
            set { this.SetValue(HasInitialFocusProperty, value); }
        }

        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register(
                "IsFocused",
                typeof(bool),
                typeof(FocusBehavior),
                new PropertyMetadata(false, (d, e) => { if ((bool)e.NewValue) ((FocusBehavior)d).AssociatedObject.Focus(); }));

        public bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        /// <summary>
        /// Handles attaching of the behavior to the control.
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += (sender, args) =>
            {
                if (this.HasInitialFocus)
                {
                    this.AssociatedObject.Focus();
                }
            };

            base.OnAttached();
        }
    }
}
