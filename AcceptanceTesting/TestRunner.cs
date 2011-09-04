using System.Text.RegularExpressions;

namespace AcceptanceTesting
{
    public class TestRunner
    {
        public Logger Output { get; set; }
        public AssemblyLoader AssemblyLoader { get; set; }
        public bool AllPassed { get; private set; }

        private static readonly string[] StepTokens = new[] { "Given", "When", "Then" };
        private const string KeywordExtractor = @"^(?<keyword>[\S]*)";

        public TestRunner()
        {
            AllPassed = true;
        }

        public void Run(string input)
        {
            foreach (var line in input.Split('\n'))
                ProcessLine(line);
        }

        private void ProcessLine(string line)
        {
            line = line.Trim();
            var keyword = Regex.Match(line, KeywordExtractor).Groups["keyword"].Value;
            var step = line.Substring(keyword.Length + 1);

            if (AllPassed)
            {
                var result = AssemblyLoader.ResultOf(step);
                AllPassed &= (result.Status == StepStatus.Passed);
                Output.WriteResult(line, result);
            }
            else
            {
                Output.WriteResult(line, StepResult.Ignored);
            }
        }
    }
}