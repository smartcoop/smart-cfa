using FluentAssertions;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Tests.Common.Factories;

namespace Smart.FA.Catalog.Tests.Specs.Steps;

[Binding]
public class TrainerSpecs
{
    private Trainer _trainer = null!;
    private Action _action = null!;

    [Given(@"I have a valid trainer")]
    public void GivenIHaveAValidTrainer()
    {
        _trainer = MockedTrainerFactory.CreateClean();
    }

    [When(@"I try to update his title with the invalid (.*)")]
    public void WhenITryToUpdateHisDescriptionWithTheInvalid(string title)
    {
        _action = () => _trainer.UpdateTitle(title);
    }

    [Then(@"the code should throw an error")]
    public void ThenTheCodeShouldThrowAnError()
    {
        _action.Should().Throw<Exception>();
    }


    [When(@"I try to update with the invalid title (.*)")]
    public void WhenITryToUpdateWithTheInvalidTitle(string title)
    {
        _action = () => _trainer.UpdateTitle(title);
    }
}




