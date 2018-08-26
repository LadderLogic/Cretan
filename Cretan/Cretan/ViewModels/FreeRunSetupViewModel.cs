using Cretan.Contracts;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;

namespace Cretan.ViewModels
{
    public class FreeRunSetupViewModel:BaseViewModel
    {
        private SegmentSetting _session;

        public FreeRunSetupViewModel(INavigationService navigationService)
        {
            Debug.WriteLine("ctor viewmodel");
            Session = new SegmentSetting() { TargetPaceInMph = 5, TolerancePercent = 10 };
            Duration = 15; //. default to 15 minutes
            HapticFeedback = true;
            Start = new DelegateCommand(StartSession);
            _navigationService = navigationService;
        }

        private void StartSession()
        {
            var navParams = new NavigationParameters();
            navParams.Add(nameof(SegmentSetting), Session);
            _navigationService.NavigateAsync("GoPage", navParams, true);


        }

        private double _duration;
        /// <summary>
        /// Duration in minutes
        /// </summary>
        public double Duration
        {
            get { return _duration; }
            set
            {
                SetProperty(ref _duration, value);
                Session.Duration = TimeSpan.FromMinutes(value);
            }
        }



        #region Session		
        public SegmentSetting Session
        {
            get { return _session; }
            set { SetProperty(ref _session, value); }
        }
        #endregion Session

        public DelegateCommand Start { get; set; }

        private INavigationService _navigationService;



        #region HapticFeedback		
        private bool _hapticFeedback;
        public bool HapticFeedback
        {
            get { return _hapticFeedback; }
            set
            {
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
