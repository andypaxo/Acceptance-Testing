using System;

namespace AcceptanceTesting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FeatureDefinitionAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ScenarioSetupAttribute : Attribute
    {
    
    }
}