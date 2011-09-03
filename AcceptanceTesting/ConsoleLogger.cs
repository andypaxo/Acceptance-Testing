using System;

namespace AcceptanceTesting
{
    public class ConsoleLogger : Logger
    {
        public void WriteResult(string line, StepResult result)
        {
            var marker =
                result.Exception != null ? 'X' :
                result == StepResult.Ok ? 'o' :
                result == StepResult.NotFound ? '?' :
                result == StepResult.Ignored ? '-' :
                '!';

            Console.WriteLine("{0} {1}", marker, line);
            if (result.Exception != null)
                Console.WriteLine(result.Exception);
        }

        public void Dispose()
        {
            Console.ReadLine();
        }
    }
}