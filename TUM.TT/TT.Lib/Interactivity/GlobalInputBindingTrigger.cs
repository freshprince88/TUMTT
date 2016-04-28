//-----------------------------------------------------------------------
// <copyright file="GlobalingTrigger.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    /// Trigger based on input bindings.
    /// </summary>
    public class GlobalInputBindingTrigger : TriggerBase<FrameworkElement>, ICommand
    {
        /// <summary>
        /// Identifies the <see cref="InputBinding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InputBindingProperty = DependencyProperty.Register(
            "InputBinding",
            typeof(InputBinding),
            typeof(GlobalInputBindingTrigger),
            new UIPropertyMetadata(null));

        // As CanExecute is constant, we do not need this event, and hence can safely
        // ignore the warning about the event not being used anywhere

        /// <summary>
        /// Notifies about whether the command can be executed or not.
        /// </summary>
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        /// <summary>
        /// Gets or sets the input binding handled by this trigger.
        /// </summary>
        public InputBinding InputBinding
        {
            get { return (InputBinding)this.GetValue(InputBindingProperty); }
            set { this.SetValue(InputBindingProperty, value); }
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>Whether the command can be executed.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The command parameter/</param>
        public void Execute(object parameter)
        {
            this.InvokeActions(parameter);
        }

        /// <summary>
        /// Invoked when the trigger is attached to an object.
        /// </summary>
        protected override void OnAttached()
        {
            if (this.InputBinding != null)
            {
                // Make the input binding call back to the trigger.
                this.InputBinding.Command = this;

                Window window = null;

                this.AssociatedObject.Loaded += (sender, args) =>
                {
                    window = Window.GetWindow(this.AssociatedObject);
                    window.InputBindings.Add(this.InputBinding);
                };

                this.AssociatedObject.Unloaded += (sender, args) =>
                {
                    window.InputBindings.Remove(this.InputBinding);
                };
            }

            base.OnAttached();
        }
    }
}
