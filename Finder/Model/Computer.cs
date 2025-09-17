using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class Computer {
        public string MachineName { get; set; }
        public string OperatingSystem { get; set; }
        public string WindowsVersion { get; set; }
        public DateTime InstallDate { get; set; }
        public string CurrentUser { get; set; }
        public string ComputerModel { get; set; }
        public string ServiceTag { get; set; }
        public string WindowsSerial { get; set; }
    }
}
