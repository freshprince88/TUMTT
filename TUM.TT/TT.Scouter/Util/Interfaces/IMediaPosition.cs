using Caliburn.Micro;
using System;

namespace TT.Scouter.Interfaces
{
    public interface IMediaPosition : IScreen
    {
        TimeSpan MediaPosition { get; set; }

        TimeSpan EndPosition { get; set; }
        bool toRallyStart { get; set; }

        void Play();
        
        void Pause();

        void Stop();

        
    }
}
