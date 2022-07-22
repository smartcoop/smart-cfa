Feature: test cases relating to creation and edition of a trainer

    @CFA-188
    Scenario: Throw exception when trying to update a trainer with an invalid title (length too big)
        Given I have a valid trainer
        When I try to update his title with the invalid <Title>
        Then the code should throw an error

    Examples:
      | Title                                                                                                                                                   |
      | aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa |
      | valid field                                                                                                                                             |
