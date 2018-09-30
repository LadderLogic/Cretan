using Cretan.Contracts;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Prism.Commands;
using Cretan.Views;

namespace Cretan.ViewModels
{
    public class SessionSummaryViewModel : BaseViewModel
    {

        private SessionProgress _progress;
        public SessionProgress Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        private List<(int Percentage, double MinutesInPace, int Score)>  _results;
        public List<(int Percentage, double MinutesInPace, int Score)>  Results
        {
            get { return _results; }
            set { SetProperty(ref _results, value); }
        }

        private double _rating;
        public double Rating
        {
            get { return _rating; }
            set { SetProperty(ref _rating, value); }
        }

        private DelegateCommand _accept;
        public DelegateCommand Accept
        {
            get { return _accept; }
            set { SetProperty(ref _accept, value); }
        }

        public SessionSummaryViewModel(INavigationService navigationService)
        {
            Accept = new DelegateCommand(
                () =>  navigationService.NavigateAsync(nameof(MainMenu))
                
                );
        }

        private void AcceptedResults()
        {
            
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            // Read session progress in by navigation
            Progress = parameters.GetValue<ProgramProgress>(nameof(ProgramProgress)).SessionProgress.FirstOrDefault();

            CalculateResults(Progress);
        }

        private void CalculateResults(SessionProgress progress)
        {
            var percentages = progress.Samples.Select(
                sample =>
                {
                    return Math.Abs(sample.pace - progress.TargetSession.TargetPaceInMph) / 100;
                }
                );

            Results = percentages.Select(p=> ((int)p,10.0, 1)).ToList();

            Rating = CalculateOverallResult(progress.Samples.Select(sample => sample.pace), progress.TargetSession.TargetPaceInMph);
        }


        /// <summary>
        /// Calculates Sample standard deviation with expected value instead of mean value
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private double CalculateOverallResult(IEnumerable<double> paceSamples, double expectedPace)
        {
            var retVal = 0.0;
            if (paceSamples.Count() > 1)
            {
                //Perform the Sum of (value-avg)_2_2      
                double sum = paceSamples.Sum(sample => Math.Pow(sample - expectedPace, 2));
                //Put it all together      
                retVal = Math.Sqrt((sum) / (paceSamples.Count() - 1));
            }
            return retVal;
        }
    }
}
