using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MimeDetective;
using MimeDetective.Definitions;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Web.Validators;

public class UpdateTrainerRequestValidator : AbstractValidator<EditProfileCommand>
{
    private readonly IOptions<S3StorageOptions> _storageOptions;

    public UpdateTrainerRequestValidator(IOptions<S3StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions;
        When(request => request.ProfilePicture is not null,
            () => RuleFor(request => request.ProfilePicture!)
                .Cascade(CascadeMode.Stop)
                .Must(IsUnderMaxSize).WithMessage(CatalogResources.ProfilePage_Image_FileTooBig)
                .MustAsync(IsCorrectTypeAsync).WithMessage(CatalogResources.ProfilePage_Image_WrongFileType));
    }

    private async Task<bool> IsCorrectTypeAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var mimeInspector = new ContentInspectorBuilder {Definitions = Default.FileTypes.Images.All()}.Build();

        var fileStream = file.OpenReadStream();
        MemoryStream memoryStream = new();
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        var result = mimeInspector.Inspect(memoryStream.ToArray());
        return !result.IsDefaultOrEmpty;
    }

    private bool IsUnderMaxSize(IFormFile file)
    {
        return file.Length < _storageOptions.Value.FileSizeLimit;
    }
}
