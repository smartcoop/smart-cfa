using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain;

public class UserChartRevision : Entity
{
    #region Private Properties

    private readonly List<TrainerApproval> _trainerApprovals = new();

    #endregion

    #region Public Properties

    public string Version { get; private set; } = null!;

    public string Title { get; private set; } = null!;

    public DateTime ValidFrom { get; private set; }

    public DateTime? ValidUntil { get; private set; }

    public virtual IReadOnlyCollection<TrainerApproval> TrainerApprovals => _trainerApprovals.AsReadOnly();

    #endregion

    #region Constructors

    protected UserChartRevision()
    {
    }

    public UserChartRevision(string title, string version, DateTime validFrom, DateTime? validUntil)
    {
        Title = title;
        Version = version;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    #endregion

}
