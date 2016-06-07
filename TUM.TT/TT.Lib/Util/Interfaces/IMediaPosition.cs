using Caliburn.Micro;
using System;

namespace TT.Lib.Interfaces
{
    public interface IMediaPosition : IScreen
    {
        TimeSpan MediaPosition { get; set; }

        TimeSpan EndPosition { get; set; }

        double Minimum { get; set; }

        double Maximum { get; set; }

        bool toRallyStart { get; set; }

        void Play();
        
        void Pause();

        void Stop();

        
    }
}
