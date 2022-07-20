using MediatR;
using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Helpers;
using Smart.FA.Catalog.Web.Extensions;
using Smart.FA.Catalog.Web.ViewModels.Trainers;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainers;

public class ProfileModel : AdminPage
{
    public IUserIdentity UserIdentity { get; }

    public string Email => UserIdentity.CurrentTrainer.Email!;

    [BindProperty]
    public EditProfileCommand EditProfileCommand { get; set; }

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

        var trainerProfile = await Mediator.Send(new GetTrainerProfileQuery(UserIdentity.CurrentTrainer.Id));
        if (trainerProfile.TrainerId is not null)
        {
            EditProfileCommand = trainerProfile.ToCommand();
            SocialNetworkViewModels = trainerProfile.Socials.ToSocialViewModels();
            // Page reload from a post, whether the underlying operation was successful or not, requires the social networks list to load again.
            ReloadSocials();
        }
    }

    public async Task<ActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    private void ReloadSocials()
    {
        SocialNetworkViewModels = EditProfileCommand
            .Socials
            .Select(keyPair => new TrainerProfile.Social { SocialNetworkId = keyPair.Key, Url = keyPair.Value }).ToSocialViewModels();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ReloadSocials();
            return Page();
        }

        //Update description
        await UpdateDescriptionAsync();

        //Upload image
        await UploadImageAsync();

        //Refresh page
        await LoadDataAsync();

        return Page();
    }

    public async Task UpdateDescriptionAsync()
    {
        EditProfileCommand.TrainerId = UserIdentity.CurrentTrainer.Id;
        var editionResponse = await Mediator.Send(EditProfileCommand);
        EditionSucceeded = !editionResponse.HasErrors();
    }

    public async Task UploadImageAsync()
    {
        if (ProfilePicture is not null)
        {
            var imageUploadRequest = new UploadTrainerProfileImageToStorageCommandRequest { TrainerId = UserIdentity.CurrentTrainer.Id, ProfilePicture = ProfilePicture! };
            var imageUploadResponse = await Mediator.Send(imageUploadRequest);
            ProfilePictureAbsoluteUrl = imageUploadResponse.ProfilePictureAbsoluteUrl;
            EditionSucceeded = !imageUploadResponse.HasErrors();
        }
    }

    protected override SideMenuItem GetSideMenuItem() => SideMenuItem.MyProfile;

    internal string GetSocialAttributeName(int socialId) => $"{nameof(EditProfileCommand)}.Socials[{socialId}]";
}
