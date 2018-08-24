using System;
using Cretan.Contracts;

namespace Cretan.Services
{
    public interface IPaceKeeper
    {
        IObservable<double> CurrentPace { get; }
        IObservable<TimeSpan> TimeLeft { get; }

        void StartSession(SessionSetting sessionSetting);
        void StopCurrentSession();
    }
}