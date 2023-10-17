using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;//.PerformanceCounter;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace RAM_Monitor
{
    internal static class Helpers
    {
        internal static readonly string DateFormat = "ddMMMyy_HHmmss".ToString();
        internal static StringBuilder builder;
        internal static PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        internal static void PerformPingOperation(int timeInSeconds, double totalRAM_MB, int delayBetweenSubsequentPingsInSecs)
        {

            var stopWatch = Stopwatch.StartNew();

            while (stopWatch.Elapsed.TotalSeconds < timeInSeconds)
            {
                double freeRAM_MB = ((ramCounter.NextValue()) / 1024);
                var ramInUse = totalRAM_MB - freeRAM_MB;
                var utilization = Math.Round((ramInUse / totalRAM_MB) * 100, 2);
                builder.AppendLine(($"{DateTime.Now.ToString(DateFormat)},{Math.Round(ramInUse, 2)},{Math.Round(freeRAM_MB, 2)},{utilization}%"));
                Thread.Sleep(delayBetweenSubsequentPingsInSecs * 1000);
            }
        }

        internal static double Total_RAM_GB()
        {
            try
            {
                ManagementObjectCollection queryCollection = (new ManagementObjectSearcher(new ObjectQuery("SELECT * FROM Win32_ComputerSystem"))).Get();
                ManagementObject systemInfo = queryCollection.Cast<ManagementObject>().FirstOrDefault();

                if (systemInfo != null)
                {
                    // Get the total physical memory (RAM) in bytes
                    ulong totalRamBytes = (ulong)systemInfo["TotalPhysicalMemory"];
                    double totalRamGB = totalRamBytes / (1024.0 * 1024.0 * 1024.0);

                    return Math.Round(totalRamGB, 3);
                    //return $"Total RAM: {totalRamGB:F3} MB";
                }
                else
                {
                    var x = "Unable to retrieve system information.";
                    Console.WriteLine(x);
                    throw new Exception(x);
                }
            }
            catch (ManagementException e)
            {
                throw new Exception("Error: " + e.Message);
            }
        }
    }
}
