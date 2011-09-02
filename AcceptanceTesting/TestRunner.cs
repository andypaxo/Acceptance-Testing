using System.IO;
using System.Text.RegularExpressions;

namespace AcceptanceTesting
{
    public class TestRunner
    {
        public Stream OutputStream { get; set; }
        public AssemblyLoader AssemblyLoader { get; set; }
        private StreamWriter output;

        private static readonly string[] StepTokens = new[] {"Given", "When"};
        private const string KeywordExtractor = @"^(?<keyword>[\S]*)";

        public void Run(string input)
        {
            output = new StreamWriter(OutputStream);
            using (output)
                ChurnInput(input);
        }

        private void ChurnInput(string input)
        {
            foreach (var line in input.Split('\n'))
                ProcessLine(line);
        }

        private void ProcessLine(string line)
        {
            var keyword = Regex.Match(line, KeywordExtractor).Groups["keyword"].Value;
            var step = line.Substring(keyword.Length + 1);

            if (AssemblyLoader.FindMethod(step))
                output.WriteLine("{0} {1}", AssemblyLoader.Ok(step) ? "/" : "X", line);
            else
                output.WriteLine("- {0}", line);
        }
    }
}