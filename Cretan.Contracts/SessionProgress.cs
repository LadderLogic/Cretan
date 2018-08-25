using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.Contracts
{
    public class SessionProgress
    {
        public SessionSetting TargetSession { get; private set; }

        public List<(TimeSpan sampleTime, double pace)> Samples { get; }

        public SessionProgress(SessionSetting targetSession)
        {
            TargetSession = targetSession;
            Samples = new List<(TimeSpan sampleTime, double pace)>();
        }
    }
}
