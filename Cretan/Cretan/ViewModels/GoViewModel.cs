using Cretan.Contracts;
using Cretan.DeviceControl;
using Cretan.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cretan.ViewModels
{
    public class GoPageViewModel : BaseViewModel
    {


        public GoPageViewModel(IPaceKeeper paceKeeper)
        {
          
            Stop = new DelegateCommand(StopSession);

            _paceKeeper = paceKeeper;

           

            

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            // Read session settings passed in by navigation
            _currentSessionSettings = parameters.GetValue<SessionSetting>(nameof(SessionSetting));

            TargetPace = _currentSessionSettings.TargetPaceInMph;


            _paceKeeper.StartSession(_currentSessionSettings);

            _paceKeeper.CurrentPace.Subscribe(pace => CurrentPace = pace);
            _paceKeeper.TimeLeft.Subscribe(timeLeft =>
            {
                TimeLeft = timeLeft;
                ProgressLeft = (_currentSessionSettings.Duration.TotalSeconds - timeLeft.TotalSeconds) / _currentSessionSettings.Duration.TotalSeconds;
            });
        }




        private void StopSession()
        {
            _paceKeeper.StopCurrentSession();
        }

        private SessionProgress _progress = new SessionProgress();



        public DelegateCommand Stop { get; set; }

        private IPaceKeeper _paceKeeper;
        private double _currentPace;

        public double CurrentPace
        {
            get { return _currentPace; }
            set { SetProperty(ref _currentPace, value); }
        }

        private double _targetPace;
        private SessionSetting _currentSessionSettings;

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


        public TimeSpan TimeLeft
        {
            get { return _timeLeft; }
            set { SetProperty(ref _timeLeft, value); }
        }


    }
}
