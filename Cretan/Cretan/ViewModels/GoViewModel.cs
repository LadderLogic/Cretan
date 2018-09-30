using Cretan.Contracts;
using Cretan.DeviceControl;
using Cretan.Services;
using Cretan.Views;
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


        public GoPageViewModel(IPaceKeeper paceKeeper, INavigationService navigationService)
        {
          
            Stop = new DelegateCommand(StopSession);
            _navigationService = navigationService;
            _paceKeeper = paceKeeper;

           

            

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            // Read session settings passed in by navigation
            
            _currentProgramSetting = parameters.GetValue<ProgramSetting>(nameof(ProgramSetting));

            


            _paceKeeper.StartProgram(_currentProgramSetting);
            _paceKeeper.CurrentSegment.Subscribe((segmentNode) =>
            {
                if ((segmentNode == null) || (segmentNode.Value == null))
                    return;
                
                TargetPace = segmentNode.Value.TargetPaceInMph;
                CurrentSegment = segmentNode.Value;
            });

            _paceKeeper.CurrentPace.Subscribe(pace => CurrentPace = pace);

            _paceKeeper.CurrentSegmentTimeLeft.Subscribe(segTimeLeft =>
            {
                if (CurrentSegment == null)
                    return;
                SegmentTimeLeft = segTimeLeft;
                SegmentProgress = (CurrentSegment.Duration.TotalSeconds - segTimeLeft.TotalSeconds) / CurrentSegment.Duration.TotalSeconds;

            });

            _paceKeeper.TimeLeft.Subscribe(timeLeft =>
            {
                ProgramTimeLeft = timeLeft;
                var totalTime = _currentProgramSetting.GetTotalTime();
                Progress = (totalTime - timeLeft).TotalSeconds / totalTime.TotalSeconds;
            });
        }

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        private SegmentSetting _currentSegment;
        public SegmentSetting CurrentSegment
        {
            get { return _currentSegment; }
            set { SetProperty(ref _currentSegment, value); }
        }


        private void StopSession()
        {
            var sessionResults = _paceKeeper.StopCurrentSession();
            var navParams = new NavigationParameters();
            navParams.Add(nameof(ProgramProgress), sessionResults);

            // TODO: Take navigation/data flow out of these pages and move it to event aggregator style service
            // centralize page flows
            _navigationService.NavigateAsync(nameof(SessionSummary), navParams);
        }


        public DelegateCommand Stop { get; set; }

        private INavigationService _navigationService;
        private IPaceKeeper _paceKeeper;
        private double _currentPace;

        public double CurrentPace
        {
            get { return _currentPace; }
            set { SetProperty(ref _currentPace, value); }
        }

        private double _targetPace;
        private ProgramSetting _currentProgramSetting;

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

        private double _segmentProgress = 0;
        public double SegmentProgress
        {
            get { return _segmentProgress; }
            set { SetProperty(ref _segmentProgress, value); }
        }


        private TimeSpan _timeLeft;


        public TimeSpan ProgramTimeLeft
        {
            get { return _timeLeft; }
            set { SetProperty(ref _timeLeft, value); }
        }

        private TimeSpan _segmentTimeLeft;
        public TimeSpan SegmentTimeLeft
        {
            get { return _segmentTimeLeft; }
            set { SetProperty(ref _segmentTimeLeft, value); }
        }
    }
}
