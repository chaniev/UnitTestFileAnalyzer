using System;
using System.IO;
using Analyzer;

namespace UnitTestFileAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 1)
            {
                Console.WriteLine("set 2 parameters!");
                Console.ReadLine();
                return;
            }
            string filename = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                var pac = new ProjectAnalyzerCollection(args[i]);
                pac.Analyze();
                if (string.IsNullOrEmpty(filename))
                    pac.Save(Console.WriteLine);
                else
                {
                    using (var file = new StreamWriter(filename, true))
                    {
                        pac.Save(file.WriteLine);
                    }
                }
            }
        }
    }
}