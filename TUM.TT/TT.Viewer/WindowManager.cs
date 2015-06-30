using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer
{
    using System.Windows;

    /// <summary>
    /// The window manager for this application.
    /// </summary>
    public class WindowManager : Caliburn.Micro.WindowManager
    {
        /// <summary>
        /// Ensures a window for a model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="view">The view.</param>
        /// <param name="isDialog">Whether the view is a dialog or not.</param>
        /// <returns>The window.</returns>
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = base.EnsureWindow(model, view, isDialog);

            if (!isDialog)
            {
                var settings = Properties.Settings.Default;

                window.Left = settings.Left;
                window.Top = settings.Top;
                window.Width = settings.Width;
                window.Height = settings.Height;
                window.WindowState = settings.WindowState;

                window.Closing += (s, a) => this.SaveWindowState((Window)s);
            }

            return window;
        }

        /// <summary>
        /// Saves the state of a window.
        /// </summary>
        /// <param name="window">The window whose state to save.</param>
        private void SaveWindowState(Window window)
        {
            var settings = Properties.Settings.Default;

            settings.WindowState = window.WindowState;
            if (settings.WindowState == WindowState.Minimized)
            {
                settings.WindowState = WindowState.Normal;
            }

            if (settings.WindowState == WindowState.Normal)
            {
                // Save window rectangle, if not maximized
                settings.Left = window.Left;
                settings.Top = window.Top;
                settings.Height = window.Height;
                settings.Width = window.Width;
            }
        }
    }
}
