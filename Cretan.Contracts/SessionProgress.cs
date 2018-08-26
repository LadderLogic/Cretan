using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.Contracts
{
    public class SessionProgress
    {
        public SegmentSetting TargetSession { get; private set; }

        public List<(TimeSpan sampleTime, double pace)> Samples { get; }

        public SessionProgress(SegmentSetting targetSession)
        {
            TargetSession = targetSession;
            Samples = new List<(TimeSpan sampleTime, double pace)>();
        }
    }
}
