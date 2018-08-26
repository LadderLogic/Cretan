using Cretan.Contracts;
using Cretan.DeviceControl;
using Cretan.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cretan.Services
{
    public class PaceKeeper : IPaceKeeper
    {
        private IGeo _geo;
        private Stopwatch _sessionWatch;

        private CancellationTokenSource mSessionMonitorToken;
        private Haptic _haptic;
        private int _paceTrackingInSeconds = 30;
        private SessionSetting _currentSessionSettings;

        private BehaviorSubject<TimeSpan> _timeLeft = new BehaviorSubject<TimeSpan>(TimeSpan.FromSeconds(0));

        private BehaviorSubject<double> _currentPace = new BehaviorSubject<double>(0);
        private Speech _speech;

        private SessionProgress _sessionProgress;

        public IObservable<double> CurrentPace
        {
            get
            {
                return _currentPace.AsObservable();
            }
        }

        public IObservable<TimeSpan> TimeLeft
        {
            get
            {
                return _timeLeft.AsObservable();
            }
        }

        public PaceKeeper(IGeo geo)
        {
            _geo = geo;
            _haptic = new Haptic();
            _speech = new Speech();
            _sessionWatch = new Stopwatch();
        }


        public void StartSession(SessionSetting sessionSetting)
        {
            StopCurrentSession();
            _sessionProgress = new SessionProgress(sessionSetting);
            _currentSessionSettings = sessionSetting;
            _geo.StartTrackingLocation();
            _geo.SpeedMph.Subscribe((newSpeed) => UpdateSpeedWithCurrentUnits(newSpeed));

            mSessionMonitorToken = new CancellationTokenSource();
            Task.Factory.StartNew(MonitorSession, mSessionMonitorToken.Token);
        }

        public SessionProgress StopCurrentSession()
        {
            _geo.StopTrackingLocation();
            if (!(mSessionMonitorToken?.IsCancellationRequested??true))
                mSessionMonitorToken?.Cancel();

            return _sessionProgress;
        }

        private void MonitorSession()
        {
            _sessionWatch.Start();

            var paceWatch = new Stopwatch();
            while (!mSessionMonitorToken.IsCancellationRequested)
            {
                _timeLeft.OnNext(_currentSessionSettings.Duration - _sessionWatch.Elapsed);
                Task.Delay(1000).Wait();

                // Sample session progress every ~5 seconds
                if ((int)(_sessionWatch.Elapsed.TotalSeconds % 5)==0)
                    _sessionProgress.Samples.Add((_sessionWatch.Elapsed, _currentPace.Value));

                if (!IsOnPace())
                {
                    if (paceWatch.IsRunning)
                    {
                        if (paceWatch.Elapsed.TotalSeconds >= _paceTrackingInSeconds)
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
            _sessionWatch.Reset();
        }

        private void UpdateSpeedWithCurrentUnits(double newSpeedInMph)
        {
            // TODO: Add conversion here from speed to pace
           
            _currentPace.OnNext(newSpeedInMph);



            // Debug
            //var loc = _geo.GetCurrentLocation();
            //DebugString = $"S {newSpeedInMph} A {loc.Accuracy} L {loc.Latitude} Lo {loc.Longitude}";
        }

        private void ResetPaceNotification()
        {

        }

        private void NotifyOffPace()
        {
            if (IsOnPace())
                return;
            if (_currentPace.Value < _currentSessionSettings.TargetPaceInMph)
                NotifyLowPace();
            else
                NotifyHighPace();
        }


        private async void NotifyHighPace()
        {
            if (_currentSessionSettings.Alerts.HasFlag(AlertType.Haptic))
            {
                await _haptic.Pulse(750, 750, TimeSpan.FromSeconds(3), mSessionMonitorToken.Token);
            }
            if (_currentSessionSettings.Alerts.HasFlag(AlertType.AudioNotification))
            {
                await _speech.SpeakText(SpeechStrings.SlowDown);
            }
        }

        private async void NotifyLowPace()
        {
            if (_currentSessionSettings.Alerts.HasFlag(AlertType.Haptic))
            {
                await _haptic.Pulse(250, 250, TimeSpan.FromSeconds(2), mSessionMonitorToken.Token);
            }
            if (_currentSessionSettings.Alerts.HasFlag(AlertType.AudioNotification))
            {
                await _speech.SpeakText(SpeechStrings.Faster);
            }
        }

        private bool IsOnPace()
        {
            return (Math.Abs(_currentPace.Value - _currentSessionSettings.TargetPaceInMph) < 
                (_currentSessionSettings.TargetPaceInMph * _currentSessionSettings.TolerancePercent / 100));
        }

    }
}
