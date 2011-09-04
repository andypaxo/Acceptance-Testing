using System;

namespace AcceptanceTesting
{
    [Serializable]
    public class StepResult
    {
        public StepStatus Status { get; private set; }
        public string Exception { get; private set; }

        public static readonly StepResult Ok = new StepResult {Status = StepStatus.Passed};
        public static readonly StepResult NotFound = new StepResult {Status = StepStatus.NotFound};
        public static readonly StepResult Ignored = new StepResult {Status = StepStatus.Ignored};

        public static StepResult Fail(string exception)
        {
            return new StepResult
            {
                Status = StepStatus.Failed,
                Exception = exception
            };
        }
    }

    [Serializable]
    public enum StepStatus
    {
        Unknown,
        Passed,
        Failed,
        NotFound,
        Ignored
    }
}