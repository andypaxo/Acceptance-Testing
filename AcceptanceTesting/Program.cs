using System.IO;

namespace AcceptanceTesting
{
    class Program
    {
        static int Main(string[] args)
        {
            var assemblyLoader = new AssemblyLoader().InitializeWith(args[0]);
            var input = GetInput(args[1]);
            
            using (var output = new ConsoleLogger())
                return RunTests(output, assemblyLoader, input) ? 0 : 1;
        }

        private static string GetInput(string filename)
        {
            string input;
            using (var reader = new StreamReader(filename))
                input = reader.ReadToEnd();
            return input;
        }

        private static bool RunTests(Logger output, AssemblyLoader assemblyLoader, string input)
        {
            var testRunner = new TestRunner
            {
                Output = output,
                AssemblyLoader = assemblyLoader
            };
            testRunner.Run(input);

            return testRunner.AllPassed;
        }
    }
}
