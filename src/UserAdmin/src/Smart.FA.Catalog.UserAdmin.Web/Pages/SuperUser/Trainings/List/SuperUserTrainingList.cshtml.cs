using System.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.UserAdmin.Application.UseCases.Queries;
using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.Extensions;
using Smart.FA.Catalog.UserAdmin.Web.Options;

namespace Smart.FA.Catalog.UserAdmin.Web.Pages.SuperUser.Trainings.List;

public class SuperUserTrainingListPageModel : PageModel
{
    private readonly ILogger<SuperUserTrainingListPageModel> _logger;
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<CatalogResources> _localizer;
    private readonly SuperUserOptions _settings;

    [BindProperty(SupportsGet = true)]
    public GetTrainingsByCriteriaQuery GetTrainingsRequest { get; set; } = null!;

    public string? ErrorMessage { get; set; }

    public PagedList<Training>? Trainings { get; set; }

    public IEnumerable<SelectListItem>? Statuses { get; set; }

    public SuperUserTrainingListPageModel(ILogger<SuperUserTrainingListPageModel> logger,
        IMediator mediator,
        IStringLocalizer<CatalogResources> localizer,
        IOptions<SuperUserOptions> settings)
    {
        _logger    = logger;
        _mediator  = mediator;
        _localizer = localizer;
        _settings  = settings.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<PageResult> OnGetAsync()
    {
        ViewData[nameof(SuperUserSideMenuItem)] = SuperUserSideMenuItem.SuperUserTrainingList;
        LoadData();

        // The forms has an input hidden with search as name.
        // This allows us to differentiate a call that comes from the form or the [previous] [next] buttons.
        if (Request.Query.Any() || Request.Query.ContainsKey("search"))
        {
            await PerformSearchAsync();
        }

        return Page();
    }

    private async Task PerformSearchAsync()
    {
        try
        {
            GetTrainingsRequest.PageSize = _settings.NumberOfTrainingsPerPage;
            Trainings = await _mediator.Send(GetTrainingsRequest);
            LoadData();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"An error occurred while retrieving training for request {GetTrainingsRequest.ToJson()}");
            ErrorMessage = CatalogResources.AnErrorOccurredWhileSearching;
        }
    }

    private void LoadData()
    {
        Statuses = TrainingStatusType.List
            .Select(statusType => new SelectListItem(_localizer[statusType.Name], statusType.Id.ToString(), GetTrainingsRequest.Status == statusType.Id));
    }

    public string SerializeHtmlForm()
    {
        var request = GetTrainingsRequest;
        var queryString = "";

        if (request.Status is not null)
        {
            queryString += $"{nameof(request.Status)}={HttpUtility.UrlEncode(request.Status.ToString())}&";
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            queryString += $"{nameof(request.Title)}={HttpUtility.UrlEncode(request.Title)}&";
        }

        if (request.Topics is not null && request.Topics.Any())
        {
            queryString += string.Join("&", request.Topics.Select(p => $"{nameof(request.Topics)}={p}"));
        }

        return queryString;
    }
}
