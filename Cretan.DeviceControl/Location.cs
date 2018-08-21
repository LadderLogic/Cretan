﻿using System;
using Xamarin.Essentials;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading;

namespace Cretan.DeviceControl
{
    public class Geo
    {
        private BehaviorSubject<Location> _currentLocation;

        public IObservable<Location> CurrentLocation {
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

        public Location GetCurrentLocation()
        {
            return _currentLocation.Value;
        }

        private ManualResetEventSlim _waitForMotion;
        private CancellationTokenSource _trackingToken;

        public Geo()
        {
            _currentLocation = new BehaviorSubject<Location>(new Location(39.5595198, -120.82769659999997));
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
                _currentLocation.OnNext(Geolocation.GetLastKnownLocationAsync().Result);

                while (true)
                {
                    // If we're not moving set speed to zero and wait for motion before we start measuring speed again
                    while(!_waitForMotion.Wait(3000, _trackingToken.Token))
                    {
                        _speedMph.OnNext(0);
                    }


                    var oldLocation = _currentLocation.Value;
                    var newLocation = Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best)).Result;
                    if (newLocation.Accuracy.Value > 20)
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
            _trackingToken.Cancel();
        }

        private void UpdateLocationAndSpeed(Location oldLocation, Location value)
        {
            _currentLocation.OnNext(value);
            //var distance = oldLocation.CalculateDistance(value, DistanceUnits.Miles);
            //var speedMph = Math.Abs(distance) / (value.TimestampUtc - oldLocation.TimestampUtc).TotalHours;
            // Xam essentials 0.10 with speed now
            var speedMph = value.Speed;
            if (!speedMph.HasValue || speedMph.Value <= 0)
                return;
            _speedMph.OnNext(speedMph.Value);
        }
    }
}
