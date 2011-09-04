using System;

namespace AcceptanceTesting
{
    [LoggerName("console")]
    public class ConsoleLogger : Logger
    {
        public override void WriteResult(string line, StepResult result)
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

        public override void Dispose()
        {
            Console.ReadLine();
        }
    }
}