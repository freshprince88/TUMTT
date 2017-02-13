﻿using System;
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

                {"PreviousRally", new KeyBinding() {Key =Key.MediaPreviousTrack, Modifiers = ModifierKeys.None}},
                {"StartRallyAtBeginning", new KeyBinding() {Key =Key.MediaPreviousTrack, Modifiers = ModifierKeys.Control}},
                {"Previous5Frames", new KeyBinding() {Key =Key.MediaPreviousTrack,Modifiers =  ModifierKeys.Shift}},
                {"PreviousFrame", new KeyBinding() {Key =Key.MediaPreviousTrack, Modifiers = ModifierKeys.Alt}},
                {"PlayPause", new KeyBinding() {Key =Key.MediaPlayPause, Modifiers = ModifierKeys.None}},
                {"NextFrame", new KeyBinding() {Key =Key.MediaNextTrack, Modifiers = ModifierKeys.Alt}},
                {"Next5Frames", new KeyBinding() {Key =Key.MediaNextTrack, Modifiers = ModifierKeys.Shift}},
                {"NextRally", new KeyBinding() {Key =Key.MediaNextTrack, Modifiers = ModifierKeys.None}},
                {"FullscreenHelper", new KeyBinding() {Key =Key.F, Modifiers = ModifierKeys.Alt}},
                {"PlayModeHelper", new KeyBinding() {Key =Key.R, Modifiers = ModifierKeys.Alt}},
                {"SelectForehand", new KeyBinding() {Key =Key.V, Modifiers = ModifierKeys.Alt}},
                {"SelectBackhand", new KeyBinding() {Key =Key.B, Modifiers = ModifierKeys.Alt}},
                {"NextStroke", new KeyBinding() {Key =Key.Right, Modifiers = ModifierKeys.Alt}},
                {"PreviousStroke", new KeyBinding() {Key =Key.Left, Modifiers = ModifierKeys.Alt}}
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