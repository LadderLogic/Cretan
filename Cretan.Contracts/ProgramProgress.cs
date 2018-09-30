using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.Contracts
{
    
    public class ProgramProgress
    {

        private List<SessionProgress> _sessionProgress;

        public ProgramProgress(ProgramSetting targetProgram)
        {
            SessionProgress = new List<SessionProgress>();
        }

        public List<SessionProgress> SessionProgress { get => _sessionProgress; private set => _sessionProgress = value; }
    }
}
