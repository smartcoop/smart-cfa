#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsModel : PageModelBase
{
    private const string TempDataTrainerIdKey = "TrainerId";

    private readonly ITrainerService _trainerService;
    private readonly ITrainerInquirySendEmailService _trainerInquirySendEmailService;

    public TrainerDetailsViewModel Trainer { get; set; } = null!;

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = false)] public TrainerInquirySendEmailRequest TrainerInquiryEmailRequest { get; set; } = null!;

    public InquirySendEmailResult? EmailSendingResult { get; set; }

    const int ItemsPerPage = 5;

    public TrainerDetailsModel(ITrainerService trainerService, ITrainerInquirySendEmailService trainerInquirySendEmailService)
    {
        _trainerService = trainerService;
        _trainerInquirySendEmailService = trainerInquirySendEmailService;
    }

    public async Task<ActionResult> OnGetAsync(int? id)
    {
        if (id is null || (Trainer = await LoadTrainerDetailsAsync(id.Value)) is null)
        {
            return RedirectToNotFound();
        }

        SetTempDataTrainerId(Trainer.Id);
        return Page();
    }

    private async Task<TrainerDetailsViewModel> LoadTrainerDetailsAsync(int id)
    {
        Trainer = await _trainerService.GetTrainerDetailsViewModelsByIdAsync(id, CurrentPage, ItemsPerPage);
        return Trainer;
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (!TempData.TryGetConvertedValue<int>(TempDataTrainerIdKey, out var trainerId))
        {
            return RedirectToPage(Routes.TrainingList);
        }

        if (ModelState.IsValid)
        {
            TrainerInquiryEmailRequest.TrainerId = trainerId;
            EmailSendingResult = await _trainerInquirySendEmailService.SendEmailAsync(TrainerInquiryEmailRequest);
        }

        await LoadTrainerDetailsAsync(trainerId);
        SetTempDataTrainerId(trainerId);
        return Page();
    }

    private void SetTempDataTrainerId(int trainerId) => TempData[TempDataTrainerIdKey] = trainerId;
}
#nullable enable
