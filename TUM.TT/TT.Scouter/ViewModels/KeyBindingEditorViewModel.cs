using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib;
using TT.Lib.Util;

namespace TT.Scouter.ViewModels
{
    public class KeyBindingEditorViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        private Key _currentKey;
        private KeyGesture _currentGesture;
        private string _selectedKeyBinding;

        public KeyBindingEditorViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Key Binding Editor";
            this.events = eventAggregator;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;

            
        }

        public IEnumerable<string> KeyBindings { get { return ShortcutFactory.Instance.KeyGestures.Select(pair => pair.Key); } }

        public string SelectedKeyBinding
        {
            get { return _selectedKeyBinding; }
            set
            {
                if (value == _selectedKeyBinding) return;
                _selectedKeyBinding = value;
                CurrentGesture = ShortcutFactory.Instance.GetKeyGestures(_selectedKeyBinding) as KeyGesture;
            }
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name) ? Application.Current.Windows.OfType<T>().Any() : Application.Current.Windows.OfType<T>().Any(wde => wde.Name.Equals(name));
        }

        public KeyGesture CurrentGesture
        {
            get { return _currentGesture; }
            set
            {
                if (value == _currentGesture) return;
                _currentGesture = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange("CurrentKeyGestureName");
            }
        }

        public string CurrentKeyGestureName
        {
            get
            {
                if (CurrentGesture == null) return String.Empty;
                return CurrentGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
            }
        }

        public void Reset()
        {
            if (string.IsNullOrEmpty(SelectedKeyBinding))
                return;

            KeyGesture initialGesture = null;
            if (ShortcutFactory.GetInitialGestures().ContainsKey(SelectedKeyBinding))
                initialGesture = ShortcutFactory.GetInitialGestures()[SelectedKeyBinding] as KeyGesture;
            
            CurrentGesture = initialGesture;
        }

        public void Accept()
        {
            if(string.IsNullOrEmpty(SelectedKeyBinding) || _currentGesture==null)
                return;

            ShortcutFactory.Instance.SetKeyGesture(SelectedKeyBinding, _currentGesture);
        }

        public void UpdateKey(ActionExecutionContext context)
        {
            var eventArgs = (KeyEventArgs) context.EventArgs;
            if (eventArgs.Key == Key.None || 
                eventArgs.Key == Key.LeftShift || 
                eventArgs.Key == Key.RightShift|| 
                eventArgs.Key == Key.LeftAlt || 
                eventArgs.Key == Key.RightAlt)
            {
                return;
            }

            KeyGesture keyGesture;
            try
            {
                keyGesture = new KeyGesture(eventArgs.Key == Key.System ? eventArgs.SystemKey : eventArgs.Key, eventArgs.KeyboardDevice.Modifiers);
            }
            catch (Exception ex)
            {
                return;
            }

            CurrentGesture = keyGesture;
            eventArgs.Handled = true;
        }

    }
}
