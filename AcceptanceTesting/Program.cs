using System.IO;

namespace AcceptanceTesting
{
    class Program
    {
        static int Main(string[] args)
        {
            var assemblyUnderTest = args[0];
            var inputFile = args[1];
            var outputType = args[2];

            var assemblyLoader = new AssemblyLoader().InitializeWith(assemblyUnderTest);
            var input = GetInput(inputFile);

            using (var output = Logger.GetLogger(outputType))
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
