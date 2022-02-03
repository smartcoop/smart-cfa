using Application.SeedWork;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Domain.Interfaces;
using Core.SeedWork;
using Core.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Commands;

public class UpdateTrainingCommandHandler : IRequestHandler<UpdateTrainingRequest, UpdateTrainingResponse>
{
    private readonly ILogger<UpdateTrainingCommandHandler> _logger;
    private readonly ITrainingRepository _trainingRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;

    public UpdateTrainingCommandHandler(ILogger<UpdateTrainingCommandHandler> logger,
        ITrainingRepository trainingRepository, ITrainerRepository trainerRepository, IUnitOfWork unitOfWork, IMailService mailService)
    {
        _logger = logger;
        _trainingRepository = trainingRepository;
        _trainerRepository = trainerRepository;
        _unitOfWork = unitOfWork;
        _mailService = mailService;
    }

    public async Task<UpdateTrainingResponse> Handle(UpdateTrainingRequest request, CancellationToken cancellationToken)
    {
        UpdateTrainingResponse resp = new();

        try
        {
            var training = await _trainingRepository.GetFullAsync(request.TrainingId, cancellationToken);
            if (training is null) throw new Exception("No trainings cannot be found with that id");
            training.UpdateDetails(request.Detail.Title!, request.Detail.Goal!, request.Detail.Methodology!,
                Language.Create(request.Detail.Language).Value);
            training.SwitchTrainingTypes(request.Types);
            training.SwitchTargetAudience(request.TargetAudiences);
            training.SwitchSlotNumberType(request.SlotNumberTypes);
            var trainers = await _trainerRepository.GetListAsync(request.TrainingId, cancellationToken);
            training.EnrollTrainers(trainers);
            training.Validate();
            _unitOfWork.RegisterDirty(training);
            _unitOfWork.Commit();

            resp.Training = training;
            resp.SetSuccess();
        }
        catch (Exception e)
        {
             _logger.LogError("{Exception}", e.ToString());
            throw;
        }

        return resp;
    }
}

public class UpdateTrainingRequest : IRequest<UpdateTrainingResponse>
{
    public int TrainingId { get; init; }
    public TrainingDetailDto Detail { get; init; } = null!;
    public List<TrainingTargetAudience>? TargetAudiences { get; init; }
    public List<TrainingType> Types { get; init; } = null!;
    public List<TrainingSlotNumberType> SlotNumberTypes { get; init; } = null!;
    public List<int> TrainerIds { get; init; } = null!;
}

public class UpdateTrainingResponse : ResponseBase
{
    public Training Training { get; set; } = null!;
}
