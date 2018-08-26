using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.Interfaces
{
    public interface IGeo
    {
        IObservable<(double Latitude, double Longitude)> CurrentLocation { get; }
        IObservable<double> SpeedMph { get; }

        (double Latitude, double Longitude) GetCurrentLocation();
        void StartTrackingLocation();
        void StopTrackingLocation();
    }
}
