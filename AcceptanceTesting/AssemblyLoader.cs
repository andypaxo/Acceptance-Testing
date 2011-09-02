﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AcceptanceTesting
{
    public class AssemblyLoader
    {
        private readonly Dictionary<string, MethodDefinition> methods;

        private AssemblyLoader(Dictionary<string, MethodDefinition> methods)
        {
            this.methods = methods;
        }

        public static AssemblyLoader CreateFrom(Assembly assembly)
        {
            var types =
                from type in assembly.GetExportedTypes()
                where type.GetCustomAttributes(typeof (FeatureDefinitionAttribute), true).Length > 0
                select Activator.CreateInstance(type);

            var methods = (
                from type in types
                from typeMethod in type.GetType().GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                select new MethodDefinition(type, typeMethod))
                .ToDictionary(x => x.Name);

            return new AssemblyLoader(methods);
        }

        public IEnumerable<string> AllMethods()
        {
            return methods.Keys;
        }

        public bool FindMethod(string name)
        {
            return methods.ContainsKey(name);
        }

        public bool Ok(string step)
        {
            try
            {
                methods[step].Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    internal class MethodDefinition
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