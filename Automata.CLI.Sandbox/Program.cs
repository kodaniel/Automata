using System.Diagnostics;

namespace Automata.CLI.Sandbox
{
    internal class Program
    {
        static string ExecutablePath => 
            Path.Combine(FindSolutionDirectoryInfo()!.FullName, @"Automata.CLI\bin\Debug\net6.0\Automata.CLI.exe");

        static void Main(string[] args)
        {
            bool debugMode = !Debugger.IsAttached;
            Process cliProcess = Process.GetCurrentProcess();
            Process vsProcess = VisualStudioAttacher.GetVisualStudioForSolution("Automata.sln");

            do
            {
                WriteWithColor("autom ", ConsoleColor.Green);

                var cliCommand = Console.ReadLine();
                if (cliCommand == "quit")
                    break;

                Process automProcess = CreateAutomataProcess(cliCommand, debugMode);
                automProcess.Start();

                if (debugMode)
                    VisualStudioAttacher.AttachVisualStudioToProcess(vsProcess, automProcess);

                automProcess.WaitForExit();

                Console.Write(automProcess.StandardOutput.ReadToEnd());

                VisualStudioAttacher.SetForegroundWindow(cliProcess.Handle);

            } while (true);
        }

        static void WriteWithColor(string value, ConsoleColor color)
        {
            var oldcolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = oldcolor;
        }

        static Process CreateAutomataProcess(string? cmd, bool debugMode)
        {
            Process automata = new Process();
            automata.StartInfo.FileName = ExecutablePath;
            automata.StartInfo.Arguments = (debugMode ? "--debug " : "") + cmd;
            automata.StartInfo.RedirectStandardInput = true;
            automata.StartInfo.RedirectStandardOutput = true;
            automata.StartInfo.CreateNoWindow = true;
            automata.StartInfo.UseShellExecute = false;

            return automata;
        }

        static DirectoryInfo? FindSolutionDirectoryInfo(string? currentPath = null)
        {
            var directory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}