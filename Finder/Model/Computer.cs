using Microsoft.Win32;
using System.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Model {
    internal class Computer {
        public string? MachineName { get; set; }
        public string? OperatingSystem { get; set; }
        public string? WindowsVersion { get; set; }
        public DateTime InstallDate { get; set; }
        public string? CurrentUser { get; set; }
        public string? ComputerModel { get; set; }
        public string? ServiceTag { get; set; }
        public string? WindowsSerial { get; set; }

        public static Computer GetComputerInfo() {
            return new Computer {
                MachineName = Environment.MachineName,
                OperatingSystem = Environment.OSVersion.ToString(),
                WindowsVersion = GetWindowsVersion(),
                ComputerModel = GetComputerModel(),
                CurrentUser = Environment.UserName,
                ServiceTag = GetServiceTag(),
                InstallDate = GetInstallDate(),
                WindowsSerial = GetWindowsSerial()
            };
        }

        private static string GetComputerModel() {
            try {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem")) {
                    foreach (var item in searcher.Get()) {
                        return item["Manufacturer"]?.ToString() + " " + item["Model"]?.ToString();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter modelo do computador: {ex.Message}");
            }
            return "Modelo desconhecido";
        }

        static string GetServiceTag() {
            try {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS")) {
                    foreach (var item in searcher.Get()) {
                        return item["SerialNumber"]?.ToString().Trim();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao obter Service Tag: {ex.Message}");
            }
            return "Service Tag não encontrada";
        }

        private static string GetWindowsVersion() {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion")) {
                if (key != null) {
                    string? productName = key.GetValue("ProductName")?.ToString();
                    string buildNumber = key.GetValue("CurrentBuild")?.ToString();

                    // Mapeamento de builds para versões comerciais
                    Dictionary<string, string> windowsVersions = new()
                    {
                        // Windows 10
                        { "10240", "Windows 10 1507" },
                        { "10586", "Windows 10 1511" },
                        { "14393", "Windows 10 1607" },
                        { "15063", "Windows 10 1703" },
                        { "16299", "Windows 10 1709" },
                        { "17134", "Windows 10 1803" },
                        { "17763", "Windows 10 1809" },
                        { "18362", "Windows 10 1903" },
                        { "18363", "Windows 10 1909" },
                        { "19041", "Windows 10 2004" },
                        { "19042", "Windows 10 20H2" },
                        { "19043", "Windows 10 21H1" },
                        { "19044", "Windows 10 21H2" },
                        { "19045", "Windows 10 22H2" },

                        // Windows 11
                        { "22000", "Windows 11 21H2" },
                        { "22621", "Windows 11 22H2" },
                        { "22631", "Windows 11 23H2" },
                        { "26100", "Windows 11 24H2" }
                    };

                    return windowsVersions.TryGetValue(buildNumber, out string? value) ? value : $"{productName} (Build {buildNumber})";
                }
            }
            return "Não foi possível obter a versão";
        }

        public static DateTime GetInstallDate() {
            using (var searcher = new ManagementObjectSearcher("SELECT InstallDate FROM Win32_OperatingSystem")) {
                foreach (ManagementObject os in searcher.Get()) {
                    string installDateStr = os["InstallDate"].ToString();
                    return ManagementDateTimeConverter.ToDateTime(installDateStr);
                }
            }
            return DateTime.MinValue; // Caso não consiga recuperar
        }

        public static string GetWindowsSerial() {
            try {
                string keyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                string digitalProductIdValueName = "DigitalProductId";

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, false)) {
                    if (key != null) {
                        byte[] digitalProductId = key.GetValue(digitalProductIdValueName) as byte[];
                        if (digitalProductId != null) {
                            return DecodeProductKey(digitalProductId);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao acessar o registro do Windows: {ex.Message}");
            }

            return "Serial não encontrado";
        }

        private static string DecodeProductKey(byte[] digitalProductId) {
            const int keyStartIndex = 52;
            char[] digits = { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'M', 'P', 'Q', 'R', 'T', 'V', 'W', 'X', 'Y', '2', '3', '4', '6', '7', '8', '9' };
            char[] decodedChars = new char[29];
            byte[] key = new byte[15];
            Array.Copy(digitalProductId, keyStartIndex, key, 0, 15);

            for (int i = 28; i >= 0; i--) {
                if ((i + 1) % 6 == 0) {
                    decodedChars[i] = '-';
                }
                else {
                    int acc = 0;
                    for (int j = 14; j >= 0; j--) {
                        acc = (acc << 8) + key[j];
                        key[j] = (byte)(acc / 24);
                        acc %= 24;
                    }
                    decodedChars[i] = digits[acc];
                }
            }

            return new string(decodedChars);
        }

        public override string ToString() {
            return $"Nome do computador: {MachineName}\nSistema Operacional: {OperatingSystem}\nVersão do Windows: {WindowsVersion}\nData de instalação: {InstallDate}\nUsuário atual: {CurrentUser}\nModelo: {ComputerModel}\nService Tag: {ServiceTag}\nSerial: {WindowsSerial}";
        }
    }
}
