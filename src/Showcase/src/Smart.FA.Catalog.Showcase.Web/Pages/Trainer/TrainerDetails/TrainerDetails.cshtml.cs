#nullable disable
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Inquiry.Trainer;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsModel : PageModelBase
{
    private readonly ITrainerService _trainerService;
    private readonly ITrainerInquirySendEmailService _trainerInquirySendEmailService;

    [TempData(Key = nameof(TrainerId))]
    public int? TrainerId { get; set; }

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

        TrainerId = Trainer.Id;
        return Page();
    }

    private async Task<TrainerDetailsViewModel> LoadTrainerDetailsAsync(int id)
    {
        Trainer = await _trainerService.GetTrainerDetailsViewModelsByIdAsync(id, CurrentPage, ItemsPerPage);
        return Trainer;
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            TrainerInquiryEmailRequest.TrainerId = TrainerId.Value;
            EmailSendingResult = await _trainerInquirySendEmailService.SendEmailAsync(TrainerInquiryEmailRequest);
        }

        TempData[nameof(TrainerId)] = TrainerId;
        await LoadTrainerDetailsAsync(TrainerId.Value);
        return Page();
    }
}
#nullable enable
