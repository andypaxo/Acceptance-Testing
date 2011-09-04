using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AcceptanceTesting
{
    internal class StepProcessor
    {

        private static readonly Dictionary<string, MethodInfo> Processors = GetStepProcessors();
        private static Dictionary<string, MethodInfo> GetStepProcessors()
        {
            var processors = new Dictionary<string, MethodInfo>();
            foreach (var method in typeof(TestRunner).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                var attribute = method.GetCustomAttributes(typeof(ProcessorAttribute), false).FirstOrDefault();
                if (attribute != null)
                    foreach (var token in ((ProcessorAttribute)attribute).Tokens)
                        processors[token] = method;
            }
            return processors;
        }

        private readonly TestRunner testRunner;

        public StepProcessor(TestRunner testRunner)
        {
            this.testRunner = testRunner;
        }

        public void Process(string step)
        {
            const string keywordExtractor = @"^(?<keyword>[\S]*)";
            var keyword = Regex.Match(step, keywordExtractor).Groups["keyword"].Value;
            step = step.Substring(keyword.Length + 1);

            if (Processors.ContainsKey(keyword))
                Processors[keyword].Invoke(testRunner, new[] { step });
            else
                throw new Exception("I do not know this keyword: " + keyword);
        }
    }

    internal class ProcessorAttribute : Attribute
    {
        public string[] Tokens { get; private set; }

        public ProcessorAttribute(params string[] tokens)
        {
            Tokens = tokens;
        }
    }
}