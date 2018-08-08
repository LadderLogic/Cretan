using System;
using Xamarin.Essentials;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Diagnostics;

namespace Cretan.DeviceControl
{
    public class Geo
    {
        private BehaviorSubject<Location> _currentLocation;

        public IObservable<Location> CurrentLocation {
            get { return _currentLocation.AsObservable();
            } }

        public Geo()
        {
            //_geoLocator = Geolocation.GetLocationAsync().Result.CalculateDistance();
            _currentLocation = new BehaviorSubject<Location>(new Location(39.5595198, -120.82769659999997));
        }

        public void StartTrackingLocation()
        {
            Task.Factory.StartNew(() =>
            {
                _currentLocation.OnNext(Geolocation.GetLocationAsync().Result);
            });
        }





    }
}
