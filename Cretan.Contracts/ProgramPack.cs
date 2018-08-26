using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.Contracts
{
    /// <summary>
    /// A set of programs.
    /// </summary>
    public class ProgramPack
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        public LinkedList<ProgramSetting> Programs { get; set; }

        public ProgramPack(string name, string description)
        {
            Name = name;
            Description = description;
            Programs = new LinkedList<ProgramSetting>();
        }
    }
}
