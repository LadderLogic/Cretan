using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.Contracts
{
    public class SessionSetting
    {
        /// <summary>
        /// Gets or sets the target pace in MPH.
        /// </summary>
        /// <value>
        /// The target pace in MPH.
        /// </value>
        public double TargetPaceInMph { get; set; }

        /// <summary>
        /// Gets or sets the alerts required when off the pace.
        /// </summary>
        /// <value>
        /// The alerts.
        /// </value>
        public AlertType Alerts { get; set; }

        /// <summary>
        /// Gets or sets the percentage of target pace missed before alerts kick in.
        /// </summary>
        /// <value>
        /// The tolerance percent.
        /// </value>
        public double TolerancePercent { get; set; }

        /// <summary>
        /// Gets or sets the warm up time before pace is measured
        /// </summary>
        /// <value>
        /// The warm up time.
        /// </value>
        public TimeSpan WarmUpTime { get; set; }


        /// <summary>
        /// Duration
        /// </summary>
        public TimeSpan Duration { get; set; } 

    }
}
