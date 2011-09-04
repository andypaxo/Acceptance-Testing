using System;
using System.Linq;
using System.Reflection;

namespace AcceptanceTesting
{
    public abstract class Logger : IDisposable
    {
        public abstract void WriteResult(string line, StepResult result);
        public abstract void WriteScenarioStart(string scenarioName);
        public abstract void Dispose();

        public static Logger GetLogger(string name)
        {
            var loggerType = (
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where typeof(Logger).IsAssignableFrom(type)
                let loggerAttributes = type.GetCustomAttributes(typeof (LoggerNameAttribute), false)
                where loggerAttributes.Length > 0 && ((LoggerNameAttribute) loggerAttributes[0]).Name == name
                select type).First();
            return (Logger)Activator.CreateInstance(loggerType, false);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal class LoggerNameAttribute : Attribute
    {
        public string Name { get; set; }

        public LoggerNameAttribute(string name)
        {
            Name = name;
        }
    }
}