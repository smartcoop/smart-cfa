#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;
using Smart.FA.Catalog.Showcase.Web.Services.Training;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Training.TrainingDetails;

public class TrainingDetailsModel : PageModelBase
{
    private const string TempDataTrainerIdKey = "TrainerId";

    private const string TempDataTrainingIdKey = "TrainingId";

    private readonly ITrainerInquirySendEmailService _trainerInquirySendEmailService;


    private readonly ITrainingService _trainingService;

    public TrainingDetailsViewModel Training { get; set; }

    public TrainingDetailsModel(ITrainerInquirySendEmailService trainerInquirySendEmailService, ITrainingService trainingService)
    {
        _trainerInquirySendEmailService = trainerInquirySendEmailService;
        _trainingService = trainingService;
    }
    public InquirySendEmailResult? EmailSendingResult { get; set; }

    [BindProperty(SupportsGet = false)] public TrainerInquirySendEmailRequest TrainerInquiryEmailRequest { get; set; } = null!;

    public async Task<ActionResult> OnGetAsync(int? id)
    {
        if (id is null || (Training = await _trainingService.GetTrainingDetailsViewModelsByIdAsync(id.Value)) is null)
        {
            return RedirectToNotFound();
        }

        SetTempDataTrainerId(Training.TrainerId);
        SetTempDataTrainingId(Training.Id);
        return Page();
    }


    public async Task<ActionResult> OnPostAsync()
    {
        if (!TempData.TryGetConvertedValue<int>(TempDataTrainerIdKey, out var trainerId) ||
            !TempData.TryGetConvertedValue<int>(TempDataTrainingIdKey, out var trainingId))
        {
            return RedirectToPage(Routes.TrainingList);
        }

        if (ModelState.IsValid)
        {
            TrainerInquiryEmailRequest.TrainerId = trainerId;
            EmailSendingResult = await _trainerInquirySendEmailService.SendEmailAsync(TrainerInquiryEmailRequest);
        }

        Training = await _trainingService.GetTrainingDetailsViewModelsByIdAsync(trainingId);
        SetTempDataTrainerId(trainerId);
        SetTempDataTrainingId(trainingId);
        return Page();
    }

    private void SetTempDataTrainerId(int trainerId) => TempData[TempDataTrainerIdKey] = trainerId;

    private void SetTempDataTrainingId(int trainingId) => TempData[TempDataTrainingIdKey] = trainingId;
}
