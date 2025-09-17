using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class Software {
        public string SoftwareName { get; set; }
        public string Owner { get; set; }
        public string Version { get; set; }
        public DateTime InstallDate { get; set; }
    }
}
