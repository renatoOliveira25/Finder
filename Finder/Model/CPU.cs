using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class CPU {
        public string CpuName { get; set; }
        public string Architecture { get; set; }
        public int NumberOfCores { get; set; }
        public float CpuUsage { get; set; }
        public int ProcessorCount { get; set; }

        public static CPU GetCPUInfo() {
            CPU cpuInfo = new();
            cpuInfo.ProcessorCount = Environment.ProcessorCount;
            try {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor")) {
                    foreach (var item in searcher.Get()) {
                        cpuInfo.CpuName = item["Name"]?.ToString() ?? "Nome desconhecido";
                        cpuInfo.Architecture = item["Architecture"]?.ToString() ?? "Arquitetura desconhecida";
                        cpuInfo.NumberOfCores = Convert.ToInt32(item["NumberOfCores"] ?? 0);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter informações da CPU: {ex.Message}");
            }

            try {
                using (var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total")) {
                    cpuCounter.NextValue();
                    Thread.Sleep(1000);
                    cpuInfo.CpuUsage = cpuCounter.NextValue();
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter uso da CPU: {ex.Message}");
            }

            return cpuInfo;
        }

        public override string ToString() {
            return $"Processador: {CpuName}\nArquitetura: {Architecture}\nQuantidade núcleos: {NumberOfCores}\nUso da CPU: {CpuUsage}\nNúmero de processadores: {ProcessorCount}";
        }
    }
}
