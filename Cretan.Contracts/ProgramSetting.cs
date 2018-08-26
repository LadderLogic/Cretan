using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.Contracts
{
    /// <summary>
    /// A collection of segments that run in sequence. Essentially, interval training
    /// </summary>
    public class ProgramSetting 
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public Guid ID { get; private set; }

        public LinkedList<SegmentSetting> Segments { get; private set; }

        public ProgramSetting(string name, string description)
        {
            Name = name;
            Description = description;
            ID = Guid.NewGuid();
            Segments = new LinkedList<SegmentSetting>();
        }

        /// <summary>
        /// When this is set to true, the alert types and tolerance percent are overriden by individual segment parameters
        /// </summary>
        public bool UseIndividualSegmentSettings { get; set; } = false;


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
        /// Gets or sets the warm up time before first segment is started
        /// </summary>
        /// <value>
        /// The warm up time.
        /// </value>
        public TimeSpan WarmUpTime { get; set; }
    }
}
