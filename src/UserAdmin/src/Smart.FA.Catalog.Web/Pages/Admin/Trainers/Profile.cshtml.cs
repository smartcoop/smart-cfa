using MediatR;
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Helpers;
using Smart.FA.Catalog.Web.ViewModels.Trainers;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainers;

public class ProfileModel : AdminPage
{
    public IUserIdentity UserIdentity { get; }

    public string Email => UserIdentity.CurrentTrainer.Email!;

    [BindProperty]
    public EditProfileCommand EditProfileCommand { get; set; } = new();

    [BindProperty]
    public IFormFile? ProfilePicture { get; set; }

    /// <summary>
    /// State boolean that indicates if the current page results from a successful profile edition.
    /// </summary>
    [TempData]
    public bool? EditionSucceeded { get; set; }

    public string ProfilePictureAbsoluteUrl { get; set; }

    protected internal ICollection<SocialNetworkViewModel> SocialNetworkViewModels { get; set; } = null!;

    public ProfileModel(IMediator mediator, IUserIdentity userIdentity, IS3StorageService storageService, IMinIoLinkGenerator minIoLinkGenerator) :
        base(mediator)
    {
        UserIdentity = userIdentity;
        ProfilePictureAbsoluteUrl = minIoLinkGenerator.GetAbsoluteTrainerProfilePictureUrl(userIdentity.CurrentTrainer.ProfileImagePath);
    }

    private async Task LoadDataAsync()
    {
        SetSideMenuItem();

        // Page reload from a post, whether the underlying operation was successful or not, requires the social networks list to load again.
        ReloadSocials();

        var trainerProfile = await Mediator.Send(new GetTrainerProfileQuery(UserIdentity.CurrentTrainer.Id));
        if (trainerProfile.TrainerId is not null)
        {
            EditProfileCommand = trainerProfile.ToCommand();
            SocialNetworkViewModels = trainerProfile.Socials.ToSocialViewModels();
        }
    }

    public async Task<ActionResult> OnGetAsync()
    {
        SetSideMenuItem();
        await LoadDataAsync().ConfigureAwait(false);

        return Page();
    }

    public async Task UpdateDescriptionAsync()
    {
        EditProfileCommand.TrainerId = UserIdentity.CurrentTrainer.Id;
        var editionResponse = await Mediator.Send(EditProfileCommand);
        EditionSucceeded = !editionResponse.HasErrors();
    }

    public async Task<ActionResult> OnPostDescriptionAsync()
    {
        await UpdateDescriptionAsync();

        await LoadDataAsync();

        return RedirectToPage();
    }

    private void ReloadSocials()
    {
        SocialNetworkViewModels = EditProfileCommand
            .Socials
            .Select(keyPair => new TrainerProfile.Social { SocialNetworkId = keyPair.Key, Url = keyPair.Value }).ToSocialViewModels();
    }

    public async Task<ActionResult> OnPostUploadProfileImageAndDescriptionAsync()
    {
        if (ModelState.IsValid)
        {
            //Update description
            await UpdateDescriptionAsync();

            //Upload image
            await UploadImageAsync();
        }

        //Refresh page
        await LoadDataAsync();
        return RedirectToPage();
    }

    public async Task UploadImageAsync()
    {
        var imageUploadRequest = new UploadTrainerProfileImageToStorageCommandRequest { TrainerId = UserIdentity.CurrentTrainer.Id, ProfilePicture = ProfilePicture };
        var imageUploadResponse = await Mediator.Send(imageUploadRequest);
        ProfilePictureAbsoluteUrl = imageUploadResponse.ProfilePictureAbsoluteUrl;
        EditionSucceeded = !imageUploadResponse.HasErrors();
    }

    public async Task<FileResult?> OnGetLoadImageAsync()
    {
        var profilePicture = await Mediator.Send(new GetTrainerProfileImageRequest { Trainer = UserIdentity.CurrentTrainer });
        var response = profilePicture.ImageStream is null ? null : new FileStreamResult(profilePicture.ImageStream, "image/jpeg");
        return response;
    }

    public async Task<ActionResult> OnPostDeleteImageAsync()
    {
        if (ProfilePicture is not null)
        {
            var imageDeletionResponse = await Mediator.Send(new DeleteTrainerProfileImageRequest { RelativeProfilePictureUrl = UserIdentity.CurrentTrainer.ProfileImagePath });
            EditionSucceeded = !imageDeletionResponse.HasErrors();
        }

        await LoadDataAsync();
        return RedirectToPage();
    }

    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyProfile;

    internal string GetSocialAttributeName(int socialId) => $"{nameof(EditProfileCommand)}.Socials[{socialId}]";
}
