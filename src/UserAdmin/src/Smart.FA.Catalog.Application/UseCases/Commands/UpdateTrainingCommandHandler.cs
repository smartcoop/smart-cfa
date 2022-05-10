using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart.FA.Catalog.Application.SeedWork;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Interfaces;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.LogEvents;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Application.UseCases.Commands;

public class UpdateTrainingCommandHandler : IRequestHandler<UpdateTrainingRequest, UpdateTrainingResponse>
{
    private readonly ILogger<UpdateTrainingCommandHandler> _logger;
    private readonly ITrainingRepository _trainingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateTrainingRequest> _validator;

    public UpdateTrainingCommandHandler(
        ILogger<UpdateTrainingCommandHandler> logger,
        ITrainingRepository trainingRepository,
        IUnitOfWork unitOfWork, IValidator<UpdateTrainingRequest> validator)
    {
        _logger = logger;
        _trainingRepository = trainingRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UpdateTrainingResponse> Handle(UpdateTrainingRequest request, CancellationToken cancellationToken)
    {
        //TODO The validator should not be called inside the handler, actually we should not even be in the Handler's Handle method if the state is invalid.
        // Checks if the trainer that made the edition is actually the creator and can therefore edit this.
        var results = await _validator.ValidateAsync(request, cancellationToken);
        if (!results.IsValid)
        {
            throw new InvalidOperationException(CatalogResources.UpdateTraining_YouCantEditATrainingYouDidntCreate);
        }

        UpdateTrainingResponse resp = new();

        try
        {
            var training = await _trainingRepository.GetFullAsync(request.TrainingId, cancellationToken);

            if (training is null)
            {
                throw new TrainingException(Errors.Training.NotFound(request.TrainingId));
            }

            training.UpdateDetails(request.DetailsDto.Title!,
                request.DetailsDto.Goal!,
                request.DetailsDto.Methodology!,
                request.DetailsDto.PracticalModalities,
                Language.Create(request.DetailsDto.Language).Value);

            training.MarkAsGivenBySmart(request.IsGivenBySmart);
            training.SwitchVatExemptionTypes(request.VatExemptionTypes);
            training.SwitchTargetAudience(request.TargetAudienceTypes);
            training.SwitchAttendanceTypes(request.AttendanceTypes);
            training.SwitchTopics(request.Topics);

            var newStatus = request.IsDraft ? TrainingStatusType.Draft : TrainingStatusType.Validated;

            var result = training.ChangeStatus(newStatus);

            if (result.IsFailure)
            {
                resp.AddErrors(result.Error);
                return resp;
            }

            training.ChangeStatus(request.IsDraft ? TrainingStatusType.Draft : TrainingStatusType.Validated);

            _unitOfWork.RegisterDirty(training);
            _unitOfWork.Commit();

            _logger.LogInformation(LogEventIds.TrainingUpdated, "Training with id {Id} has been updated", training.Id);

            resp.Training = training;
            resp.SetSuccess();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Editing training {trainingId} resulted into an error", request.TrainingId);
            throw;
        }

        return resp;
    }
}

public class UpdateTrainingRequest : IRequest<UpdateTrainingResponse>
{
    public int TrainingId { get; init; }
    public TrainingLocalizedDetailsDto DetailsDto { get; init; } = null!;
    public List<TargetAudienceType>? TargetAudienceTypes { get; init; }
    public List<VatExemptionType> VatExemptionTypes { get; init; } = null!;
    public List<AttendanceType> AttendanceTypes { get; init; } = null!;
    public List<Topic> Topics { get; init; } = null!;
    public List<int> TrainerIds { get; init; } = null!;
    public bool IsDraft { get; set; }
    public bool IsGivenBySmart { get; set; }
}

public class UpdateTrainingResponse : ResponseBase
{
    public Training Training { get; set; } = null!;
}

/// <summary>
/// Validates a <see cref="UpdateTrainingRequest" />.
/// </summary>
public class UpdateTrainingRequestValidator : AbstractValidator<UpdateTrainingRequest>
{
    private readonly CatalogContext _catalogContext;
    private readonly IUserIdentity _userIdentity;

    public UpdateTrainingRequestValidator(CatalogContext catalogContext, IUserIdentity userIdentity)
    {
        _catalogContext = catalogContext;
        _userIdentity = userIdentity;

        // Rule that checks if the trainer editing a training is actually the creator or an admin.
        RuleFor(request => request)
            .MustAsync(BeTheTrainingCreatorOrAdminAsync)
            .WithMessage(CatalogResources.UpdateTraining_YouCantEditATrainingYouDidntCreate);
    }

    public async Task<bool> BeTheTrainingCreatorOrAdminAsync(UpdateTrainingRequest request, CancellationToken cancellationToken)
    {
        // SuperUser are free to do whatever they want :).
        if (_userIdentity.IsSuperUser)
        {
            return true;
        }

        // At this point the training is already cached in the CatalogContext,  no need to select just the id.
        var training = await _catalogContext.Trainings.FirstAsync(training => training.Id == request.TrainingId, cancellationToken);

        return request.TrainerIds.Contains(training.CreatedBy);
    }
}
