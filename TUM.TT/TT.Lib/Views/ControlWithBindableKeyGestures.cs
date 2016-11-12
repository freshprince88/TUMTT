using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using Caliburn.Micro;
using TT.Lib.Interactivity;

namespace TT.Lib.Views
{
    /// <summary>
    /// This overwrites UserControl to enable bindable KeyBindings created externally
    /// </summary>
    public class ControlWithBindableKeyGestures : UserControl
    {
        protected static readonly DependencyProperty
            BindableKeyGesturesProperty = DependencyProperty.Register(
                "BindableKeyGestures", typeof(Dictionary<string, KeyGesture>),
                typeof(ControlWithBindableKeyGestures), new FrameworkPropertyMetadata(null, OnBindableKeyGesturesChanged)
            );

        public Dictionary<string, KeyGesture> BindableKeyGestures
        {
            get { return (Dictionary<string, KeyGesture>)GetValue(BindableKeyGesturesProperty); }
            set { SetValue(BindableKeyGesturesProperty, value); }
        }

        private static void
            OnBindableKeyGesturesChanged(DependencyObject d,
                DependencyPropertyChangedEventArgs e)
        {
            var userControl = d as ControlWithBindableKeyGestures;

            if (userControl==null || userControl.BindableKeyGestures == null || !userControl.BindableKeyGestures.Any())
                return;

            //Set key bindings
            foreach (var keyGesture in userControl.BindableKeyGestures)
            {
                //method to be called
                var actionMessage = new ActionMessage { MethodName = keyGesture.Key };

                //key gesture that triggers binding
                var command = new GlobalInputBindingTrigger();
                command.Actions.Add(actionMessage);
                command.InputBinding = new KeyBinding(new RoutedCommand(), keyGesture.Value);

                //set
                var triggers = Interaction.GetTriggers(userControl);
                triggers.Add(command);
            }
           
        }
    }

}