﻿using System.IO;
using System.Text.RegularExpressions;

namespace AcceptanceTesting
{
    public class TestRunner
    {
        public Stream OutputStream { get; set; }
        public AssemblyLoader AssemblyLoader { get; set; }
        private StreamWriter output;
        public bool AllPassed { get; private set; }

        private static readonly string[] StepTokens = new[] { "Given", "When", "Then" };
        private const string KeywordExtractor = @"^(?<keyword>[\S]*)";

        public TestRunner()
        {
            AllPassed = true;
        }

        public void Run(string input)
        {
            output = new StreamWriter(OutputStream);
            using (output)
                ChurnInput(input);
        }

        private void ChurnInput(string input)
        {
            DisplayLoadedMethods();

            foreach (var line in input.Split('\n'))
                ProcessLine(line);
        }

        private void DisplayLoadedMethods()
        {
            output.WriteLine("Methods found:");
            foreach (var method in AssemblyLoader.AllMethods())
                output.WriteLine(method);
            output.WriteLine();
        }

        private void ProcessLine(string line)
        {
            line = line.Trim();
            var keyword = Regex.Match(line, KeywordExtractor).Groups["keyword"].Value;
            var step = line.Substring(keyword.Length + 1);

            if (AllPassed && AssemblyLoader.FindMethod(step))
            {
                var result = AssemblyLoader.ResultOf(step);
                AllPassed &= result.Passed;
                output.WriteLine("{0} {1}", result.Passed ? "/" : "X", line);
                if (!string.IsNullOrEmpty(result.Exception))
                    output.WriteLine("  {0}", result.Exception);
            }
            else
            {
                output.WriteLine("- " + line);
            }
        }
    }
}