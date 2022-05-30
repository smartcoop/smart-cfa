#nullable disable
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Showcase.Domain.Common.Enums;
using Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact;
using Smart.FA.Catalog.Showcase.Web.Services.Trainer;

namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerDetails;

public class TrainerDetailsModel : PageModelBase
{
    private readonly ITrainerService _trainerService;

    public TrainerDetailsViewModel Trainer { get; set; } = null!;

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = false)] public InquirySendEmailRequest TrainerInquiryEmailRequest { get; set; } = null!;

    public InquirySendEmailResult? EmailSendingResult { get; set; }

    const int ItemsPerPage = 5;

    public TrainerDetailsModel(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    public async Task<ActionResult> OnGetAsync(int? id)
    {
        if (id is null || (Trainer = await _trainerService.GetTrainerDetailsViewModelsByIdAsync(id.Value, CurrentPage, ItemsPerPage)) is null)
        {
            return RedirectToNotFound();
        }
        return Page();
    }
}
#nullable enable
