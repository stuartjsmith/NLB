using System;
using System.IO;
using System.Threading;

namespace NLB
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string folderPath = args[0];
            string writeableLocation = args[1];
            string id = args[2];

            string flagFile = Path.Combine(writeableLocation, id + "_END");
            File.Delete(flagFile);
            string resultsFile = Path.Combine(writeableLocation, id + "_RESULTS");
            File.Delete(resultsFile);

            Console.Out.WriteLine("Now monitoring " + folderPath + " for all file changes");
            Console.Out.WriteLine("Will continue monitoring until file " + flagFile + " is created");
            Console.Out.WriteLine("Results will be written to " + resultsFile);

            var buildWatcher = new BuildWatcher(folderPath, flagFile);
            while (File.Exists(flagFile) == false)
            {
                Thread.Sleep(5000);
            }

            buildWatcher.EndMonitoring();
            // Write the string to a file.            
            var file = new StreamWriter(resultsFile);
            foreach (string filePath in buildWatcher.BuildArtefacts)
            {
                file.WriteLine(filePath);
            }

            file.Close();
        }
    }
}