namespace AcceptanceTesting
{
    public class TestRunner
    {
        private readonly AssemblyLoader assemblyLoader;
        private readonly Logger output;
        private readonly StepProcessor processor;
        public bool AllPassed { get; private set; }

        public TestRunner(Logger output, AssemblyLoader assemblyLoader)
        {
            AllPassed = true;
            this.output = output;
            this.assemblyLoader = assemblyLoader;
            processor = new StepProcessor(this);
        }

        public void Run(string input)
        {
            foreach (var line in input.Split('\n'))
                ProcessLine(line);
        }

        private void ProcessLine(string line)
        {
            line = line.Trim();
            if (line.Length > 0)
                processor.Process(line);
        }

        [Processor("Scenario:")]
        private void StartScenario(string scenarioName)
        {
            AllPassed = true;
            output.WriteScenarioStart(scenarioName);
        }

        [Processor("Given", "And", "When", "Then")]
        private void ProcessStep(string step)
        {
            if (AllPassed)
            {
                var result = assemblyLoader.ResultOf(step);
                AllPassed &= (result.Status == StepStatus.Passed);
                output.WriteResult(step, result);
            }
            else
            {
                output.WriteResult(step, StepResult.Ignored);
            }
        }
    }
}