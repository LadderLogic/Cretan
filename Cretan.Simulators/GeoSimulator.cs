using Cretan.Interfaces;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Cretan.Simulators
{
    public class GeoSimulator : IGeo
    {
        private BehaviorSubject<(double Latitude, double Longitude)> _currentLocation;

        public IObservable<(double Latitude, double Longitude)> CurrentLocation
        {
            get
            {
                return _currentLocation.AsObservable();
            }
        }

        private BehaviorSubject<double> _speedMph;
        private CancellationTokenSource mCancellationToken;

        public IObservable<double> SpeedMph
        {
            get
            {
                return _speedMph.AsObservable();
            }
        }

        public (double Latitude, double Longitude) GetCurrentLocation()
        {
            return (39.5595198, -120.82769659999997);
        }

        public double SimulatedSpeed { get; set; } = 6.0;

        public void StartTrackingLocation()
        {
            mCancellationToken = new CancellationTokenSource();
            Task.Run(() =>
            {
                _speedMph.OnNext(SimulatedSpeed);
                Task.Delay(1000).Wait();
            }, mCancellationToken.Token);
        }

        public void StopTrackingLocation()
        {
            mCancellationToken?.Cancel();
        }

        public GeoSimulator()
        {
            _speedMph = new BehaviorSubject<double>(0);
            _currentLocation = new BehaviorSubject<(double Latitude, double Longitude)>((0, 0));
        }
    }
}
