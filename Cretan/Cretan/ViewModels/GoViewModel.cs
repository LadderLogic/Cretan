using Cretan.Contracts;
using Cretan.DeviceControl;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Cretan.ViewModels
{
    public class GoViewModel : BaseViewModel
    {
        private Geo _geo;

        public GoViewModel(SessionSetting sessionSetting)
        {
            TargetPace = sessionSetting.TargetPaceInMph;
            _geo = new Geo();
            _geo.StartTrackingLocation();
            _geo.SpeedMph.Subscribe((newSpeed) => UpdateSpeedWithCurrentUnits(newSpeed));
            Stop = new DelegateCommand(StopSession);
        }

        private void StopSession()
        {
            MessagingCenter.Send(this, Messages.StopSession, _progress);
        }

        private SessionProgress _progress = new SessionProgress();

        private void UpdateSpeedWithCurrentUnits(double newSpeedInMph)
        {
            
            CurrentSpeed = newSpeedInMph;
            // TODO: Need unit converter based on settings
            CurrentPace = newSpeedInMph;

            // Debug
            var loc = _geo.GetCurrentLocation();
            DebugString = $"S {newSpeedInMph} A {loc.Accuracy} L {loc.Latitude} Lo {loc.Longitude}";
        }

        public DelegateCommand Stop { get; set; }

        private double _currentPace;

        public double CurrentPace
        {
            get { return _currentPace; }
            set { SetProperty(ref _currentPace, value); }
        }

        private double _targetPace;

        public double TargetPace
        {
            get { return _targetPace; }
            set { SetProperty(ref _targetPace, value); }
        }


        private double _currentSpeed = 0;
        public double CurrentSpeed
        {
            get { return _currentSpeed; }
            set { SetProperty(ref _currentSpeed, value); }
        }

    }
}
