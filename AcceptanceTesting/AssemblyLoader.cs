using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AcceptanceTesting
{
    [Serializable]
    public class AssemblyLoader
    {
        private AssemblyAnalyzer analyzer;

        public AssemblyLoader InitializeWith(string path)
        {
            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = Path.GetDirectoryName(path),
                ConfigurationFile = path + ".config",
            };
            var appDomain = AppDomain.CreateDomain("SubjectUnderTest", null, appDomainSetup);
            analyzer = ((AssemblyAnalyzer)
                appDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName,
                    typeof (AssemblyAnalyzer).FullName));
            analyzer.Load(path);

            return this;
        }

        public StepResult ResultOf(string step)
        {
            return analyzer.ResultOf(step);
        }

        public void RunScenarioSetup()
        {
            analyzer.RunScenarioSetup();
        }

        [Serializable]
        private class AssemblyAnalyzer : MarshalByRefObject
        {
            private Assembly loadedAssembly;
            private IEnumerable<MethodDefinition> methods;

            public void Load(string assemblyPath)
            {
                loadedAssembly = Assembly.LoadFrom(assemblyPath);
                Initialize();
            }

            public void Load(Assembly assembly)
            {
                loadedAssembly = assembly;
                Initialize();
            }

            private void Initialize()
            {
                var types =
                    from type in loadedAssembly.GetExportedTypes()
                    where type.GetCustomAttributes(typeof (FeatureDefinitionAttribute), true).Length > 0
                    select Activator.CreateInstance(type);

                methods = (
                    from type in types
                    from typeMethod in
                        type.GetType().GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly |
                            BindingFlags.Public)
                    select new MethodDefinition(type, typeMethod))
                    .ToList();
            }

            public StepResult ResultOf(string step)
            {
                var method = methods.FirstOrDefault(x => x.Matches(step));
                if (method == null)
                    return StepResult.NotFound;

                try
                {
                    method.Invoke(step);
                    return StepResult.Ok;
                }
                catch (Exception ex)
                {
                    var exception = (ex.InnerException ?? ex).ToString();
                    return StepResult.Fail(exception);
                }
            }

            public void RunScenarioSetup()
            {
                foreach(var method in methods.Where(x => x.IsScenarioSetup))
                    method.Invoke("");
            }
        }
    }
}