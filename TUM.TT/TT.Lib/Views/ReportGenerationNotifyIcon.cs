using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TT.Lib.Properties;
using Timer = System.Windows.Forms.Timer;

namespace TT.Lib.Views
{
    public class ReportGenerationNotifyIcon
    {
        private const int BalloonTipDuration = 30000; // ms
        private readonly NotifyIcon _notifyIcon;
        private readonly Timer _timer;
        private Icon[] _animateIcons;
        private int _animIconCtr;

        public ReportGenerationNotifyIcon(int animateInterval = 63)
        {
            var assembly = Assembly.GetExecutingAssembly();
            IEnumerable<string> iconResources =
                assembly.GetManifestResourceNames()
                    .Where(s => Regex.IsMatch(s, ".*r_icons.*\\.ico"))
                    .OrderBy(s => s, Comparer<string>.Create((a, b) =>
                    {
                        if (a.Length < b.Length)
                            return -1;
                        if (a.Length > b.Length)
                            return 1;
                        return string.CompareOrdinal(a, b);
                    }));
            _animateIcons = new Icon[iconResources.Count()];
            for (var i = 0; i < _animateIcons.Length; i++)
            {
                var iconStream = assembly.GetManifestResourceStream(iconResources.ElementAt(i));
                if (iconStream != null)
                    _animateIcons[i] = new Icon(iconStream);
            }

            _notifyIcon = new NotifyIcon
            {
                Text = Resources.notification_generating,
                Icon = _animateIcons[0],
                BalloonTipText = Resources.notification_generated_text,
                BalloonTipTitle = Resources.notification_generated_title
            };

            EventHandler dClickOrBalloonClick = (sender, args) =>
            {
                var notifyIcon = (NotifyIcon)sender;
                if (notifyIcon?.Tag == null)
                    return;

                Debug.WriteLine($"ReportGenerationNotifyIcon click on NotifyIcon or BallonTip): opening path '{notifyIcon.Tag}' and hiding NotifyIcon (Thread '{Thread.CurrentThread.Name}')");
                Process.Start((string)notifyIcon.Tag);
                notifyIcon.Visible = false;
            };
            _notifyIcon.DoubleClick += dClickOrBalloonClick;
            _notifyIcon.BalloonTipClicked += dClickOrBalloonClick;

            _timer = new Timer
            {
                Interval = animateInterval
            };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _notifyIcon.Icon = _animateIcons[_animIconCtr++];
            if (_animIconCtr >= _animateIcons.Length)
                _animIconCtr = 0;
        }

        public object Tag
        {
            get { return _notifyIcon.Tag; }
            set { _notifyIcon.Tag = value; }
        }

        public bool Visible
        {
            get { return _notifyIcon.Visible; }
            set
            {
                _notifyIcon.Visible = value;
                if (!value)
                    StopAnimating();
            }
        }

        public bool Animating => _timer.Enabled;

        public void Animate()
        {
            _timer.Start();
            _notifyIcon.Visible = true;
            _notifyIcon.Text = Resources.notification_generating;
            _notifyIcon.Tag = null;
        }

        public void StopAnimating()
        {
            _timer.Stop();
            _animIconCtr = 0;
            _notifyIcon.Icon = _animateIcons[0];
            _notifyIcon.Text = Resources.notification_generated_doubleclick;
        }

        public void Dispose()
        {
            StopAnimating();
            _timer.Dispose();

            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();

            foreach (var animateIcon in _animateIcons)
            {
                animateIcon.Dispose();
            }
            _animateIcons = null;
        }

        public void ShowBaloonTip()
        {
            _notifyIcon.Text = Resources.notification_generated_doubleclick;
            _notifyIcon.ShowBalloonTip(BalloonTipDuration);
        }
    }
}
