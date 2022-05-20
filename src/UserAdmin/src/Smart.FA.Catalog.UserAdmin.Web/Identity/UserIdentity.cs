using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.Models;
using Smart.FA.Catalog.UserAdmin.Domain.Services;

namespace Smart.FA.Catalog.UserAdmin.Web.Identity;

/// <inheritdoc />
public class UserIdentity : IUserIdentity
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <inheritdoc />
    public int Id => Identity?.Id ?? 0;

    /// <inheritdoc />
    public CustomIdentity Identity => (_httpContextAccessor.HttpContext?.User.Identity as CustomIdentity)!;

    /// <inheritdoc />
    public Trainer CurrentTrainer => Identity?.Trainer!;

    /// <inheritdoc />
    public bool IsSuperUser => _httpContextAccessor.HttpContext!.User.IsInRole("SuperUser");

    public UserIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
