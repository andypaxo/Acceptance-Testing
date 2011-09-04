using System;
using AcceptanceTesting;

namespace ExampleTestSuite
{
    [FeatureDefinition]
    public class ExampleFeature
    {
        private bool setupCompleted;

        [ScenarioSetup]
        public void SetupScenario()
        {
            setupCompleted = true;
        }

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

        public void the_test_should_be_set_up_correctly()
        {
            if (!setupCompleted)
                throw new Exception("Set up was not completed");
        }
    }
}
