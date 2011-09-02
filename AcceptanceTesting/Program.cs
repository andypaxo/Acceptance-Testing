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
            var assemblyLoader  = args.Length > 0
                ? new AssemblyLoader().InitializeWith(args[0])
                : new AssemblyLoader().InitializeWith(Assembly.GetExecutingAssembly());

            var testRunner = new TestRunner
            {
                OutputStream = Console.OpenStandardOutput(),
                AssemblyLoader = assemblyLoader
            };
            testRunner.Run(input);
        }
    }
}
