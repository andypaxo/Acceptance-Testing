using System.IO;
using System.Text.RegularExpressions;

namespace AcceptanceTesting
{
    public class TestRunner
    {
        public Stream OutputStream;
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
            var prose = line.Substring(keyword.Length + 1);

            output.WriteLine("{0}:{1}", keyword, prose);
        }
    }
}