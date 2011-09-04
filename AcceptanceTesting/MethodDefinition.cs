using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AcceptanceTesting
{
    internal class MethodDefinition
    {
        public bool IsScenarioSetup { get; private set; }

        private readonly object instance;
        private readonly MethodInfo method;
        private readonly Regex name;
        private ParameterInfo[] parameters;

        public MethodDefinition(object instance, MethodInfo method)
        {
            this.instance = instance;
            this.method = method;

            IsScenarioSetup = method.GetCustomAttributes(typeof(ScenarioSetupAttribute), true).Length > 0;
            if (!IsScenarioSetup)
            {
                parameters = method.GetParameters();
                name = new Regex(GetPattern(method));
            }
            else
            {
                parameters = new ParameterInfo[0];
                name = null;
            }
        }

        private string GetPattern(MethodInfo method)
        {
            var words = method.Name.Split('_');

            foreach (var parameter in parameters)
            {
                var paramLocation = Array.IndexOf(words, parameter.Name);
                if (paramLocation >= 0)
                    words[paramLocation] = string.Format("(?<{0}>'[^']*')", parameter.Name);
            }

            return string.Join(" ", words);
        }

        public void Invoke(string step)
        {
            var invocationParameters = parameters.Length == 0
            ? null
            : GetParametersFrom(step);
            method.Invoke(instance, invocationParameters);
        }

        private object[] GetParametersFrom(string step)
        {
            var match = name.Match(step);
            return (
            from parameter in parameters
            select match.Groups[parameter.Name].Value
            ).ToArray();
        }

        public bool Matches(string step)
        {
            return name != null && name.IsMatch(step);
        }
    }
}