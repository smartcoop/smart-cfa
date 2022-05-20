using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.ValueObjects;
using Smart.FA.Catalog.UserAdmin.Infrastructure.Persistence;

namespace Smart.FA.Catalog.UserAdmin.Application.UseCases.Commands;

/// <summary>
/// Updates the first name, last name and email of a <see cref="Trainer" />.
/// </summary>
/// <param name="TrainerId">The id of the training to update.</param>
/// <param name="FirstName">The trainer's first name.</param>
/// <param name="LastName">The trainer's last name.</param>
/// <param name="Email">The email address of the trainer.</param>
public record UpdateTrainerIdentityCommand(int TrainerId, string FirstName, string LastName, string Email) : IRequest<Unit>;

/// <summary>
/// Handles <see cref="UpdateTrainerIdentityCommand" />.
/// </summary>
public class UpdateTrainerCommandHandler : IRequestHandler<UpdateTrainerIdentityCommand, Unit>
{
    private readonly CatalogContext _catalogContext;

    public UpdateTrainerCommandHandler(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<Unit> Handle(UpdateTrainerIdentityCommand command, CancellationToken cancellationToken)
    {
        var trainer = await _catalogContext.Trainers
            .FirstAsync(trainer => trainer.Id == command.TrainerId, cancellationToken);

        trainer.Rename(Name.Create(command.FirstName, command.LastName).Value);
        trainer.ChangeEmail(command.Email);

        await _catalogContext.SaveChangesAsync(cancellationToken);

        // Returns void as the feedback of this operation is irrelevant for the end user.
        return Unit.Value;
    }
}
