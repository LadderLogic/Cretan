using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cretan.Contracts;
using Cretan.DeviceControl;
using Prism.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cretan.ViewModels
{
    public class PaceDefinitionViewModel : BaseViewModel
    {
        private SessionSetting _session;

        public PaceDefinitionViewModel()
        {
            Debug.WriteLine("ctor viewmodel");
            Session = new SessionSetting() { TargetPaceInMph = 5, TolerancePercent = 10 };
            Start = new DelegateCommand(StartSession);
            _geo = new Geo();
        }

        private void StartSession()
        {
            //MessagingCenter.Send(this, Messages.StartSession, Session);
            _geo.StartTrackingLocation();
            _geo.CurrentLocation.Subscribe((newLocation) => CurrentLocation = newLocation);
            
        }


        #region Session		
        public SessionSetting Session
        {
            get { return _session; }
            set { SetProperty(ref _session, value); }
        }
        #endregion Session
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set { SetProperty(ref _currentLocation, value); }
        }
        

        public DelegateCommand Start { get; set; }

        private Geo _geo;


        #region HapticFeedback		
        private bool _hapticFeedback;
        public bool HapticFeedback
        {
            get { return _hapticFeedback; }
            set {
                Debug.WriteLine("Haptic Feedback");

                SetProperty(ref _hapticFeedback, value);
                if (value)
                    Session.Alerts |= AlertType.Haptic;
                else
                    Session.Alerts &= ~AlertType.Haptic;
            }
        }
        #endregion HapticFeedback

        #region AudioFeedback		
        private bool _audioFeedback;
        public bool AudioFeedback
        {
            get { return _audioFeedback; }
            set
            {
                SetProperty(ref _audioFeedback, value);
                if (value)
                    Session.Alerts |= AlertType.AudioNotification;
                else
                    Session.Alerts &= ~AlertType.AudioNotification;
            }
        }
        #endregion AudioFeedback

        #region MediaVolumeFeedback		
        private bool _mediaVolumeFeedback;
        private Location _currentLocation;

        public bool MediaVolumeFeedback
        {
            get { return _mediaVolumeFeedback; }
            set
            {
                SetProperty(ref _mediaVolumeFeedback, value);
                if (value)
                    Session.Alerts |= AlertType.MediaVolume;
                else
                    Session.Alerts &= ~AlertType.MediaVolume;
            }
        }
        #endregion MediaVolumeFeedback


    }
}
