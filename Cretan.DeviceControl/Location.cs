using System;
using Xamarin.Essentials;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading;
using Cretan.Interfaces;
using System.Linq;

namespace Cretan.DeviceControl
{
    public class Geo : IGeo
    {
        private BehaviorSubject<(double Latitude, double Longitude)> _currentLocation;

        public IObservable<(double Latitude, double Longitude)> CurrentLocation {
            get { return _currentLocation.AsObservable();
            } }

        private BehaviorSubject<double> _speedMph;

        public IObservable<double> SpeedMph
        {
            get
            {
                return _speedMph.AsObservable();
            }
        }

        public (double Latitude, double Longitude) GetCurrentLocation()
        {
            return _currentLocation.Value;
        }

        private ManualResetEventSlim _waitForMotion;
        private CancellationTokenSource _trackingToken;

        public Geo()
        {
            _currentLocation = new BehaviorSubject<(double Longitude, double Latitude)>((39.5595198, -120.82769659999997));
            _speedMph = new BehaviorSubject<double>(0.0);
            _waitForMotion = new ManualResetEventSlim(false);
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            if ((e.Reading.Acceleration.X > 1) || (e.Reading.Acceleration.Y > 1) || (e.Reading.Acceleration.Z > 1))
                _waitForMotion.Set();
        }


        public void StartTrackingLocation()
        {

            _trackingToken = new CancellationTokenSource();
            // Start the accelerometer to sense motion before start calculating speed using GPS
            Accelerometer.Start(SensorSpeed.UI);

            Task.Factory.StartNew(() =>
            {
                var firstLocation = Geolocation.GetLastKnownLocationAsync().Result;
                _currentLocation.OnNext((firstLocation.Latitude, firstLocation.Longitude));

                while (true)
                {
                    // If we're not moving set speed to zero and wait for motion before we start measuring speed again
                    while(!_waitForMotion.Wait(3000, _trackingToken.Token))
                    {
                        _speedMph.OnNext(0);
                    }


                    var oldLocation = _currentLocation.Value;
                    var newLocation = Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best)).Result;
                    if (newLocation.Accuracy.Value > 10)
                        continue;
                    UpdateLocationAndSpeed(oldLocation, newLocation);
                    _waitForMotion.Reset();
                }
            }, _trackingToken.Token);
        }

        public void StopTrackingLocation()
        {
            Accelerometer.Stop();
            _waitForMotion.Set();
            if (!(_trackingToken?.IsCancellationRequested ?? true))
                _trackingToken.Cancel();
        }

        private void UpdateLocationAndSpeed((double Latitude, double Longitude) oldLocation, Location value)
        {
            
            _currentLocation.OnNext((value.Latitude, value.Longitude));
           
            var speedMps = value.Speed;
            if (!speedMps.HasValue || speedMps.Value <= 0)
                return;
            _speedMph.OnNext(Converters.MeterPerSecondToMilesPerHour(speedMps.Value));
        }


    }
}
