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
            var interactive = args.Length > 3 && args[3] == "interactive";

            var assemblyLoader = new AssemblyLoader().InitializeWith(assemblyUnderTest);
            var input = GetInput(inputFile);

            int result;
            using (var output = Logger.GetLogger(outputType))
                result = RunTests(output, assemblyLoader, input) ? 0 : 1;

            if (interactive)
                System.Console.ReadKey();

            return result;
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
