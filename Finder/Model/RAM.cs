using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class RAM {
        public long TotalMemoryKB { get; set; }
        public long FreeMemoryKB { get; set; }
        public long AvaliableMemoryKB { get; set; }

        public static RAM GetRAMInfo() {
            RAM ramInfo = new();

            try {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem")) {
                    foreach (var item in searcher.Get()) {
                        ramInfo.TotalMemoryKB = Convert.ToInt64(item["TotalVisibleMemorySize"] ?? 0);
                        ramInfo.FreeMemoryKB = Convert.ToInt64(item["FreePhysicalMemory"] ?? 0);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter informações da memória: {ex.Message}");
            }

            try {
                using (var ramCounter = new PerformanceCounter("Memory", "Available MBytes")) {
                    ramCounter.NextValue();
                    System.Threading.Thread.Sleep(100);
                    ramInfo.AvaliableMemoryKB = (long)(ramCounter.NextValue() * 1024);
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter memória disponível: {ex.Message}");
            }

            return ramInfo;
        }

        public override string ToString() {
            return $"Memória Total: {(TotalMemoryKB / (1024 * 1024)):F2} GB\n" +
                   $"Memória Livre: {(FreeMemoryKB / (1024 * 1024)):F2} GB\n" +
                   $"Memória Disponível: {(AvaliableMemoryKB / (1024 * 1024)):F2} GB";
        }
    }
}
