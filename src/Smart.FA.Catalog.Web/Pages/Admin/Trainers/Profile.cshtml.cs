using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.Trainers;

namespace Web.Pages.Admin.Trainers;

public class ProfileModel : AdminPage
{
    public IUserIdentity UserIdentity { get; }

    [BindProperty]
    public EditProfileCommand? EditProfileCommand { get; set; }

    /// <summary>
    /// State boolean that indicates if the current page results from a successful profile edition.
    /// </summary>
    internal bool EditionSucceeded { get; set; }

    protected internal ICollection<SocialNetworkViewModel> SocialNetworkViewModels { get; set; }

    public ProfileModel(IMediator mediator, IUserIdentity userIdentity) : base(mediator)
    {
        UserIdentity = userIdentity;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        SetSideMenuItem();
        await LoadDataAsync().ConfigureAwait(false);

        // Trainer was not found.
        if (EditProfileCommand is null)
        {
            return RedirectToPage("/404");
        }

        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            ParseSocialMedias();
            EditProfileCommand!.TrainerId = UserIdentity.CurrentTrainer.Id;
            var editionResponse = await Mediator.Send(EditProfileCommand);
            EditionSucceeded    = !editionResponse.HasErrors();
        }

        SetSideMenuItem();

        // Page reload from a post, whether the underlying operation was successful or not, requires the social networks list to load again.
        await LoadSocialsAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        SetSideMenuItem();
        var trainerProfile = await Mediator.Send(new GetTrainerProfileQuery(UserIdentity.CurrentTrainer.Id));
        if (trainerProfile.TrainerId is not null)
        {
            EditProfileCommand      = trainerProfile.ToCommand();
            SocialNetworkViewModels = trainerProfile.Socials.ToSocialViewModels();
        }
    }

    private async Task LoadSocialsAsync()
    {
        var trainerProfile      = await Mediator.Send(new GetTrainerProfileQuery(UserIdentity.CurrentTrainer .Id));
        SocialNetworkViewModels = trainerProfile.Socials.ToSocialViewModels();
    }

    private void ParseSocialMedias()
    {
        // The request gives us a collection of the following key par values for social networks.:
        // "social-" + [SocialId] + [url value of the profile]
        EditProfileCommand.Socials = Request.Form
            .Where(formElement => formElement.Key.StartsWith("social-", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(key => key.Key.Split("-")[1], value => value.Value.ToString());
    }

    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyProfile;
}
