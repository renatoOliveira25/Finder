using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Finder.Model {
    internal class Disk {
        public string DiskName { get; set; }
        public long TotalSizeBytes { get; set; }
        public long FreeSpaceBytes { get; set; }
        public decimal TotalSizeGB { get; set; }
        public decimal FreeSpaceGB { get; set; }

        public static List<Disk> GetDiskInfo() {
            List<Disk> disks = new();

            try {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType=3")) {
                    foreach (var item in searcher.Get()) {
                        string name = item["Name"]?.ToString() ?? "Nome desconhecido";
                        long totalSize = item["Size"] != null ? Convert.ToInt64(item["Size"]) : 0;
                        long freeSpace = item["FreeSpace"] != null ? Convert.ToInt64(item["FreeSpace"]) : 0;

                        disks.Add(new Disk(name, totalSize, freeSpace));
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter informações dos discos: {ex.Message}");
            }

            return disks;
        }

        public Disk(string name, long totalSize, long freeSpace) {
            DiskName = name;
            TotalSizeBytes = totalSize;
            FreeSpaceBytes = freeSpace;
            TotalSizeGB = totalSize > 0 ? Math.Round((decimal)totalSize / (1024 * 1024 * 1024), 2) : 0;
            FreeSpaceGB = freeSpace > 0 ? Math.Round((decimal)freeSpace / (1024 * 1024 * 1024), 2) : 0;
        }

        public override string ToString() {
            return $"Nome: {DiskName}, Tamanho total: {TotalSizeGB:F2} GB, Espaço livre: {FreeSpaceGB:F2} GB";
        }
    }
}
