using System;
using AcceptanceTesting;

namespace ExampleTestSuite
{
    [FeatureDefinition]
    public class ExampleFeature
    {
        public void something_easy_has_happened()
        {

        }

        public void something_more_difficult_has_happened()
        {
            
        }
        
        public void it_should_fail_because_reason(string reason)
        {
            throw new Exception(reason);
        }
    }
}
