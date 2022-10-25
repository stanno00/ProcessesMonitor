using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ProcessesMonitorTest")]

namespace ProcessesMonitor
{
    class ProcessesChecker
    {
        public static void Main(string[] arg)
        {
            Console.WriteLine("Enter Name maxLifeTime monitoringFrequency");
            string[] s = GetInput(Console.ReadLine().Split());
            while (s.Length != 3)
            {
                Console.WriteLine("Enter Name maxLifeTime monitoringFrequency");
                s = GetInput(Console.ReadLine().Split());
            }

            string processesName = s[0];
            int maxLifeTime = 0;
            int monitorFrequency = 0;

            bool correctTypeInput = false;
            while (!correctTypeInput)
            {
                var input = CorrectInput(s);
                if (input.Length == 3)
                {
                    maxLifeTime = Int32.Parse(s[1]);
                    monitorFrequency = Int32.Parse(s[2]);
                }
                if (correctTypeInput == false)
                {
                    s = GetInput(Console.ReadLine().Split());
                }
            }

            if (processesName.Contains(".exe"))
            {
                processesName.Replace(".exe", "");
            }

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(monitorFrequency);
            string daco = "";

            var timer = new Timer(
                (e) => { daco = CheckForProcesses(processesName, maxLifeTime); },
                null, startTimeSpan, periodTimeSpan);

            Console.WriteLine("Press q to exit");

            do
            {
            } while (Console.ReadKey(true).Key != ConsoleKey.Q);

            Console.WriteLine("Shutting down");
        }

        public static string CheckForProcesses(string processesName, int maxLifeTime)
        {
            Process[] allProcesses = Process.GetProcessesByName(processesName);

            if (allProcesses.Length != 0)
            {
                if (allProcesses[0].StartTime + TimeSpan.FromMinutes(maxLifeTime) < DateTime.Now)
                {
                    allProcesses[0].Kill();
                    Console.WriteLine("Processes " + processesName + " was ended at " + DateTime.Now);
                    return "Processes " + processesName + " was ended at " + DateTime.Now;
                }
            }

            return "";
        }

        public static string[] GetInput(string[] s)
        {
            if (s.Length == 3)
            {
                return s;
            }

            if (s.Length > 3)
            {
                Console.WriteLine("Too many parameters");
            }

            if (s.Length < 3)
            {
                Console.WriteLine("Missing parameters");
            }

            return new string[] { };
        }

        public static string[] CorrectInput(string[] input)
        {
            bool correctTypeInput = true;
            int maxLifeTime = 0;
            int monitorFrequency = 0;
            
            if (!int.TryParse(input[1], out maxLifeTime))
            {
                correctTypeInput = false;
                Console.WriteLine("Second parameter must be a number");
            }

            if (!int.TryParse(input[2], out monitorFrequency))
            {
                correctTypeInput = false;
                Console.WriteLine("Third parameter must be a number");
            }

            if (correctTypeInput)
            {
                return input;
            }

            return new string[] { };
        }
    }
}