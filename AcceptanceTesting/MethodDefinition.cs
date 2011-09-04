using System.Reflection;
using System.Text.RegularExpressions;

namespace AcceptanceTesting
{
    internal class MethodDefinition
    {
        private readonly object instance;
        private readonly MethodInfo method;
        private readonly Regex name;

        public MethodDefinition(object instance, MethodInfo method)
        {
            this.instance = instance;
            this.method = method;

            name = new Regex(method.Name.Replace('_', ' '));
        }

        public void Invoke()
        {
            method.Invoke(instance, null);
        }

        public bool Matches(string step)
        {
            return name.IsMatch(step);
        }
    }
}