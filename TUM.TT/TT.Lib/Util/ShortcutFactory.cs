using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Interactivity;
using Microsoft.Win32;

namespace TT.Lib.Util
{
    public class ShortcutFactory
    {
        private static ShortcutFactory _instance;
        private readonly string _regKey;

        private readonly Dictionary<string, InputGesture> _keyGestures;
        public Dictionary<string, InputGesture> KeyGestures
        {
            get { return new Dictionary<string, InputGesture>(_keyGestures); }
        }
        
        private ShortcutFactory()
        {
            _regKey = Registry.CurrentUser + "\\SOFTWARE\\TUM.TT\\TT.VIEWER";
            _keyGestures = GetInitialGestures();

            UpdateSavedShortcuts();

        }

        public static ShortcutFactory Instance
        {
            get { return _instance ?? (_instance = new ShortcutFactory()); }
        }
        
        #region Initial Gestures

        public static Dictionary<string, InputGesture> GetInitialGestures()
        {
            var dictionary = new Dictionary<string, InputGesture>
            {
                {"PreviousRally", new KeyGesture(Key.MediaPreviousTrack, ModifierKeys.None)},
                {"StartRallyAtBeginning", new KeyGesture(Key.MediaPreviousTrack, ModifierKeys.Control)},
                {"Previous5Frames", new KeyGesture(Key.MediaPreviousTrack, ModifierKeys.Shift)},
                {"PreviousFrame", new KeyGesture(Key.MediaPreviousTrack, ModifierKeys.Alt)},
                {"PlayPause", new KeyGesture(Key.MediaPlayPause, ModifierKeys.None)},
                {"NextFrame", new KeyGesture(Key.MediaNextTrack, ModifierKeys.Alt)},
                {"Next5Frames", new KeyGesture(Key.MediaNextTrack, ModifierKeys.Shift)},
                {"NextRally", new KeyGesture(Key.MediaNextTrack, ModifierKeys.None)},
                {"FullscreenHelper", new KeyGesture(Key.F, ModifierKeys.Alt)},
                {"PlayModeHelper", new KeyGesture(Key.R, ModifierKeys.Alt)},
                {"SelectForehand", new KeyGesture(Key.V, ModifierKeys.Alt)},
                {"SelectBackhand", new KeyGesture(Key.B, ModifierKeys.Alt)},
                {"NextStroke", new KeyGesture(Key.Right, ModifierKeys.Alt)},
                {"PreviousStroke", new KeyGesture(Key.Left, ModifierKeys.Alt)}

            };
            return dictionary;
        }

        #endregion

        #region Key Gestures

        public InputGesture GetKeyGestures(string commandName)
        {
            if (_keyGestures.ContainsKey(commandName))
                return _keyGestures[commandName];

            return null;
        }

        public void SetKeyGesture(string commandName, InputGesture gesture)
        {
            if (_keyGestures.ContainsKey(commandName) && Equals(_keyGestures[commandName], gesture))
                return;

            _keyGestures[commandName] = gesture;
            UpdateRegistry(commandName);
        }

        #endregion

        #region Registry

        public void UpdateRegistry(string commandName)
        {
            var keyGesture = _keyGestures[commandName] as KeyGesture;
            if (keyGesture == null)
                return;

            var entry = (int) keyGesture.Key + ";" + (int) keyGesture.Modifiers;
            Registry.SetValue(_regKey, commandName, entry);
        }

        public void UpdateSavedShortcuts()
        {
            /*Viewer_MediaView_Play
                     Key=876;Modifier=86576
                     * 876;86576
                     */

            foreach (var entry in _keyGestures.Keys.ToList())
            {
                var savedEntry = Registry.GetValue(_regKey, entry, null) as string;
                if (savedEntry != null)
                {
                    var values = savedEntry.Split(new[] {';'});
                    var key = Convert.ToInt32(values[0]);
                    var modifiers = Convert.ToInt32(values[1]);

                    var keyEnum = (Key) key;
                    var modifiersEnum = (ModifierKeys)modifiers;

                    _keyGestures[entry] = new KeyGesture(keyEnum, modifiersEnum);
                }
            }
        }

        #endregion
    }
}