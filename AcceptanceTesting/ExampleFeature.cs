using System;

namespace AcceptanceTesting
{
    [FeatureDefinition]
    public class ExampleFeature
    {
        public void I_want_to_do_things()
        {
            Console.WriteLine("I did things!");
        }

        public void things_should_be_done()
        {
            Console.WriteLine("Things were done!");
        }
    }
}