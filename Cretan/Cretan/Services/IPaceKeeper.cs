using System;
using System.Collections.Generic;
using Cretan.Contracts;

namespace Cretan.Services
{
    public interface IPaceKeeper
    {
        IObservable<double> CurrentPace { get; }
        IObservable<TimeSpan> CurrentSegmentTimeLeft { get; }

        IObservable<TimeSpan> TimeLeft { get; }

        IObservable<LinkedListNode<SegmentSetting>> CurrentSegment { get; }

        void StartProgram(ProgramSetting programSetting);

        ProgramProgress StopCurrentSession();
    }
}