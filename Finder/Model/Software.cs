using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class Software {
        public string SoftwareName { get; set; }
        public string Owner { get; set; }
        public string Version { get; set; }
        public DateTime InstallDate { get; set; }

        public static List<Software> GetInstalledSoftware() {
            List<Software> softwares = new();

            string[] registryKeys = new[]
            {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        };

            foreach (string keyPath in registryKeys) {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath)) {
                    if (key == null) continue;

                    foreach (string subkeyName in key.GetSubKeyNames()) {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName)) {
                            string name = subkey?.GetValue("DisplayName") as string;
                            string publisher = subkey?.GetValue("Publisher") as string;
                            string version = subkey?.GetValue("DisplayVersion") as string;
                            string installDateStr = subkey?.GetValue("InstallDate") as string;

                            if (string.IsNullOrWhiteSpace(name)) continue;

                            DateTime installDate = DateTime.MinValue;
                            if (!string.IsNullOrWhiteSpace(installDateStr) && installDateStr.Length == 8) {
                                // Formato esperado: YYYYMMDD
                                if (DateTime.TryParseExact(installDateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)) {
                                    installDate = parsedDate;
                                }
                            }

                            softwares.Add(new Software {
                                SoftwareName = name,
                                Owner = publisher ?? "Desconhecido",
                                Version = version ?? "N/A",
                                InstallDate = installDate
                            });
                        }
                    }
                }
            }

            return softwares;
        }

        public override string ToString() {
            string dataInstalacao = InstallDate != DateTime.MinValue
                ? InstallDate.ToString("dd/MM/yyyy")
                : "Desconhecida";

            return $"Nome: {SoftwareName}\n" +
                   $"Fabricante: {Owner}\n" +
                   $"Versão: {Version}\n" +
                   $"Data de Instalação: {dataInstalacao}\n" + 
                   $"------------------------------------------------";
        }
    }
}
