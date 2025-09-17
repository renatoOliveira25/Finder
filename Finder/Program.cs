using Finder.Model;

var computerInfo = Computer.GetComputerInfo();
var cpuInfo = CPU.GetCPUInfo();
var diskInfo = Disk.GetDiskInfo();
var ramInfo = RAM.GetRAMInfo();
var softwareInfo = Software.GetInstalledSoftware();

Console.WriteLine(computerInfo.ToString() + "\n");

Console.WriteLine(cpuInfo.ToString() + "\n");

foreach(var disk in  diskInfo) {
    Console.WriteLine(disk.ToString());
}

Console.WriteLine("\n" + ramInfo.ToString() + "\n");

foreach (var software in softwareInfo) {
    Console.WriteLine(software.ToString());
}