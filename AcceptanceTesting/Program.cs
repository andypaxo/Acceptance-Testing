using System;
using System.Reflection;

namespace AcceptanceTesting
{
    class Program
    {
        private const string input =
@"Given I want to do things
Then things should be done";

        static void Main(string[] args)
        {
            var testRunner = new TestRunner
            {
                OutputStream = Console.OpenStandardOutput(),
                AssemblyLoader = AssemblyLoader.CreateFrom(Assembly.GetExecutingAssembly())
            };
            testRunner.Run(input);

            Console.ReadKey();
        }
    }
}
