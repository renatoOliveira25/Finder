using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class CPU {
        public string CpuName { get; set; }
        public string Architeture { get; set; }
        public int NumberOfCores { get; set; }
        public decimal CpuUsage { get; set; }
        public int ProcessorCount { get; set; }
    }
}
