using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Discord.WebSocket;

namespace AnotherMyouri.Mischief
{
    public static class Utilities
    {
        private static readonly Random Random = new Random();
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPhysicallyInstalledSystemMemory(out long totalMemoryInKilobytes);

        private static float InstalledMemory()
        {
            GetPhysicallyInstalledSystemMemory(out var memKb);
            var memoryInMb = (float) memKb / 1024;
            return memoryInMb;
        }

        public static float RamUsage()
        {
            var freeMemory = GetRamCounter();
            var totalMemory = InstalledMemory();
            var usedMemory = totalMemory - freeMemory;
            var memUsage = usedMemory / totalMemory * 100;
            return memUsage;
        }

        public static float GetCpuCounter()
        {
            var cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
            var initialValue = cpuCounter.NextValue();
            Thread.Sleep(1000);
            var cpuValue = cpuCounter.NextValue();
            return cpuValue;
        }

        private static float GetRamCounter()
        {
            var memCounter = new PerformanceCounter
            {
                CategoryName = "Memory",
                CounterName = "Available MBytes"
            };
            var initialValue = memCounter.NextValue();
            Thread.Sleep(1000);
            var ramValue = memCounter.NextValue();
            return ramValue;
        }

        private static int GetRolePosition(SocketGuildUser u1)
        {
            return u1.Roles.Select(r => r.Position).Prepend(0).Max();
        }

        public static bool CanInteractRole(SocketGuildUser user, SocketRole role)
        {
            return GetRolePosition(user) > role.Position;
        }

        public static bool CanInteractUser(SocketGuildUser u1, SocketGuildUser u2)
        {
            return GetRolePosition(u1) > GetRolePosition(u2);
        }

        public static string GetRandomAlphaNumeric(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static int RandomColor()
        {
            return Random.Next(1, 255);
        }

    }
}