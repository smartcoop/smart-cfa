using Core.Domain;
using Core.SeedWork;
using MediatR;

namespace Application.UseCases.DomainEventHandlers;

public class ValidateTrainingDomainEventHandler : INotificationHandler<ValidateTrainingEvent>
{
    private readonly MessageBus _messageBus;

    public ValidateTrainingDomainEventHandler(MessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public Task Handle(ValidateTrainingEvent notification, CancellationToken cancellationToken)
    {
        _messageBus.ValidateTrainingMessage(notification.TrainingId,
            notification.TrainingName, notification.TrainersId);

        return Task.CompletedTask;
    }
}
