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

        private readonly Dictionary<string, KeyBinding> _keyGestures;
        public Dictionary<string, KeyBinding> KeyGestures
        {
            get { return new Dictionary<string, KeyBinding>(_keyGestures); }
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

        public static Dictionary<string, KeyBinding> GetInitialGestures()
        {
            var dictionary = new Dictionary<string, KeyBinding>
            {
                //{"PreviousRally", new KeyBinding() {Key =){Key = Key.F, Modifiers= ModifierKeys.None}},

                {"PreviousRally", new KeyBinding() {Key =Key.A, Modifiers = ModifierKeys.Shift }},
                {"NextRally", new KeyBinding() {Key =Key.D, Modifiers = ModifierKeys.Shift}},
                {"PreviousStroke", new KeyBinding() {Key =Key.A, Modifiers = ModifierKeys.None}},
                {"NextStroke", new KeyBinding() {Key =Key.D, Modifiers = ModifierKeys.None}},
                
                {"StartRallyAtBeginning", new KeyBinding() {Key =Key.MediaPreviousTrack, Modifiers = ModifierKeys.Alt | ModifierKeys.Control  } },
                {"Previous5Frames", new KeyBinding() {Key =Key.MediaPreviousTrack,Modifiers =  ModifierKeys.Shift}},
                {"PreviousFrame", new KeyBinding() {Key =Key.MediaPreviousTrack, Modifiers = ModifierKeys.Alt}},
                {"PlayPause", new KeyBinding() {Key =Key.S, Modifiers = ModifierKeys.None}},
                {"NextFrame", new KeyBinding() {Key =Key.MediaNextTrack, Modifiers = ModifierKeys.Alt}},
                {"Next5Frames", new KeyBinding() {Key =Key.MediaNextTrack, Modifiers = ModifierKeys.Shift}},
                
                //{"NextRally", new KeyBinding() {Key =Key.Right, Modifiers = ModifierKeys.Alt | ModifierKeys.Control }},
                {"FullscreenHelper", new KeyBinding() {Key =Key.F, Modifiers = ModifierKeys.Alt}},
                {"PlayModeHelper", new KeyBinding() {Key =Key.R, Modifiers = ModifierKeys.Alt}},

                {"SelectForehand", new KeyBinding() {Key =Key.B, Modifiers = ModifierKeys.None}},
                {"SelectBackhand", new KeyBinding() {Key =Key.N, Modifiers = ModifierKeys.None}},
                {"SelectStepAround", new KeyBinding() {Key =Key.M, Modifiers = ModifierKeys.None}},


                

                // only Technique (Modifier = ALT)

                {"SelectPush", new KeyBinding() {Key =Key.G, Modifiers = ModifierKeys.Alt}},
                {"SelectPushAggressive", new KeyBinding() {Key =Key.T, Modifiers = ModifierKeys.Alt}},
                {"SelectFlip", new KeyBinding() {Key =Key.H, Modifiers = ModifierKeys.Alt}},
                {"SelectBanana", new KeyBinding() {Key =Key.Z, Modifiers = ModifierKeys.Alt}},
                {"SelectTopspin", new KeyBinding() {Key =Key.J, Modifiers = ModifierKeys.Alt}},
                {"SelectBlock", new KeyBinding() {Key =Key.K, Modifiers = ModifierKeys.Alt}},
                
                {"SelectSmash", new KeyBinding() {Key =Key.L, Modifiers = ModifierKeys.Alt}},
                {"SelectChop", new KeyBinding() {Key =Key.O, Modifiers = ModifierKeys.Alt}},
                {"SelectLob", new KeyBinding() {Key =Key.Oem3, Modifiers = ModifierKeys.Alt}},
                {"SelectCounter", new KeyBinding() {Key =Key.P, Modifiers = ModifierKeys.Alt}},
                {"SelectSpecial", new KeyBinding() {}},

                {"SelectPendulum", new KeyBinding() {Key =Key.G, Modifiers = ModifierKeys.Alt}},
                {"SelectReverse", new KeyBinding() {Key =Key.H, Modifiers = ModifierKeys.Alt}},
                {"SelectTomahawk", new KeyBinding() {Key =Key.J, Modifiers = ModifierKeys.Alt}},
                {"SelectSpecialServe", new KeyBinding() {Key =Key.K, Modifiers = ModifierKeys.Alt}},

                // Technique Options (Modifier = NONE)

                {"SelectSpinOrChopOption", new KeyBinding() {Key =Key.U, Modifiers = ModifierKeys.None}},
                {"SelectTempoOption", new KeyBinding() {Key =Key.I, Modifiers = ModifierKeys.None}},

                //Forehand + Technique (Modifier = NONE)

                {"SelectForehandPush", new KeyBinding() {Key =Key.G, Modifiers = ModifierKeys.None}},
                {"SelectForehandPushAggressive", new KeyBinding() {Key =Key.T, Modifiers = ModifierKeys.None}},
                {"SelectForehandFlip", new KeyBinding() {Key =Key.H, Modifiers = ModifierKeys.None}},
                {"SelectForehandBanana", new KeyBinding() {Key =Key.Z, Modifiers = ModifierKeys.None}},
                {"SelectForehandTopspin", new KeyBinding() {Key =Key.J, Modifiers = ModifierKeys.None}},
                {"SelectForehandBlock", new KeyBinding() {Key =Key.K, Modifiers = ModifierKeys.None}},
                
                {"SelectForehandSmash", new KeyBinding() {Key =Key.L, Modifiers = ModifierKeys.None}},
                {"SelectForehandChop", new KeyBinding() {Key =Key.O, Modifiers = ModifierKeys.None}},
                {"SelectForehandLob", new KeyBinding() {Key =Key.Oem3, Modifiers = ModifierKeys.None}},
                {"SelectForehandCounter", new KeyBinding() {Key =Key.P, Modifiers = ModifierKeys.None}},
                {"SelectForehandSpecial", new KeyBinding() {}},

                {"SelectForehandPendulum", new KeyBinding() {Key =Key.G, Modifiers = ModifierKeys.None}},
                {"SelectForehandReverse", new KeyBinding() {Key =Key.H, Modifiers = ModifierKeys.None}},
                {"SelectForehandTomahawk", new KeyBinding() {Key =Key.J, Modifiers = ModifierKeys.None}},
                {"SelectForehandSpecialServe", new KeyBinding() {Key =Key.K, Modifiers = ModifierKeys.None}},

                 //Backhand + Technique (Modifier = SHIFT)

                {"SelectBackhandPush", new KeyBinding() {Key =Key.G, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandPushAggressive", new KeyBinding() {Key =Key.T, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandFlip", new KeyBinding() {Key =Key.H, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandBanana", new KeyBinding() {Key =Key.Z, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandTopspin", new KeyBinding() {Key =Key.J, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandBlock", new KeyBinding() {Key =Key.K, Modifiers = ModifierKeys.Shift}},

                {"SelectBackhandSmash", new KeyBinding() {Key =Key.L, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandChop", new KeyBinding() {Key =Key.O, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandLob", new KeyBinding() {Key =Key.Oem3, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandCounter", new KeyBinding() {Key =Key.P, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandSpecial", new KeyBinding() {}},

                {"SelectBackhandPendulum", new KeyBinding() {Key =Key.G, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandReverse", new KeyBinding() {Key =Key.H, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandTomahawk", new KeyBinding() {Key =Key.J, Modifiers = ModifierKeys.Shift}},
                {"SelectBackhandSpecialServe", new KeyBinding() {Key =Key.K, Modifiers = ModifierKeys.Shift}},





            };
            return dictionary;
        }

        #endregion

        #region Key Gestures

        public KeyBinding GetKeyGestures(string commandName)
        {
            if (_keyGestures.ContainsKey(commandName))
                return _keyGestures[commandName];

            return null;
        }

        public void SetKeyGesture(string commandName, KeyBinding gesture)
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
            var keyGesture = _keyGestures[commandName] as KeyBinding;
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

                    _keyGestures[entry] = new KeyBinding()
                    {
                        Key = keyEnum,
                        Modifiers = modifiersEnum
                    };
                }
            }
        }

        #endregion
    }
}