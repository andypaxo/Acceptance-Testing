using System;
using System.IO;

namespace AcceptanceTesting
{
    class Program
    {
        static int Main(string[] args)
        {
            var assemblyLoader  = new AssemblyLoader().InitializeWith(args[0]);

            string input;
            using (var reader = new StreamReader(args[1]))
                input = reader.ReadToEnd();

            var testRunner = new TestRunner
            {
                OutputStream = Console.OpenStandardOutput(),
                AssemblyLoader = assemblyLoader
            };
            testRunner.Run(input);

            Console.ReadLine();

            return testRunner.AllPassed ? 0 : 1;
        }
    }
}
