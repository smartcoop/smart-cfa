using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Authorization.Role;

namespace Smart.FA.Catalog.Web.Identity;

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
    public bool IsSuperUser => _httpContextAccessor.HttpContext!.User.IsInRole(Roles.SuperUser);

    /// <inheritdoc />
    public bool IsShareholder => _httpContextAccessor.HttpContext!.User.IsInRole(Roles.Shareholder);

    public UserIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
