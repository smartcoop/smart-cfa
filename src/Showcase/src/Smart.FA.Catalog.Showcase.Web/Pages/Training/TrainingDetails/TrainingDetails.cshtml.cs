#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;
using Smart.FA.Catalog.Showcase.Web.Options;


namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingDetails;

public class TrainingDetailsModel : PageModelBase
{
    private const string TempDataTrainerIdKey = "TrainerId";

    private const string TempDataTrainingIdKey = "TrainingId";

    private readonly ITrainerInquirySendEmailService _trainerInquirySendEmailService;

    private readonly Infrastructure.Data.CatalogShowcaseContext _context;

    private readonly MinIOOptions _minIoSettings;

    public TrainingDetailsViewModel Training { get; set; }

    public TrainingDetailsModel(Infrastructure.Data.CatalogShowcaseContext context, IOptions<MinIOOptions> minIOSettings, ITrainerInquirySendEmailService trainerInquirySendEmailService)
    {
        _context = context;
        _minIoSettings = minIOSettings.Value ?? throw new InvalidOperationException($"MinIO options not defined");
        _trainerInquirySendEmailService = trainerInquirySendEmailService;
    }
    public InquirySendEmailResult? EmailSendingResult { get; set; }

    [BindProperty(SupportsGet = false)] public TrainerInquirySendEmailRequest TrainerInquiryEmailRequest { get; set; } = null!;

    public async Task<ActionResult> OnGetAsync(int? id)
    {
        if (id is null || (Training = await LoadTrainingDetailsAsync(id.Value)) is null)
        {
            return RedirectToNotFound();
        }

        SetTempDataTrainerId(Training.TrainerId);
        SetTempDataTrainingId(Training.Id);
        return Page();
    }

    private TrainingDetailsViewModel MapTrainingDetails(List<Domain.Models.TrainingDetails> trainingDetails)
    {
        if (trainingDetails is null)
        {
            return null;
        }

        var firstLine = trainingDetails.FirstOrDefault();
        return new TrainingDetailsViewModel
        {
            Id = firstLine.Id,
            TrainingTitle = firstLine.TrainingTitle,
            Goal = firstLine.Goal,
            Methodology = firstLine.Methodology,
            PracticalModalities = firstLine.PracticalModalities,
            TrainerFirstName = firstLine.TrainerFirstName,
            TrainerLastName = firstLine.TrainerLastName,
            TrainerId = firstLine.TrainerId,
            TrainerTitle = firstLine.TrainerTitle,
            Status = TrainingStatusType.FromValue(firstLine.StatusId),
            Topics = trainingDetails.Select(x => Topic.FromValue(x.TrainingTopicId)).ToList(),
            Languages = trainingDetails.Select(x => x.Language).Distinct().ToList(),
            TrainerProfileImageUrl = _minIoSettings.GenerateMinIoTrainerProfileUrl(firstLine.ProfileImagePath)
        };
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (!TempData.TryGetConvertedValue<int>(TempDataTrainerIdKey, out var trainerId))
        {
            return RedirectToPage(Routes.TrainingList);
        }

        if (!TempData.TryGetConvertedValue<int>(TempDataTrainingIdKey, out var trainingId))
        {
            return RedirectToPage(Routes.TrainingList);
        }

        if (ModelState.IsValid)
        {
            TrainerInquiryEmailRequest.TrainerId = trainerId;
            EmailSendingResult = await _trainerInquirySendEmailService.SendEmailAsync(TrainerInquiryEmailRequest);
        }

        await LoadTrainingDetailsAsync(trainingId);
        SetTempDataTrainerId(trainerId);
        SetTempDataTrainingId(trainingId);
        return Page();
    }

    private void SetTempDataTrainerId(int trainerId) => TempData[TempDataTrainerIdKey] = trainerId;

    private void SetTempDataTrainingId(int trainingId) => TempData[TempDataTrainingIdKey] = trainingId;

    private async Task<TrainingDetailsViewModel> LoadTrainingDetailsAsync(int id)
    {
        var trainingDetails = await _context.TrainingDetails.Where(training => training.Id == id).ToListAsync();

        if (!trainingDetails.Any())
        {
            return null;
        }

        Training = MapTrainingDetails(trainingDetails);
        return Training;
    }
}
