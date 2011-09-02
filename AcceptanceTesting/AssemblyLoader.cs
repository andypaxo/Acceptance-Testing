using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AcceptanceTesting
{
    public class AssemblyLoader
    {
        private readonly IEnumerable<string> methodNames;

        private AssemblyLoader(IEnumerable<string> methodNames)
        {
            this.methodNames = methodNames;
            foreach (var methodName in methodNames)
                System.Console.WriteLine(methodName);
        }

        public static AssemblyLoader CreateFrom(Assembly assembly)
        {
            var methodNames = assembly.GetExportedTypes()
                .Where(x => x.GetCustomAttributes(typeof (FeatureDefinitionAttribute), true).Length > 0)
                .SelectMany(x => x.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => x.Name);

            return new AssemblyLoader(methodNames);
        }
    }
}