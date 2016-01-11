using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;

namespace TT.Lib.Managers
{
    public class MatchManager : IMatchManager
    {

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }

        private Match _match;
        public Match Match
        {
            get
            {
                return _match;
            }
            set
            {
                if (_match != value)
                {
                    _match = value;
                    Events.PublishOnUIThread(new MatchOpenedEvent(Match));
                }               
            }
        }

        public MatchManager(IEventAggregator aggregator)
        {
            Events = aggregator;
        }

    }
}
