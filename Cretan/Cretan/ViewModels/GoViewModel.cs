using Cretan.Contracts;
using Cretan.DeviceControl;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cretan.ViewModels
{
    public class GoViewModel : BaseViewModel
    {
        private Geo _geo;
        private Stopwatch mSessionWatch;

        public GoViewModel(SessionSetting sessionSetting)
        {
            mCurrentSessionSettings = sessionSetting;
            TargetPace = sessionSetting.TargetPaceInMph;
            _geo = new Geo();
            _haptic = new Haptic();
            mSessionWatch = new Stopwatch();
            Stop = new DelegateCommand(StopSession);
            StartSession();
        }



        private void StartSession()
        {
            mSessionWatch.Start();
            _geo.StartTrackingLocation();
            _geo.SpeedMph.Subscribe((newSpeed) => UpdateSpeedWithCurrentUnits(newSpeed));
            mSessionMonitorToken = new CancellationTokenSource();
            Task.Factory.StartNew(MonitorSession, mSessionMonitorToken.Token);
        }

        private void MonitorSession()
        {
            var paceWatch = new Stopwatch();
            while(!mSessionMonitorToken.IsCancellationRequested)
            {
                TimeLeft = mCurrentSessionSettings.Duration - mSessionWatch.Elapsed;
                ProgressLeft = mSessionWatch.Elapsed.TotalSeconds / mCurrentSessionSettings.Duration.TotalSeconds;
                Task.Delay(1000).Wait();

                if (!IsOnPace())
                {
                    if (paceWatch.IsRunning)
                    {
                        if (paceWatch.Elapsed.TotalSeconds >= 15)
                        {
                            NotifyOffPace();
                            paceWatch.Restart();
                        }
                    }
                    else
                    {
                        paceWatch.Start();
                    }
                }
                else
                {
                    if (paceWatch.IsRunning)
                    {
                        paceWatch.Stop();
                        ResetPaceNotification();
                    }
                }
                
            }
        }

        private void ResetPaceNotification()
        {
            
        }

        private void NotifyOffPace()
        {
            if (CurrentPace < TargetPace)
                NotifyLowPace();
            else
                NotifyHighPace();
        }

        private void NotifyHighPace()
        {
            if (mCurrentSessionSettings.Alerts.HasFlag(AlertType.Haptic))
            {
                _haptic.Pulse(500, 100, TimeSpan.FromSeconds(3), mSessionMonitorToken.Token);
            }
        }

        private void NotifyLowPace()
        {
            if (mCurrentSessionSettings.Alerts.HasFlag(AlertType.Haptic))
            {
                _haptic.Pulse(200, 200, TimeSpan.FromSeconds(2), mSessionMonitorToken.Token);
            }

        }

        private bool IsOnPace()
        {
            return (Math.Abs(CurrentPace - TargetPace) < (TargetPace * mCurrentSessionSettings.TolerancePercent / 100));
        }

        private void StopSession()
        {
            _geo.StopTrackingLocation();
            mSessionMonitorToken.Cancel();
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
        private SessionSetting mCurrentSessionSettings;

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

        private double _progressLeft = 0;
        public double ProgressLeft
        {
            get { return _progressLeft; }
            set { SetProperty(ref _progressLeft, value); }
        }


        private TimeSpan _timeLeft;
        private CancellationTokenSource mSessionMonitorToken;
        private Haptic _haptic;

        public TimeSpan TimeLeft
        {
            get { return _timeLeft; }
            set { SetProperty(ref _timeLeft, value); }
        }


    }
}
