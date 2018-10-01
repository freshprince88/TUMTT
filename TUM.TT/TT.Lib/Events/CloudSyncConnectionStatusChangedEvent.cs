using System;
using System.Collections.Generic;
using TT.Lib.Managers;

namespace TT.Lib.Events
{
    public class CloudSyncConnectionStatusChangedEvent
    {
        /// <summary>
        /// Gets the match affected by this event.
        /// </summary>
        public ConnectionStatus ConnectionStatus { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStatus"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public CloudSyncConnectionStatusChangedEvent(ConnectionStatus status)
        {
            this.ConnectionStatus = status;
        }
    }
}