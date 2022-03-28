using FluentValidation;
using MimeDetective;
using MimeDetective.Definitions;
using Smart.FA.Catalog.Application.UseCases.Commands;

namespace Smart.FA.Catalog.Web.Validators;

public class UpdateTrainerRequestValidator : AbstractValidator<EditProfileCommand>
{
    public UpdateTrainerRequestValidator()
    {
        When(request => request.ProfilePicture is not null,
            () => RuleFor(request => request.ProfilePicture!)
                .Cascade(CascadeMode.Stop)
                .Must(IsUnderMaxSize).WithMessage("The file is too big")
                .MustAsync(IsCorrectTypeAsync).WithMessage("The type is not supported"));
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
        return file.Length < 1000000;
    }
}
