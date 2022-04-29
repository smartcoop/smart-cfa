using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.ViewModels.Trainers;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainers;

public class ProfileModel : AdminPage
{
    public IUserIdentity UserIdentity { get; }

    [BindProperty] public EditProfileCommand? EditProfileCommand { get; set; }

    /// <summary>
    /// State boolean that indicates if the current page results from a successful profile edition.
    /// </summary>
    internal bool EditionSucceeded { get; set; }

    public Stream? ProfilePicture { get; set; }

    protected internal ICollection<SocialNetworkViewModel> SocialNetworkViewModels { get; set; } = null!;

    public ProfileModel(IMediator mediator, IUserIdentity userIdentity, IS3StorageService storageService) :
        base(mediator)
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
            if (EditProfileCommand.ProfilePicture is not null)
            {
                var imageUploadRequest = new UploadImageToStorageCommandRequest {Trainer = UserIdentity.CurrentTrainer, ProfilePicture = EditProfileCommand.ProfilePicture};
                var profileResult      = await Mediator.Send(imageUploadRequest);
                ProfilePicture = profileResult.ProfilePictureStream;
            }

            EditionSucceeded = !editionResponse.HasErrors();
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
        var trainerProfile = await Mediator.Send(new GetTrainerProfileQuery(UserIdentity.CurrentTrainer.Id));
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

    public async Task<FileResult?> OnGetLoadImageAsync()
    {
        var profilePicture = await Mediator.Send(new GetTrainerProfileImageRequest {Trainer = UserIdentity.CurrentTrainer});
        var response       = profilePicture.ImageStream is null ? null : new FileStreamResult(profilePicture.ImageStream, "image/jpeg");
        return response;
    }

    public async Task<ActionResult> OnPostDeleteImageAsync()
    {
        if (EditProfileCommand?.ProfilePicture is not null)
        {
            await Mediator.Send(new DeleteTrainerProfileImageRequest {Trainer = UserIdentity.CurrentTrainer});
        }

        await LoadDataAsync();
        return Page();
    }

    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyProfile;
}
