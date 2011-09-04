Scenario: Fail the rest of the test after the first failure
 Given I cannot find the first step
 And something easy has happened
 Then nothing else should pass

Scenario: Fail with a reason
 Given something easy has happened
 And something more difficult has happened
 Then it should fail because 'The task was too difficult'

Scenario: Pass after setup
 Given something easy has happened
 Then the test should be set up correctly