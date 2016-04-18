﻿using Caliburn.Micro;
using System;

namespace TT.Scouter.Interfaces
{
    public interface IMediaPosition : IScreen
    {
        TimeSpan MediaPosition { get; }

        void Play();

        void Pause();

        void Stop();
    }
}