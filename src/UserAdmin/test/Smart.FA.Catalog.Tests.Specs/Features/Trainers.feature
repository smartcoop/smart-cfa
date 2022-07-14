Feature: test cases relating to creation and edition of a trainer

    @CFA-188
    Scenario: Throw exception when trying to create a trainer with an invalid title (length too big)
        Given I have a valid trainer
        When I try to update his title with the invalid <Title>
        Then the code should throw an error

    Examples:
      | Title                                                                                                                                                   |
      | aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa |
      | valid field                                                                                                                                             |
    Scenario: Throw exception when trying to create a trainer with an invalid title (length too big) - different input
        Given I have a valid trainer
        When I try to update with the invalid title aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
        Then the code should throw an error

