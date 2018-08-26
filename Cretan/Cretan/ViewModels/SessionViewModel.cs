using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cretan.Contracts;

namespace Cretan.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private SegmentSetting _sessionSettings;

        public SessionViewModel(SegmentSetting settings)
        {
            SessionSettings = settings;

        }


        #region CurrentPace		
        private double _currentPace;
        public double CurrentPace
        {
            get { return _currentPace; }
            set { SetProperty(ref _currentPace, value); }
        }
        #endregion CurrentPace



        public SegmentSetting SessionSettings
        {
            get
            {
                return _sessionSettings;
            }

            set
            {
                _sessionSettings = value;
            }
        }
    }
}
