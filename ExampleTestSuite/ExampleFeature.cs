﻿using System;
using AcceptanceTesting;

namespace ExampleTestSuite
{
    [FeatureDefinition]
    public class ExampleFeature
    {
        public void I_have_logged_in()
        {
        }

        public void I_should_see_a_failed_login()
        {
            throw new Exception("This feature failed on purpose");
        }
    }
}