﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AcceptanceTesting
{
    [Serializable]
    public class AssemblyLoader
    {
        private AssemblyLocator locator;

        public AssemblyLoader InitializeWith(Assembly assembly)
        {
            locator = new AssemblyLocator();
            locator.Load(assembly);
            return this;
        }

        public AssemblyLoader InitializeWith(string path)
        {
            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = Path.GetDirectoryName(path),
                ConfigurationFile = path + ".config",
            };
            var appDomain = AppDomain.CreateDomain("SubjectUnderTest", null, appDomainSetup);
            locator = ((AssemblyLocator)
                appDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName,
                    typeof(AssemblyLocator).FullName));
            locator.Load(path);
            
            return this;
        }

        public IEnumerable<string> AllMethods()
        {
            return locator.AllMethods();
        }

        public bool FindMethod(string name)
        {
            return locator.FindMethod(name);
        }

        public StepResult ResultOf(string step)
        {
            return locator.ResultOf(step);
        }
    }

    [Serializable]
    public class StepResult
    {
        public bool Passed { get; private set; }
        public Exception Exception { get; private set; }

        public static readonly StepResult Ok = new StepResult {Passed = true};
        public static StepResult Fail(Exception exception)
        {
            return new StepResult
            {
                Passed = false,
                Exception = exception
            };
        }
    }

    [Serializable]
    internal class AssemblyLocator : MarshalByRefObject
    {
        private Assembly loadedAssembly;
        private Dictionary<string, MethodDefinition> methods;

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
                where type.GetCustomAttributes(typeof(FeatureDefinitionAttribute), true).Length > 0
                select Activator.CreateInstance(type);

            methods = (
                from type in types
                from typeMethod in type.GetType().GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                select new MethodDefinition(type, typeMethod))
                .ToDictionary(x => x.Name);
        }

        public IEnumerable<string> AllMethods()
        {
            return methods.Keys.ToArray();
        }

        public bool FindMethod(string name)
        {
            return methods.ContainsKey(name);
        }

        public StepResult ResultOf(string step)
        {
            try
            {
                methods[step].Invoke();
                return StepResult.Ok;
            }
            catch (Exception ex)
            {
                return StepResult.Fail(ex);
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
        }
    }
}