using System;

namespace AcceptanceTesting
{
    [LoggerName("console")]
    public class ConsoleLogger : Logger
    {
        public override void WriteResult(string line, StepResult result)
        {
            var marker =
                result.Status == StepStatus.Failed  ? 'X' :
                result.Status == StepStatus.Passed ? 'o' :
                result.Status == StepStatus.NotFound ? '?' :
                result.Status == StepStatus.Ignored ? '-' :
                '!';

            Console.WriteLine("{0} {1}", marker, line);
            if (result.Exception != null)
                Console.WriteLine(result.Exception);
        }

        public override void Dispose()
        {
        }
    }
}