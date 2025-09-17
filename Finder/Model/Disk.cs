using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class Disk {
        public string DiskName { get; set; }
        public long TotalSizeBytes { get; set; }
        public long FreeSpaceBytes { get; set; }
        public decimal TotalSizeGB { get; set; }
        public decimal FreeSpaceGB { get; set; }
    }
}
