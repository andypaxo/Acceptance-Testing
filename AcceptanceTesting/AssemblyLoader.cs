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
                    method.Invoke();
                    return StepResult.Ok;
                }
                catch (Exception ex)
                {
                    var exception = (ex.InnerException ?? ex).ToString();
                    return StepResult.Fail(exception);
                }
            }
        }

        private class MethodDefinition
        {
            private readonly object instance;
            private readonly MethodInfo method;

            public string Name { get; private set; }

            public MethodDefinition(object instance, MethodInfo method)
            {
                this.instance = instance;
                this.method = method;
                Name = method.Name.Replace('_', ' ');
            }

            public void Invoke()
            {
                method.Invoke(instance, null);
            }

            public bool Matches(string step)
            {
                return Name == step;
            }
        }
    }
}