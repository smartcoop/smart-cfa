using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Models;
using Smart.FA.Catalog.Core.Services;

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
    public bool IsSuperUser => _httpContextAccessor.HttpContext!.User.IsInRole("SuperUser");

    public UserIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
