using System;
using System.Reflection;

namespace AcceptanceTesting
{
    class Program
    {
        private const string input =
@"Given I have logged in
Then I should see a failed login";

        static void Main(string[] args)
        {
            var assembly = args.Length > 0
                ? Assembly.LoadFrom(args[0])
                : Assembly.GetExecutingAssembly();
            var assemblyLoader = AssemblyLoader.CreateFrom(assembly);

            var testRunner = new TestRunner
            {
                OutputStream = Console.OpenStandardOutput(),
                AssemblyLoader = assemblyLoader
            };
            testRunner.Run(input);
        }
    }
}
