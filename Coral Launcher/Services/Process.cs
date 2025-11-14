using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using CoralLauncher.Core;

namespace Coral_Launcher.Services
{
    internal class ProcessMonitor
    {
        private Thread monitoringThread;
        private bool isMonitoring;
        private readonly string[] processNames =
{
    "HTTPDebuggerUI",
    "Injector",
    "Kernel Injector Usermode",
    "Extreme Injector v3",
    "ProcessHacker",
    "UuuClient",
    "Cheat Engine",
    "cheatengine-x86_64-SSE4-AVX2",
    "cheatengine-x86_64",
    "cheatengine-i386",
    "Wireshark",
    "CheatEngine",
    "x64dbg",
    "OllyDbg",
    "Winject",
    "ExtremeInjector",
    "GameGuardian",
    "FridaServer",
    "PEInjector",
    "HackersDelight",
    "kdmapper",
    "mapper",
    "karima",
    "Undetected Cheat",
    "Google Cheatz",
    "Google Cheats",
    "Karima Cheat",
    "Karima Free",
    "Karima UD",
    "Raax",
    "Raax Cheat",
    "dotPeek64",
    "dotPeek32",
    "dotpeek",
    "perfmon",
    "1hack",
    "onehack",
    "cheatt",
    "cheetos",
    "ud cheat",
    "cheat",
    "ud cheeto"
};

        private readonly string[] processesToKill =
        {
            "FortniteClient-Win64-Shipping",
            "Coral_EAC",
            "FortniteClient-Win64-Shipping_BE",
            "FortniteClient-Win64-Shipping_EAC",
            "FortniteLauncher"
        };

        public void StartMonitoring()
        {
            if (isMonitoring) return;
            isMonitoring = true;
            monitoringThread = new Thread(MonitorProcess) { IsBackground = true };
            monitoringThread.Start();
        }

        public void StopMonitoring()
        {
            isMonitoring = false;
            monitoringThread?.Join();
        }

        private void MonitorProcess()
        {
            while (isMonitoring)
            {
                foreach (var processName in processNames)
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    if (processes.Length > 0)
                    {
                        foreach (var process in processes)
                        {
                            try
                            {
                                process.Kill();
                                process.WaitForExit();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error terminating process {processName}: {ex.Message}");
                            }
                        }

                        foreach (var processNameToKill in processesToKill)
                        {
                            Process[] processesToKillArray = Process.GetProcessesByName(processNameToKill);
                            foreach (var processToKill in processesToKillArray)
                            {
                                try
                                {
                                    processToKill.Kill();
                                    processToKill.WaitForExit();
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Error terminating process {processNameToKill}: {ex.Message}");
                                }
                            }
                        }

                        // Close the current application immediately
                        Environment.Exit(0);

                        return; // Ensures no further checks are performed after the exit
                    }
                }

                Thread.Sleep(1500);
            }
        }
    }
}

