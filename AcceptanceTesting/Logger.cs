using System;

namespace AcceptanceTesting
{
    public interface Logger : IDisposable
    {
        void WriteResult(string line, StepResult result);
    }
}